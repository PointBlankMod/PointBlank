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
    [PointBlankCommand("Owner", 1)]
    internal class CommandOwner : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Owner"
        };

        public override string Help => "Sets the server owner";

        public override string Usage => Commands[0] + " <owner>";

        public override string DefaultPermission => "unturned.commands.server.owner";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(!PlayerTool.tryGetSteamID(args[0], out CSteamID id))
            {
                UnturnedChat.SendMessage(executor, "Invalid ID!", ConsoleColor.Red);
                return;
            }

            SteamAdminlist.ownerID = id;
            UnturnedChat.SendMessage(executor, "Server owner set!", ConsoleColor.Green);
        }
    }
}
