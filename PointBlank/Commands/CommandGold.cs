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
    [PointBlankCommand("Gold", 0)]
    internal class CommandGold : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Gold"
        };

        public override string Help => Translation.Gold_Help;

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.server.gold";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            Provider.isGold = true;
            UnturnedChat.SendMessage(executor, Translation.Gold_Set, ConsoleColor.Green);
        }
    }
}
