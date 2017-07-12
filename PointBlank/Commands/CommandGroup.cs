using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Groups;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Player;
using UnityEngine;
using PointBlank.API.Collections;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Group", 1)]
    internal class CommandGroup : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Group"
        };

        public override string Help => Translations["Group_Help"];

        public override string Usage => Commands[0] + string.Format(Translations["Group_Usage"], Translations["Group_Commands_Help"]);

        public override string DefaultPermission => "pointblank.commands.admin.group";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(StringComparer.InvariantCultureIgnoreCase.Compare(Translations["Group_Commands_List"], args[0]) == 0)
            {
                UnturnedChat.SendMessage(executor, string.Join(",", GroupManager.Groups.Select(a => a.Name).ToArray()), ConsoleColor.Green);
            }
            else if(StringComparer.InvariantCultureIgnoreCase.Compare(Translations["Group_Commands_IDs"], args[0]) == 0)
            {
                UnturnedChat.SendMessage(executor, string.Join(",", GroupManager.Groups.Select(a => a.ID).ToArray()), ConsoleColor.Green);
            }
            else if(StringComparer.InvariantCultureIgnoreCase.Compare(Translations["Group_Commands_Help"], args[0]) == 0)
            {
                UnturnedChat.SendMessage(executor, Commands[0] + " " + Translations["Group_Commands_Reload"], ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translations["Group_Usage_Add"], Translations["Group_Commands_Add"]), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translations["Group_Usage_IDs"], Translations["Group_Commands_IDs"]), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translations["Group_Usage_List"], Translations["Group_Commands_List"]), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translations["Group_Usage_Permissions"], Translations["Group_Commands_Permissions"]), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translations["Group_Usage_Remove"], Translations["Group_Commands_Remove"]), ConsoleColor.Green);
            }
            else if(StringComparer.InvariantCultureIgnoreCase.Compare(Translations["Group_Commands_Permissions"], args[0]) == 0)
            {
                Permissions(executor, args);
            }
            else if(StringComparer.InvariantCultureIgnoreCase.Compare(Translations["Group_Commands_Add"], args[0]) == 0)
            {
                Add(executor, args);
            }
            else if(StringComparer.InvariantCultureIgnoreCase.Compare(Translations["Group_Commands_Remove"], args[0]) == 0)
            {
                Remove(executor, args);
            }
            else if(StringComparer.InvariantCultureIgnoreCase.Compare(Translations["Group_Commands_Reload"], args[0]) == 0)
            {
                GroupManager.Reload();
                UnturnedChat.SendMessage(executor, Translations["Group_Reloaded"], ConsoleColor.Green);
            }
        }

        #region Funtions
        private void Permissions(UnturnedPlayer executor, string[] args)
        {
            if(args.Length < 2)
            {
                UnturnedChat.SendMessage(executor, Translations["Base_NotEnoughArgs"], ConsoleColor.Red);
                return;
            }
            Group group = Group.Find(args[1]);
            if (group == null)
            {
                UnturnedChat.SendMessage(executor, Translations["Group_NotFound"], ConsoleColor.Red);
                return;
            }

            UnturnedChat.SendMessage(executor, string.Join(",", group.Permissions), ConsoleColor.Green);
        }

        private void Add(UnturnedPlayer executor, string[] args)
        {
            if(args.Length < 3)
            {
                UnturnedChat.SendMessage(executor, Translations["Base_NotEnoughArgs"], ConsoleColor.Red);
                return;
            }
            if (Group.Exists(args[1]))
            {
                UnturnedChat.SendMessage(executor, Translations["Group_Exists"], ConsoleColor.Red);
                return;
            }

            GroupManager.AddGroup(args[1], args[2], false, -1, Color.clear);
            UnturnedChat.SendMessage(executor, Translations["Group_Added"], ConsoleColor.Green);
        }

        private void Remove(UnturnedPlayer executor, string[] args)
        {
            if(args.Length < 2)
            {
                UnturnedChat.SendMessage(executor, Translations["Base_NotEnoughArgs"], ConsoleColor.Red);
                return;
            }
            Group group = Group.Find(args[1]);
            if (group == null)
            {
                UnturnedChat.SendMessage(executor, Translations["Group_NotFound"], ConsoleColor.Red);
                return;
            }

            GroupManager.RemoveGroup(group);
            UnturnedChat.SendMessage(executor, Translations["Group_Removed"], ConsoleColor.Green);
        }
        #endregion
    }
}
