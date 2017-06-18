using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using Steamworks;
using SDG.Unturned;

namespace PointBlank.Commands
{
    [PointBlankCommand("Ban", 1)]
    internal class CommandBan : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "ban"
        };

        public override string Help => "Bans a player";

        public override string Usage => Commands[0] + " <player> [duration] [reason]";

        public override string DefaultPermission => "unturned.commands.admin.ban";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            UnturnedPlayer player;
            uint duration;
            string reason;

            if(!UnturnedPlayer.TryGetPlayer(args[0], out player) || (executor == player && executor != null))
            {
                UnturnedChat.SendMessage(executor, "Player not found!", ConsoleColor.Red);
                return;
            }
            if(args.Length < 2 || uint.TryParse(args[1], out duration))
                duration = SteamBlacklist.PERMANENT;
            if (args.Length < 3)
                reason = "Undefined";
            else
                reason = args[2];

            UnturnedChat.SendMessage(executor, player.PlayerName + " has been banned!", ConsoleColor.Green);
            SteamBlacklist.ban(player.SteamID, player.SteamIP, (executor == null ? CSteamID.Nil : executor.SteamID), reason, duration);
        }
    }
}
