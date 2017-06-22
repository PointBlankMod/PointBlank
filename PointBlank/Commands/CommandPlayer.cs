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
    [PointBlankCommand("Player", 1)]
    internal class CommandPlayer : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Player"
        };

        public override string Help => Translation.Player_Help;

        public override string Usage => Commands[0] + Translation.Player_Usage;

        public override string DefaultPermission => "pointblank.commands.admin.player";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(args[0].ToLower() == "help")
            {
                UnturnedChat.SendMessage(executor, Commands[0] + Translation.Player_Group, ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + Translation.Player_Group_Add, ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + Translation.Player_Group_Remove, ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + Translation.Player_Permissions, ConsoleColor.Green);
            }
            else if(args[0].ToLower() == "permissions")
            {
                Permissions(executor, args);
            }
            else if(args[0].ToLower() == "groups")
            {
                Groups(executor, args);
            }
        }

        #region Functions
        private void Permissions(UnturnedPlayer executor, string[] args)
        {
            if (args.Length < 2)
            {
                UnturnedChat.SendMessage(executor, Translation.Base_NotEnoughArgs, ConsoleColor.Red);
                return;
            }
            if(!UnturnedPlayer.TryGetPlayer(args[1], out UnturnedPlayer ply))
            {
                UnturnedChat.SendMessage(executor, Translation.Base_InvalidPlayer, ConsoleColor.Red);
                return;
            }

            UnturnedChat.SendMessage(executor, string.Join(",", ply.Permissions), ConsoleColor.Green);
        }

        private void Groups(UnturnedPlayer executor, string[] args)
        {

        }
        #endregion
    }
}
