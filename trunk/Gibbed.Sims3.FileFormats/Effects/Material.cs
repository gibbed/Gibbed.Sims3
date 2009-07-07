using System;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class Material : IEffectFormat
    {
        public short MinimumVersion { get { return 0; } }
        public short MaximumVersion { get { return 0; } }

        public UInt64 Unknown1;
        public UInt64 Unknown2;

        public void Serialize(Stream output, short version)
        {
            throw new NotImplementedException();
        }

        private void DeserializeData(Stream input)
        {
            input.ReadValueU64(false);
            byte type = input.ReadValueU8();

            switch (type)
            {
                case 0: input.ReadValueU32(false); break;
                case 1: input.ReadValueU32(false); break;
                case 2: input.ReadValueU8(); break;
                case 3:
                {
                    int count = input.ReadValueU16(false);
                    for (int i = 0; i < count; i++)
                    {
                        input.ReadValueU32(false);
                    }
                    break;
                }
                case 4:
                {
                    int count = input.ReadValueU16(false);
                    for (int i = 0; i < count; i++)
                    {
                        input.ReadValueU32(false);
                    }
                    break;
                }
                case 5:
                {
                    int count = input.ReadValueU16(false);
                    for (int i = 0; i < count; i++)
                    {
                        input.ReadValueU8();
                    }
                    break;
                }
                case 6: input.ReadValueU64(false); break;
            }
        }

        public void Deserialize(Stream input, short version)
        {
            this.Unknown1 = input.ReadValueU64(false);
            this.Unknown2 = input.ReadValueU64(false);
            
            int count = input.ReadValueS32(false);
            for (int i = 0; i < count; i++)
            {
                this.DeserializeData(input);
            }
        }
    }
}
