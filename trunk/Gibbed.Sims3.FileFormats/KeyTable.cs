using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats
{
    public class KeyTable : IFormat
    {
        public List<ResourceKey> Keys;

        public void Serialize(Stream output)
        {
            output.WriteS32(this.Keys.Count);
            foreach (ResourceKey key in this.Keys)
            {
                output.WriteU32(key.TypeId);
                output.WriteU32(key.GroupId);
                output.WriteU64(key.InstanceId);
            }
        }

        public void Deserialize(Stream input)
        {
            this.Keys = new List<ResourceKey>();
            int count = input.ReadS32();
            for (int i = 0; i < count; i++)
            {
                ResourceKey key;
                key.TypeId = input.ReadU32();
                key.GroupId = input.ReadU32();
                key.InstanceId = input.ReadU64();
                this.Keys.Add(key);
            }
        }
    }
}
