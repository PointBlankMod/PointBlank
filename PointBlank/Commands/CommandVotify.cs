using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;

namespace PointBlank.Commands
{
    [PointBlankCommand("Votify", 6)]
    internal class CommandVotify : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Votify"
        };

        public override string Help => "Sets the voting options";

        public override string Usage => Commands[0] + "";

        public override string DefaultPermission => throw new NotImplementedException();
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
