﻿using System;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Effects
{
    public class U008456A0 : IFormat
    {
        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            int count = input.ReadValueS32(false);
            for (int i = 0; i < count; i++)
            {
                input.ReadValueU32(false);
                input.ReadValueU32(false);
                input.ReadValueU32(false);
            }
        }
    }
}
