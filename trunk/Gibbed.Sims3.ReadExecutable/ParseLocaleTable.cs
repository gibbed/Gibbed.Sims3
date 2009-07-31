using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NConsoler;
using Gibbed.Helpers;

namespace Gibbed.Sims3.ReadExecutable
{
    struct LocaleOffset
    {
        public int Id;
        public UInt32 Name;
        public UInt32 Language;
        public UInt32 Region;
    }

    struct LocaleData
    {
        public int Id;
        public string Name;
        public string Language;
        public string Region;
    }

    internal partial class Program
    {
        [Action(Description = "Parse locale tables")]
        public static void ParseLocaleTable
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

            if (version.LocaleTable == 0)
            {
                Console.WriteLine("No locale table to parse.");
                input.Close();
                return;
            }

            Executable exe = new Executable();
            exe.Read(input);

            input.Seek(exe.GetFileOffset(version.LocaleTable), SeekOrigin.Begin);

            int count = 0;
            List<LocaleOffset> offsets = new List<LocaleOffset>();
            while (true)
            {
                UInt32 nameOffset = input.ReadValueU32();
                UInt32 bOffset = input.ReadValueU32();
                UInt32 cOffset = input.ReadValueU32();

                if (nameOffset == 0 || bOffset == 0 || cOffset == 0)
                {
                    break;
                }

                LocaleOffset offset = new LocaleOffset();
                offset.Id = count;
                offset.Name = nameOffset;
                offset.Language = bOffset;
                offset.Region = cOffset;
                offsets.Add(offset);

                count++;
            }

            List<LocaleData> locales = new List<LocaleData>();
            foreach (LocaleOffset offset in offsets)
            {
                LocaleData locale = new LocaleData();
                locale.Id = offset.Id;
                input.Seek(exe.GetFileOffset(offset.Name), SeekOrigin.Begin);
                locale.Name = input.ReadStringUTF16Z();
                input.Seek(exe.GetFileOffset(offset.Language), SeekOrigin.Begin);
                locale.Language = input.ReadStringUTF16Z();
                input.Seek(exe.GetFileOffset(offset.Region), SeekOrigin.Begin);
                locale.Region = input.ReadStringUTF16Z();
                locales.Add(locale);
            }

            StreamWriter writer = new StreamWriter("locales.txt",  false, Encoding.Unicode);

            foreach (LocaleData locale in locales)
            {
                string[] data;

                writer.WriteLine("|-");
                writer.WriteLine("| <tt>{0:X2}</tt>", locale.Id);
                writer.WriteLine("| {0}", locale.Id);
                writer.WriteLine("| {0}", locale.Name);
                
                data = locale.Language.Split('^');
                writer.WriteLine("| {0}", data[1]);

                data = locale.Region.Split('^');
                writer.WriteLine("| {0}", data[1]);

                writer.WriteLine();
            }

            writer.Flush();
            writer.Close();

            input.Close();
        }
    }
}
