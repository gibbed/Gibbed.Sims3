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
using Gibbed.Sims3.Helpers;

namespace Gibbed.Sims3.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles("C:\\Users\\Rick\\Desktop\\blendgeom", "*.blendgeom", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                Stream input = File.OpenRead(file);

                BlendGeometry facialBlend = new BlendGeometry();
                facialBlend.Deserialize(input);

                input.Close();
            }
        }
    }
}
