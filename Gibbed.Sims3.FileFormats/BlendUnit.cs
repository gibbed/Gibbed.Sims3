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
            this.Version = input.ReadValueU32();

            long keyTableOffset = input.ReadValueS32();
            int keyTableSize = input.ReadValueS32();
            keyTableOffset += input.Position;
            keyTableOffset -= 4;

            if (this.Version >= 3)
            {
                this.LocalizationKey = input.ReadValueU64();
            }

            int count = input.ReadValueS32();
            if (count < 1 || count > 2)
            {
                throw new InvalidOperationException("count cannot be < 1 or > 2");
            }

            this.FacialBlendResourceKeyIndices = new int[count];
            for (int i = 0; i < count; i++)
            {
                this.FacialBlendResourceKeyIndices[i] = input.ReadValueS32();
            }

            if (this.Version < 3)
            {
                this.LocalizationKey = input.ReadValueU64();
            }

            this.Bidirectional = input.ReadValueBoolean();
            this.Category = input.ReadValueU32();
            this.Region = input.ReadValueU32();
            this.SortIndex = this.Version < 3 ? 0 : input.ReadValueU32();

            input.Seek(keyTableOffset, SeekOrigin.Begin);
            this.KeyTable = new KeyTable();
            this.KeyTable.Deserialize(input);
        }
    }
}
