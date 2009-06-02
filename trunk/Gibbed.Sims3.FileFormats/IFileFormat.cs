using System.IO;

namespace Gibbed.Sims3.FileFormats
{
    public interface IFileFormat
    {
        void Serialize(Stream output);
        void Deserialize(Stream input);
    }
}
