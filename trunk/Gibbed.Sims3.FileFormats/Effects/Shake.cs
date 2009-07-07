﻿using System;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class Shake : IEffectFormat
    {
        public short MinimumVersion { get { return 1; } }
        public short MaximumVersion { get { return 1; } }

        public void Serialize(Stream output, short version)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input, short version)
        {
            input.ReadValueU32(false);
            input.ReadValueU32(false);
            new U007E2000().Deserialize(input);
            new U007E2000().Deserialize(input);
            input.ReadValueU32(false);
            input.ReadValueU8();
            input.ReadValueU32(false);
        }
    }
}
