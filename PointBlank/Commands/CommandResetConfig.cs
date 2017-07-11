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
    [PointBlankCommand("ResetConfig", 0)]
    internal class CommandResetConfig : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "ResetConfig"
        };

        public override string Help => Translations["ResetConfig_Help"];

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.admin.resetconfig";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            Provider.resetConfig();
            UnturnedChat.SendMessage(executor, Translations["ResetConfig_Reset"], ConsoleColor.Green);
        }
    }
}
