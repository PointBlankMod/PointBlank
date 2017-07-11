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
    [PointBlankCommand("Time", 1)]
    internal class CommandTime : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Time"
        };

        public override string Help => Translations["Time_Help"];

        public override string Usage => Commands[0] + Translations["Time_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.time";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if (Provider.isServer && Level.info.type == ELevelType.ARENA)
            {
                UnturnedChat.SendMessage(executor, Translations["Base_NoArenaTime"], ConsoleColor.Red);
                return;
            }
            if (Provider.isServer && Level.info.type == ELevelType.HORDE)
            {
                UnturnedChat.SendMessage(executor, Translations["Base_NoHordeTime"], ConsoleColor.Red);
                return;
            }

            if(!uint.TryParse(args[1], out uint time))
            {
                UnturnedChat.SendMessage(executor, Translations["Time_Invalid"], ConsoleColor.Red);
                return;
            }

            LightingManager.time = time;
            UnturnedChat.SendMessage(executor, string.Format(Translations["Time_Set"], time), ConsoleColor.Green);
        }
    }
}
