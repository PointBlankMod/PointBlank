using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using CommandWindow = SDG.Unturned.CommandWindow;
using UnityEngine;

namespace PointBlank.Commands
{
    [Command("Help", 0)]
    internal class CommandHelp : Command
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "help",
            "HELP"
        };

        public override string Help => "Shows all commands on the server or information of a specific command";

        public override string Usage => "help [command]";

        public override string DefaultPermission => "unturned.commands.nonadmin.help";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(args.Length > 0)
            {
                Command cmd = CommandManager.Commands.FirstOrDefault(a => a.Commands.Contains(args[0]) && a.Enabled);

                if(cmd == null)
                {
                    if (executor == null)
                        CommandWindow.Log("No such command found!", ConsoleColor.Red);
                    else
                        executor.SendMessage("No such command found!", Color.red);
                    return;
                }
                if (executor == null)
                    CommandWindow.Log(cmd.Help, ConsoleColor.Green);
                else
                    executor.SendMessage(cmd.Help, Color.green);
                return;
            }

            if (executor == null)
                CommandWindow.Log(string.Join(",", CommandManager.Commands.Where(a => a.Enabled).Select(a => a.Commands[0]).ToArray()), ConsoleColor.Green);
            else
                executor.SendMessage(string.Join(",", CommandManager.Commands.Where(a => a.Enabled).Select(a => a.Commands[0]).ToArray()), Color.green);
        }
    }
}
