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

        public void Serialize(Stream output, FacialBlend parent)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input, FacialBlend parent)
        {
            this.Unknown1 = input.ReadValueU32();
            this.Unknown2 = input.ReadValueU32();

            if (parent.Version < 7)
            {
                this.Unknown3 = input.ReadResourceKeyTGI();
            }
            else
            {
                this.Unknown3 = parent.KeyTable.Keys[input.ReadValueS32()];
            }
        }
    }

    public class FacialBlendRegion
    {
        public UInt32 Flags;
        public List<FacialBlendEntry> BlendEntries;
        public List<FacialBlendEntry> BoneEntries;

        public void Serialize(Stream output, FacialBlend parent)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input, FacialBlend parent)
        {
            this.Flags = input.ReadValueU32();
            int count;

            count = input.ReadValueS32();
            this.BlendEntries = new List<FacialBlendEntry>();
            for (int i = 0; i < count; i++)
            {
                FacialBlendEntry entry = new FacialBlendEntry();
                entry.Deserialize(input, parent);
                this.BlendEntries.Add(entry);
            }

            count = input.ReadValueS32();
            this.BoneEntries = new List<FacialBlendEntry>();
            for (int i = 0; i < count; i++)
            {
                FacialBlendEntry entry = new FacialBlendEntry();
                entry.Deserialize(input, parent);
                this.BoneEntries.Add(entry);
            }
        }
    }

    public class FacialBlend : IFormat
    {
        public UInt32 Version;
        public KeyTable KeyTable;
        public string Name;
        public UInt32 Unknown2;
        public ResourceKey Unknown3;
        public List<FacialBlendRegion> Regions;

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            this.Version = input.ReadValueU32();
            if (this.Version > 8)
            {
                throw new InvalidOperationException("bad version");
            }

            if (this.Version >= 7)
            {
                this.KeyTable = input.ReadKeyTable(4);
            }

            this.Name = input.ReadStringUTF16(input.ReadValueU8(), false);
            this.Unknown2 = input.ReadValueU32();
            this.Unknown3 = (this.Version < 8) ? new ResourceKey(0, 0, 0) : input.ReadResourceKeyTGI();

            int count = input.ReadValueS32();
            this.Regions = new List<FacialBlendRegion>();
            for (int i = 0; i < count; i++)
            {
                FacialBlendRegion block = new FacialBlendRegion();
                block.Deserialize(input, this);
                this.Regions.Add(block);
            }
        }
    }
}
