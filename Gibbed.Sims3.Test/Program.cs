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
            byte[] u123 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            byte[] u222 = new byte[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };

            Stream data = new MemoryStream();
            Database db = new Database(data, false);

            db.SetResource(new ResourceKey(1, 2, 3), u123);
            db.SetResource(new ResourceKey(2, 2, 2), u222);

            db.Commit(true);

            byte[] d123 = db.GetResource(new ResourceKey(1, 2, 3));
            byte[] d222 = db.GetResource(new ResourceKey(2, 2, 2));
        }
    }
}
