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
    [PointBlankCommand("Whitelisted", 0)]
    internal class CommandWhitelisted : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Whitelisted"
        };

        public override string Help => "Sets the server to whitelist only";

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.server.whitelisted";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            Provider.isWhitelisted = true;
            UnturnedChat.SendMessage(executor, "Server now in whitelist only mode!", ConsoleColor.Green);
        }
    }
}
