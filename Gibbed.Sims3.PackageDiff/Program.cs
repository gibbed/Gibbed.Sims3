using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.Sims3.FileFormats;
using Gibbed.Sims3.ResourceLookup;
using System.Windows.Forms;
using System.Linq;
using Gibbed.Helpers;

namespace Gibbed.Sims3.PackageDiff
{
    internal class Program
    {
        private static void LoadLookup()
        {
            Lookup.Reset();
            string basePath = Path.Combine(Application.StartupPath, "lists");

            string[] files;

            files = Directory.GetFiles(Path.Combine(basePath, "fnv32"), "*.txt", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                Lookup.LoadFiles32(file);
            }

            files = Directory.GetFiles(Path.Combine(basePath, "fnv64"), "*.txt", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                Lookup.LoadFiles64(file);
            }

            Lookup.LoadGroups(Path.Combine(basePath, "groups.xml"));
            Lookup.LoadTypes(Path.Combine(basePath, "types.xml"));
        }

        private static void AddMapNames(Database db)
        {
            foreach (ResourceKey key in db.Keys.Where(candidate => candidate.TypeId == 0x0166038C))
            {
                Stream stream = db.GetResourceStream(key);

                KeyNameMapFile keyNameMap = new KeyNameMapFile();
                keyNameMap.Read(stream);

                foreach (KeyValuePair<UInt64, string> value in keyNameMap.Map)
                {
                    UInt64 realHash = value.Value.HashFNV64();

                    if (realHash == value.Key)
                    {
                        Lookup.Files[value.Key] = value.Value;
                    }
                    else if ((realHash & 0x7FFFFFFFFFFFFFFF) == value.Key)
                    {
                        Lookup.Files[value.Key] = "*" + value.Value;
                    }
                    else if (value.Value.HashFNV32() == value.Key)
                    {
                        Lookup.Files[value.Key] = ":" + value.Value;
                    }
                }
            }
        }

        private static string LookupName(ResourceKey key)
        {
            string rez = "";
            rez += Lookup.Groups.ContainsKey(key.GroupId) ? Lookup.Groups[key.GroupId] : ("#" + key.GroupId.ToString("X8"));
            rez += "\\";
            rez += Lookup.Files.ContainsKey(key.InstanceId) ? Lookup.Files[key.InstanceId] : ("#" + key.InstanceId.ToString("X16"));
            rez += ".";
            rez += Lookup.Types.ContainsKey(key.TypeId) ? Lookup.Types[key.TypeId].Extension : "bnry";
            rez += " (" + (Lookup.Types.ContainsKey(key.TypeId) ? Lookup.Types[key.TypeId].FourCC : ("#" + key.TypeId.ToString("X8"))) + ")";
            return rez;
        }

        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                return;
            }

            LoadLookup();

            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            string firstPath = args[0];
            Stream firstStream = File.OpenRead(firstPath);
            Database firstDb = new Database(firstStream);

            string secondPath = args[1];
            Stream secondStream = File.OpenRead(secondPath);
            Database secondDb = new Database(secondStream);

            AddMapNames(firstDb);
            AddMapNames(secondDb);

            foreach (KeyValuePair<ResourceKey, Database.Entry> entry in firstDb.Entries)
            {
                if (secondDb.Entries.ContainsKey(entry.Key) == false)
                {
                    //Console.WriteLine("Only in {0}: {1}  {2}", firstPath, entry.Key, LookName(entry.Key));
                }
            }

            foreach (KeyValuePair<ResourceKey, Database.Entry> entry in secondDb.Entries)
            {
                if (firstDb.Entries.ContainsKey(entry.Key) == false)
                {
                    Console.WriteLine("Only in {0}: {1}  {2}", secondPath, entry.Key, LookupName(entry.Key));
                }
            }

            foreach (KeyValuePair<ResourceKey, Database.Entry> entry in firstDb.Entries)
            {
                if (secondDb.Entries.ContainsKey(entry.Key) == false)
                {
                    continue;
                }

                ResourceKey key = entry.Key;

                bool different = false;

                if (firstDb.Entries[key].DecompressedSize != secondDb.Entries[key].DecompressedSize)
                {
                    different = true;
                }
                else
                {
                    byte[] firstData = firstDb.GetRawResource(key);
                    byte[] secondData = secondDb.GetRawResource(key);

                    if (firstData.Length != secondData.Length)
                    {
                        different = true;
                    }
                    else
                    {
                        string firstHash = BitConverter.ToString(md5.ComputeHash(firstData));
                        string secondHash = BitConverter.ToString(md5.ComputeHash(secondData));

                        if (firstHash != secondHash)
                        {
                            different = true;
                        }
                    }
                }

                if (different == true)
                {
                    Console.WriteLine("Different: {0}  {1}", key, LookupName(key));
                }
            }

            secondStream.Close();
            firstStream.Close();
        }
    }
}
