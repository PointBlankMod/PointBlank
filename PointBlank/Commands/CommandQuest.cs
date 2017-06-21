using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Quest", 1)]
    internal class CommandQuest : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Quest"
        };

        public override string Help => "Adds a quest to a player";

        public override string Usage => Commands[0] + " <quest> [player]";

        public override string DefaultPermission => "unturned.commands.admin.quest";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            UnturnedPlayer ply;
            ushort quest;

            if (!ushort.TryParse(args[0], out quest))
            {
                UnturnedChat.SendMessage(executor, "Invalid quest ID!", ConsoleColor.Red);
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

            ply.Player.quests.sendAddQuest(quest);
            UnturnedChat.SendMessage(executor, "Quest " + quest + " has been added to " + ply.PlayerName, ConsoleColor.Green);
        }
    }
}
