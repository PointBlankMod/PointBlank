using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Collections;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Quest", 1)]
    internal class CommandQuest : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Quest"
        };

        public override string Help => Translation.Quest_Help;

        public override string Usage => Commands[0] + Translation.Quest_Usage;

        public override string DefaultPermission => "unturned.commands.admin.quest";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if (!ushort.TryParse(args[0], out ushort quest))
            {
                UnturnedChat.SendMessage(executor, Translation.Quest_Invalid, ConsoleColor.Red);
                return;
            }
            if(args.Length < 2 || !UnturnedPlayer.TryGetPlayer(args[1], out UnturnedPlayer ply))
            {
                if(executor == null)
                {
                    UnturnedChat.SendMessage(executor, Translation.Base_InvalidPlayer, ConsoleColor.Red);
                    return;
                }

                ply = executor;
            }

            ply.Player.quests.sendAddQuest(quest);
            UnturnedChat.SendMessage(executor, string.Format(Translation.Quest_Added, quest, ply.PlayerName), ConsoleColor.Green);
        }
    }
}
