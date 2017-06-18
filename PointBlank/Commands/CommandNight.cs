using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;

namespace PointBlank.Commands
{
    [PointBlankCommand("Night", 0)]
    internal class CommandNight : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Night"
        };

        public override string Help => "Sets the time to night";

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.admin.night";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            if (Provider.isServer && Level.info.type == ELevelType.ARENA)
            {
                UnturnedChat.SendMessage(executor, "Can't set time on arena!", ConsoleColor.Red);
                return;
            }
            if (Provider.isServer && Level.info.type == ELevelType.HORDE)
            {
                UnturnedChat.SendMessage(executor, "Can't set time on horde!", ConsoleColor.Red);
                return;
            }

            LightingManager.time = (uint)(LightingManager.cycle * (LevelLighting.bias + LevelLighting.transition));
            UnturnedChat.SendMessage(executor, "Time set to night!", ConsoleColor.Green);
        }
    }
}
