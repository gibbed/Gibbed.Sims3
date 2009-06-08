using System;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class Screen : IEffectFormat
    {
        public short MinimumVersion { get { return 1; } }
        public short MaximumVersion { get { return 1; } }

        public void Serialize(Stream output, short version)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input, short version)
        {
            input.ReadU8();
            input.ReadU32(false);

            new U007E4350().Deserialize(input);

            new U007E2000().Deserialize(input);
            new U007E2000().Deserialize(input);

            input.ReadU32(false);
            input.ReadU32(false);
            input.ReadU32(false);
            input.ReadU32(false);
            
            input.ReadU64(false);

            new U00825AA0().Deserialize(input);

            new U00821780().Deserialize(input);

            new U00821DB0().Deserialize(input);

            new U00821C60().Deserialize(input);

            new U00821CD0().Deserialize(input);

            new U00821E00().Deserialize(input);
        }
    }
}
