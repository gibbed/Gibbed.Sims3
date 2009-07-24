using System;
using System.Collections.Generic;
using System.Text;

namespace Gibbed.Sims3Game.Commander
{
    public class Startup
    {
        [Sims3.SimIFace.Tunable]
        private static bool Initialized = false;

        static Startup()
        {
            Commands.RegisterCommands();
        }
    }
}
