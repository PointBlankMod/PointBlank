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

namespace PointBlank.Framework.Overrides
{
    internal class _VehicleManager
    {
        [Detour(typeof(VehicleManager), "respawnVehicles", BindingFlags.NonPublic | BindingFlags.Instance)]
        private void respawnVehicles()
        {
            FieldInfo frespawnVehicleIndex = typeof(VehicleManager).GetField("respawnVehicleIndex", BindingFlags.NonPublic | BindingFlags.Static);
            FieldInfo finstanceCount = typeof(VehicleManager).GetField("instanceCount", BindingFlags.NonPublic | BindingFlags.Static);

            if (Level.info == null || Level.info.type == ELevelType.ARENA)
            {
                return;
            }
            if ((int)frespawnVehicleIndex.GetValue(null) >= VehicleManager.vehicles.Count)
            {
                frespawnVehicleIndex.SetValue(null, (ushort)(VehicleManager.vehicles.Count - 1));
            }
            InteractableVehicle interactableVehicle = VehicleManager.vehicles[(int)frespawnVehicleIndex.GetValue(null)];
            frespawnVehicleIndex.SetValue(null, (int)frespawnVehicleIndex.GetValue(null) + 1);
            if ((int)frespawnVehicleIndex.GetValue(null) >= VehicleManager.vehicles.Count)
            {
                frespawnVehicleIndex.SetValue(null, 0);
            }
            if ((interactableVehicle.isExploded && Time.realtimeSinceStartup - interactableVehicle.lastExploded > Provider.modeConfigData.Vehicles.Respawn_Time) || (interactableVehicle.isDrowned && Time.realtimeSinceStartup - interactableVehicle.lastUnderwater > Provider.modeConfigData.Vehicles.Respawn_Time))
            {
                if (!interactableVehicle.isEmpty)
                {
                    return;
                }
                VehicleSpawnpoint vehicleSpawnpoint = null;
                if (VehicleManager.vehicles.Count < (int)Level.vehicles)
                {
                    vehicleSpawnpoint = LevelVehicles.spawns[UnityEngine.Random.Range(0, LevelVehicles.spawns.Count)];
                    ushort num = 0;
                    while ((int)num < VehicleManager.vehicles.Count)
                    {
                        if ((VehicleManager.vehicles[(int)num].transform.position - vehicleSpawnpoint.point).sqrMagnitude < 64f)
                        {
                            return;
                        }
                        num += 1;
                    }
                }
                ServerEvents.RunVehicleRemoved(interactableVehicle);
                VehicleManager.instance.channel.send("tellVehicleDestroy", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    interactableVehicle.instanceID
                });
                if (vehicleSpawnpoint != null)
                {
                    Vector3 point = vehicleSpawnpoint.point;
                    point.y += 0.5f;
                    ushort vehicle = LevelVehicles.getVehicle(vehicleSpawnpoint);
                    VehicleAsset vehicleAsset = (VehicleAsset)Assets.find(EAssetType.VEHICLE, vehicle);
                    if (vehicleAsset != null)
                    {
                        finstanceCount.SetValue(null, (uint)finstanceCount.GetValue(null) + 1u);
                        typeof(VehicleManager).GetMethod("addVehicle", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(VehicleManager.instance, new object[] { vehicle, point, Quaternion.Euler(0f, vehicleSpawnpoint.angle, 0f), false, false, false, 65535, false, 65535, 65535, CSteamID.Nil, CSteamID.Nil, false, null, null, (uint)finstanceCount.GetValue(null), VehicleManager.getVehicleRandomTireAliveMask(vehicleAsset) });
                        VehicleManager.instance.channel.openWrite();
                        VehicleManager.instance.sendVehicle(VehicleManager.vehicles[VehicleManager.vehicles.Count - 1]);
                        VehicleManager.instance.channel.closeWrite("tellVehicle", ESteamCall.OTHERS, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
                    }
                }
            }
        }
    }
}
