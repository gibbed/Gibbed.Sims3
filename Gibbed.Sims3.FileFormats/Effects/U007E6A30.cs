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
            input.ReadValueU16(false);
            input.ReadValueU32(false);
            new U007E69B0().Deserialize(input);
            input.ReadValueU32(true);
            input.ReadValueU32(true);
            input.ReadValueU32(true);
        }
    }
}
