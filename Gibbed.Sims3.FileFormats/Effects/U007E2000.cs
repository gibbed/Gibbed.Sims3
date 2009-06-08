using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class U007E2000 : IFormat
    {
        public List<uint> Unknown;

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            this.Unknown = new List<uint>();
            int count = input.ReadS32(false);
            for (int i = 0; i < count; i++)
            {
                this.Unknown.Add(input.ReadU32(false));
            }
        }
    }
}
