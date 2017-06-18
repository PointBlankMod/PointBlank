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
    [PointBlankCommand("Cheats", 0)]
    internal class CommandCheats : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "cheats"
        };

        public override string Help => "Enables cheats for the server";

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.server.cheats";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            Provider.hasCheats = true;
            UnturnedChat.SendMessage(executor, "Cheats have been enabled!", ConsoleColor.Green);
        }
    }
}
