using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NConsoler;
using Gibbed.Helpers;

namespace Gibbed.Sims3.ListNatives
{
    internal partial class Program
    {
        [Action(Description = "List natives")]
        public static void Go()
        {
            string path = (string)Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Sims\\The Sims 3", "Install Dir", null);

            if (path == null)
            {
                Console.WriteLine("Unable to determin The Sims 3 directory.");
                return;
            }

            path = Path.Combine(Path.Combine(Path.Combine(path, "Game"), "Bin"), "TS3.exe");
            if (File.Exists(path) == false)
            {
                Console.WriteLine("TS3.exe not found.");
                return;
            }

            Stream input = File.OpenRead(path);
            Executable exe = new Executable();
            exe.Read(input);

            foreach (UInt32 virtualAddress in NativeTables127)
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
