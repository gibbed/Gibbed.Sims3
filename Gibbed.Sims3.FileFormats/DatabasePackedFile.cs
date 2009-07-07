using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats
{
    public class DatabasePackedFile
	{
        public struct Entry
        {
            public ResourceKey Key;
            #region public bool Compressed;
            public bool Compressed
            {
                get
                {
                    return this.CompressionFlags == -1;
                }
            }
            #endregion

            public Int64 Offset;
            public uint CompressedSize;
            public uint DecompressedSize;
            public short CompressionFlags;
            public ushort Flags;
        }

        public bool Big;
		public Version Version = new Version();
        public List<Entry> Entries = new List<Entry>();
        public long IndexOffset;
        public int IndexType;

		public void Read(Stream input)
		{
			Int64 indexCount;
			Int64 indexSize;
			Int64 indexOffset;

			string magic = input.ReadStringASCII(4);
            if (magic != "DBPF" && magic != "DBBF") // DBPF & DBBF
			{
				throw new NotAPackageException();
			}

            this.Big = magic == "DBBF";

            if (this.Big == true)
			{
                BigHeader header = input.ReadStructure<BigHeader>();

				if (header.IndexVersion != 3)
				{
                    throw new DatabasePackedFileException("index version was not 3");
				}

				// Nab useful stuff
				this.Version = new Version(header.MajorVersion, header.MinorVersion);
				indexCount = header.IndexCount;
				indexOffset = header.IndexOffset;
				indexSize = header.IndexSize;
			}
			else
			{
                Header header = input.ReadStructure<Header>();

				if (header.IndexVersion != 3)
				{
                    throw new DatabasePackedFileException("index version was not 3");
				}

				// Nab useful stuff
				this.Version = new Version(header.MajorVersion, header.MinorVersion);
				indexCount = header.IndexCount;
				indexOffset = header.IndexOffset;
				indexSize = header.IndexSize;
			}

            this.IndexOffset = indexOffset;
            this.Entries.Clear();

			if (indexCount > 0)
			{
				// Read index
				input.Seek(indexOffset, SeekOrigin.Begin);

				int presentPackageValues = input.ReadValueS32();
                this.IndexType = presentPackageValues;
				if ((presentPackageValues & ~7) != 0)
				{
					throw new InvalidDataException("don't know how to handle this index data");
				}

                bool hasPackageTypeId = (presentPackageValues & (1 << 0)) == 1 << 0;
                bool hasPackageGroupId = (presentPackageValues & (1 << 1)) == 1 << 1;
                bool hasPackageHiInstanceId = (presentPackageValues & (1 << 2)) == 1 << 2;

                uint packageTypeId = hasPackageTypeId ? input.ReadValueU32() : 0xFFFFFFFF;
                uint packageGroupId = hasPackageGroupId ? input.ReadValueU32() : 0xFFFFFFFF;
                uint packageHiInstanceId = hasPackageHiInstanceId ? input.ReadValueU32() : 0xFFFFFFFF;

				for (int i = 0; i < indexCount; i++)
				{
                    Entry entry = new Entry();

                    entry.Key.TypeId = hasPackageTypeId ? packageTypeId : input.ReadValueU32();
                    entry.Key.GroupId = hasPackageGroupId ? packageGroupId : input.ReadValueU32();
                    entry.Key.InstanceId = 0;
                    entry.Key.InstanceId |= (hasPackageHiInstanceId ? packageHiInstanceId : input.ReadValueU32());
                    entry.Key.InstanceId <<= 32;
                    entry.Key.InstanceId |= input.ReadValueU32();

                    entry.Offset = (this.Big == true) ? input.ReadValueS64() : input.ReadValueS32();
					entry.CompressedSize = input.ReadValueU32();
					entry.DecompressedSize = input.ReadValueU32();

                    // compressed bit
                    if ((entry.CompressedSize & 0x80000000) == 0x80000000)
                    {
                        entry.CompressedSize &= ~0x80000000;
                        entry.CompressionFlags = input.ReadValueS16();
                        entry.Flags = input.ReadValueU16();
                    }
                    else
                    {
                        if (entry.CompressedSize != entry.DecompressedSize)
                        {
                            entry.CompressionFlags = -1;
                        }
                        else
                        {
                            entry.CompressionFlags = 0;
                        }

                        entry.Flags = 0;

                        throw new DatabasePackedFileException("strange index data");
                    }

                    if (entry.CompressionFlags != 0 && entry.CompressionFlags != -1)
                    {
                        throw new DatabasePackedFileException("bad compression flags");
                    }

					this.Entries.Add(entry);
				}
			}
		}

        public void WriteHeader(Stream output, long indexOffset, long indexSize)
		{
            if (this.Big == true)
            {
                output.WriteStringASCII("DBBF");
                BigHeader header = new BigHeader();
                header.MajorVersion = this.Version.Major;
                header.MinorVersion = this.Version.Minor;
                header.IndexVersion = 3;
                header.IndexCount = this.Entries.Count;
                header.IndexOffset = indexOffset;
                header.IndexSize = indexSize;
                output.WriteStructure<BigHeader>(header);
            }
            else
            {
                output.WriteStringASCII("DBPF");
                Header header = new Header();
                header.MajorVersion = this.Version.Major;
                header.MinorVersion = this.Version.Minor;
                header.IndexVersion = 3;
                header.IndexCount = this.Entries.Count;
                header.IndexOffset = (int)indexOffset;
                header.IndexSize = (int)indexSize;
                output.WriteStructure<Header>(header);
            }
		}

		public void WriteIndex(Stream output)
		{
            if (this.Entries.Count == 0)
            {
                output.WriteValueU32(0); // present package values
            }
            else
            {
                bool hasPackageTypeId = true;
                bool hasPackageGroupId = true;
                bool hasPackageHiInstanceId = true;

                uint packageTypeId = this.Entries[0].Key.TypeId;
                uint packageGroupId = this.Entries[0].Key.GroupId;
                uint packageHiInstanceId = (uint)(this.Entries[0].Key.InstanceId >> 32);

                for (int i = 1; i < this.Entries.Count; i++)
                {
                    hasPackageTypeId = hasPackageTypeId && (packageTypeId == this.Entries[i].Key.TypeId);
                    hasPackageGroupId = hasPackageGroupId && (packageGroupId == this.Entries[i].Key.GroupId);
                    hasPackageHiInstanceId = hasPackageHiInstanceId && (packageHiInstanceId == (uint)(this.Entries[i].Key.InstanceId >> 32));

                    if (hasPackageTypeId == false && hasPackageGroupId == false && hasPackageHiInstanceId == false)
                    {
                        break;
                    }
                }

                int presentPackageValues = 0;
                presentPackageValues |= (hasPackageTypeId ? 1 : 0) << 0;
                presentPackageValues |= (hasPackageGroupId ? 1 : 0) << 1;
                presentPackageValues |= (hasPackageHiInstanceId ? 1 : 0) << 2;

                output.WriteValueS32(presentPackageValues);

                if (hasPackageTypeId == true)
                {
                    output.WriteValueU32(packageTypeId);
                }

                if (hasPackageGroupId == true)
                {
                    output.WriteValueU32(packageGroupId);
                }

                if (hasPackageHiInstanceId == true)
                {
                    output.WriteValueU32(packageHiInstanceId);
                }

                foreach (Entry entry in this.Entries)
                {
                    if (hasPackageTypeId == false)
                    {
                        output.WriteValueU32(entry.Key.TypeId);
                    }

                    if (hasPackageGroupId == false)
                    {
                        output.WriteValueU32(entry.Key.GroupId);
                    }

                    if (hasPackageHiInstanceId == false)
                    {
                        output.WriteValueU32((UInt32)(entry.Key.InstanceId >> 32));
                    }

                    output.WriteValueU32((UInt32)(entry.Key.InstanceId & 0xFFFFFFFF));

                    if (this.Big == true)
                    {
                        output.WriteValueS64(entry.Offset);
                    }
                    else
                    {
                        output.WriteValueS32((int)entry.Offset);
                    }

                    output.WriteValueU32(entry.CompressedSize);
                    output.WriteValueU32(entry.DecompressedSize);
                    output.WriteValueS16(entry.CompressionFlags);
                    output.WriteValueU16(entry.Flags);
                }
            }
		}

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct Header
        {
            //public uint Magic;		// 00
            public int MajorVersion;	// 04
            public int MinorVersion;	// 08
            public uint Unknown0C;		// 0C
            public uint Unknown10;		// 10
            public uint Unknown14;		// 14 - always 0?
            public uint Unknown18;		// 18 - always 0?
            public uint Unknown1C;		// 1C - always 0?
            public uint Unknown20;		// 20
            public int IndexCount;		// 24 - Number of index entries in the package.
            public uint Unknown28;		// 28
            public int IndexSize;		// 2C - The total size in bytes of index entries.
            public uint Unknown30;		// 30
            public uint Unknown34;		// 34
            public uint Unknown38;		// 38
            public uint IndexVersion;	// 3C - Always 3?
            public int IndexOffset;		// 40 - Absolute offset in package to the index header.
            public uint Unknown44;		// 44
            public uint Unknown48;		// 48
            public uint Unknown4C;		// 4C
            public uint Unknown50;		// 50
            public uint Unknown54;		// 54
            public uint Unknown58;		// 58
            public uint Unknown5C;		// 5C
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct BigHeader
        {
            //public uint Magic;		// 00
            public int MajorVersion;	// 04
            public int MinorVersion;	// 08
            public uint Unknown0C;		// 0C
            public uint Unknown10;		// 10
            public uint Unknown14;		// 14 - always 0?
            public uint Unknown18;		// 18 - always 0?
            public uint Unknown1C;		// 1C - always 0?
            public uint Unknown20;		// 20
            public int IndexCount;		// 24 - Number of index entries in the package.
            public Int64 IndexSize;		// 28 - The total size in bytes of index entries.
            public uint Unknown30;		// 30
            public uint IndexVersion;	// 34 - Always 3?
            public Int64 IndexOffset;	// 38 - Absolute offset in package to the index header.
            public uint Unknown40;		// 40
            public uint Unknown44;		// 44
            public uint Unknown48;		// 48
            public uint Unknown4C;		// 4C
            public uint Unknown50;		// 50
            public uint Unknown54;		// 54
            public uint Unknown58;		// 58
            public uint Unknown5C;		// 5C
            public uint Unknown60;		// 60
            public uint Unknown64;		// 64
            public uint Unknown68;		// 68
            public uint Unknown6C;		// 6C
            public uint Unknown70;		// 70
            public uint Unknown74;		// 74
        }
	}
}
