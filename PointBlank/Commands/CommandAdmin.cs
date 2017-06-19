using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using UnityEngine;
using Steamworks;
using SDG.Unturned;

namespace PointBlank.Commands
{
    [PointBlankCommand("Admin", 1)]
    internal class CommandAdmin : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "admin"
        };

        public override string Help => "Admins a player";

        public override string Usage => Commands[0] + " <player>";

        public override string DefaultPermission => "unturned.commands.admin.admin";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if (!PlayerTool.tryGetSteamID(args[0], out CSteamID player))
            {
                UnturnedChat.SendMessage(executor, "Invalid player!", ConsoleColor.Red);
                return;
            }

            if (executor == null)
            {
                SteamAdminlist.admin(player, CSteamID.Nil);
                CommandWindow.Log(player + " has been set as admin!", ConsoleColor.Green);

            }
            else
            {
                SteamAdminlist.admin(player, executor.SteamID);
                executor.SendMessage(player + " has been set as admin!", Color.green);
            }
        }
    }
}
