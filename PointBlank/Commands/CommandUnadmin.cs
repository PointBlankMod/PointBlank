using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;

namespace PointBlank.Commands
{
    [PointBlankCommand("Unadmin", 1)]
    internal class CommandUnadmin : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Unadmin"
        };

        public override string Help => "Unadmin a player";

        public override string Usage => Commands[0] + " <player>";

        public override string DefaultPermission => "unturned.commands.admin.unadmin";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(!UnturnedPlayer.TryGetPlayer(args[0], out UnturnedPlayer ply))
            {
                UnturnedChat.SendMessage(executor, "Invalid player!", ConsoleColor.Red);
                return;
            }
        }
    }
}
