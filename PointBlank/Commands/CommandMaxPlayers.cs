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
    [PointBlankCommand("MaxPlayers", 1)]
    internal class CommandMaxPlayers : PointBlankCommand
    {
        #region Info
        public static readonly byte MIN_NUMBER = 1;
        public static readonly byte MAX_NUMBER = 48;
        #endregion

        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "MaxPlayers"
        };

        public override string Help => Translations["MaxPlayers_Help"];

        public override string Usage => Commands[0] + Translations["MaxPlayers_Usage"];

        public override string DefaultPermission => "unturned.commands.server.maxplayers";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(!byte.TryParse(args[0], out byte max))
            {
                UnturnedChat.SendMessage(executor, Translations["MaxPlayers_Invalid"], ConsoleColor.Red);
                return;
            }

            if(max > MAX_NUMBER)
            {
                UnturnedChat.SendMessage(executor, string.Format(Translations["MaxPlayers_TooHigh"], MAX_NUMBER), ConsoleColor.Red);
                return;
            }
            else if(max < MIN_NUMBER)
            {
                UnturnedChat.SendMessage(executor, string.Format(Translations["MaxPlayers_TooLow"], MIN_NUMBER), ConsoleColor.Red);
                return;
            }

            Provider.maxPlayers = max;
            UnturnedChat.SendMessage(executor, string.Format(Translations["MaxPlayers_Set"], max), ConsoleColor.Green);
        }
    }
}
