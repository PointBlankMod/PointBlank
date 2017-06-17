﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using CM = SDG.Unturned.ChatManager;

namespace PointBlank.Commands
{
    [Command("Chatrate", 1)]
    internal class CommandChatrate : Command
    {
        #region Info
        private static readonly float MIN_NUMBER = 1f;
        private static readonly float MAX_NUMBER = 60f;
        #endregion

        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "chatrate",
            "CHATRATE"
        };

        public override string Help => "Sets the chat rate";

        public override string Usage => Commands[0] + " <rate>";

        public override string DefaultPermission => "unturned.commands.admin.chatrate";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(!float.TryParse(args[0], out float rate))
            {
                ChatManager.SendMessage(executor, "Invalid chat rate number!", ConsoleColor.Red);
                return;
            }
            if(rate < MIN_NUMBER)
            {
                ChatManager.SendMessage(executor, "The chat rate can't be lower than " + MIN_NUMBER.ToString(), ConsoleColor.Red);
                return;
            }
            else if(rate > MAX_NUMBER)
            {
                ChatManager.SendMessage(executor, "The chat rate can't be higher than " + MAX_NUMBER.ToString(), ConsoleColor.Red);
                return;
            }

            CM.chatrate = rate;
            ChatManager.SendMessage(executor, "Chat rate set to " + rate.ToString(), ConsoleColor.Green);
        }
    }
}
