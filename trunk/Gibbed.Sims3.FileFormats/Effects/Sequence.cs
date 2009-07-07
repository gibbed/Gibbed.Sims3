using System;
using System.IO;
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
            int count = input.ReadValueS32(false);
            for (int i = 0; i < count; i++)
            {
                input.ReadValueU32(true);
                input.ReadValueU32(true);
                input.ReadStringASCIIZ();
            }
        }

        public void Deserialize(Stream input, short version)
        {
            this.DeserializeArray(input);
            input.ReadValueU32(false);
        }
    }
}
