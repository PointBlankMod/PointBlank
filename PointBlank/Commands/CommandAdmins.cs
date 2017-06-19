using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using UnityEngine;
using SDG.Unturned;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Admins", 0)]
    internal class CommandAdmins : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Admins"
        };

        public override string Help => Translation.Admins_Help;

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.nonadmin.admins";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            string admins = string.Join(",", Provider.clients.Where(a => a.isAdmin).Select(a => a.playerID.playerName).ToArray());

            if(executor == null)
            {
                CommandWindow.Log(Translation.Admins_List + admins, ConsoleColor.Green);
            }
            else
            {
                if(Provider.hideAdmins && !executor.HasPermission("unturned.revealadmins"))
                {
                    executor.SendMessage(Translation.Admins_Hidden, Color.red);
                    return;
                }

                executor.SendMessage(Translation.Admins_List + admins, Color.green);
            }
        }
    }
}
