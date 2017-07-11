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
    [PointBlankCommand("HideAdmins", 0)]
    internal class CommandHideAdmins : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "HideAdmins"
        };

        public override string Help => Translations["HideAdmins_Help"];

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.server.hideadmins";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            Provider.hideAdmins = true;
            UnturnedChat.SendMessage(executor, Translations["HideAdmins_Set"], ConsoleColor.Green);
        }
    }
}
