using Gibbed.Sims3.FileFormats;

namespace Gibbed.Sims3.ExportableContent
{
    public interface IExportableContent
    {
        void ImportContent(PropertyStream stream);
        void ExportContent(PropertyStream stream);
    }
}
