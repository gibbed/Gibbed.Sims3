using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class VisualEffect : IEffectFormat
    {
        public short MinimumVersion { get { return 1; } }
        public short MaximumVersion { get { return 1; } }

        public uint Unknown1;
        public uint Unknown2;
        public uint Unknown3;
        public float Unknown4;
        public float Unknown5;
        public uint Unknown6;
        public byte Unknown7;
        public U007E2000 Unknown8;
        public float Unknown9;
        public float Unknown10;
        public float Unknown11;
        public uint Unknown12;
        public List<UnknownType> Unknown13;

        public void Serialize(Stream output, short version)
        {
            output.WriteValueU32(this.Unknown1, false);
            output.WriteValueU32(this.Unknown2, false);
            output.WriteValueU32(this.Unknown3, false);
            output.WriteValueF32(this.Unknown4, true);
            output.WriteValueF32(this.Unknown5, true);
            output.WriteValueU32(this.Unknown6, false);
            output.WriteValueU8(this.Unknown7);
            this.Unknown8.Serialize(output);
            output.WriteValueF32(this.Unknown9, true);
            output.WriteValueF32(this.Unknown10, true);
            output.WriteValueF32(this.Unknown11, true);
            output.WriteValueU32(this.Unknown12, false);
            output.WriteValueS32(this.Unknown13.Count);
            foreach (UnknownType unknownType in this.Unknown13)
            {
                unknownType.Serialize(output, version);
            }
        }

        public void Deserialize(Stream input, short version)
        {
            this.Unknown1 = input.ReadValueU32(false);
            this.Unknown2 = input.ReadValueU32(false);
            this.Unknown3 = input.ReadValueU32(false);
            this.Unknown4 = input.ReadValueF32(true);
            this.Unknown5 = input.ReadValueF32(true);
            this.Unknown6 = input.ReadValueU32(false);
            this.Unknown7 = input.ReadValueU8();
            
            this.Unknown8 = new U007E2000();
            this.Unknown8.Deserialize(input);

            this.Unknown9 = input.ReadValueF32(true);
            this.Unknown10 = input.ReadValueF32(true);
            this.Unknown11 = input.ReadValueF32(true);
            this.Unknown12 = input.ReadValueU32(false);

            this.Unknown13 = new List<UnknownType>();
            int count = input.ReadValueS32(false);
            for (int i = 0; i < count; i++)
            {
                UnknownType unknownType = new UnknownType();
                unknownType.Deserialize(input, version);
                this.Unknown13.Add(unknownType);
            }
        }

        public class UnknownType
        {
            public byte Unknown01;
            public uint Unknown02;
            public ushort Unknown03;
            public uint Unknown04;
            public U007E69B0 Unknown05;
            public float Unknown06;
            public float Unknown07;
            public float Unknown08;
            public byte Unknown09;
            public byte Unknown10;
            public U008456A0 Unknown11;
            public uint Unknown12;
            public uint Unknown13;
            public uint Unknown14;
            public uint Unknown15;
            public uint Unknown16;
            public uint Unknown17;
            public uint Unknown18;
            public uint Unknown19;
            public ushort Unknown20;
            public ushort Unknown21;
            public uint Unknown22;
            public uint Unknown23;
            public byte Unknown24;
            public byte Unknown25;

            public void Serialize(Stream output, short version)
            {
                output.WriteValueU8(this.Unknown01);
                output.WriteValueU32(this.Unknown02, false);
                output.WriteValueU16(this.Unknown03, false);
                output.WriteValueU32(this.Unknown04, false);
                this.Unknown05.Serialize(output);
                output.WriteValueF32(this.Unknown06, true);
                output.WriteValueF32(this.Unknown07, true);
                output.WriteValueF32(this.Unknown08, true);
                output.WriteValueU8(this.Unknown09);
                output.WriteValueU8(this.Unknown10);
                this.Unknown11.Serialize(output);
                output.WriteValueU32(this.Unknown12, false);
                output.WriteValueU32(this.Unknown13, false);
                output.WriteValueU32(this.Unknown14, false);
                output.WriteValueU32(this.Unknown15, false);
                output.WriteValueU32(this.Unknown16, false);
                output.WriteValueU32(this.Unknown17, false);
                output.WriteValueU32(this.Unknown18, false);
                output.WriteValueU32(this.Unknown19, false);
                output.WriteValueU16(this.Unknown20, false);
                output.WriteValueU16(this.Unknown21, false);
                output.WriteValueU32(this.Unknown22, false);
                output.WriteValueU32(this.Unknown23, false);

                if (version >= 2)
                {
                    output.WriteValueU8(this.Unknown24);
                    output.WriteValueU8(this.Unknown25);
                }
            }

            public void Deserialize(Stream input, short version)
            {
                this.Unknown01 = input.ReadValueU8();
                this.Unknown02 = input.ReadValueU32(false);
                this.Unknown03 = input.ReadValueU16(false);
                this.Unknown04 = input.ReadValueU32(false);

                this.Unknown05 = new U007E69B0();
                this.Unknown05.Deserialize(input);

                this.Unknown06 = input.ReadValueF32(true);
                this.Unknown07 = input.ReadValueF32(true);
                this.Unknown08 = input.ReadValueF32(true);
                this.Unknown09 = input.ReadValueU8();
                this.Unknown10 = input.ReadValueU8();

                this.Unknown11 = new U008456A0();
                this.Unknown11.Deserialize(input);

                this.Unknown12 = input.ReadValueU32(false);
                this.Unknown13 = input.ReadValueU32(false);
                this.Unknown14 = input.ReadValueU32(false);
                this.Unknown15 = input.ReadValueU32(false);
                this.Unknown16 = input.ReadValueU32(false);
                this.Unknown17 = input.ReadValueU32(false);
                this.Unknown18 = input.ReadValueU32(false);
                this.Unknown19 = input.ReadValueU32(false);
                this.Unknown20 = input.ReadValueU16(false);
                this.Unknown21 = input.ReadValueU16(false);
                this.Unknown22 = input.ReadValueU32(false);
                this.Unknown23 = input.ReadValueU32(false);

                if (version >= 2)
                {
                    this.Unknown24 = input.ReadValueU8();
                    this.Unknown25 = input.ReadValueU8();
                }
            }
        }
    }
}
