using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;

namespace PointBlank.Commands
{
    [PointBlankCommand("Flag", 2)]
    internal class CommandFlag : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "flag"
        };

        public override string Help => "Sets the quest flag of the player";

        public override string Usage => Commands[0] + " <flag> <value> [player]";

        public override string DefaultPermission => "unturned.commands.admin.flag";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            ushort flag;
            short value;
            UnturnedPlayer player;

            if (!ushort.TryParse(args[0], out flag))
            {
                UnturnedChat.SendMessage(executor, "Invalid flag ID!", ConsoleColor.Red);
                return;
            }
            if (!short.TryParse(args[1], out value))
            {
                UnturnedChat.SendMessage(executor, "Invalid value ID!", ConsoleColor.Red);
                return;
            }
            if (args.Length < 3 || !UnturnedPlayer.TryGetPlayer(args[2], out player))
            {
                if (executor == null)
                {
                    UnturnedChat.SendMessage(executor, "Invalid player!", ConsoleColor.Red);
                    return;
                }
                player = executor;
            }

            player.Player.quests.sendSetFlag(flag, value);
            UnturnedChat.SendMessage(executor, "The quest has been set for " + player.PlayerName, ConsoleColor.Green);
        }
    }
}
