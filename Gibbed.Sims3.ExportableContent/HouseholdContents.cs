using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gibbed.Helpers;
using Gibbed.Sims3.FileFormats;

namespace Gibbed.Sims3.ExportableContent
{
    public class HouseholdContents : IExportableContent
    {
        private const UInt32 HashHousehold = 0x846D3A8C; // "Household".FNV32()
        private const UInt32 HashInventoryCount = 0x68d30928; // "InventoryCount".FNV32()
        private const UInt32 HashInventoryKeyIndices = 0xA3B065D7; // "InventoryKeyIndices".FNV32()

        public Household Household;
        public int[] Inventories;

        public void ImportContent(PropertyStream stream)
        {
            PropertyStream child = stream.GetChild(HashHousehold);
            this.Household = new Household();
            this.Household.ImportContent(child);

            int count = stream.GetS32(HashInventoryCount);
            this.Inventories = stream.GetS32s(HashInventoryKeyIndices);

            if (this.Inventories.Length != count)
            {
                throw new Exception("count mismatch");
            }
        }

        public void ExportContent(PropertyStream stream)
        {
            throw new NotImplementedException();
        }
    }
}