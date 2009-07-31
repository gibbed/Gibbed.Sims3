using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Security.Cryptography;
using NConsoler;
using Gibbed.Helpers;

namespace Gibbed.Sims3.ReadExecutable
{
    internal partial class Program
    {
        [Action(Description = "Parse native script function tables")]
        public static void ParseScriptFunctionTables
            (
                [Optional(null, "exe")]
                string path
            )
        {
            if (path == null)
            {
                path = FindExecutablePath();
            }

            if (path == null)
            {
                Console.WriteLine("Unable to local TS3.exe. Please specify a path to it.");
                return;
            }

            Stream input = File.OpenRead(path);
            string hash = GetMd5Hash(input);
            ExecutableVersion version = GetExecutableVersion(hash);

            if (version == null)
            {
                Console.WriteLine("Executable information for this version (md5 hash of {0} is unavailable.", hash);
                input.Close();
                return;
            }

            if (version.ScriptTables == null || version.ScriptTables.Count == 0)
            {
                Console.WriteLine("No native script tables to parse.");
                input.Close();
                return;
            }

            Executable exe = new Executable();
            exe.Read(input);

            foreach (UInt32 virtualAddress in version.ScriptTables)
            {
                UInt32 fileOffset = exe.GetFileOffset(virtualAddress);
                if (fileOffset == 0)
                {
                    Console.WriteLine("Failed to get file offset for virtual address {0:X8}", virtualAddress);
                    return;
                }

                input.Seek(fileOffset, SeekOrigin.Begin);

                Dictionary<UInt32, UInt32> natives = new Dictionary<UInt32, UInt32>();
                while (true)
                {
                    UInt32 nativeAddress = input.ReadValueU32();
                    if (nativeAddress == 0)
                    {
                        break;
                    }

                    UInt32 nameAddress = input.ReadValueU32();
                    natives.Add(nameAddress, nativeAddress);
                }

                Console.WriteLine("Table {0:X8}", virtualAddress);
                foreach (KeyValuePair<UInt32, UInt32> native in natives)
                {
                    UInt32 nameOffset = exe.GetFileOffset(native.Key);
                    if (nameOffset == 0)
                    {
                        Console.WriteLine("  {0:X8} => {1:X8}", native.Key, native.Value);
                    }
                    else
                    {
                        input.Seek(nameOffset, SeekOrigin.Begin);
                        Console.WriteLine("  '{0}' => {1:X8}", input.ReadStringASCIIZ(), native.Value);
                    }
                }
            }

            input.Close();
        }
    }
}
