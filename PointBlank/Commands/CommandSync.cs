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
    [PointBlankCommand("Sync", 0)]
    internal class CommandSync : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Sync"
        };

        public override string Help => "Sets the server to sync mode";

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.server.sync";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            PlayerSavedata.hasSync = true;
            UnturnedChat.SendMessage(executor, "The server has been set to sync!", ConsoleColor.Green);
        }
    }
}
