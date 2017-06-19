using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;

namespace PointBlank.Commands
{
    [PointBlankCommand("Save", 0)]
    internal class CommandSave : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Save"
        };

        public override string Help => "Saves all the data";

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.admin.save";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            SaveManager.save();
            UnturnedChat.SendMessage(executor, "Data has been saved!", ConsoleColor.Green);
        }
    }
}
