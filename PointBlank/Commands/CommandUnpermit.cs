using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using Steamworks;
using PointBlank.API.Collections;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Unpermit", 1)]
    internal class CommandUnpermit : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Unpermit"
        };

        public override string Help => Translations["Unpermit_Help"];

        public override string Usage => Commands[0] + Translations["Unpermit_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.unpermit";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(!PlayerTool.tryGetSteamID(args[0], out CSteamID id))
            {
                UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                return;
            }

            if (!SteamWhitelist.unwhitelist(id))
            {
                UnturnedChat.SendMessage(executor, string.Format(Translations["Unpermit_NotWhitelisted"], id), ConsoleColor.Red);
                return;
            }
            UnturnedChat.SendMessage(executor, string.Format(Translations["Unpermit_Unpermit"], id), ConsoleColor.Green);
        }
    }
}
