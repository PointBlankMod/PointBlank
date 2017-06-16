using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using LevelManager = SDG.Unturned.LevelManager;

namespace PointBlank.Commands
{
    [Command("Airdrop", 0)]
    public class CommandAirdrop : Command
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "airdrop",
            "AIRDROP"
        };

        public override string Help => "Activates an airdrop";

        public override string Usage => "airdrop";

        public override string DefaultPermission => "unturned.commands.admin.airdrop";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if (LevelManager.hasAirdrop)
                return;

            LevelManager.airdropFrequency = 0u;
        }
    }
}
