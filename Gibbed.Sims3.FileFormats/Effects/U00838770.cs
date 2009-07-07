using System;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class U00838770 : IFormat
    {
        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            input.ReadValueU32(true);
            input.ReadValueU32(true);
            input.ReadValueU32(true);
            input.ReadValueU32(true);
            input.ReadValueU32(true);
            input.ReadValueU32(true);
            input.ReadValueU32(false);
        }
    }
}
