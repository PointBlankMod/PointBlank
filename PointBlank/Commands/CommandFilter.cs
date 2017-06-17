using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using Provider = SDG.Unturned.Provider;

namespace PointBlank.Commands
{
    [Command("Filter", 0)]
    internal class CommandFilter : Command
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "filter",
            "Filter",
            "FILTER"
        };

        public override string Help => "Filters names";

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.server.filter";

        public override bool AllowRuntime => false;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            Provider.filterName = true;
        }
    }
}
