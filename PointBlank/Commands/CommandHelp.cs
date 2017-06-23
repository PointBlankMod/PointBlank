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
    [PointBlankCommand("Help", 0)]
    internal class CommandHelp : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Help"
        };

        public override string Help => Translation.Help_Help;

        public override string Usage => Commands[0] + Translation.Help_Usage;

        public override string DefaultPermission => "unturned.commands.nonadmin.help";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(args.Length > 0)
            {
                PointBlankCommand cmd = CommandManager.Commands.FirstOrDefault(a => a.Commands.FirstOrDefault(b => b.ToLower() == args[0].ToLower()) != null && a.Enabled);

                if(cmd == null)
                {
                    UnturnedChat.SendMessage(executor, Translation.Base_CommandInvalid, ConsoleColor.Red);
                    return;
                }
                UnturnedChat.SendMessage(executor, cmd.Help, ConsoleColor.Green);
                return;
            }

            UnturnedChat.SendMessage(executor, string.Join(",", CommandManager.Commands.Where(a => a.Enabled).Select(a => a.Commands[0]).ToArray()), ConsoleColor.Green);
        }
    }
}
