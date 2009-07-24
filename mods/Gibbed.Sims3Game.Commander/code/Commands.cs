using Sims3.SimIFace;
using Sims3.UI;

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
