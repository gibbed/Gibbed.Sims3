using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats
{
    public static partial class KeyTableHelper
    {
        public static KeyTable ReadKeyTable(this Stream stream, int headerSize)
        {
            long keyTableOffset = stream.ReadValueS32();
            int keyTableSize = stream.ReadValueS32();
            keyTableOffset += stream.Position;
            keyTableOffset -= headerSize;

            KeyTable keyTable = new KeyTable();

            long originalPosition = stream.Position;
            stream.Seek(keyTableOffset, SeekOrigin.Begin);
            keyTable.Deserialize(stream);
            stream.Seek(originalPosition, SeekOrigin.Begin);

            return keyTable;
        }
    }

    public class KeyTable : IFormat
    {
        public List<ResourceKey> Keys;

        public void Serialize(Stream output)
        {
            output.WriteValueS32(this.Keys.Count);
            foreach (ResourceKey key in this.Keys)
            {
                output.WriteValueU32(key.TypeId);
                output.WriteValueU32(key.GroupId);
                output.WriteValueU64(key.InstanceId);
            }
        }

        public void Deserialize(Stream input)
        {
            this.Keys = new List<ResourceKey>();
            int count = input.ReadValueS32();
            for (int i = 0; i < count; i++)
            {
                ResourceKey key;
                key.TypeId = input.ReadValueU32();
                key.GroupId = input.ReadValueU32();
                key.InstanceId = input.ReadValueU64();
                this.Keys.Add(key);
            }
        }
    }
}
