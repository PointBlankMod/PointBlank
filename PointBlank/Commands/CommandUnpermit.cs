using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using Steamworks;

namespace PointBlank.Commands
{
    [PointBlankCommand("Unpermit", 1)]
    internal class CommandUnpermit : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Unpermit"
        };

        public override string Help => "Unwhitelists a player";

        public override string Usage => Commands[0] + " <player>";

        public override string DefaultPermission => "unturned.commands.admin.unpermit";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(!PlayerTool.tryGetSteamID(args[0], out CSteamID id))
            {
                UnturnedChat.SendMessage(executor, "Invalid player!", ConsoleColor.Red);
                return;
            }

            if (!SteamWhitelist.unwhitelist(id))
            {
                UnturnedChat.SendMessage(executor, id + " has not been whitelisted!", ConsoleColor.Red);
                return;
            }
            UnturnedChat.SendMessage(executor, id + " has been unwhitelisted!", ConsoleColor.Green);
        }
    }
}
