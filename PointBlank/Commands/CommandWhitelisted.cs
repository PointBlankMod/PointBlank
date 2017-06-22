using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Whitelisted", 0)]
    internal class CommandWhitelisted : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Whitelisted"
        };

        public override string Help => Translation.Whitelisted_Help;

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.server.whitelisted";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            Provider.isWhitelisted = true;
            UnturnedChat.SendMessage(executor, Translation.Whitelisted_Set, ConsoleColor.Green);
        }
    }
}
