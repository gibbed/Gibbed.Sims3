using System;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats
{
    public class BlendUnit : IFormat
    {
        public UInt32 Version;
        public KeyTable KeyTable;
        public UInt64 LocalizationKey;
        public int[] FacialBlendResourceKeyIndices;
        public bool Bidirectional;
        public UInt32 Category;
        public UInt32 Region;
        public UInt32 SortIndex;

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            this.Version = input.ReadU32();

            long keyTableOffset = input.ReadS32();
            int keyTableSize = input.ReadS32();
            keyTableOffset += input.Position;
            keyTableOffset -= 4;

            if (this.Version >= 3)
            {
                this.LocalizationKey = input.ReadU64();
            }

            int count = input.ReadS32();
            if (count < 1 || count > 2)
            {
                throw new InvalidOperationException("count cannot be < 1 or > 2");
            }

            this.FacialBlendResourceKeyIndices = new int[count];
            for (int i = 0; i < count; i++)
            {
                this.FacialBlendResourceKeyIndices[i] = input.ReadS32();
            }

            if (this.Version < 3)
            {
                this.LocalizationKey = input.ReadU64();
            }

            this.Bidirectional = input.ReadBoolean();
            this.Category = input.ReadU32();
            this.Region = input.ReadU32();
            this.SortIndex = this.Version < 3 ? 0 : input.ReadU32();

            input.Seek(keyTableOffset, SeekOrigin.Begin);
            this.KeyTable = new KeyTable();
            this.KeyTable.Deserialize(input);
        }
    }
}
