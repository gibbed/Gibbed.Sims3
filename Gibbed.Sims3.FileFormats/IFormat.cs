using System.IO;

namespace Gibbed.Sims3.FileFormats
{
    public interface IFormat
    {
        void Serialize(Stream output);
        void Deserialize(Stream input);
    }
}
