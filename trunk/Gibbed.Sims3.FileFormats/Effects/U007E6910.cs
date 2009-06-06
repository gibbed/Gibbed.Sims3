using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            input.ReadU64(false);
            input.ReadU8();
            byte unk = input.ReadU8();
            if ((unk & 0x80) == 0x80)
            {
                unk &= 0x7F;
                input.ReadU32(false);
            }
            input.ReadU8();
            input.ReadU8();
            input.ReadU16(false);
            input.ReadU32(false);
            input.ReadU64(false);
        }
    }
}
