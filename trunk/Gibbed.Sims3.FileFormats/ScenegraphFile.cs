using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats
{
    public class ScenegraphFile : IFormat
    {
        public Boolean LittleEndian;
        public UInt32 Version;

        public int Unknown1;
        public List<ResourceKey> TextureKeys;
        public List<ResourceKey> Unknown2Keys;
        public List<ResourceKey> ChunkKeys;
        public List<Stream> ChunkData;

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        private ResourceKey[] ReadResourceKeys(Stream input, int count)
        {
            // this may need to be checked for compatability with big-endian scenegraph files.
            ResourceKey[] keys = new ResourceKey[count];
            for (int i = 0; i < count; i++)
            {
                ResourceKey key;
                if (this.LittleEndian == true)
                {
                    key.InstanceId = input.ReadU64();
                }
                else
                {
                    key.InstanceId = 0;
                    key.InstanceId |= input.ReadU32();
                    key.InstanceId |= (UInt64)(input.ReadU32()) << 32;
                }
                key.TypeId = input.ReadU32();
                key.GroupId = input.ReadU32();
                keys[i] = key;
            }

            return keys;
        }

        private struct ChunkLocation
        {
            public UInt32 Offset;
            public UInt32 Size;
        }

        public void Deserialize(Stream input)
        {
            this.Version = input.ReadU32();

            // Detect little endian mode.
            if ((this.Version & 0xFFFF0000) != 0 && (this.Version & 0x0000FFFF) == 0)
            {
                this.LittleEndian = false;
            }
            else
            {
                this.LittleEndian = true;
            }

            this.Unknown1 = input.ReadS32(this.LittleEndian);
            int unknown2Count = input.ReadS32(this.LittleEndian);
            int textureCount = input.ReadS32(this.LittleEndian);
            int chunkCount = input.ReadS32(this.LittleEndian);

            if (unknown2Count != 0)
            {
                throw new Exception();
            }

            this.ChunkKeys = new List<ResourceKey>(this.ReadResourceKeys(input, chunkCount));
            this.Unknown2Keys = new List<ResourceKey>(this.ReadResourceKeys(input, unknown2Count));
            this.TextureKeys = new List<ResourceKey>(this.ReadResourceKeys(input, textureCount));

            ChunkLocation[] chunks = new ChunkLocation[chunkCount];
            for (int i = 0; i < chunkCount; i++)
            {
                chunks[i].Size = input.ReadU32(this.LittleEndian);
                chunks[i].Offset = input.ReadU32(this.LittleEndian);
            }

            this.ChunkData = new List<Stream>();
            for (int i = 0; i < chunkCount; i++)
            {
                input.Seek(chunks[i].Offset, SeekOrigin.Begin);
                byte[] data = new byte[chunks[i].Size];
                input.Read(data, 0, data.Length);
                this.ChunkData.Add(new MemoryStream(data));
            }
        }
    }
}
