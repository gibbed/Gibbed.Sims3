using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.Sims3.FileFormats;

namespace Gibbed.Sims3.PackageInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] paths = Directory.GetFiles(".", "*.package", SearchOption.AllDirectories);
            Dictionary<uint, int> typeCounts = new Dictionary<uint, int>();
            Dictionary<uint, List<string>> typePaths = new Dictionary<uint, List<string>>();

            foreach (string path in paths)
            {
                Stream input = File.OpenRead(path);

                Database db;

                try
                {
                    db = new Database(input, true);
                }
                catch (NotAPackageException)
                {
                    Console.WriteLine("bad file: {0}", path);
                    input.Close();
                    continue;
                }

                foreach (ResourceKey key in db.Entries.Keys)
                {
                    if (typeCounts.ContainsKey(key.TypeId) == false)
                    {
                        typeCounts[key.TypeId] = 0;
                    }

                    if (typePaths.ContainsKey(key.TypeId) == false)
                    {
                        typePaths[key.TypeId] = new List<string>();
                    }

                    typeCounts[key.TypeId]++;

                    if (typePaths[key.TypeId].Contains(path) == false)
                    {
                        typePaths[key.TypeId].Add(path);
                    }

                    if (key.InstanceId == 0xebb33014677ef1f3 || key.InstanceId == 0xf3f17e671430b3eb)
                    {
                        Console.WriteLine(key.ToString());
                    }
                }

                input.Close();
            }

            foreach (KeyValuePair<uint, int> typeCount in typeCounts)
            {
                //Console.WriteLine("{0:X8} : {1}", typeCount.Key, typeCount.Value);

                if (typePaths.ContainsKey(typeCount.Key) == true && typeCount.Value < 20)
                {
                    foreach (string path in typePaths[typeCount.Key])
                    {
                        //Console.WriteLine(path);
                    }
                }
            }
        }
    }
}
