using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Name", 1)]
    internal class CommandName : PointBlankCommand
    {
        #region Info
        private static readonly byte MIN_LENGTH = 5;
        private static readonly byte MAX_LENGTH = 50;
        #endregion

        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Name"
        };

        public override string Help => "Sets the server name";

        public override string Usage => Commands[0] + " <name>";

        public override string DefaultPermission => "unturned.commands.server.name";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            string name = string.Join(" ", args);

            if(name.Length > MAX_LENGTH)
            {
                UnturnedChat.SendMessage(executor, "The name is too long!", ConsoleColor.Red);
                return;
            }
            else if(name.Length < MIN_LENGTH)
            {
                UnturnedChat.SendMessage(executor, "The name is too short!", ConsoleColor.Red);
                return;
            }

            Provider.serverName = name;
            UnturnedChat.SendMessage(executor, "Server name set to " + name);
        }
    }
}
