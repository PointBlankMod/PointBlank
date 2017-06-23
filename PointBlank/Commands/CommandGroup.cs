using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Groups;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Player;
using UnityEngine;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Group", 1)]
    internal class CommandGroup : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Group"
        };

        public override string Help => Translation.Group_Help;

        public override string Usage => Commands[0] + string.Format(Translation.Group_Usage, Translation.Group_Commands_Help);

        public override string DefaultPermission => "pointblank.commands.admin.group";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(args[0].ToLower() == Translation.Group_Commands_List.ToLower())
            {
                UnturnedChat.SendMessage(executor, string.Join(",", GroupManager.Groups.Select(a => a.Name).ToArray()), ConsoleColor.Green);
            }
            else if(args[0].ToLower() == Translation.Group_Commands_IDs.ToLower())
            {
                UnturnedChat.SendMessage(executor, string.Join(",", GroupManager.Groups.Select(a => a.ID).ToArray()), ConsoleColor.Green);
            }
            else if(args[0].ToLower() == Translation.Group_Commands_Help.ToLower())
            {
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translation.Group_Usage_Add, Translation.Group_Commands_Add), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translation.Group_Usage_IDs, Translation.Group_Commands_IDs), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translation.Group_Usage_List, Translation.Group_Commands_List), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translation.Group_Usage_Permissions, Translation.Group_Commands_Permissions), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translation.Group_Usage_Remove, Translation.Group_Commands_Remove), ConsoleColor.Green);
            }
            else if(args[0].ToLower() == Translation.Group_Commands_Permissions.ToLower())
            {
                Permissions(executor, args);
            }
            else if(args[0].ToLower() == Translation.Group_Commands_Add.ToLower())
            {
                Add(executor, args);
            }
            else if(args[0].ToLower() == Translation.Group_Commands_Remove.ToLower())
            {
                Remove(executor, args);
            }
        }

        #region Funtions
        private void Permissions(UnturnedPlayer executor, string[] args)
        {
            if(args.Length < 2)
            {
                UnturnedChat.SendMessage(executor, Translation.Base_NotEnoughArgs, ConsoleColor.Red);
                return;
            }
            Group group = Group.Find(args[1]);
            if (group == null)
            {
                UnturnedChat.SendMessage(executor, Translation.Group_NotFound, ConsoleColor.Red);
                return;
            }

            UnturnedChat.SendMessage(executor, string.Join(",", group.Permissions), ConsoleColor.Green);
        }

        private void Add(UnturnedPlayer executor, string[] args)
        {
            if(args.Length < 3)
            {
                UnturnedChat.SendMessage(executor, Translation.Base_NotEnoughArgs, ConsoleColor.Red);
                return;
            }
            if (Group.Exists(args[1]))
            {
                UnturnedChat.SendMessage(executor, Translation.Group_Exists, ConsoleColor.Red);
                return;
            }

            GroupManager.AddGroup(args[1], args[2], false, -1, Color.clear);
            UnturnedChat.SendMessage(executor, Translation.Group_Added, ConsoleColor.Green);
        }

        private void Remove(UnturnedPlayer executor, string[] args)
        {
            if(args.Length < 2)
            {
                UnturnedChat.SendMessage(executor, Translation.Base_NotEnoughArgs, ConsoleColor.Red);
                return;
            }
            Group group = Group.Find(args[1]);
            if (group == null)
            {
                UnturnedChat.SendMessage(executor, Translation.Group_NotFound, ConsoleColor.Red);
                return;
            }

            GroupManager.RemoveGroup(group);
            UnturnedChat.SendMessage(executor, Translation.Group_Removed, ConsoleColor.Green);
        }
        #endregion
    }
}
