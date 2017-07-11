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
    [PointBlankCommand("Queue", 1)]
    internal class CommandQueue : PointBlankCommand
    {
        #region Info
        private static readonly byte MAX_NUMBER = 64;
        #endregion

        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Queue"
        };

        public override string Help => Translations["Queue_Help"];

        public override string Usage => Commands[0] + Translations["Queue_Usage"];

        public override string DefaultPermission => "unturned.commands.server.queue";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(!byte.TryParse(args[0], out byte queue))
            {
                UnturnedChat.SendMessage(executor, Translations["Queue_Invalid"], ConsoleColor.Red);
                return;
            }
            if(queue > MAX_NUMBER)
            {
                UnturnedChat.SendMessage(executor, string.Format(Translations["Queue_TooHigh"], MAX_NUMBER), ConsoleColor.Red);
                return;
            }

            Provider.queueSize = queue;
            UnturnedChat.SendMessage(executor, string.Format(Translations["Queue_Set"], queue), ConsoleColor.Green);
        }
    }
}
