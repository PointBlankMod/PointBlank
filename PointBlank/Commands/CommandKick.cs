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
    [PointBlankCommand("Kick", 1)]
    internal class CommandKick : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Kick"
        };

        public override string Help => Translation.Kick_Help;

        public override string Usage => Commands[0] + Translation.Kick_Usage;

        public override string DefaultPermission => "unturned.commands.admin.kick";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            string reason;

            if(!UnturnedPlayer.TryGetPlayer(args[0], out UnturnedPlayer ply))
            {
                UnturnedChat.SendMessage(executor, Translation.Base_InvalidPlayer, ConsoleColor.Red);
                return;
            }
            if (args.Length < 2)
                reason = Translation.Kick_Reason;
            else
                reason = args[1];

            Provider.kick(ply.SteamID, reason);
            UnturnedChat.SendMessage(executor, string.Format(Translation.Kick_Kicked, ply.PlayerName), ConsoleColor.Green);
        }
    }
}
