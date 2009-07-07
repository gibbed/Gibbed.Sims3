using System;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class U007E69B0 : IFormat
    {
        public float Unknown1;
        public float Unknown2;
        public float Unknown3;
        public float Unknown4;
        public float Unknown5;
        public float Unknown6;
        public float Unknown7;
        public float Unknown8;
        public float Unknown9;

        public void Serialize(Stream output)
        {
            output.WriteValueF32(this.Unknown1, true);
            output.WriteValueF32(this.Unknown2, true);
            output.WriteValueF32(this.Unknown3, true);
            output.WriteValueF32(this.Unknown4, true);
            output.WriteValueF32(this.Unknown5, true);
            output.WriteValueF32(this.Unknown6, true);
            output.WriteValueF32(this.Unknown7, true);
            output.WriteValueF32(this.Unknown8, true);
            output.WriteValueF32(this.Unknown9, true);
        }

        public void Deserialize(Stream input)
        {
            this.Unknown1 = input.ReadValueF32(true);
            this.Unknown2 = input.ReadValueF32(true);
            this.Unknown3 = input.ReadValueF32(true);
            this.Unknown4 = input.ReadValueF32(true);
            this.Unknown5 = input.ReadValueF32(true);
            this.Unknown6 = input.ReadValueF32(true);
            this.Unknown7 = input.ReadValueF32(true);
            this.Unknown8 = input.ReadValueF32(true);
            this.Unknown9 = input.ReadValueF32(true);
        }
    }
}
