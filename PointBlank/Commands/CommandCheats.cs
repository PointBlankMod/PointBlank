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
    [PointBlankCommand("Cheats", 0)]
    internal class CommandCheats : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Cheats"
        };

        public override string Help => Translation.Cheats_Help;

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.server.cheats";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            Provider.hasCheats = true;
            UnturnedChat.SendMessage(executor, Translation.Cheats_Enabled, ConsoleColor.Green);
        }
    }
}
