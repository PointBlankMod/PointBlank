using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Permits", 0)]
    internal class CommandPermits : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Permits"
        };

        public override string Help => Translation.Permits_Help;

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.admin.permits";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            UnturnedChat.SendMessage(executor, string.Format(Translation.Permits_List, string.Join(",", SteamWhitelist.list.Select(a => a.steamID.ToString()).ToArray())), ConsoleColor.Green);
        }
    }
}
