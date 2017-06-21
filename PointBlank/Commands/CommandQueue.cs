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

        public override string Help => Translation.Queue_Help;

        public override string Usage => Commands[0] + Translation.Queue_Usage;

        public override string DefaultPermission => "unturned.commands.server.queue";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(!byte.TryParse(args[0], out byte queue))
            {
                UnturnedChat.SendMessage(executor, Translation.Queue_Invalid, ConsoleColor.Red);
                return;
            }
            if(queue > MAX_NUMBER)
            {
                UnturnedChat.SendMessage(executor, string.Format(Translation.Queue_TooHigh, MAX_NUMBER), ConsoleColor.Red);
                return;
            }

            Provider.queueSize = queue;
            UnturnedChat.SendMessage(executor, string.Format(Translation.Queue_Set, queue), ConsoleColor.Green);
        }
    }
}
