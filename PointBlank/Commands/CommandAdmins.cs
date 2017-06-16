using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using UnityEngine;
using Provider = SDG.Unturned.Provider;
using CommandWindow = SDG.Unturned.CommandWindow;

namespace PointBlank.Commands
{
    [Command("Admins", 0)]
    internal class CommandAdmins : Command
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "admins",
            "ADMINS"
        };

        public override string Help => "Shows a list of admins on the server";

        public override string Usage => "admins";

        public override string DefaultPermission => "unturned.commands.nonadmin.admins";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            string admins = string.Join(",", Provider.clients.Where(a => a.isAdmin).Select(a => a.playerID.playerName).ToArray());

            if(executor == null)
            {
                CommandWindow.Log("Admins: " + admins, ConsoleColor.Green);
            }
            else
            {
                if(Provider.hideAdmins && !executor.HasPermission("unturned.revealadmins"))
                {
                    executor.SendMessage("The admins are currently hidden!", Color.red);
                    return;
                }

                executor.SendMessage("Admins: " + admins, Color.green);
            }
        }
    }
}
