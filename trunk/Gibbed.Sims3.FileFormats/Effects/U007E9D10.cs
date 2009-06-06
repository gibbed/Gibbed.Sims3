using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class U007E9D10 : IFormat
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
                new U008411D0().Deserialize(input);
            }
        }
    }
}
