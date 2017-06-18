using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using UnityEngine;
using SDG.Unturned;
using Steamworks;
using CMD = PointBlank.API.Commands.Command;
using CM = PointBlank.API.Unturned.Chat.ChatManager;

namespace PointBlank.Commands
{
    [Command("Kill", 0)]
    internal class CommandKill : CMD
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "kill",
            "Kill",
            "KILL"
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
                    CM.SendMessage(executor, "Player not found!", ConsoleColor.Red);
                    return;
                }

                ply = executor;
            }

            ply.Player.life.askDamage(255, Vector3.up * 10f, EDeathCause.KILL, ELimb.SKULL, (executor == null ? CSteamID.Nil : executor.SteamID), out EPlayerKill kill);
            CM.SendMessage(executor, ply.PlayerName + " has been killed", ConsoleColor.Green);
        }
    }
}
