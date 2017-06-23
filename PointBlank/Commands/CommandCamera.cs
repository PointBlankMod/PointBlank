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
    [PointBlankCommand("Camera", 1)]
    internal class CommandCamera : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "Camera"
        };

        public override string Help => Translation.Camera_Help;

        public override string Usage => Commands[0] + Translation.Camera_Usage;

        public override string DefaultPermission => "unturned.commands.server.camera";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.LOADING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            string pos = args[0].ToLower();
            ECameraMode mode;

            switch (pos.ToUpperInvariant()) // I know you like to use Lower, ati, but some characters cannot be converted to invariant as lowercase
            {
                case "FIRST":
                    mode = ECameraMode.FIRST;
                    break;
                case "THIRD":
                    mode = ECameraMode.THIRD;
                    break;
                case "VEHICLE":
                    mode = ECameraMode.VEHICLE;
                    break;
                case "BOTH":
                    mode = ECameraMode.BOTH;
                    break;
                default:
                    mode = ECameraMode.BOTH;
                    break;
            }
            Provider.cameraMode = mode;
            UnturnedChat.SendMessage(executor, string.Format(Translation.Camera_SetTo, mode.ToString()), ConsoleColor.Green);
        }
    }
}
