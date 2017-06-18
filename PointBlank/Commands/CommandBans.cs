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
    [PointBlankCommand("Bans", 0)]
    internal class CommandBans : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "bans"
        };

        public override string Help => "Shows the current bans";

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.admin.bans";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            UnturnedChat.SendMessage(executor, string.Join(",", SteamBlacklist.list.Select(a => a.playerID.ToString()).ToArray()), ConsoleColor.Green);
        }
    }
}
