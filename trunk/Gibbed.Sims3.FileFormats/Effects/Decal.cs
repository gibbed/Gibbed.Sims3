using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class Decal : IEffectFormat
    {
        public short MinimumVersion { get { return 1; } }
        public short MaximumVersion { get { return 2; } }

        public void Serialize(Stream output, short version)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input, short version)
        {
            input.ReadU32(false);
            input.ReadU64(false);
            input.ReadU8();
            input.ReadU32(false);
            input.ReadU8();
            input.ReadU32(false);
            
            new U007E2000().Deserialize(input);
            new U007E2000().Deserialize(input);
            new U007E2000().Deserialize(input);

            new U007E4350().Deserialize(input);

            new U007E2000().Deserialize(input);

            input.ReadU32(false);
            input.ReadU32(false);
            input.ReadU32(false);
            input.ReadU32(false);

            input.ReadU32(true);
            input.ReadU32(true);

            input.ReadU64(false);

            if (version >= 2)
            {
                input.ReadU8();
            }
        }
    }
}
