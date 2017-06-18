using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using Provider = SDG.Unturned.Provider;

namespace PointBlank.Commands
{
    [Command("Cheats", 0)]
    internal class CommandCheats : Command
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "cheats",
            "Cheats",
            "CHEATS"
        };

        public override string Help => "Enables cheats for the server";

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.server.cheats";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            Provider.hasCheats = true;
            ChatManager.SendMessage(executor, "Cheats have been enabled!", ConsoleColor.Green);
        }
    }
}
