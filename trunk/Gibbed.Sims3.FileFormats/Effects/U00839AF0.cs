﻿using System;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class U00839AF0 : IFormat
    {
        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            input.ReadValueU32(true);
            input.ReadValueU32(true);
            input.ReadValueU32(true);
            input.ReadValueU32(true);

            input.ReadValueU32(false);
            input.ReadValueU32(false);
            input.ReadValueU32(false);

            new U007E2000().Deserialize(input);

            input.ReadValueU8();
        }
    }
}
