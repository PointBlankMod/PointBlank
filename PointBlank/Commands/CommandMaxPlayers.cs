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
    [PointBlankCommand("MaxPlayers", 1)]
    internal class CommandMaxPlayers : PointBlankCommand
    {
        #region Info
        public static readonly byte MIN_NUMBER = 1;
        public static readonly byte MAX_NUMBER = 48;
        #endregion

        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "maxplayers"
        };

        public override string Help => "Sets the max amount of players that can join";

        public override string Usage => Commands[0] + " <max players>";

        public override string DefaultPermission => "unturned.commands.server.maxplayers";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(!byte.TryParse(args[0], out byte max))
            {
                UnturnedChat.SendMessage(executor, "Invalid number!", ConsoleColor.Red);
                return;
            }

            if(max > MAX_NUMBER)
            {
                UnturnedChat.SendMessage(executor, "Number can't be higher than " + MAX_NUMBER, ConsoleColor.Red);
                return;
            }
            else if(max < MIN_NUMBER)
            {
                UnturnedChat.SendMessage(executor, "Number can't be lower than " + MIN_NUMBER, ConsoleColor.Red);
                return;
            }

            Provider.maxPlayers = max;
        }
    }
}
