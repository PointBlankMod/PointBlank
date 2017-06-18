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
    [PointBlankCommand("Airdrop", 0)]
    internal class CommandAirdrop : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "airdrop"
        };

        public override string Help => "Activates an airdrop";

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.admin.airdrop";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if (LevelManager.hasAirdrop)
                return;

            LevelManager.airdropFrequency = 0u;
            UnturnedChat.SendMessage(executor, "Airdrop spawned successfully!", ConsoleColor.Green);
        }
    }
}
