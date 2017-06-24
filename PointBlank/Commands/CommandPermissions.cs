using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
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
            
        }
    }
}
