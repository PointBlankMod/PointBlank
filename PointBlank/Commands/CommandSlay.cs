using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Slay", 1)]
    internal class CommandSlay : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Slay"
        };

        public override string Help => Translation.Slay_Help;

        public override string Usage => Commands[0] + Translation.Slay_Usage;

        public override string DefaultPermission => "unturned.commands.admin.slay";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(!UnturnedPlayer.TryGetPlayer(args[0], out UnturnedPlayer ply))
            {
                UnturnedChat.SendMessage(executor, Translation.Base_InvalidPlayer, ConsoleColor.Red);
                return;
            }

            SteamBlacklist.ban(ply.SteamID, ply.SteamIP, executor?.SteamID ?? CSteamID.Nil, (args.Length > 1 ? args[1] : "Undefined"), SteamBlacklist.PERMANENT);
            if (ply.SteamPlayer.player != null)
                ply.Player.life.askDamage(255, Vector3.up * 101f, EDeathCause.KILL, ELimb.SKULL, executor?.SteamID ?? CSteamID.Nil, out EPlayerKill ePlayerKill);
            UnturnedChat.SendMessage(executor, string.Format(Translation.Slay_Slay, ply.PlayerName), ConsoleColor.Red);
        }
    }
}
