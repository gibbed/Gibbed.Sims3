using System;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class U007E6A30 : IFormat
    {
        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            input.ReadU16(false);
            input.ReadU32(false);
            new U007E69B0().Deserialize(input);
            input.ReadU32(true);
            input.ReadU32(true);
            input.ReadU32(true);
        }
    }
}
