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
    [PointBlankCommand("Welcome", 1)]
    internal class CommandWelcome : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Welcome"
        };

        public override string Help => "Sets the server welcome message";

        public override string Usage => Commands[0] + " <message>";

        public override string DefaultPermission => "unturned.commands.server.welcome";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            ChatManager.welcomeText = string.Join(" ", args);
            UnturnedChat.SendMessage(executor, "Welcome message set!", ConsoleColor.Green);
        }
    }
}
