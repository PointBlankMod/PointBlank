using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using Steamworks;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Unban", 1)]
    internal class CommandUnban : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Unban"
        };

        public override string Help => Translation.Unban_Help;

        public override string Usage => Commands[0] + Translation.Unban_Usage;

        public override string DefaultPermission => "unturned.commands.admin.unban";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(!PlayerTool.tryGetSteamID(args[0], out CSteamID id))
            {
                UnturnedChat.SendMessage(executor, Translation.Base_InvalidPlayer, ConsoleColor.Red);
                return;
            }

            if (!SteamBlacklist.unban(id))
            {
                UnturnedChat.SendMessage(executor, string.Format(Translation.Unban_NotBanned, id), ConsoleColor.Red);
                return;
            }
            UnturnedChat.SendMessage(executor, string.Format(Translation.Unban_Unban, id), ConsoleColor.Green);
        }
    }
}
