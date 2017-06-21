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
    [PointBlankCommand("Password", 0)]
    internal class CommandPassword : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Password"
        };

        public override string Help => Translation.Password_Help;

        public override string Usage => Commands[0] + Translation.Password_Usage;

        public override string DefaultPermission => "unturned.commands.server.password";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(args.Length < 1 || args[0].Length < 1)
            {
                Provider.serverPassword = string.Empty;
                UnturnedChat.SendMessage(executor, Translation.Password_Removed, ConsoleColor.Green);
                return;
            }

            Provider.serverPassword = args[0];
            UnturnedChat.SendMessage(executor, string.Format(Translation.Password_Set, args[0]), ConsoleColor.Green);
        }
    }
}
