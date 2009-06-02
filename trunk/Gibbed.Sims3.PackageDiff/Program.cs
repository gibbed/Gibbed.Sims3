using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.Sims3.FileFormats;

namespace Gibbed.Sims3.PackageDiff
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                return;
            }

            string firstPath = args[0];
            Stream firstStream = File.OpenRead(firstPath);
            Database firstDb = new Database(firstStream);

            string secondPath = args[1];
            Stream secondStream = File.OpenRead(secondPath);
            Database secondDb = new Database(secondStream);

            foreach (KeyValuePair<ResourceKey, Database.Entry> entry in firstDb.Entries)
            {
                if (secondDb.Entries.ContainsKey(entry.Key) == false)
                {
                    Console.WriteLine("Only in {0}: {1}", firstPath, entry.Key);
                }
            }

            foreach (KeyValuePair<ResourceKey, Database.Entry> entry in secondDb.Entries)
            {
                if (firstDb.Entries.ContainsKey(entry.Key) == false)
                {
                    Console.WriteLine("Only in {0}: {1}", secondPath, entry.Key);
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
                        for (int i = 0; i < firstData.Length; i++)
                        {
                            if (firstData[i] != secondData[i])
                            {
                                different = true;
                                break;
                            }
                        }
                    }
                }

                if (different == true)
                {
                    Console.WriteLine("Resource {0} differs", key);
                }
            }

            secondStream.Close();
            firstStream.Close();
        }
    }
}
