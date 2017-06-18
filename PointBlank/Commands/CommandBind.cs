using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;

namespace PointBlank.Commands
{
    [PointBlankCommand("Bind", 1)]
    internal class CommandBind : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "bind"
        };

        public override string Help => "Binds the server to a specific IP";

        public override string Usage => Commands[0] + " <IP>";

        public override string DefaultPermission => "unturned.commands.server.bind";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if (!Parser.checkIP(args[0]))
            {
                CommandWindow.Log("Invalid IP!", ConsoleColor.Red);
                return;
            }

            Provider.ip = Parser.getUInt32FromIP(args[0]);
        }
    }
}
