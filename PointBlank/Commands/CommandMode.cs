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
    [PointBlankCommand("Mode", 1)]
    internal class CommandMode : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "mode"
        };

        public override string Help => "Sets the difficulty of the server";

        public override string Usage => Commands[0] + " <difficulty(easy/normal/hard)>";

        public override string DefaultPermission => "unturned.commands.server.mode";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if (args[0].ToLower() == "easy")
                Provider.mode = EGameMode.EASY;
            else if (args[0].ToLower() == "normal")
                Provider.mode = EGameMode.NORMAL;
            else if (args[0].ToLower() == "hard")
                Provider.mode = EGameMode.HARD;
            else
            {
                UnturnedChat.SendMessage(executor, "Invalid difficulty!", ConsoleColor.Red);
                return;
            }

            UnturnedChat.SendMessage(executor, "Difficulty set to " + Provider.mode.ToString(), ConsoleColor.Green);
        }
    }
}
