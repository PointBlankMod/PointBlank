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
    [PointBlankCommand("PvE", 0)]
    internal class CommandPvE : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "PvE"
        };

        public override string Help => "Sets the server to PvE";

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.server.pve";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            Provider.isPvP = false;
            UnturnedChat.SendMessage(executor, "Server is now in PvE mode!", ConsoleColor.Green);
        }
    }
}
