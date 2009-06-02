using System;
using System.Collections.Generic;
using Gibbed.Sims3.FileFormats;

namespace Gibbed.Sims3.ExportableContent
{
    public class Household : IExportableContent
    {
        public string Name;
        public int FamilyFunds;
        public Int64 ExportTime;
        public string BioText;
        public bool LifetimeHappinessNotificationShown;
        public List<SimDescription> Members;
        public Dictionary<UInt64, UInt64> Relationships;

        public void ImportContent(PropertyStream stream)
        {
            this.Name = stream.GetString(Hashes.FamilyName);
            this.FamilyFunds = stream.GetS32(Hashes.FamilyFunds);
            this.ExportTime = stream.GetS64(Hashes.ExportTime);
            this.BioText = stream.GetString(Hashes.BioText);
            this.LifetimeHappinessNotificationShown = stream.GetBoolean(Hashes.LifetimeHappinessNotificationShown);

            this.Members = new List<SimDescription>();
            PropertyStream sims = stream.GetChild(Hashes.Members);
            int scount = sims.GetS32(Hashes.SimDescriptionCount);
            for (uint i = 0; i < scount; i++)
            {
                PropertyStream child = sims.GetChild(i);
                SimDescription sim = new SimDescription();
                sim.ImportContent(child);
                this.Members.Add(sim);
            }

            this.Relationships = new Dictionary<ulong, ulong>();
            PropertyStream relationships = stream.GetChild(Hashes.Relationships);
            uint ucount = relationships.GetU32(Hashes.NumRelationships);
            for (uint i = 0; i < ucount; i++)
            {
                PropertyStream child = relationships.GetChild(i);
                UInt64 a = child.GetU64(Hashes.CurrentSimDescId);
                UInt64 b = child.GetU64(Hashes.OtherSimDescId);
                this.Relationships.Add(a, b);
            }
        }

        public void ExportContent(PropertyStream stream)
        {
            throw new NotImplementedException();
        }
    }
}
