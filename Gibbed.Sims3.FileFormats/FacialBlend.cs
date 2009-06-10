using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats
{
    public class FacialBlendEntry
    {
        public UInt32 Unknown1;
        public UInt32 Unknown2;
        public ResourceKey Unknown3;

        public void Serialize(Stream output, UInt32 version, KeyTable keyTable)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input, UInt32 version, KeyTable keyTable)
        {
            this.Unknown1 = input.ReadU32();
            this.Unknown2 = input.ReadU32();

            if (version < 7)
            {
                this.Unknown3 = input.ReadResourceKey();
            }
            else
            {
                this.Unknown3 = keyTable.Keys[input.ReadS32()];
            }
        }
    }

    public class FacialBlendBlock
    {
        public UInt32 Unknown1;
        public List<FacialBlendEntry> Unknown2;
        public List<FacialBlendEntry> Unknown3;

        public void Serialize(Stream output, UInt32 version, KeyTable keyTable)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input, UInt32 version, KeyTable keyTable)
        {
            this.Unknown1 = input.ReadU32();
            int count;

            count = input.ReadS32();
            this.Unknown2 = new List<FacialBlendEntry>();
            for (int i = 0; i < count; i++)
            {
                FacialBlendEntry entry = new FacialBlendEntry();
                entry.Deserialize(input, version, keyTable);
                this.Unknown2.Add(entry);
            }

            count = input.ReadS32();
            this.Unknown3 = new List<FacialBlendEntry>();
            for (int i = 0; i < count; i++)
            {
                FacialBlendEntry entry = new FacialBlendEntry();
                entry.Deserialize(input, version, keyTable);
                this.Unknown3.Add(entry);
            }
        }
    }

    public class FacialBlend : IFormat
    {
        public UInt32 Version;
        public KeyTable KeyTable;
        public string Unknown1;
        public UInt32 Unknown2;
        public ResourceKey Unknown3;
        public List<FacialBlendBlock> Unknown4;

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            this.Version = input.ReadU32();
            if (this.Version > 8)
            {
                throw new InvalidOperationException("bad version");
            }

            if (this.Version >= 7)
            {
                long keyTableOffset = input.ReadS32();
                int keyTableSize = input.ReadS32();
                keyTableOffset += input.Position;
                keyTableOffset -= 4;

                this.KeyTable = new KeyTable();

                long originalPosition = input.Position;
                input.Seek(keyTableOffset, SeekOrigin.Begin);
                this.KeyTable.Deserialize(input);
                input.Seek(originalPosition, SeekOrigin.Begin);
            }

            this.Unknown1 = input.ReadUTF16(input.ReadU8(), false);
            this.Unknown2 = input.ReadU32();
            this.Unknown3 = (this.Version < 8) ? new ResourceKey(0, 0, 0) : input.ReadResourceKey();

            int count = input.ReadS32();
            this.Unknown4 = new List<FacialBlendBlock>();
            for (int i = 0; i < count; i++)
            {
                FacialBlendBlock block = new FacialBlendBlock();
                block.Deserialize(input, this.Version, this.KeyTable);
                this.Unknown4.Add(block);
            }
        }
    }
}
