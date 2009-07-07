using System;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class MetaParticles : IEffectFormat
    {
        public short MinimumVersion { get { return 1; } }
        public short MaximumVersion { get { return 1; } }

        public void Serialize(Stream output, short version)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input, short version)
        {
            // 00800610

            input.ReadValueU32(false);
            input.ReadValueU32(false);

            input.ReadValueU32(true);
            input.ReadValueU32(true);

            input.ReadValueU32(false);

            input.ReadValueU32(true);
            input.ReadValueU32(true);
            input.ReadValueU32(true);
            input.ReadValueU32(true);

            new U007F32D0().Deserialize(input);

            input.ReadValueU32(true);
            input.ReadValueU32(true);

            new U007F32D0().Deserialize(input);

            input.ReadValueU32(false);

            new U007E2000().Deserialize(input);

            input.ReadValueU32(false);
            input.ReadValueU32(false);

            new U007E2000().Deserialize(input);

            input.ReadValueU32(false);

            new U007E2000().Deserialize(input);
            new U007E2000().Deserialize(input);
            new U007E2000().Deserialize(input);

            input.ReadValueU32(false);
            input.ReadValueU32(false);
            input.ReadValueU32(false);
            input.ReadValueU32(false);
            input.ReadValueU32(false);
            input.ReadValueU32(false);

            new U007E4350().Deserialize(input);

            input.ReadValueU32(true);
            input.ReadValueU32(true);
            input.ReadValueU32(true);

            new U007E2000().Deserialize(input);

            input.ReadValueU32(false);

            input.ReadStringASCIIZ();
            input.ReadStringASCIIZ();

            input.ReadValueU8();

            input.ReadValueU32(true);
            input.ReadValueU32(true);
            input.ReadValueU32(true);
            input.ReadValueU32(true);
            input.ReadValueU32(true);
            input.ReadValueU32(true);

            input.ReadValueU32(false);
            input.ReadValueU32(false);
            input.ReadValueU32(false);

            input.ReadValueU32(true);
            input.ReadValueU32(true);
            input.ReadValueU32(true);

            input.ReadValueU32(false);
            input.ReadValueU32(false);

            new U007FB250().Deserialize(input);

            input.ReadValueU8();
            input.ReadValueU8();
            input.ReadValueU8();
            input.ReadValueU8();

            new U007E4350().Deserialize(input);

            new U007E2000().Deserialize(input);

            new U007E9D10().Deserialize(input);

            input.ReadValueU32(false);
            input.ReadValueU32(false);
            input.ReadValueU32(false);
            input.ReadValueU32(false);
            input.ReadValueU32(false);
            input.ReadValueU32(false);
            input.ReadValueU32(false);

            input.ReadValueU32(true);
            input.ReadValueU32(true);

            input.ReadValueU64(false);
            input.ReadValueU64(false);
            input.ReadValueU64(false);

            // array of 2
            new U00839AF0().Deserialize(input);
            new U00839AF0().Deserialize(input);

            input.ReadValueU32(true);
            input.ReadValueU32(true);
            input.ReadValueU32(true);

            input.ReadValueU32(false);
            input.ReadValueU32(false);
            input.ReadValueU32(false);

            input.ReadValueU32(true);
            input.ReadValueU32(true);
            input.ReadValueU32(true);

            new U00839B70().Deserialize(input);

            new U007FB2B0().Deserialize(input);

            input.ReadValueU32(false);
        }
    }
}
