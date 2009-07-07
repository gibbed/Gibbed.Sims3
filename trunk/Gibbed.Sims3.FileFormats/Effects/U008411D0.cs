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
            input.ReadValueU32(false);
            input.ReadValueU64(false);
            input.ReadValueU32(false);
            input.ReadValueU32(false);
            input.ReadValueU32(false);
            input.ReadValueU32(false);
            input.ReadValueU32(false);

            input.ReadStringASCIIZ();
            input.ReadStringASCIIZ();

            new U007E4350().Deserialize(input);
        }
    }
}
