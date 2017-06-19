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
    [PointBlankCommand("Time", 1)]
    internal class CommandTime : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Time"
        };

        public override string Help => "Sets the time";

        public override string Usage => Commands[0] + " <time>";

        public override string DefaultPermission => "unturned.commands.admin.time";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if (Provider.isServer && Level.info.type == ELevelType.ARENA)
            {
                UnturnedChat.SendMessage(executor, "Can't set time on arena!", ConsoleColor.Red);
                return;
            }
            if (Provider.isServer && Level.info.type == ELevelType.HORDE)
            {
                UnturnedChat.SendMessage(executor, "Can't set time on horde!", ConsoleColor.Red);
                return;
            }

            if(!uint.TryParse(args[1], out uint time))
            {
                UnturnedChat.SendMessage(executor, "Invalid time!", ConsoleColor.Red);
                return;
            }

            LightingManager.time = time;
            UnturnedChat.SendMessage(executor, "Time set to " + time, ConsoleColor.Green);
        }
    }
}
