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
    [PointBlankCommand("Votify", 6)]
    internal class CommandVotify : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Votify"
        };

        public override string Help => "Sets the voting options";

        public override string Usage => Commands[0] + " <allowed> <pass cooldown> <fail cooldown> <duration> <percentage> <player count>";

        public override string DefaultPermission => "unturned.commands.server.votify";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            ChatManager.voteAllowed = (args[0].ToLower() == "y");

            if(!float.TryParse(args[1], out float passCooldown))
            {
                UnturnedChat.SendMessage(executor, "Invalid pass cooldown!", ConsoleColor.Red);
                return;
            }
            if (!float.TryParse(args[2], out float failCooldown))
            {
                UnturnedChat.SendMessage(executor, "Invalid fail cooldown!", ConsoleColor.Red);
                return;
            }
            if (!float.TryParse(args[3], out float duration))
            {
                UnturnedChat.SendMessage(executor, "Invalid duration!", ConsoleColor.Red);
                return;
            }
            if (!float.TryParse(args[4], out float percentage))
            {
                UnturnedChat.SendMessage(executor, "Invalid percentage!", ConsoleColor.Red);
                return;
            }
            if(!byte.TryParse(args[5], out byte players))
            {
                UnturnedChat.SendMessage(executor, "Invalid player count!", ConsoleColor.Red);
                return;
            }

            ChatManager.voteDuration = duration;
            ChatManager.voteFailCooldown = failCooldown;
            ChatManager.votePassCooldown = passCooldown;
            ChatManager.votePercentage = percentage;
            ChatManager.votePlayers = players;

            UnturnedChat.SendMessage(executor, "Vote settings set!", ConsoleColor.Green);
        }
    }
}
