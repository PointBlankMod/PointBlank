using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using UnityEngine;
using SDG.Unturned;
using Steamworks;

namespace PointBlank.Commands
{
    [PointBlankCommand("Kill", 0)]
    internal class CommandKill : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "kill"
        };

        public override string Help => "Kills a player";

        public override string Usage => Commands[0] + " [player]";

        public override string DefaultPermission => "unturned.commands.admin.kill";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            UnturnedPlayer ply;

            if(args.Length < 1 || UnturnedPlayer.TryGetPlayer(args[0], out ply))
            {
                if(executor == null)
                {
                    UnturnedChat.SendMessage(executor, "Player not found!", ConsoleColor.Red);
                    return;
                }

                ply = executor;
            }

            ply.Player.life.askDamage(255, Vector3.up * 10f, EDeathCause.KILL, ELimb.SKULL, (executor == null ? CSteamID.Nil : executor.SteamID), out EPlayerKill kill);
            UnturnedChat.SendMessage(executor, ply.PlayerName + " has been killed", ConsoleColor.Green);
        }
    }
}
