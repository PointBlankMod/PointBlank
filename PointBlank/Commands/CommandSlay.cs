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

        public override string Help => "Slays a player";

        public override string Usage => Commands[0] + " <player> [reason]";

        public override string DefaultPermission => "unturned.commands.admin.slay";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(!UnturnedPlayer.TryGetPlayer(args[0], out UnturnedPlayer ply))
            {
                UnturnedChat.SendMessage(executor, "Invalid player!", ConsoleColor.Red);
                return;
            }

            SteamBlacklist.ban(ply.SteamID, ply.SteamIP, (executor == null ? CSteamID.Nil : executor.SteamID), (args.Length > 1 ? args[1] : "Undefined"), SteamBlacklist.PERMANENT);
            if (ply.SteamPlayer.player != null)
                ply.Player.life.askDamage(255, Vector3.up * 101f, EDeathCause.KILL, ELimb.SKULL, (executor == null ? CSteamID.Nil : executor.SteamID), out EPlayerKill ePlayerKill);
            UnturnedChat.SendMessage(executor, ply.PlayerName + " has been slayed!", ConsoleColor.Red);
        }
    }
}
