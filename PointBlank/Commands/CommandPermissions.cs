using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;

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

        public override string Help => throw new NotImplementedException();

        public override string Usage => throw new NotImplementedException();

        public override string DefaultPermission => throw new NotImplementedException();
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
