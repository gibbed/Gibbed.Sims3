using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class Model : IEffectFormat
    {
        public short MinimumVersion { get { return 1; } }
        public short MaximumVersion { get { return 1; } }

        public void Serialize(Stream output, short version)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input, short version)
        {
            input.ReadU32(false);
            input.ReadU64(false);
            input.ReadU32(false);

            input.ReadU32(true);
            input.ReadU32(true);
            input.ReadU32(true);

            input.ReadU32(false);

            new U00809120().Deserialize(input);

            input.ReadU64(false);
            input.ReadU8();
        }
    }
}
