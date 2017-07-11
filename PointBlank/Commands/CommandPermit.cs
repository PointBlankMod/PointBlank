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
    [PointBlankCommand("Permit", 2)]
    internal class CommandPermit : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Permit"
        };

        public override string Help => Translations["Permit_Help"];

        public override string Usage => Commands[0] + Translations["Permit_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.permit";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(!PlayerTool.tryGetSteamID(args[0], out CSteamID id))
            {
                UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                return;
            }

            SteamWhitelist.whitelist(id, args[1], executor?.SteamID ?? CSteamID.Nil);
            UnturnedChat.SendMessage(executor, string.Format(Translations["Permit_Added"], id), ConsoleColor.Green);
        }
    }
}
