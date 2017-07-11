using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;
using PointBlank.API.Collections;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Log", 4)]
    internal class CommandLog : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Log"
        };

        public override string Help => Translations["Log_Help"];

        public override string Usage => Commands[0] + Translations["Log_Usage"];

        public override string DefaultPermission => "unturned.commands.server.log";

        public override EAllowedCaller AllowedCaller => EAllowedCaller.SERVER;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            CommandWindow.shouldLogChat = (args[0].ToLower() == "y");
            CommandWindow.shouldLogJoinLeave = (args[1].ToLower() == "y");
            CommandWindow.shouldLogDeaths = (args[2].ToLower() == "y");
            CommandWindow.shouldLogAnticheat = (args[3].ToLower() == "y");
            CommandWindow.Log(Translations["Log_Set"], ConsoleColor.Green);
        }
    }
}
