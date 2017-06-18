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
    [Command("Kick", 1)]
    internal class CommandKick : Command
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "kick",
            "Kick",
            "KICK"
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
                ChatManager.SendMessage(executor, "Player not found!", ConsoleColor.Red);
                return;
            }
            if (args.Length < 2)
                reason = "Undefined";
            else
                reason = args[1];

            Provider.kick(ply.SteamID, reason);
            ChatManager.SendMessage(executor, ply.PlayerName + " has been kicked!", ConsoleColor.Green);
        }
    }
}
