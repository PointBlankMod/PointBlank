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
            CSteamID player;
            uint duration;
            string reason;
            uint ip;

            if (!PlayerTool.tryGetSteamID(args[0], out player) || (executor != null && executor.SteamID == player))
            {
                UnturnedChat.SendMessage(executor, "Player not found!", ConsoleColor.Red);
                return;
            }
            if (SteamGameServerNetworking.GetP2PSessionState(player, out P2PSessionState_t p2PSessionState_t))
                ip = p2PSessionState_t.m_nRemoteIP;
            else
                ip = 0u;
            if (args.Length < 2 || uint.TryParse(args[1], out duration))
                duration = SteamBlacklist.PERMANENT;
            if (args.Length < 3)
                reason = "Undefined";
            else
                reason = args[2];

            UnturnedChat.SendMessage(executor, player + " has been banned!", ConsoleColor.Green);
            SteamBlacklist.ban(player, ip, (executor == null ? CSteamID.Nil : executor.SteamID), reason, duration);
        }
    }
}
