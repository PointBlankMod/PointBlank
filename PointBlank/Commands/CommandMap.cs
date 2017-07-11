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
    [PointBlankCommand("Map", 1)]
    internal class CommandMap : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Map"
        };

        public override string Help => Translation.Map_Help;

        public override string Usage => Commands[0] + Translation.Map_Usage;

        public override string DefaultPermission => "unturned.commands.server.map";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if (!Level.exists(args[0]))
            {
                UnturnedChat.SendMessage(executor, Translation.Map_Invalid, ConsoleColor.Red);
                return;
            }

            Provider.map = args[0];
            UnturnedChat.SendMessage(executor, string.Format(Translation.Map_Set, args[0]), ConsoleColor.Green);
        }
    }
}
