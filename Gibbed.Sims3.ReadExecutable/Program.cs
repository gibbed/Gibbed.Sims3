using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml.Serialization;
using NConsoler;

namespace Gibbed.Sims3.ReadExecutable
{
    internal partial class Program
    {
        private static string FindExecutablePath()
        {
            string path = (string)Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Sims\\The Sims 3", "Install Dir", null);

            if (path == null)
            {
                return null;
            }

            path = Path.Combine(Path.Combine(Path.Combine(path, "Game"), "Bin"), "TS3.exe");
            if (File.Exists(path) == false)
            {
                Console.WriteLine("TS3.exe not found.");
                return null;
            }

            return path;
        }

        private static string GetMd5Hash(Stream input)
        {
            long position = input.Position;
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(input);
            input.Seek(position, SeekOrigin.Begin);
            return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
        }

        private static ExecutableVersion GetExecutableVersion(string hash)
        {
            string versionPath = Path.Combine(Path.Combine(Application.StartupPath, "lists"), "executable_versions");
            versionPath = Path.Combine(versionPath, Path.ChangeExtension(hash, ".xml"));

            if (File.Exists(versionPath) == false)
            {
                return null;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(ExecutableVersion));
            return (ExecutableVersion)serializer.Deserialize(File.OpenRead(versionPath));
        }

        public static void Main(string[] args)
        {
            Consolery.Run(typeof(Program), args);
            //ParseScriptFunctionTables(null);
        }
    }
}
