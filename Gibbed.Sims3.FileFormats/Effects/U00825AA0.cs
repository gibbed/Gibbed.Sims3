using System;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class U00825AA0 : IFormat
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
                input.ReadU8();
                input.ReadU8();
                input.ReadU64(false);
                new U00821D60().Deserialize(input);
            }
        }
    }
}
