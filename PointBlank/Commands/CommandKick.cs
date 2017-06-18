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
    [PointBlankCommand("Kick", 1)]
    internal class CommandKick : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "kick"
        };

        public override string Help => "Kicks a player from the server";

        public override string Usage => Commands[0] + " <player> [reason]";

        public override string DefaultPermission => "unturned.commands.admin.kick";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            UnturnedPlayer ply;
            string reason;

            if(!UnturnedPlayer.TryGetPlayer(args[0], out ply))
            {
                UnturnedChat.SendMessage(executor, "Player not found!", ConsoleColor.Red);
                return;
            }
            if (args.Length < 2)
                reason = "Undefined";
            else
                reason = args[1];

            Provider.kick(ply.SteamID, reason);
            UnturnedChat.SendMessage(executor, ply.PlayerName + " has been kicked!", ConsoleColor.Green);
        }
    }
}
