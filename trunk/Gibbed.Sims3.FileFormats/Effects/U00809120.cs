using System;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class U00809120 : IFormat
    {
        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            int count = input.ReadS32(false);
            for (int i = 0; i < count; i++)
            {
                input.ReadU32(true);
                input.ReadU32(true);
                new U007E2000().Deserialize(input);
                input.ReadU32(false);
                input.ReadU32(false);
                input.ReadU8();
                input.ReadU8();
            }
        }
    }
}
