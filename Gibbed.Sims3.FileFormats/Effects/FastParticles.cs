﻿using System;
using System.IO;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class FastParticles : IEffectFormat
    {
        public short MinimumVersion { get { return 1; } }
        public short MaximumVersion { get { return 1; } }

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
