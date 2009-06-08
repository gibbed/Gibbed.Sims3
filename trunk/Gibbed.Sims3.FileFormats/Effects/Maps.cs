using System;
using System.IO;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class Maps : IEffectFormat
    {
        public short MinimumVersion { get { return 0; } }
        public short MaximumVersion { get { return 0; } }

        public void Serialize(Stream output, short version)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input, short version)
        {
            throw new NotImplementedException();
        }
    }
}
