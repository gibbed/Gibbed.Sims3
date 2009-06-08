using System;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class U008411D0 : IFormat
    {
        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            input.ReadU32(false);
            input.ReadU64(false);
            input.ReadU32(false);
            input.ReadU32(false);
            input.ReadU32(false);
            input.ReadU32(false);
            input.ReadU32(false);

            input.ReadASCIIZ();
            input.ReadASCIIZ();

            new U007E4350().Deserialize(input);
        }
    }
}
