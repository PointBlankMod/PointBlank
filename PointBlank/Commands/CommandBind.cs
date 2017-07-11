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
    [PointBlankCommand("Bind", 1)]
    internal class CommandBind : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Bind"
        };

        public override string Help => Translations["Bind_Help"];

        public override string Usage => Commands[0] + Translations["Bind_Usage"];

        public override string DefaultPermission => "unturned.commands.server.bind";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if (!Parser.checkIP(args[0]))
            {
                CommandWindow.Log(Translations["Bind_InvalidIP"], ConsoleColor.Red);
                return;
            }

            Provider.ip = Parser.getUInt32FromIP(args[0]);
        }
    }
}
