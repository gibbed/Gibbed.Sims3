using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats
{
    public struct KeyEntry
    {
        public UInt32 Unknown1;
        public UInt32 Unknown2;
        public UInt32 Unknown3;
        public UInt32 Unknown4;
    }

    public class KeyTableData : IFormat
    {
        public List<KeyEntry> Keys;

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            this.Keys = new List<KeyEntry>();

            int count = input.ReadS32();
            for (int i = 0; i < count; i++)
            {
                KeyEntry key;
                key.Unknown1 = input.ReadU32();
                key.Unknown2 = input.ReadU32();
                key.Unknown3 = input.ReadU32();
                key.Unknown4 = input.ReadU32();
                this.Keys.Add(key);
            }
        }
    }
}
