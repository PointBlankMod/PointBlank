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
    [PointBlankCommand("Port", 1)]
    internal class CommandPort : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Port"
        };

        public override string Help => Translations["Port_Help"];

        public override string Usage => Commands[0] + Translations["Port_Usage"];

        public override string DefaultPermission => "unturned.commands.server.port";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(!ushort.TryParse(args[0], out ushort port))
            {
                UnturnedChat.SendMessage(executor, Translations["Port_Invalid"], ConsoleColor.Red);
                return;
            }

            Provider.port = port;
            UnturnedChat.SendMessage(executor, string.Format(Translations["Port_Set"], port), ConsoleColor.Green);
        }
    }
}
