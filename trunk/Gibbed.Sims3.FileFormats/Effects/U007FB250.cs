using System;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class U007FB250 : IFormat
    {
        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            int count = input.ReadValueS32(false);
            for (int i = 0; i < count; i++)
            {
                new U008386C0().Deserialize(input);
            }
        }
    }
}
