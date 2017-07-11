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
    [PointBlankCommand("Welcome", 1)]
    internal class CommandWelcome : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Welcome"
        };

        public override string Help => Translations["Welcome_Help"];

        public override string Usage => Commands[0] + Translations["Welcome_Usage"];

        public override string DefaultPermission => "unturned.commands.server.welcome";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            ChatManager.welcomeText = string.Join(" ", args);
            UnturnedChat.SendMessage(executor, Translations["Welcome_Set"], ConsoleColor.Green);
        }
    }
}
