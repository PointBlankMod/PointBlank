using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Server;

namespace PointBlank.Framework.Overrides
{
    internal static class _InteractableVehicle
    {
        [Detour(typeof(InteractableVehicle), "init", BindingFlags.Public | BindingFlags.Instance)]
        public static void init(this InteractableVehicle vehicle)
        {
            ServerEvents.RunVehicleCreated(vehicle);

            DetourManager.CallOriginal(typeof(InteractableVehicle).GetMethod("init"), vehicle, new object[0]);
        }
    }
}
