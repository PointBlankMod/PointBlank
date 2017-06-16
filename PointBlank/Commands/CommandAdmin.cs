using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using UnityEngine;
using Steamworks;
using CommandWindow = SDG.Unturned.CommandWindow;
using SteamAdminlist = SDG.Unturned.SteamAdminlist;

namespace PointBlank.Commands
{
    [Command("Admin", 1)]
    internal class CommandAdmin : Command
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "admin",
            "ADMIN"
        };

        public override string Help => "Admins a player";

        public override string Usage => "admin <player>";

        public override string DefaultPermission => "unturned.commands.admin.admin";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            UnturnedPlayer player;

            if(!UnturnedPlayer.TryGetPlayer(args[0], out player))
            {
                if (executor == null)
                    CommandWindow.Log("Player not found!", ConsoleColor.Red);
                else
                    executor.SendMessage("Player not found!", Color.red);
                return;
            }

            if(executor == null)
            {
                SteamAdminlist.admin(player.SteamID, CSteamID.Nil);
                CommandWindow.Log(player.PlayerName + " has been set as admin!", ConsoleColor.Green);
                
            }
            else
            {
                SteamAdminlist.admin(player.SteamID, executor.SteamID);
                executor.SendMessage(player.PlayerName + " has been set as admin!", Color.green);
            }
            player.SendMessage("You are now admin!", Color.green);
        }
    }
}
