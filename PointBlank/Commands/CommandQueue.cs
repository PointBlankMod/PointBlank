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
    [PointBlankCommand("Queue", 1)]
    internal class CommandQueue : PointBlankCommand
    {
        #region Info
        private static readonly byte MAX_NUMBER = 64;
        #endregion

        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Queue"
        };

        public override string Help => "Sets the maximum queue";

        public override string Usage => Commands[0] + " <queue>";

        public override string DefaultPermission => "unturned.commands.server.queue";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(!byte.TryParse(args[0], out byte queue))
            {
                UnturnedChat.SendMessage(executor, "Invalid queue number!", ConsoleColor.Red);
                return;
            }
            if(queue > MAX_NUMBER)
            {
                UnturnedChat.SendMessage(executor, "Queue too high!", ConsoleColor.Red);
                return;
            }

            Provider.queueSize = queue;
            UnturnedChat.SendMessage(executor, "Queue set to " + queue, ConsoleColor.Green);
        }
    }
}
