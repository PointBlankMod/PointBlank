using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;

namespace PointBlank.Commands
{
    [PointBlankCommand("Reputation", 1)]
    internal class CommandReputation : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Rep",
            "Reputation"
        };

        public override string Help => "Gives a player reputation";

        public override string Usage => Commands[0] + " <reputation> [player]";

        public override string DefaultPermission => "unturned.commands.admin.reputation";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            UnturnedPlayer ply;
            int rep;

            if(!int.TryParse(args[0], out rep))
            {
                UnturnedChat.SendMessage(executor, "Invalid reputation amount!", ConsoleColor.Red);
                return;
            }
            if(args.Length < 2 || !UnturnedPlayer.TryGetPlayer(args[1], out ply))
            {
                if(executor == null)
                {
                    UnturnedChat.SendMessage(executor, "Invalid player!", ConsoleColor.Red);
                    return;
                }

                ply = executor;
            }

            ply.Player.skills.askRep(rep);
            UnturnedChat.SendMessage(executor, "Giving " + ply.PlayerName + " " + rep + " reputation", ConsoleColor.Green);
        }
    }
}
