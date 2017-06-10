using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UnityEngine;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Detour;
using Steamworks;

namespace PointBlank.Framework.Overrides
{
    internal class _StructureManager
    {
        [Detour(typeof(StructureManager), "dropStructure", BindingFlags.Public | BindingFlags.Static)]
        public static void dropStructure(Structure structure, Vector3 point, float angle_x, float angle_y, float angle_z, ulong owner, ulong group)
        {
            ItemStructureAsset itemStructureAsset = (ItemStructureAsset)Assets.find(EAssetType.ITEM, structure.id);
            if (itemStructureAsset != null)
            {
                Vector3 eulerAngles = Quaternion.Euler(-90f, angle_y, 0f).eulerAngles;
                angle_x = (float)(Mathf.RoundToInt(eulerAngles.x / 2f) * 2);
                angle_y = (float)(Mathf.RoundToInt(eulerAngles.y / 2f) * 2);
                angle_z = (float)(Mathf.RoundToInt(eulerAngles.z / 2f) * 2);
                byte b;
                byte b2;
                StructureRegion structureRegion;
                if (Regions.tryGetCoordinate(point, out b, out b2) && StructureManager.tryGetRegion(b, b2, out structureRegion))
                {
                    StructureData structureData = new StructureData(structure, point, MeasurementTool.angleToByte(angle_x), MeasurementTool.angleToByte(angle_y), MeasurementTool.angleToByte(angle_z), owner, group, Provider.time);
                    structureRegion.structures.Add(structureData);
                    StructureManager.instance.channel.send("tellStructure", ESteamCall.ALL, b, b2, StructureManager.STRUCTURE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                    {
                        b,
                        b2,
                        structure.id,
                        structureData.point,
                        structureData.angle_x,
                        structureData.angle_y,
                        structureData.angle_z,
                        owner,
                        group,
                        (uint)typeof(StructureManager).GetField("instanceCount", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)
                    });
                    ServerEvents.RunStructureCreated(structureData);
                }
            }
        }

        [SteamCall]
        [Detour(typeof(StructureManager), "tellTakeStructure", BindingFlags.Public | BindingFlags.Instance)]
        public void tellTakeStructure(CSteamID steamID, byte x, byte y, ushort index, Vector3 ragdoll)
        {
            StructureRegion structureRegion;
            if (StructureManager.instance.channel.checkServer(steamID) && StructureManager.tryGetRegion(x, y, out structureRegion))
            {
                StructureData data = structureRegion.structures[index];

            }

            DetourManager.CallOriginal(typeof(StructureManager).GetMethod("tellTakeStructure", BindingFlags.Instance | BindingFlags.Public), StructureManager.instance, x, y, index, ragdoll);
        }
    }
}
