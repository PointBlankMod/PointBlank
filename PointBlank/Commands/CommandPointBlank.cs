using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Plugins;
using PointBlank.API.Groups;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Collections;
using PointBlank.API.Steam;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("PointBlank", 1)]
    internal class CommandPointBlank : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "pb",
            "PointBlank"
        };

        public override string Help => Translations["PointBlank_Help"];

        public override string Usage => Commands[0] + Translations["PointBlank_Usage"];

        public override string DefaultPermission => "pointblank.commands.admin.pointblank";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if(StringComparer.InvariantCultureIgnoreCase.Compare("reloadsteam", args[0]) == 0)
            {
                SteamGroupManager.Reload();
                UnturnedChat.SendMessage(executor, Translations["PointBlank_ReloadSteam"], ConsoleColor.Green);
            }
            else if(StringComparer.InvariantCultureIgnoreCase.Compare("reloadall", args[0]) == 0)
            {
                SteamGroupManager.Reload();
                UnturnedServer.ReloadPlayers();
                GroupManager.Reload();
                UnturnedChat.SendMessage(executor, Translations["PointBlank_ReloadAll"], ConsoleColor.Green);
            }
            else if(StringComparer.InvariantCultureIgnoreCase.Compare("version", args[0]) == 0)
            {
                UnturnedChat.SendMessage(executor, string.Format(Translations["PointBlank_Version"], PointBlankInfo.Version), ConsoleColor.Green);
            }
            else if(StringComparer.InvariantCultureIgnoreCase.Compare("restartplugins", args[0]) == 0)
            {
                PluginManager.Reload();
                UnturnedChat.SendMessage(executor, Translations["PointBlank_RestartPlugins"], ConsoleColor.Green);
            }
            else
            {
                UnturnedChat.SendMessage(executor, Translations["PointBlank_Invalid"], ConsoleColor.Red);
            }
        }
    }
}
