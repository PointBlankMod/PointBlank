using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;

namespace PointBlank.Commands
{
    [PointBlankCommand("Experience", 1)]
    internal class CommandExperience : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "xp",
            "experience"
        };

        public override string Help => "Gives the player experience";

        public override string Usage => Commands[0] + " <amount> [player]";

        public override string DefaultPermission => "unturned.commands.admin.experience";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            uint xp;
            UnturnedPlayer player;

            if(!uint.TryParse(args[0], out xp))
            {
                UnturnedChat.SendMessage(executor, "Invalid amount of experience!", ConsoleColor.Red);
                return;
            }
            if(args.Length < 2 || !UnturnedPlayer.TryGetPlayer(args[1], out player))
            {
                if(executor == null)
                {
                    UnturnedChat.SendMessage(executor, "Invalid player!", ConsoleColor.Red);
                    return;
                }

                player = executor;
            }

            player.Player.skills.askAward(xp);
            UnturnedChat.SendMessage(executor, "Gave " + player.PlayerName + " " + xp + " experience", ConsoleColor.Green);
        }
    }
}
