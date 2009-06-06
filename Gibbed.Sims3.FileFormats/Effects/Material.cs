using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class Material : IEffectFormat
    {
        public short MinimumVersion { get { return 0; } }
        public short MaximumVersion { get { return 0; } }

        public void Serialize(Stream output, short version)
        {
            throw new NotImplementedException();
        }

        private void DeserializeData(Stream input)
        {
            input.ReadU64(false);
            byte type = input.ReadU8();

            switch (type)
            {
                case 0: input.ReadU32(false); break;
                case 1: input.ReadU32(false); break;
                case 2: input.ReadU8(); break;
                case 3:
                {
                    int count = input.ReadU16(false);
                    for (int i = 0; i < count; i++)
                    {
                        input.ReadU32(false);
                    }
                    break;
                }
                case 4:
                {
                    int count = input.ReadU16(false);
                    for (int i = 0; i < count; i++)
                    {
                        input.ReadU32(false);
                    }
                    break;
                }
                case 5:
                {
                    int count = input.ReadU16(false);
                    for (int i = 0; i < count; i++)
                    {
                        input.ReadU8();
                    }
                    break;
                }
                case 6: input.ReadU64(false); break;
            }
        }

        public void Deserialize(Stream input, short version)
        {
            input.ReadU64(false);
            input.ReadU64(false);
            
            int count = input.ReadS32(false);
            for (int i = 0; i < count; i++)
            {
                this.DeserializeData(input);
            }
        }
    }
}
