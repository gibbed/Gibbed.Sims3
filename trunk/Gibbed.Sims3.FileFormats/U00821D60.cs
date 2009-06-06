﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats
{
    public class U00821D60 : IFormat
    {
        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            int count = input.ReadS32(false);
            for (int i = 0; i < count; i++)
            {
                input.ReadU8();
            }
        }
    }
}