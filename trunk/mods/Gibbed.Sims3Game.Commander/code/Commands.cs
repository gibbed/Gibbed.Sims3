using System;
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
            Registry.Register("te", "Throw an exception to test exception handling.", new CommandHandler(Commands.OnThrowException));
        }

        private static int OnDIGL(object[] parameters)
        {
            SimpleMessageDialog.Show("Commander", "Yep.");
            return 0;
        }

        private static void ThrowExceptionFunction()
        {
            throw new InvalidProgramException("this is a test of EnableScriptError");
        }

        private static int OnThrowException(object[] paramenters)
        {
            Simulator.AddObject(new Sims3.UI.OneShotFunctionTask(new Sims3.UI.Function(ThrowExceptionFunction)));
            return 0;
        }
    }
}
