using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
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
            "Admin",
            "ADMIN"
        };

        public override string Help => "Admins a player";

        public override string Usage => Commands[0] + " <player>";

        public override string DefaultPermission => "unturned.commands.admin.admin";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if (!UnturnedPlayer.TryGetPlayer(args[0], out UnturnedPlayer player))
            {
                ChatManager.SendMessage(executor, "Player not found!", ConsoleColor.Red);
                return;
            }

            if (executor == null)
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
