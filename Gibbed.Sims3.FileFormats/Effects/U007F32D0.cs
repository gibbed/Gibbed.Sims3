﻿using System;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class U007F32D0 : IFormat
    {
        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            input.ReadU32(true);
            input.ReadU32(true);
            input.ReadU32(true);
            input.ReadU32(true);
            input.ReadU32(true);
            input.ReadU32(true);
        }
    }
}