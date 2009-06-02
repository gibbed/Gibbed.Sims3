using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using NConsoler;

namespace Gibbed.Sims3.Sassy
{
    internal partial class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Gibbed's Sassy Tool");
            Console.WriteLine("For use with signed assemblies in The Sims 3.");
            Console.WriteLine("");

            Consolery.Run(typeof (Program), args);
        }
    }
}
