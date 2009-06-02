using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gibbed.Helpers;
using Gibbed.Sims3.FileFormats;

namespace Gibbed.Sims3.ExportableContent
{
    public class SimDescription : IExportableContent
    {
        public string FirstName;
        public string LastName;
        public UInt32 Flags;

        public void ImportContent(PropertyStream stream)
        {
            this.FirstName = stream.GetString(Hashes.FirstName);
            this.LastName = stream.GetString(Hashes.LastName);
            this.Flags = stream.GetU32(Hashes.Flags);
        }

        public void ExportContent(PropertyStream stream)
        {
            throw new NotImplementedException();
        }
    }
}
