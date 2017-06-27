using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Permissions", 1)]
    internal class CommandPermissions : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "p",
            "Permissions"
        };

        public override string Help => Translation.Permissions_Help;

        public override string Usage => string.Format(Translation.Permissions_Usage, Translation.Permissions_Commands_Help);

        public override string DefaultPermission => "pointblank.commands.admin.permissions";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(StringComparer.InvariantCultureIgnoreCase.Compare(args[0], Translation.Permissions_Commands_Help) == 0)
            {
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translation.Permissions_Group, Translation.Permissions_Commands_Group), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translation.Permissions_Group_Modify, Translation.Permissions_Commands_Group, Translation.Permissions_Commands_Add), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translation.Permissions_Group_Modify, Translation.Permissions_Commands_Group, Translation.Permissions_Commands_Remove), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translation.Permissions_Player, Translation.Permissions_Commands_Player), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translation.Permissions_Player_Modify, Translation.Permissions_Commands_Player, Translation.Permissions_Commands_Add), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translation.Permissions_Player_Modify, Translation.Permissions_Commands_Player, Translation.Permissions_Commands_Remove), ConsoleColor.Green);
            }
        }

        #region Functions
        private void Player(UnturnedPlayer executor, string[] args)
        {

        }

        private void Group(UnturnedPlayer executor, string[] args)
        {

        }
        #endregion
    }
}
