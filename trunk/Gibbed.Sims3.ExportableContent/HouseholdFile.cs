using System.IO;
using Gibbed.Helpers;
using Gibbed.Sims3.FileFormats;

namespace Gibbed.Sims3.ExportableContent
{
    public class HouseholdFile
    {
        public HouseholdContents HouseholdContents;

        public void Read(Stream input)
        {
            uint version = input.ReadValueU32();
            PropertyStream root = new PropertyStream();
            root.Read(input);

            this.HouseholdContents = new HouseholdContents();
            this.HouseholdContents.ImportContent(root);
        }

        public void Write(Stream output)
        {
            output.WriteValueU32(4);

            PropertyStream root = new PropertyStream();
            this.HouseholdContents.ExportContent(root);
            root.Write(output);
        }
    }
}
