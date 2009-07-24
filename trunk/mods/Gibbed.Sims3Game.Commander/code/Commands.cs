using System;
using System.Collections.Generic;
using System.Text;
using Sims3.UI;
using Sims3.SimIFace;
using Sims3.Gameplay;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Objects;
using Sims3.Gameplay.Interfaces;
using Sims3.SimIFace.CAS;

namespace Gibbed.Sims3Game.Commander
{
    public class Commands
    {
        private static CommandRegistry Registry;

        public static void RegisterCommands()
        {
            Registry = new CommandRegistry();
            Registry.Register("digl", "Did I Get Loaded?", new CommandHandler(Commands.OnDIGL));
        }

        private static int OnDIGL(object[] parameters)
        {
            SimpleMessageDialog.Show("Commander", "Yep.");
            return 0;
        }
    }
}
