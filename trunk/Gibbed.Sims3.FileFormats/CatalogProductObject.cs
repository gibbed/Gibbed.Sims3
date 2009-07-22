using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats
{
    public class CatalogProductObject : CatalogProduct
    {
        public class PresetReference
        {
            public bool Internal;
            public Stream Stream;
            public string Name;
            public UInt32 Unknown;
        }

        public class UnknownClass1
        {
            public UInt32 Unknown00;
            public UInt32 Unknown04;
            public UInt32 Unknown08;
            public UInt32 Unknown0C;
            public UInt32 Unknown10;
            public ResourceKey Unknown18;

            public UnknownClass1(Stream input, KeyTable keyTable)
            {
                this.Deserialize(input, keyTable);
            }

            public void Serialize(Stream output, KeyTable keyTable)
            {
                throw new NotImplementedException();
            }

            public void Deserialize(Stream input, KeyTable keyTable)
            {
                this.Unknown00 = input.ReadValueU32();
                this.Unknown04 = input.ReadValueU32();
                this.Unknown08 = input.ReadValueU32();
                this.Unknown0C = input.ReadValueU32();
                this.Unknown10 = input.ReadValueU32();
                this.Unknown18 = keyTable.Keys[input.ReadValueS32()];
            }
        }

        public List<PresetReference> DesignModePresets;

        public ResourceKey Unknown90;

        public UInt32 UnknownA0;
        public UInt32 UnknownA8;
        public UInt32 UnknownAC;
        public UInt32 UnknownDC;
        public UInt32 UnknownE0;

        public List<UnknownClass1> UnknownCC;
        public bool UnknownF8;

        public ResourceKey Unknown100;

        public UInt32 UnknownE4;

        public UInt32 UnknownB0;
        public UInt32 UnknownB4;
        public UInt64 UnknownB8;
        public UInt64 UnknownC0;
        public UInt32 UnknownC8;

        public ResourceKey Unknown110;

        public UInt32 UnknownA4;

        public string UnknownString1;
        public string UnknownString2;

        public UInt32 Unknown150;
        public UInt32 Unknown154;

        public class UnknownClass2
        {
            public UInt32 Unknown0;
            public UInt32 Unknown4;

            public UnknownClass2(Stream input)
            {
                this.Deserialize(input);
            }

            public void Serialize(Stream output)
            {
                output.WriteValueU32(this.Unknown0);
                output.WriteValueU32(this.Unknown4);
            }

            public void Deserialize(Stream input)
            {
                this.Unknown0 = input.ReadValueU32();
                this.Unknown4 = input.ReadValueU32();
            }
        }

        public List<UnknownClass2> Unknown158;

        public ResourceKey Unknown120;

        private void FixUnknownC8()
        {
            if (this.UnknownC8 == 0x00000000 || this.UnknownC8 == 0x80000000)
            {
                if ((this.UnknownA0 & 0x08) == 0x08)
                {
                    this.UnknownC8 |= 0x02;
                }

                if ((this.UnknownA0 & 0x10) == 0x10)
                {
                    this.UnknownC8 |= 0x04;
                }

                if ((this.UnknownA0 & 0x20) == 0x20)
                {
                    this.UnknownC8 |= 0x08u;
                }

                if ((this.UnknownA0 & 0x80) == 0x80)
                {
                    this.UnknownC8 |= 0x20;
                }

                if ((this.UnknownA0 & 0x800) == 0x800)
                {
                    this.UnknownC8 |= 0x40;
                }

                if ((this.UnknownA0 & 0x1000) == 0x1000)
                {
                    this.UnknownC8 |= 0x80;
                }
                
                if ((this.UnknownA0 & 0x2000) == 0x2000)
                {
                    this.UnknownC8 |= 0x200;
                }

                if ((this.UnknownA0 & 0x4000) == 0x4000)
                {
                    this.UnknownC8 |= 0x10;
                }
            }
        }

        public override void Deserialize(Stream input)
        {
            uint version = input.ReadValueU32();

            if (version < 17)
            {
                throw new FormatException("unsupported version < 17");
            }
            else if (version > 21)
            {
                throw new FormatException("unsupported version > 21");
            }

            KeyTable keyTable = input.ReadKeyTable(4);

            this.DesignModePresets = new List<PresetReference>();
            if (version >= 17)
            {
                uint count = input.ReadValueU32();
                for (uint i = 0; i < count; i++)
                {
                    PresetReference reference = new PresetReference();
                    reference.Internal = version >= 21 ? input.ReadValueBoolean() : false;

                    if (reference.Internal == true)
                    {
                        reference.Stream = input.ReadToMemoryStream(input.ReadValueU32());
                    }
                    else
                    {
                        reference.Name = input.ReadStringUTF16(input.ReadValueU32());
                    }

                    reference.Unknown = input.ReadValueU32();
                    this.DesignModePresets.Add(reference);
                }
            }

            base.Deserialize(input);

            this.Unknown90 = keyTable.Keys[input.ReadValueS32()];

            this.UnknownA0 = input.ReadValueU32();
            this.UnknownA8 = input.ReadValueU32();
            this.UnknownAC = input.ReadValueU32();
            this.UnknownDC = input.ReadValueU32();
            this.UnknownE0 = input.ReadValueU32();

            this.UnknownCC = new List<UnknownClass1>();
            {
                byte count = input.ReadValueU8();
                for (byte i = 0; i < count; i++)
                {
                    this.UnknownCC.Add(new UnknownClass1(input, keyTable));
                }
            }

            this.UnknownF8 = input.ReadValueBoolean();

            this.Unknown100 = keyTable.Keys[input.ReadValueS32()];

            this.UnknownE4 = input.ReadValueU32();

            if (version >= 20)
            {
                this.UnknownB0 = input.ReadValueU32();
                this.UnknownB4 = input.ReadValueU32();
                this.UnknownB8 = input.ReadValueU64();
                this.UnknownC0 = input.ReadValueU64();
                this.UnknownC8 = input.ReadValueU32();
            }
            else
            {
                this.UnknownB0 = input.ReadValueU32();
                this.UnknownB4 = input.ReadValueU32();
                this.UnknownB8 = input.ReadValueU64();
                this.UnknownC8 = input.ReadValueU32();
            }

            if (this.UnknownB0 == 0)
            {
                this.UnknownB0 = 0x80000000;
            }

            if (this.UnknownB4 == 0)
            {
                this.UnknownB4 = 0x80000000;
            }

            if (this.UnknownB8 == 0)
            {
                this.UnknownB8 = 0x8000000000000000;
            }

            if (this.UnknownC0 == 0)
            {
                this.UnknownC0 = 0x8000000000000000;
            }

            if (this.UnknownC8 == 0)
            {
                this.UnknownC8 = 0x80000000;
            }

            this.FixUnknownC8();

            this.Unknown110 = keyTable.Keys[input.ReadValueS32()];

            this.UnknownA4 = input.ReadValueU32();

            this.UnknownString1 = input.ReadStringUTF16(input.ReadValueU8(), false);
            this.UnknownString2 = input.ReadStringUTF16(input.ReadValueU8(), false);

            if (version >= 19)
            {
                this.Unknown150 = input.ReadValueU32();
                this.Unknown154 = input.ReadValueU32();
                this.Unknown158 = new List<UnknownClass2>();
                uint count = input.ReadValueU32();
                for (uint i = 0; i < count; i++)
                {
                    this.Unknown158.Add(new UnknownClass2(input));
                }
            }

            if (version >= 18)
            {
                this.Unknown120 = keyTable.Keys[input.ReadValueS32()];
            }
        }
    }
}
