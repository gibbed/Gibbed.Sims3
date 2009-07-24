using System.Collections.Generic;
using System.Runtime.InteropServices;
using Sims3.SimIFace;

namespace Gibbed.Sims3Game.Commander
{
    internal class CommandRegistry
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct CommandInfo
        {
            public CommandHandler mHandler;
            public string mDescription;

            public CommandInfo(CommandHandler handler, string description)
            {
                this.mHandler = handler;
                this.mDescription = description;
            }
        }

        private Dictionary<string, CommandInfo> mCommands = new Dictionary<string, CommandInfo>();

        public void Register(string name, string description, CommandHandler handler)
        {
            this.Register(name, description, handler, false);
        }

        public void Register(string name, string description, CommandHandler handler, bool bHidden)
        {
            CommandInfo info = new CommandInfo(handler, description);
            this.mCommands.Add(name, info);
            CommandSystem.RegisterCommand(name, description, handler, bHidden);
        }

        public void Unregister(string name)
        {
            if (this.mCommands.ContainsKey(name))
            {
                CommandSystem.UnregisterCommand(name);
                this.mCommands.Remove(name);
            }
        }

        public void UnregisterAll()
        {
            foreach (string str in this.mCommands.Keys)
            {
                CommandSystem.UnregisterCommand(str);
            }
            this.mCommands.Clear();
        }
    }

}
