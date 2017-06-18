using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;

namespace PointBlank.Commands
{
    [PointBlankCommand("Cycle", 1)]
    internal class CommandCycle : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "cycle"
        };

        public override string Help => "Sets the cycle for time";

        public override string Usage => Commands[0] + " <cycle>";

        public override string DefaultPermission => "unturned.commands.admin.cycle";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(Provider.isServer && Level.info.type == ELevelType.ARENA)
            {
                UnturnedChat.SendMessage(executor, "Can't set cycle on arena!", ConsoleColor.Red);
                return;
            }
            if (Provider.isServer && Level.info.type == ELevelType.HORDE)
            {
                UnturnedChat.SendMessage(executor, "Can't set cycle on horde!", ConsoleColor.Red);
                return;
            }
            if (!uint.TryParse(args[0], out uint cycle))
            {
                UnturnedChat.SendMessage(executor, "Invalid cycle number!", ConsoleColor.Red);
                return;
            }

            LightingManager.cycle = cycle;
            UnturnedChat.SendMessage(executor, "Set cycle to " + cycle.ToString(), ConsoleColor.Green);
        }
    }
}
