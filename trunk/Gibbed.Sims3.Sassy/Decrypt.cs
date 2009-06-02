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
        [Action(Description = "Decrypt a signed assembly.")]
        public static void Decrypt(
            [Required(Description = "input signed assembly")]
            string inputPath,
            [Required(Description = "output decrypted assembly")]
            string outputPath)
        {
            Stream input = File.OpenRead(inputPath);

            bool isEncrypted = input.ReadBoolean();
            UInt32 typeId = input.ReadU32();

            if (isEncrypted == false || typeId != 0x2BC4F79F)
            {
                Console.WriteLine("Not an encrypted assembly that I know how to handle.");
                input.Close();
                return;
            }

            byte[] theirSum = new byte[64];
            input.Read(theirSum, 0, theirSum.Length);

            ushort blocks = input.ReadU16();

            byte[] table = new byte[blocks * 8];
            input.Read(table, 0, table.Length);

            // Calculate initial seed
            UInt32 seed = 0;
            for (int i = 0; i < blocks; i++)
            {
                seed += BitConverter.ToUInt32(table, i * 8);
            }
            seed = (UInt32)(table.Length - 1) & seed;

            Stream output = File.Open(outputPath, FileMode.Create, FileAccess.Write);

            // Decrypt data
            for (int i = 0; i < blocks; i++)
            {
                byte[] data = new byte[512];

                if ((table[(i * 8)] & 1) == 0) // non-empty block
                {
                    input.Read(data, 0, 512);
                    for (int j = 0; j < 512; j++)
                    {
                        byte value = data[j];
                        data[j] ^= table[seed];
                        seed = (UInt32)((seed + value) % table.Length);
                    }
                }

                output.Write(data, 0, data.Length);
            }

            output.Close();
            input.Close();

            Console.WriteLine("Done.");
        }
    }
}
