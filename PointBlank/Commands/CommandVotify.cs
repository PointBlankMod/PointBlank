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
    [PointBlankCommand("Votify", 6)]
    internal class CommandVotify : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Votify"
        };

        public override string Help => Translation.Votify_Help;

        public override string Usage => Commands[0] + Translation.Votify_Usage;

        public override string DefaultPermission => "unturned.commands.server.votify";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            ChatManager.voteAllowed = (StringComparer.InvariantCultureIgnoreCase.Compare("y", args[0]) == 0);

            if(!float.TryParse(args[1], out float passCooldown))
            {
                UnturnedChat.SendMessage(executor, Translation.Votify_Pass, ConsoleColor.Red);
                return;
            }
            if (!float.TryParse(args[2], out float failCooldown))
            {
                UnturnedChat.SendMessage(executor, Translation.Votify_Fail, ConsoleColor.Red);
                return;
            }
            if (!float.TryParse(args[3], out float duration))
            {
                UnturnedChat.SendMessage(executor, Translation.Votify_Duration, ConsoleColor.Red);
                return;
            }
            if (!float.TryParse(args[4], out float percentage))
            {
                UnturnedChat.SendMessage(executor, Translation.Votify_Percentage, ConsoleColor.Red);
                return;
            }
            if(!byte.TryParse(args[5], out byte players))
            {
                UnturnedChat.SendMessage(executor, Translation.Votify_Count, ConsoleColor.Red);
                return;
            }

            ChatManager.voteDuration = duration;
            ChatManager.voteFailCooldown = failCooldown;
            ChatManager.votePassCooldown = passCooldown;
            ChatManager.votePercentage = percentage;
            ChatManager.votePlayers = players;

            UnturnedChat.SendMessage(executor, Translation.Votify_Set, ConsoleColor.Green);
        }
    }
}
