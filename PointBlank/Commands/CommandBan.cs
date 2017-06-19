using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using Steamworks;
using SDG.Unturned;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Ban", 1)]
    internal class CommandBan : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Ban"
        };

        public override string Help => Translation.Ban_Help;

        public override string Usage => Commands[0] + Translation.Ban_Usage;

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
                UnturnedChat.SendMessage(executor, Translation.Base_InvalidPlayer, ConsoleColor.Red);
                return;
            }
            if (SteamGameServerNetworking.GetP2PSessionState(player, out P2PSessionState_t p2PSessionState_t))
                ip = p2PSessionState_t.m_nRemoteIP;
            else
                ip = 0u;
            if (args.Length < 2 || uint.TryParse(args[1], out duration))
                duration = SteamBlacklist.PERMANENT;
            if (args.Length < 3)
                reason = Translation.Ban_Reason;
            else
                reason = args[2];

            SteamBlacklist.ban(player, ip, (executor == null ? CSteamID.Nil : executor.SteamID), reason, duration);
            UnturnedChat.SendMessage(executor, string.Format(Translation.Ban_Success, player), ConsoleColor.Green);
        }
    }
}
