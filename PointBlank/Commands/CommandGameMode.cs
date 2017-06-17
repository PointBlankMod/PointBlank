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
    [Command("GameMode", 1)]
    internal class CommandGameMode : Command
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "gamemode",
            "GAMEMODE",
            "Gamemode",
            "GameMode"
        };

        public override string Help => "Sets the server game mode";

        public override string Usage => Commands[0] + " <game mode>";

        public override string DefaultPermission => "unturned.commands.server.gamemode";

        public override bool AllowRuntime => false;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            Provider.selectedGameModeName = args[0];
            ChatManager.SendMessage(executor, "Setting game mode to " + args[0], ConsoleColor.Green);
        }
    }
}
