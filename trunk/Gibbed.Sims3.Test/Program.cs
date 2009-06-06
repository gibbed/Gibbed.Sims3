using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Gibbed.Helpers;
using Gibbed.RefPack;
using System.Runtime.InteropServices;
using Gibbed.Sims3.FileFormats;
using Gibbed.Sims3.ExportableContent;

namespace Gibbed.Sims3.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Stream input = File.OpenRead("C:\\Users\\Rick\\Desktop\\effect\\effect\\effects\\#0051185B\\Sims3Effects.effects");
            new EffectsFile().Deserialize(input);
        }
    }
}
