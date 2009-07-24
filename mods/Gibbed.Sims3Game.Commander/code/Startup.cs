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
