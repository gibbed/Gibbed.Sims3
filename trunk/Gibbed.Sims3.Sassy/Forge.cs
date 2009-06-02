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
        [Action(Description = "*ADVANCED* Forge a signed assembly.")]
        public static void Forge(
            [Required(Description = "original signed assembly")]
            string originalPath,
            [Required(Description = "input modified assembly")]
            string inputPath,
            [Required(Description = "output forged assembly")]
            string outputPath)
        {
            Stream original = File.OpenRead(originalPath);

            bool isEncrypted = original.ReadBoolean();
            UInt32 typeId = original.ReadU32();

            if (isEncrypted == false || typeId != 0x2BC4F79F)
            {
                Console.WriteLine("Not an encrypted assembly that I know how to handle.");
                original.Close();
                return;
            }

            byte[] theirSum = new byte[64];
            original.Read(theirSum, 0, theirSum.Length);

            ushort blocks = original.ReadU16();

            byte[] table = new byte[blocks * 8];
            original.Read(table, 0, table.Length);
            original.Close();

            // Calculate initial seed
            UInt32 seed = 0;
            for (int i = 0; i < blocks; i++)
            {
                seed += BitConverter.ToUInt32(table, i * 8);
            }
            seed = (UInt32)(table.Length - 1) & seed;

            Stream input = File.OpenRead(inputPath);

            if (input.Length != blocks * 512)
            {
                Console.WriteLine("Size mismatch, original assembly and modified assembly must have the same size! ({0} vs {1})", input.Length, blocks * 512);
                input.Close();
                original.Close();
                return;
            }

            Stream output = File.Open(outputPath, FileMode.Create, FileAccess.Write);
            output.WriteBoolean(true);
            output.WriteU32(0x2BC4F79F);
            output.Write(theirSum, 0, theirSum.Length);
            output.WriteU16(blocks);
            output.Write(table, 0, table.Length);

            // Encrypt data
            for (int i = 0; i < blocks; i++)
            {
                byte[] data = new byte[512];
                input.Read(data, 0, data.Length);

                if ((table[(i * 8)] & 1) == 0) // non-empty block
                {
                    for (int j = 0; j < 512; j++)
                    {
                        data[j] ^= table[seed];
                        seed = (UInt32)((seed + data[j]) % table.Length);
                    }

                    output.Write(data, 0, data.Length);
                }
                else // validate this is an empty block
                {
                    for (int j = 0; j < data.Length; j++)
                    {
                        if (data[j] != 0)
                        {
                            Console.WriteLine("Block {0} should have been empty, but wasn't!", i);
                            input.Close();
                            output.Close();
                            return;
                        }
                    }
                }
            }

            output.Close();
            original.Close();

            Console.WriteLine("Done.");
        }
    }
}
