using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;

namespace PointBlank.Commands
{
    [PointBlankCommand("Help", 0)]
    internal class CommandHelp : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "help"
        };

        public override string Help => "Shows all commands on the server or information of a specific command";

        public override string Usage => Commands[0] + " [command]";

        public override string DefaultPermission => "unturned.commands.nonadmin.help";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(args.Length > 0)
            {
                PointBlankCommand cmd = CommandManager.Commands.FirstOrDefault(a => a.Commands.Contains(args[0]) && a.Enabled);

                if(cmd == null)
                {
                    UnturnedChat.SendMessage(executor, "No such command found!", ConsoleColor.Red);
                    return;
                }
                UnturnedChat.SendMessage(executor, cmd.Help, ConsoleColor.Green);
                return;
            }

            UnturnedChat.SendMessage(executor, string.Join(",", CommandManager.Commands.Where(a => a.Enabled).Select(a => a.Commands[0]).ToArray()), ConsoleColor.Green);
        }
    }
}
