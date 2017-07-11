using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using Steamworks;
using PointBlank.API.Collections;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Spy", 1)]
    internal class CommandSpy : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Spy"
        };

        public override string Help => Translations["Spy_Help"];

        public override string Usage => Commands[0] + Translations["Spy_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.spy";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(!UnturnedPlayer.TryGetPlayer(args[0], out UnturnedPlayer ply))
            {
                UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                return;
            }

            ply.Player.sendScreenshot(executor?.SteamID ?? CSteamID.Nil, null);
            UnturnedChat.SendMessage(executor, string.Format(Translations["Spy_Spy"], ply.PlayerName), ConsoleColor.Red);
        }
    }
}
