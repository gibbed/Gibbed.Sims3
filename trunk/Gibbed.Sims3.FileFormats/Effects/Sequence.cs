using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class Sequence : IEffectFormat
    {
        public short MinimumVersion { get { return 1; } }
        public short MaximumVersion { get { return 1; } }

        public void Serialize(Stream output, short version)
        {
            throw new NotImplementedException();
        }

        private void DeserializeArray(Stream input)
        {
            int count = input.ReadS32(false);
            for (int i = 0; i < count; i++)
            {
                input.ReadU32(true);
                input.ReadU32(true);
                input.ReadASCIIZ();
            }
        }

        public void Deserialize(Stream input, short version)
        {
            this.DeserializeArray(input);
            input.ReadU32(false);
        }
    }
}
