using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Collections;
using PBGroup = PointBlank.API.Groups.Group;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Permissions", 1)]
    internal class CommandPermissions : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "p",
            "Permissions"
        };

        public override string Help => Translations["Permissions_Help"];

        public override string Usage => string.Format(Translations["Permissions_Usage"], Translations["Permissions_Commands_Help"]);

        public override string DefaultPermission => "pointblank.commands.admin.permissions";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(StringComparer.InvariantCultureIgnoreCase.Compare(args[0], Translations["Permissions_Commands_Help"]) == 0)
            {
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translations["Permissions_Group"], Translations["Permissions_Commands_Group"]), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translations["Permissions_Group_Modify"], Translations["Permissions_Commands_Group"], Translations["Permissions_Commands_Add"]), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translations["Permissions_Group_Modify"], Translations["Permissions_Commands_Group"], Translations["Permissions_Commands_Remove"]), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translations["Permissions_Player"], Translations["Permissions_Commands_Player"]), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translations["Permissions_Player_Modify"], Translations["Permissions_Commands_Player"], Translations["Permissions_Commands_Add"]), ConsoleColor.Green);
                UnturnedChat.SendMessage(executor, Commands[0] + string.Format(Translations["Permissions_Player_Modify"], Translations["Permissions_Commands_Player"], Translations["Permissions_Commands_Remove"]), ConsoleColor.Green);
            }
            else if(StringComparer.InvariantCultureIgnoreCase.Compare(args[0], Translations["Permissions_Commands_Group"]) == 0)
            {
                Group(executor, args);
            }
            else if(StringComparer.InvariantCultureIgnoreCase.Compare(args[0], Translations["Permissions_Commands_Player"]) == 0)
            {
                Player(executor, args);
            }
        }

        #region Functions
        private void Player(UnturnedPlayer executor, string[] args)
        {
            if(args.Length < 4)
            {
                UnturnedChat.SendMessage(executor, Translations["Base_NotEnoughArgs"], ConsoleColor.Red);
                return;
            }
            if(!UnturnedPlayer.TryGetPlayer(args[1], out UnturnedPlayer player))
            {
                UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                return;
            }
            
            if(StringComparer.InvariantCultureIgnoreCase.Compare(args[2], Translations["Permissions_Commands_Add"]) == 0)
            {
                player.AddPermission(args[3]);
                UnturnedChat.SendMessage(executor, string.Format(Translations["Permissions_Add_Success"], args[3], player.PlayerName), ConsoleColor.Green);
            }
            else if(StringComparer.InvariantCultureIgnoreCase.Compare(args[2], Translations["Permissions_Commands_Remove"]) == 0)
            {
                player.RemovePermission(args[3]);
                UnturnedChat.SendMessage(executor, string.Format(Translations["Permissions_Remove_Success"], args[3], player.PlayerName), ConsoleColor.Green);
            }
        }

        private void Group(UnturnedPlayer executor, string[] args)
        {
            if (args.Length < 4)
            {
                UnturnedChat.SendMessage(executor, Translations["Base_NotEnoughArgs"], ConsoleColor.Red);
                return;
            }
            if(!PBGroup.TryFindGroup(args[1], out PBGroup group))
            {
                UnturnedChat.SendMessage(executor, Translations["Base_InvalidGroup"], ConsoleColor.Red);
                return;
            }

            if (StringComparer.InvariantCultureIgnoreCase.Compare(args[2], Translations["Permissions_Commands_Add"]) == 0)
            {
                group.AddPermission(args[3]);
                UnturnedChat.SendMessage(executor, string.Format(Translations["Permissions_Add_Success"], args[3], group.Name), ConsoleColor.Green);
            }
            else if (StringComparer.InvariantCultureIgnoreCase.Compare(args[2], Translations["Permissions_Commands_Remove"]) == 0)
            {
                group.RemovePermission(args[3]);
                UnturnedChat.SendMessage(executor, string.Format(Translations["Permissions_Remove_Success"], args[3], group.Name), ConsoleColor.Green);
            }
        }
        #endregion
    }
}
