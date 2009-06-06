using System.IO;

namespace Gibbed.Sims3.FileFormats
{
    public interface IEffectFormat
    {
        short MinimumVersion { get; }
        short MaximumVersion { get; }

        void Serialize(Stream output, short version);
        void Deserialize(Stream input, short version);
    }
}
