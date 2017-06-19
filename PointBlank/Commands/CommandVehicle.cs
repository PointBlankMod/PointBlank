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
    [PointBlankCommand("Vehicle", 1)]
    internal class CommandVehicle : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "v",
            "Vehicle"
        };

        public override string Help => "Spawns a vehicle";

        public override string Usage => Commands[0] + " <vehicle> [player]";

        public override string DefaultPermission => "unturned.commands.admin.vehicle";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            UnturnedPlayer ply;
            VehicleAsset vehicle;
            ushort id;

            if(!ushort.TryParse(args[0], out id))
            {
                VehicleAsset[] vehicles = Assets.find(EAssetType.VEHICLE) as VehicleAsset[];

                vehicle = vehicles.Where(a => a != null).OrderBy(a => a.vehicleName.Length).FirstOrDefault(a => a.vehicleName.ToLower().Contains(args[0].ToLower()));
            }
            else
            {
                vehicle = Assets.find(EAssetType.VEHICLE, id) as VehicleAsset;
            }
            if(vehicle == null)
            {
                UnturnedChat.SendMessage(executor, "Could not find vehicle!", ConsoleColor.Red);
                return;
            }

            if (args.Length < 2 || UnturnedPlayer.TryGetPlayer(args[1], out ply))
            {
                if (executor == null)
                {
                    UnturnedChat.SendMessage(executor, "Player not found!", ConsoleColor.Red);
                    return;
                }

                ply = executor;
            }
            if(!VehicleTool.giveVehicle(ply.Player, vehicle.id))
            {
                UnturnedChat.SendMessage(executor, "Could not spawn vehicle!", ConsoleColor.Red);
                return;
            }
            UnturnedChat.SendMessage(executor, vehicle.vehicleName + " has been spawned!", ConsoleColor.Green);
        }
    }
}
