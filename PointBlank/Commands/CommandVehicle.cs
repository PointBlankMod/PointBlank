using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Vehicle", 1)]
    internal class CommandVehicle : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "v",
            "Vehicle"
        };

        public override string Help => Translations["Vehicle_Help"];

        public override string Usage => Commands[0] + Translations["Vehicle_Usage"];

        public override string DefaultPermission => "unturned.commands.admin.vehicle";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            VehicleAsset vehicle;

            if(!ushort.TryParse(args[0], out ushort id))
            {
                VehicleAsset[] vehicles = Assets.find(EAssetType.VEHICLE) as VehicleAsset[];

                vehicle = vehicles.Where(a => a != null).OrderBy(a => a.vehicleName.Length).FirstOrDefault(a => a.vehicleName.ToLower().Contains(args[0].ToLower()));
            }
            else
                vehicle = Assets.find(EAssetType.VEHICLE, id) as VehicleAsset;
            if(vehicle == null)
            {
                UnturnedChat.SendMessage(executor, Translations["Vehicle_Invalid"], ConsoleColor.Red);
                return;
            }

            if (args.Length < 2 || UnturnedPlayer.TryGetPlayer(args[1], out UnturnedPlayer ply))
            {
                if (executor == null)
                {
                    UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                    return;
                }

                ply = executor;
            }
            if(!VehicleTool.giveVehicle(ply.Player, vehicle.id))
            {
                UnturnedChat.SendMessage(executor, string.Format(Translations["Vehicle_Fail"], vehicle.vehicleName), ConsoleColor.Red);
                return;
            }
            UnturnedChat.SendMessage(executor, string.Format(Translations["Vehicle_Spawn"], vehicle.vehicleName), ConsoleColor.Green);
        }
    }
}
