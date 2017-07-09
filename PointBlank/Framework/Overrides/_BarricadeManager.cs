using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Unturned;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Barricade;
using SDG.Unturned;
using UnityEngine;
using Steamworks;

namespace PointBlank.Framework.Overrides
{
    internal class _BarricadeManager
    {
        [Detour(typeof(BarricadeManager), "dropBarricade", BindingFlags.Public | BindingFlags.Static)]
        public static Transform dropBarricade(Barricade barricade, Transform hit, Vector3 point, float angle_x, float angle_y, float angle_z, ulong owner, ulong group)
        {
            // #Nelson doesn't know how to optimize!!!
            ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset)Assets.find(EAssetType.ITEM, barricade.id);
            Transform result = null;
            if (itemBarricadeAsset == null) return result;
            Vector3 eulerAngles = BarricadeManager.getRotation(itemBarricadeAsset, angle_x, angle_y, angle_z).eulerAngles;
            angle_x = (float)(Mathf.RoundToInt(eulerAngles.x / 2f) * 2);
            angle_y = (float)(Mathf.RoundToInt(eulerAngles.y / 2f) * 2);
            angle_z = (float)(Mathf.RoundToInt(eulerAngles.z / 2f) * 2);
            byte b3;
            byte b4;
            BarricadeRegion barricadeRegion2;
            bool cancel = false;

            if (hit != null && hit.transform.CompareTag("Vehicle")) // Placed on vehicle???
            {
                byte b;
                byte b2;
                ushort num;
                BarricadeRegion barricadeRegion;
                if (!BarricadeManager.tryGetPlant(hit, out b, out b2, out num, out barricadeRegion)) return result;
                BarricadeData barricadeData = new BarricadeData(barricade, point, MeasurementTool.angleToByte(angle_x), MeasurementTool.angleToByte(angle_y), MeasurementTool.angleToByte(angle_z), owner, @group, Provider.time);
                ServerEvents.RunBarricadeCreated(barricadeData, ref cancel);
                if (cancel)
                    return null;
                barricadeRegion.barricades.Add(barricadeData);
                uint num2 = (uint)typeof(BarricadeManager).GetField("", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) + 1u;
                result = (Transform)typeof(BarricadeManager).GetMethod("spawnBarricade", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(BarricadeManager.instance, new object[] { barricadeRegion, barricade.id, barricade.state, barricadeData.point, barricadeData.angle_x, barricadeData.angle_y, barricadeData.angle_z, 100, barricadeData.owner, barricadeData.@group, num2 });
                BarricadeManager.instance.channel.send("tellBarricade", ESteamCall.OTHERS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    b,
                    b2,
                    num,
                    barricade.id,
                    barricade.state,
                    barricadeData.point,
                    barricadeData.angle_x,
                    barricadeData.angle_y,
                    barricadeData.angle_z,
                    barricadeData.owner,
                    barricadeData.@group,
                    num2
                });
            }
            else if (Regions.tryGetCoordinate(point, out b3, out b4) && BarricadeManager.tryGetRegion(b3, b4, 65535, out barricadeRegion2))
            {
                BarricadeData barricadeData2 = new BarricadeData(barricade, point, MeasurementTool.angleToByte(angle_x), MeasurementTool.angleToByte(angle_y), MeasurementTool.angleToByte(angle_z), owner, @group, Provider.time);
                ServerEvents.RunBarricadeCreated(barricadeData2, ref cancel);
                if (cancel)
                    return null;
                barricadeRegion2.barricades.Add(barricadeData2);
                uint num3 = (uint)typeof(BarricadeManager).GetField("", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) + 1u;
                result = (Transform)typeof(BarricadeManager).GetMethod("spawnBarricade", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(BarricadeManager.instance, new object[] { barricadeRegion2, barricade.id, barricade.state, barricadeData2.point, barricadeData2.angle_x, barricadeData2.angle_y, barricadeData2.angle_z, 100, barricadeData2.owner, barricadeData2.@group, num3 });
                BarricadeManager.instance.channel.send("tellBarricade", ESteamCall.OTHERS, b3, b4, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    b3,
                    b4,
                    65535,
                    barricade.id,
                    barricade.state,
                    barricadeData2.point,
                    barricadeData2.angle_x,
                    barricadeData2.angle_y,
                    barricadeData2.angle_z,
                    barricadeData2.owner,
                    barricadeData2.@group,
                    num3
                });
            }
            return result;
        }

        [SteamCall]
        [Detour(typeof(BarricadeManager), "askSalvageBarricade", BindingFlags.Public | BindingFlags.Instance)]
        public void askSalvageBarricade(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
        {
            BarricadeRegion barricadeRegion;
            bool cancel = false;

            if (BarricadeManager.tryGetRegion(x, y, plant, out barricadeRegion))
            {
                BarricadeData data = barricadeRegion.barricades[(int)index];

                BarricadeEvents.RunBarricadeSalvage(UnturnedBarricade.Create(data), ref cancel);
            }

            if (!cancel)
                DetourManager.CallOriginal(typeof(BarricadeManager).GetMethod("askSalvageBarricade", BindingFlags.Public | BindingFlags.Instance), BarricadeManager.instance, steamID, x, y, plant, index);
        }
    }
}
