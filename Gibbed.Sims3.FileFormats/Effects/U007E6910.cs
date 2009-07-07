using System;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class U007E6910 : IFormat
    {
        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            input.ReadValueU64(false);
            input.ReadValueU8();
            byte unk = input.ReadValueU8();
            if ((unk & 0x80) == 0x80)
            {
                unk &= 0x7F;
                input.ReadValueU32(false);
            }
            input.ReadValueU8();
            input.ReadValueU8();
            input.ReadValueU16(false);
            input.ReadValueU32(false);
            input.ReadValueU64(false);
        }
    }
}
