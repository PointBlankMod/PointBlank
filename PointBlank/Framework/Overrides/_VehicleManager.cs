using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UnityEngine;
using Steamworks;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Server;
using PointBlank.API;

namespace PointBlank.Framework.Overrides
{
    internal class _VehicleManager
    {
        [SteamCall]
        [Detour(typeof(VehicleManager), "tellVehicleDestroy", BindingFlags.Public | BindingFlags.Instance)]
        public void tellVehicleDestroy(CSteamID steamID, uint instanceID)
        {
            if (VehicleManager.instance.channel.checkServer(steamID))
            {
                InteractableVehicle vehicle = VehicleManager.vehicles.FirstOrDefault(a => a.instanceID == instanceID);

                if (vehicle == null)
                    return;

                ServerEvents.RunVehicleRemoved(vehicle);
            }

            DetourManager.CallOriginal(typeof(VehicleManager).GetMethod("tellVehicleDestroy", BindingFlags.Instance | BindingFlags.Public), VehicleManager.instance, steamID, instanceID);
        }
    }
}
