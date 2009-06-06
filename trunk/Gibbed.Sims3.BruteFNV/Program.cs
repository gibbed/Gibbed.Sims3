﻿using System;
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
            string letters = "abcdefghijklmnopqrstuvwxyz0123456789.";
            List<uint> hash32 = new List<uint>(new uint[] { 0xF0FF5598, 0x025C90A6 });
            List<ulong> hash64 = new List<ulong>(new ulong[] { 0xAD46C72AB29ACF48 });
            
            string prefix = "";

            for (int length = 1; length <= 6; length++)
            {
                Console.WriteLine("Welcome to round " + length.ToString() + "!");

                int[] state = new int[length];

                bool go = true;
                if (length >= printStatus)
                {
                    Console.Write(letters[0]);
                }
                while (go)
                {
                    string text = prefix;
                    for (int i = 0; i < length; i++)
                    {
                        text += letters[state[i]];
                    }

                    //Console.WriteLine(text);
                    if (hash32.Contains(text.FNV32()))
                    {
                        Console.WriteLine();
                        Console.WriteLine("match: " + text + " => " + text.FNV32().ToString("X8"));
                    }

                    if (hash64.Contains(text.FNV64()))
                    {
                        Console.WriteLine();
                        Console.WriteLine("match: " + text + " => " + text.FNV64().ToString("X16"));
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