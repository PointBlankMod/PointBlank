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
    [PointBlankCommand("Permits", 0)]
    internal class CommandPermits : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Permits"
        };

        public override string Help => "Shows whitelisted IDs";

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.admin.permits";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            UnturnedChat.SendMessage(executor, "Permits: " + string.Join(",", SteamWhitelist.list.Select(a => a.steamID.ToString()).ToArray()));
        }
    }
}
