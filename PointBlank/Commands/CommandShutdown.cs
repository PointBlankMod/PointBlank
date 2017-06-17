using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using Provider = SDG.Unturned.Provider;

namespace PointBlank.Commands
{
    [Command("Shutdown", 0)]
    internal class CommandShutdown : Command
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "shutdown",
            "Shutdown",
            "ShutDown",
            "SHUTDOWN"
        };

        public override string Help => "Shuts down the unturned server";

        public override string Usage => Commands[0] + " [seconds until shutdown]";

        public override string DefaultPermission => "unturned.commands.admin.shutdown";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            int time = 0;
            bool timed = false;

            if(args.Length > 0)
                if(int.TryParse(args[0], out time))
                    timed = true;
            if (timed)
                Provider.shutdown(time);
            else
                Provider.shutdown();
        }
    }
}
