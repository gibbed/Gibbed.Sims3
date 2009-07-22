using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats
{
    public class CatalogProduct : IFormat
    {
        public UInt64 CatalogName; // 30
        public UInt64 Description; // 38
        public string Unknown40;
        public string Unknown50;
        public UInt32 Unknown60;
        public UInt32 Unknown64;
        public UInt32 Unknown68;
        public byte StatusFlag; // 6C
        public UInt64 Unknown70;
        public float EnvironmentScore; // 78
        public UInt32 FireType; // 80
        public UInt32 Unknown84;
        public bool IsStealable; // 88
        public bool IsReposesable; // 89

        public string SubSort; // unused?
        public byte UnknownByte;

        public virtual void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public virtual void Deserialize(Stream input)
        {
            uint version = input.ReadValueU32();

            if (version >= 10)
            {
                this.CatalogName = input.ReadValueU64();
                this.Description = input.ReadValueU64();
            }

            this.Unknown40 = input.ReadStringUTF16(input.ReadValueU8(), false);
            this.Unknown50 = input.ReadStringUTF16(input.ReadValueU8(), false);

            this.Unknown60 = input.ReadValueU32();
            this.Unknown64 = input.ReadValueU32();
            this.Unknown68 = input.ReadValueU32();
            this.StatusFlag = input.ReadValueU8();
            
            if (version < 5)
            {
                if ((this.StatusFlag & 2) == 2)
                {
                    this.StatusFlag |= 0x10;
                }
                else
                {
                    this.StatusFlag |= 0x20;
                }
            }

            if (version < 6)
            {
                this.SubSort = input.ReadStringUTF16(input.ReadValueU8(), false);
            }

            if (version >= 9)
            {
                this.Unknown70 = input.ReadValueU64();
            }
            else
            {
                ResourceKey key = input.ReadResourceKeyIGT();
                this.Unknown70 = key.InstanceId;
            }

            this.UnknownByte = input.ReadValueU8();

            if (version >= 3)
            {
                this.EnvironmentScore = input.ReadValueF32();
            }

            if (version >= 7)
            {
                this.FireType = input.ReadValueU32();
            }

            if (version >= 8)
            {
                this.IsStealable = input.ReadValueBoolean();
            }

            if (version >= 11)
            {
                this.IsReposesable = input.ReadValueBoolean();
            }

            if (version >= 12)
            {
                this.Unknown84 = input.ReadValueU32();
            }

            if (version < 10)
            {
                this.CatalogName = input.ReadValueU64();
                this.Description = input.ReadValueU64();
            }
        }
    }
}
