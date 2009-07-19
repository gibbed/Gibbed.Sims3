using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gibbed.Helpers;
using Gibbed.RefPack;

namespace Gibbed.Sims3.FileFormats
{
    public class Database
    {
        public class Entry
        {
            public bool Compressed;
            public uint CompressedSize;
            public uint DecompressedSize;
        }

        private class StreamEntry : Entry
        {
            public Int64 Offset;
            public short CompressedFlags;
            public ushort Flags;
        }

        private class MemoryEntry : Entry
        {
            public byte[] Data;
        }

        private Stream Stream;
        private long BaseOffset;
        private long EndOfDataOffset;

        public ReadOnlyDictionary<ResourceKey, Entry> Entries;
        private Dictionary<ResourceKey, Entry> _Entries;
        private Dictionary<ResourceKey, StreamEntry> OriginalEntries;

        public Database(Stream stream)
            : this(stream, true)
        {
        }

        public Database(Stream stream, bool readExisting)
        {
            if (stream.CanSeek == false || stream.CanRead == false)
            {
                throw new ArgumentException("stream must have seek / read access", "stream");
            }

            this.Stream = stream;
            this.BaseOffset = this.Stream.Position;

            this._Entries = new Dictionary<ResourceKey, Entry>();
            this.Entries = new ReadOnlyDictionary<ResourceKey, Entry>(this._Entries);

            this.OriginalEntries = new Dictionary<ResourceKey, StreamEntry>();

            if (readExisting == true)
            {
                DatabasePackedFile dbpf = new DatabasePackedFile();
                dbpf.Read(stream);

                this.EndOfDataOffset = 0;

                foreach (DatabasePackedFile.Entry entry in dbpf.Entries)
                {
                    this._Entries.Add(entry.Key, new StreamEntry()
                        {
                            Compressed = entry.Compressed,
                            Offset = entry.Offset,
                            CompressedSize = entry.CompressedSize,
                            DecompressedSize = entry.DecompressedSize,
                            CompressedFlags = entry.CompressionFlags,
                            Flags = entry.Flags,
                        });

                    if (entry.Offset + entry.CompressedSize > this.EndOfDataOffset)
                    {
                        this.EndOfDataOffset = entry.Offset + entry.CompressedSize;
                    }
                }
            }
            else
            {
                this.EndOfDataOffset = 0;
            }
        }

        public void DeleteResource(ResourceKey key)
        {
            if (this.Stream.CanWrite == false)
            {
                throw new NotSupportedException();
            }

            if (this._Entries.ContainsKey(key) == false)
            {
                throw new KeyNotFoundException();
            }

            if (this._Entries[key] is StreamEntry)
            {
                this.OriginalEntries[key] = (StreamEntry)this._Entries[key];
            }

            this._Entries.Remove(key);
        }

        public void MoveResource(ResourceKey oldKey, ResourceKey newKey)
        {
            if (this.Stream.CanWrite == false)
            {
                throw new NotSupportedException();
            }

            if (this._Entries.ContainsKey(oldKey) == false)
            {
                throw new KeyNotFoundException();
            }
            else if (this._Entries.ContainsKey(newKey) == true)
            {
                throw new ArgumentException("database already contains the new resource key", "newKey");
            }

            Entry entry = this._Entries[oldKey];

            if (entry is StreamEntry)
            {
                this.OriginalEntries[oldKey] = (StreamEntry)entry;
            }

            this._Entries.Remove(oldKey);
            this._Entries.Add(newKey, entry);
        }

        public byte[] GetResource(ResourceKey key)
        {
            if (this._Entries.ContainsKey(key) == false)
            {
                throw new KeyNotFoundException();
            }

            if (this._Entries[key] is StreamEntry)
            {
                StreamEntry entry = (StreamEntry)this._Entries[key];

                this.Stream.Seek(this.BaseOffset + entry.Offset, SeekOrigin.Begin);

                byte[] data;
                if (entry.Compressed == true)
                {
                    data = this.Stream.RefPackDecompress();

                    if (data.Length != entry.DecompressedSize)
                    {
                        throw new InvalidOperationException("decompressed data not same length as specified in index");
                    }
                }
                else
                {
                    data = new byte[entry.DecompressedSize];
                    this.Stream.Read(data, 0, data.Length);
                }

                return data;
            }
            else if (this._Entries[key] is MemoryEntry)
            {
                return (byte[])(((MemoryEntry)this._Entries[key]).Data.Clone());
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public Stream GetResourceStream(ResourceKey key)
        {
            return new MemoryStream(this.GetResource(key));
        }

        public byte[] GetRawResource(ResourceKey key)
        {
            if (this._Entries.ContainsKey(key) == false || !(this._Entries[key] is StreamEntry))
            {
                throw new KeyNotFoundException();
            }

            StreamEntry entry = (StreamEntry)this._Entries[key];
            this.Stream.Seek(this.BaseOffset + entry.Offset, SeekOrigin.Begin);

            byte[] data = new byte[entry.CompressedSize];
            this.Stream.Read(data, 0, data.Length);
            return data;
        }

        public void SetResource(ResourceKey key, byte[] data)
        {
            if (this.Stream.CanWrite == false)
            {
                throw new NotSupportedException();
            }

            if (this._Entries.ContainsKey(key) && this._Entries[key] is StreamEntry)
            {
                this.OriginalEntries[key] = (StreamEntry)this._Entries[key];
            }

            MemoryEntry entry = new MemoryEntry();
            entry.Compressed = false;
            entry.CompressedSize = entry.DecompressedSize = (uint)data.Length;
            entry.Data = (byte[])(data.Clone());

            this._Entries[key] = entry;
        }

        public void SetResourceStream(ResourceKey key, Stream stream)
        {
            byte[] data = new byte[stream.Length];

            long oldPosition = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(data, 0, data.Length);
            stream.Seek(oldPosition, SeekOrigin.Begin);

            this.SetResource(key, data);
        }

        public void Commit()
        {
            this.Commit(false);
        }

        public void Commit(bool cleanCommit)
        {
            if (this.Stream.CanWrite == false)
            {
                throw new NotSupportedException();
            }

            DatabasePackedFile dbpf = new DatabasePackedFile();
            dbpf.Version = new Version(2, 0);

            if (cleanCommit == false)
            {
                if (this.EndOfDataOffset == 0)
                {
                    // new archive
                    this.Stream.Seek(this.BaseOffset, SeekOrigin.Begin);
                    dbpf.WriteHeader(this.Stream, 0, 0);
                    this.EndOfDataOffset = this.Stream.Position - this.BaseOffset;
                }

                foreach (KeyValuePair<ResourceKey, Entry> kvp in this._Entries)
                {
                    DatabasePackedFile.Entry entry = new DatabasePackedFile.Entry();
                    entry.Key = kvp.Key;

                    if (kvp.Value is MemoryEntry)
                    {
                        MemoryEntry memory = (MemoryEntry)kvp.Value;

                        byte[] compressed;
                        bool success = memory.Data.RefPackCompress(out compressed);
                        if (success == true)
                        {
                            entry.DecompressedSize = (uint)(memory.Data.Length);
                            entry.CompressedSize = (uint)(compressed.Length) | 0x80000000;
                            entry.CompressionFlags = -1;
                            entry.Flags = 1;
                            memory.Data = compressed;
                        }
                        else
                        {
                            entry.DecompressedSize = memory.DecompressedSize;
                            entry.CompressedSize = memory.CompressedSize | 0x80000000;
                            entry.CompressionFlags = 0;
                            entry.Flags = 1;
                        }

                        // Is this replacing old data?
                        if (this.OriginalEntries.ContainsKey(kvp.Key) == true)
                        {
                            StreamEntry stream = this.OriginalEntries[kvp.Key];

                            // Let's see if the new data can fit where the old data was
                            if (memory.Data.Length <= stream.CompressedSize)
                            {
                                entry.Offset = stream.Offset;
                                this.Stream.Seek(this.BaseOffset + stream.Offset, SeekOrigin.Begin);
                                this.Stream.Write(memory.Data, 0, memory.Data.Length);
                            }
                            else
                            {
                                entry.Offset = this.EndOfDataOffset;
                                this.Stream.Seek(this.BaseOffset + this.EndOfDataOffset, SeekOrigin.Begin);
                                this.Stream.Write(memory.Data, 0, memory.Data.Length);
                                this.EndOfDataOffset += memory.Data.Length;
                            }
                        }
                        // New data
                        else
                        {
                            entry.Offset = this.EndOfDataOffset;
                            this.Stream.Seek(this.BaseOffset + this.EndOfDataOffset, SeekOrigin.Begin);
                            this.Stream.Write(memory.Data, 0, memory.Data.Length);
                            this.EndOfDataOffset += memory.Data.Length;
                        }
                    }
                    else if (kvp.Value is StreamEntry)
                    {
                        StreamEntry stream = (StreamEntry)kvp.Value;
                        entry.CompressedSize = stream.CompressedSize | 0x80000000;
                        entry.DecompressedSize = stream.DecompressedSize;
                        entry.Offset = stream.Offset;
                        entry.CompressionFlags = stream.CompressedFlags;
                        entry.Flags = stream.Flags;
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }

                    dbpf.Entries.Add(entry);
                }

                this.Stream.Seek(this.BaseOffset + this.EndOfDataOffset, SeekOrigin.Begin);
                dbpf.WriteIndex(this.Stream);

                long indexSize = (this.Stream.Position - (this.BaseOffset + this.EndOfDataOffset));

                this.Stream.Seek(this.BaseOffset, SeekOrigin.Begin);
                dbpf.WriteHeader(this.Stream, this.EndOfDataOffset, indexSize);
            }
            else
            {
                Stream clean;
                string tempFileName = null;

                // Packages greater than five mb will be cleaned with a file supported stream
                if (this.Stream.Length >= (1024 * 1024) * 5)
                {
                    tempFileName = Path.GetTempFileName();
                    clean = File.Open(tempFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                }
                else
                {
                    clean = new MemoryStream();
                }

                dbpf.WriteHeader(clean, 0, 0);
                this.EndOfDataOffset = clean.Position;

                foreach (KeyValuePair<ResourceKey, Entry> kvp in this._Entries)
                {
                    DatabasePackedFile.Entry entry = new DatabasePackedFile.Entry();
                    entry.Key = kvp.Key;

                    if (kvp.Value is MemoryEntry)
                    {
                        MemoryEntry memory = (MemoryEntry)kvp.Value;

                        byte[] compressed;
                        bool success = memory.Data.RefPackCompress(out compressed);
                        if (success == true)
                        {
                            entry.DecompressedSize = (uint)(memory.Data.Length);
                            entry.CompressedSize = (uint)(compressed.Length) | 0x80000000;
                            entry.CompressionFlags = -1;
                            entry.Flags = 1;
                            entry.Offset = this.EndOfDataOffset;
                            memory.Data = compressed;
                        }
                        else
                        {
                            entry.DecompressedSize = memory.DecompressedSize;
                            entry.CompressedSize = memory.CompressedSize | 0x80000000;
                            entry.CompressionFlags = 0;
                            entry.Flags = 1;
                            entry.Offset = this.EndOfDataOffset;
                        }

                        clean.Write(memory.Data, 0, memory.Data.Length);
                        this.EndOfDataOffset += memory.Data.Length;
                    }
                    else if (kvp.Value is StreamEntry)
                    {
                        StreamEntry stream = (StreamEntry)kvp.Value;
                        entry.CompressedSize = stream.CompressedSize | 0x80000000;
                        entry.DecompressedSize = stream.DecompressedSize;
                        entry.CompressionFlags = stream.CompressedFlags;
                        entry.Flags = stream.Flags;

                        entry.Offset = this.EndOfDataOffset;

                        // Copy data
                        this.Stream.Seek(this.BaseOffset + stream.Offset, SeekOrigin.Begin);
                        byte[] data = new byte[4096];
                        int left = (int)stream.CompressedSize;
                        while (left > 0)
                        {
                            int block = Math.Min(left, (int)data.Length);
                            this.Stream.Read(data, 0, block);
                            clean.Write(data, 0, block);
                            left -= block;
                        }

                        this.EndOfDataOffset += stream.CompressedSize;
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }

                    dbpf.Entries.Add(entry);
                }

                dbpf.WriteIndex(clean);

                long indexSize = clean.Position - this.EndOfDataOffset;

                clean.Seek(0, SeekOrigin.Begin);
                dbpf.WriteHeader(clean, this.EndOfDataOffset, indexSize);

                // copy clean to real stream
                {
                    this.Stream.Seek(this.BaseOffset, SeekOrigin.Begin);
                    clean.Seek(0, SeekOrigin.Begin);
                    
                    byte[] data = new byte[4096];
                    long left = clean.Length;
                    while (left > 0)
                    {
                        int block = (int)Math.Min(left, data.Length);
                        clean.Read(data, 0, block);
                        this.Stream.Write(data, 0, block);
                        left -= block;
                    }
                }

                this.Stream.SetLength(this.BaseOffset + this.EndOfDataOffset + indexSize);
                clean.Close();

                if (tempFileName != null)
                {
                    File.Delete(tempFileName);
                }
            }

            this._Entries.Clear();
            this.OriginalEntries.Clear();

            foreach (DatabasePackedFile.Entry entry in dbpf.Entries)
            {
                this._Entries.Add(entry.Key, new StreamEntry()
                {
                    Compressed = entry.Compressed,
                    Offset = entry.Offset,
                    CompressedSize = entry.CompressedSize,
                    DecompressedSize = entry.DecompressedSize,
                    CompressedFlags = entry.CompressionFlags,
                    Flags = entry.Flags,
                });
            }
        }

        public void Rollback()
        {
            if (this.Stream.CanWrite == false)
            {
                throw new NotSupportedException();
            }

            foreach (KeyValuePair<ResourceKey, StreamEntry> entry in this.OriginalEntries)
            {
                this._Entries[entry.Key] = entry.Value;
            }

            foreach (KeyValuePair<ResourceKey, Entry> entry in this._Entries.Where(entry => entry.Value is MemoryEntry).ToList())
            {
                this._Entries.Remove(entry.Key);
            }
        }
    }
}
