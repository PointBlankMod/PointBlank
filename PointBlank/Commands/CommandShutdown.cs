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
    [PointBlankCommand("Shutdown", 0)]
    internal class CommandShutdown : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "shutdown"
        };

        public override string Help => Translations["Shutdown_Help"];

        public override string Usage => Commands[0] + Translations["Shutdown_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.shutdown";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            int time = 0;
            bool timed = false;

            if(args.Length > 0)
                if(int.TryParse(args[0], out time))
                    timed = true;
            if (timed)
                Provider.shutdown(time);
            else
                Provider.shutdown();
        }
    }
}
