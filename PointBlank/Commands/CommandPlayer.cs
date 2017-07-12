using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Groups;
using PointBlank.API.Collections;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Player", 1)]
    internal class CommandPlayer : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Player"
        };

        public override string Help => Translations["Player_Help"];

        public override string Usage => Commands[0] + string.Format(Translations["Player_Usage"], Translations["Player_Commands_Help"]);

        public override string DefaultPermission => "pointblank.commands.admin.player";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if (StringComparer.InvariantCultureIgnoreCase.Compare(args[0], Translations["Player_Commands_Help"]) == 0)
            {
                UnturnedChat.SendMessage(executor, Commands[0] + " " + Translations["Player_Commands_Reload"], ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translations["Player_Group"], Translations["Player_Commands_Groups"]), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translations["Player_Group_Modify"], Translations["Player_Commands_Groups"], Translations["Player_Commands_Groups_Add"]), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translations["Player_Group_Modify"], Translations["Player_Commands_Groups"], Translations["Player_Commands_Groups_Remove"]), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translations["Player_Permissions"], Translations["Player_Commands_Permissions"]), ConsoleColor.Green);
            }
            else if (StringComparer.InvariantCultureIgnoreCase.Compare(args[0], Translations["Player_Commands_Permissions"]) == 0)
            {
                Permissions(executor, args);
            }
            else if (StringComparer.InvariantCultureIgnoreCase.Compare(args[0], Translations["Player_Commands_Groups"]) == 0)
            {
                Groups(executor, args);
            }
            else if(StringComparer.InvariantCultureIgnoreCase.Compare(args[0], Translations["Player_Commands_Reload"]) == 0)
            {
                UnturnedServer.ReloadPlayers();
                UnturnedChat.SendMessage(executor, Translations["Player_Reloaded"], ConsoleColor.Green);
            }
        }

        #region Functions
        private void Permissions(UnturnedPlayer executor, string[] args)
        {
            if (args.Length < 2)
            {
                UnturnedChat.SendMessage(executor, Translations["Base_NotEnoughArgs"], ConsoleColor.Red);
                return;
            }
            if(!UnturnedPlayer.TryGetPlayer(args[1], out UnturnedPlayer ply))
            {
                UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                return;
            }

            UnturnedChat.SendMessage(executor, string.Join(",", ply.Permissions), ConsoleColor.Green);
        }

        private void Groups(UnturnedPlayer executor, string[] args)
        {
            if(args.Length < 2)
            {
                UnturnedChat.SendMessage(executor, Translations["Base_NotEnoughArgs"], ConsoleColor.Red);
                return;
            }
            if (!UnturnedPlayer.TryGetPlayer(args[1], out UnturnedPlayer ply))
            {
                UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                return;
            }
            if(args.Length == 2)
            {
                UnturnedChat.SendMessage(executor, string.Join(",", ply.Groups.Select(a => a.ID).ToArray()), ConsoleColor.Green);
                return;
            }

            if (args.Length < 4)
            {
                UnturnedChat.SendMessage(executor, Translations["Base_NotEnoughArgs"], ConsoleColor.Red);
                return;
            }
            Group group = Group.Find(args[3]);
            if(group == null)
            {
                UnturnedChat.SendMessage(executor, Translations["Player_Group_Invalid"], ConsoleColor.Red);
                return;
            }

            if(StringComparer.InvariantCultureIgnoreCase.Compare(args[2], Translations["Player_Commands_Groups_Add"]) == 0)
            {
                ply.AddGroup(group);
                UnturnedChat.SendMessage(executor, Translations["Player_Group_Added"], ConsoleColor.Green);
            }
            else if(StringComparer.InvariantCultureIgnoreCase.Compare(args[2], Translations["Player_Commands_Groups_Remove"]) == 0)
            {
                ply.RemoveGroup(group);
                UnturnedChat.SendMessage(executor, Translations["Player_Group_Removed"], ConsoleColor.Green);
            }
        }
        #endregion
    }
}
