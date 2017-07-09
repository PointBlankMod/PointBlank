using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Storm", 0)]
    internal class CommandStorm : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Storm"
        };

        public override string Help => Translation.Storm_Help;

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.admin.storm";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            switch (LevelLighting.rainyness)
            {
                case ELightingRain.NONE:
                    LightingManager.rainFrequency = 0u;
                    break;
                case ELightingRain.DRIZZLE:
                    LightingManager.rainDuration = 0u;
                    break;
            }
            UnturnedChat.SendMessage(executor, Translation.Storm_Change, ConsoleColor.Green);
        }
    }
}
