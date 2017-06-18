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
    [PointBlankCommand("Map", 1)]
    internal class CommandMap : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "map"
        };

        public override string Help => "Sets the server map";

        public override string Usage => Commands[0] + " <map>";

        public override string DefaultPermission => "unturned.commands.server.map";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if (!Level.exists(args[0]))
            {
                UnturnedChat.SendMessage(executor, "Invalid map!", ConsoleColor.Red);
                return;
            }

            Provider.map = args[0];
        }
    }
}
