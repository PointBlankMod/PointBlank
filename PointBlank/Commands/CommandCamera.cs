using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using ECameraMode = SDG.Unturned.ECameraMode;
using Provider = SDG.Unturned.Provider;

namespace PointBlank.Commands
{
    [Command("Camera", 1)]
    internal class CommandCamera : Command
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "camera",
            "CAMERA"
        };

        public override string Help => "Sets the allowed camera location";

        public override string Usage => Commands[0] + " <first/third/both/vehicle>";

        public override string DefaultPermission => "unturned.commands.server.camera";

        public override bool AllowRuntime => false;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            string pos = args[0].ToLower();
            ECameraMode mode;

            switch (pos)
            {
                case "first":
                    mode = ECameraMode.FIRST;
                    break;
                case "third":
                    mode = ECameraMode.THIRD;
                    break;
                case "vehicle":
                    mode = ECameraMode.VEHICLE;
                    break;
                case "both":
                    mode = ECameraMode.BOTH;
                    break;
                default:
                    mode = ECameraMode.BOTH;
                    break;
            }
            Provider.cameraMode = mode;
        }
    }
}
