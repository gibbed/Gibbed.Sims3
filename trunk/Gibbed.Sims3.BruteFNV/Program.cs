using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gibbed.Helpers;
using System.Runtime.InteropServices;

namespace Gibbed.Sims3.BruteFNV
{
    class Program
    {
        static void Main(string[] args)
        {
            const int printStatus = 4;
            //string letters = "abcdefghijklmnopqrstuvwxyz0123456789.";
            string letters = " !\"#$%&'()*+,-./0123456789:;<=>?@[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";

            List<uint> hash24 = new List<uint>(new uint[] { });
            List<uint> hash32 = new List<uint>(new uint[] { 0x8DDDDDDD, 0xFFFFFFFF, 0xBAADC0DE });
            List<ulong> hash64 = new List<ulong>(new ulong[] { });
            
            string prefix = "";
            string suffix = "";

            for (int length = 1; length <= 6; length++)
            {
                Console.WriteLine("Welcome to round " + length.ToString() + "!");

                int[] state = new int[length];

                if (length >= printStatus)
                {
                    Console.Write(letters[0]);
                }

                bool go = true;
                while (go)
                {
                    string text = prefix;
                    for (int i = 0; i < length; i++)
                    {
                        text += letters[state[i]];
                    }
                    text += suffix;

                    if (hash24.Count > 0)
                    {
                        if (hash24.Contains(text.HashFNV24()))
                        {
                            Console.WriteLine();
                            Console.WriteLine("match24: " + text + " => " + text.HashFNV24().ToString("X8"));
                        }
                    }

                    if (hash32.Count > 0)
                    {
                        if (hash32.Contains(text.HashFNV32()))
                        {
                            Console.WriteLine();
                            Console.WriteLine("match32: " + text + " => " + text.HashFNV32().ToString("X8"));
                        }
                    }

                    if (hash64.Count > 0)
                    {
                        if (hash64.Contains(text.HashFNV64()))
                        {
                            Console.WriteLine();
                            Console.WriteLine("match64: " + text + " => " + text.HashFNV64().ToString("X16"));
                        }
                    }

                    state[0]++;
                    for (int i = 0; i < length; i++)
                    {
                        if (state[i] >= letters.Length)
                        {
                            if (i + 1 >= length)
                            {
                                go = false;
                                break;
                            }

                            state[i] = 0;
                            state[i + 1]++;

                            if (length >= printStatus && i + 2 >= length && state[i + 1] < letters.Length)
                            {
                                Console.Write(letters[state[i + 1]]);
                            }
                        }
                    }
                }

                if (length >= printStatus)
                {
                    Console.WriteLine();
                }
            }
        }
    }
}
