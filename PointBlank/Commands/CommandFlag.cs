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
    [PointBlankCommand("Flag", 2)]
    internal class CommandFlag : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Flag"
        };

        public override string Help => Translation.Flag_Help;

        public override string Usage => Commands[0] + Translation.Flag_Usage;

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
                UnturnedChat.SendMessage(executor, Translation.Flag_InvalidFlag, ConsoleColor.Red);
                return;
            }
            if (!short.TryParse(args[1], out value))
            {
                UnturnedChat.SendMessage(executor, Translation.Flag_InvalidValue, ConsoleColor.Red);
                return;
            }
            if (args.Length < 3 || !UnturnedPlayer.TryGetPlayer(args[2], out player))
            {
                if (executor == null)
                {
                    UnturnedChat.SendMessage(executor, Translation.Base_InvalidPlayer, ConsoleColor.Red);
                    return;
                }
                player = executor;
            }

            player.Player.quests.sendSetFlag(flag, value);
            UnturnedChat.SendMessage(executor, string.Format(Translation.Flag_Set, player.PlayerName), ConsoleColor.Green);
        }
    }
}
