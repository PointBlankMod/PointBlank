using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Bans", 0)]
    internal class CommandBans : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Bans"
        };

        public override string Help => Translations["Bans_Help"];

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.admin.bans";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            UnturnedChat.SendMessage(executor, Translations["Bans_List"] + string.Join(",", SteamBlacklist.list.Select(a => a.playerID.ToString()).ToArray()), ConsoleColor.Green);
        }
    }
}
