using System;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class U008386C0 : IFormat
    {
        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            input.ReadU32(false);
            input.ReadU32(true);
            input.ReadU32(true);
            input.ReadU32(true);
            input.ReadU32(true);
            input.ReadU32(true);
            input.ReadU32(true);
        }
    }
}
