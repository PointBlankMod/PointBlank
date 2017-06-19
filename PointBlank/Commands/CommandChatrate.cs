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
    [PointBlankCommand("Chatrate", 1)]
    internal class CommandChatrate : PointBlankCommand
    {
        #region Info
        private static readonly float MIN_NUMBER = 1f;
        private static readonly float MAX_NUMBER = 60f;
        #endregion

        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Chatrate"
        };

        public override string Help => Translation.Chatrate_Help;

        public override string Usage => Commands[0] + Translation.Chatrate_Usage;

        public override string DefaultPermission => "unturned.commands.admin.chatrate";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(!float.TryParse(args[0], out float rate))
            {
                UnturnedChat.SendMessage(executor, Translation.Chatrate_Invalid, ConsoleColor.Red);
                return;
            }
            if(rate < MIN_NUMBER)
            {
                UnturnedChat.SendMessage(executor, string.Format(Translation.Chatrate_TooLow, MIN_NUMBER), ConsoleColor.Red);
                return;
            }
            else if(rate > MAX_NUMBER)
            {
                UnturnedChat.SendMessage(executor, string.Format(Translation.Chatrate_TooHigh, MAX_NUMBER), ConsoleColor.Red);
                return;
            }

            ChatManager.chatrate = rate;
            UnturnedChat.SendMessage(executor, string.Format(Translation.Chatrate_SetTo, rate), ConsoleColor.Green);
        }
    }
}
