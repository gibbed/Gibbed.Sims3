using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gibbed.Helpers;
using NConsoler;

namespace Gibbed.Sims3.Sassy
{
    internal partial class Program
    {
        [Action(Description = "Fake sign an assembly.")]
        public static void FakeSign(
            [Required(Description = "input assembly")]
            string inputPath,
            [Required(Description = "output signed assembly")]
            string outputPath)
        {
            Stream input = File.OpenRead(inputPath);
            ushort blocks = (ushort)((input.Length / 512) + ((input.Length % 512 > 0) ? 1 : 0));

            byte[] sum = new byte[64];
            byte[] sumMessage = Encoding.ASCII.GetBytes("This is a fake signed assembly.");
            Array.Copy(sumMessage, sum, sumMessage.Length);

            byte[] table = new byte[blocks * 8];

            // Calculate initial seed
            UInt32 seed = 0;
            for (int i = 0; i < blocks; i++)
            {
                //table[i * 8] = 1;
                seed += BitConverter.ToUInt32(table, i * 8);
            }
            seed = (UInt32)(table.Length - 1) & seed;

            Stream output = File.Open(outputPath, FileMode.Create, FileAccess.Write);
            output.WriteValueBoolean(true);
            output.WriteValueU32(0x2BC4F79F);
            output.Write(sum, 0, sum.Length);
            output.WriteValueU16(blocks);
            output.Write(table, 0, table.Length);

            // Encrypt data
            for (int i = 0; i < blocks; i++)
            {
                byte[] data = new byte[512];
                input.Read(data, 0, data.Length);

                for (int j = 0; j < 512; j++)
                {
                    data[j] ^= table[seed];
                    seed = (UInt32)((seed + data[j]) % table.Length);
                }

                output.Write(data, 0, data.Length);
            }

            output.Close();
            input.Close();

            Console.WriteLine("Done.");
        }
    }
}
