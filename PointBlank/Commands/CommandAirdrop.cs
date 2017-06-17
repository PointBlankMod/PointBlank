using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using LevelManager = SDG.Unturned.LevelManager;

namespace PointBlank.Commands
{
    [Command("Airdrop", 0)]
    internal class CommandAirdrop : Command
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "airdrop",
            "Airdrop",
            "AIRDROP"
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
            ChatManager.SendMessage(executor, "Airdrop spawned successfully!", ConsoleColor.Green);
        }
    }
}
