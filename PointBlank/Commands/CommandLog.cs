using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;

namespace PointBlank.Commands
{
    [PointBlankCommand("Log", 4)]
    internal class CommandLog : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "log"
        };

        public override string Help => "Sets unturned logging";

        public override string Usage => Commands[0] + " <chat(y/n)> <join/leave(y/n)> <deaths(y/n)> <anticheat(y/n)>";

        public override string DefaultPermission => "unturned.commands.server.log";

        public override EAllowedCaller AllowedCaller => EAllowedCaller.SERVER;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            CommandWindow.shouldLogChat = (args[0].ToLower() == "y");
            CommandWindow.shouldLogJoinLeave = (args[1].ToLower() == "y");
            CommandWindow.shouldLogDeaths = (args[2].ToLower() == "y");
            CommandWindow.shouldLogAnticheat = (args[3].ToLower() == "y");
            CommandWindow.Log("Logging changed!", ConsoleColor.Green);
        }
    }
}
