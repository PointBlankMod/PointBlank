using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using Provider = SDG.Unturned.Provider;
using Level = SDG.Unturned.Level;
using ELevelType = SDG.Unturned.ELevelType;
using LightingManager = SDG.Unturned.LightingManager;
using LevelLighting = SDG.Unturned.LevelLighting;

namespace PointBlank.Commands
{
    [Command("Day", 0)]
    internal class CommandDay : Command
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "day",
            "Day",
            "DAY"
        };

        public override string Help => "Sets the time to day";

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.admin.day";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if (Provider.isServer && Level.info.type == ELevelType.ARENA)
            {
                ChatManager.SendMessage(executor, "Can't set time on arena!", ConsoleColor.Red);
                return;
            }
            if (Provider.isServer && Level.info.type == ELevelType.HORDE)
            {
                ChatManager.SendMessage(executor, "Can't set time on horde!", ConsoleColor.Red);
                return;
            }

            LightingManager.time = (uint)(LightingManager.cycle * LevelLighting.transition);
        }
    }
}
