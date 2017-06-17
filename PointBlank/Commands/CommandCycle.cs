using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using LightingManager = SDG.Unturned.LightingManager;
using Provider = SDG.Unturned.Provider;
using Level = SDG.Unturned.Level;
using ELevelType = SDG.Unturned.ELevelType;

namespace PointBlank.Commands
{
    [Command("Cycle", 1)]
    internal class CommandCycle : Command
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "cycle",
            "Cycle",
            "CYCLE"
        };

        public override string Help => "Sets the cycle for time";

        public override string Usage => Commands[0] + " <cycle>";

        public override string DefaultPermission => "unturned.commands.admin.cycle";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(Provider.isServer && Level.info.type == ELevelType.ARENA)
            {
                ChatManager.SendMessage(executor, "Can't set cycle on arena!", ConsoleColor.Red);
                return;
            }
            if (Provider.isServer && Level.info.type == ELevelType.HORDE)
            {
                ChatManager.SendMessage(executor, "Can't set cycle on horde!", ConsoleColor.Red);
                return;
            }
            if (!uint.TryParse(args[0], out uint cycle))
            {
                ChatManager.SendMessage(executor, "Invalid cycle number!", ConsoleColor.Red);
                return;
            }

            LightingManager.cycle = cycle;
            ChatManager.SendMessage(executor, "Set cycle to " + cycle.ToString(), ConsoleColor.Green);
        }
    }
}
