using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats
{
    public class KeyNameMapFile
    {
        public int Version;
        public Dictionary<UInt64, string> Map;

        public void Read(Stream input)
        {
            this.Version = input.ReadS32();
            if (this.Version != 1)
            {
                throw new InvalidOperationException();
            }

            int count = input.ReadS32();
            this.Map = new Dictionary<ulong, string>();
            for (int i = 0; i < count; i++)
            {
                UInt64 hash = input.ReadU64();
                uint length = input.ReadU32();
                string name = length == 0 ? "" : input.ReadASCII(length);
                this.Map[hash] = name;
            }
        }
    }
}
