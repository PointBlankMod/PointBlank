using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;

namespace PointBlank.Commands
{
    [PointBlankCommand("Filter", 0)]
    internal class CommandFilter : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "filter"
        };

        public override string Help => "Filters names";

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.server.filter";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            Provider.filterName = true;
        }
    }
}
