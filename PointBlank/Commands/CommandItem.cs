using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using CMD = PointBlank.API.Commands.Command;

namespace PointBlank.Commands
{
    [Command("Item", 1)]
    internal class CommandItem : CMD
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "i",
            "I",
            "item",
            "Item",
            "ITEM"
        };

        public override string Help => "Gives the player a specific item";

        public override string Usage => Commands[0] + " <item> [player]";

        public override string DefaultPermission => "pointblank.commands.admin.item";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            
        }
    }
}
