using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UBarricade = SDG.Unturned.Barricade;
using UnityEngine;
using PointBlank.API.Unturned.Server;

namespace PointBlank.API.Unturned.Barricade
{
    /// <summary>
    /// The unturned barricade instance
    /// </summary>
    public class UnturnedBarricade
    {
        #region Properties
        // Important information
        /// <summary>
        /// The Barricade Data
        /// </summary>
        public BarricadeData Data { get; private set; }
        /// <summary>
        /// The Barricade
        /// </summary>
        public UBarricade Barricade => Data.barricade;
        /// <summary>
        /// The barricade asset
        /// </summary>
        public ItemBarricadeAsset Asset => Barricade.asset;
        /// <summary>
        /// The gameobject of the barricade
        /// </summary>
        public GameObject GameObject => Asset.barricade;

        // Data information
        /// <summary>
        /// SteamID of owner
        /// </summary>
        public ulong Owner => Data.owner;
        /// <summary>
        /// ID of Group possessing ownership of barricade
        /// </summary>
        public ulong Group => Data.group;

        // Asset information
        /// <summary>
        /// ID of barricade
        /// </summary>
        public ushort ID => Asset.id;

        // GameObject information
        /// <summary>
        /// Location of Barricade
        /// </summary>
        public Vector3 Position
        {
            get => Data.point;
            set
            {
                bool cancel = false;

                BarricadeEvents.RunBarricadeDestroy(this, ref cancel);
                if (cancel)
                    return;
                BarricadeManager.dropBarricade(Barricade, null, Data.point, MeasurementTool.byteToAngle(Data.angle_x), MeasurementTool.byteToAngle(Data.angle_y), MeasurementTool.byteToAngle(Data.angle_z), Owner, Group);
                GameObject.transform.position.Set(value.x, value.y, value.z);
                if (!BarricadeManager.tryGetInfo(GameObject.transform, out byte x, out byte y, out ushort plant, out ushort index, out BarricadeRegion region))
                    return;
                region.barricades.RemoveAt(index);
                if (plant == 65535)
                {
                    BarricadeManager.instance.channel.send("tellTakeBarricade", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                    {
                        x,
                        y,
                        plant,
                        index
                    });
                }
                else
                {
                    BarricadeManager.instance.channel.send("tellTakeBarricade", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                    {
                        x,
                        y,
                        plant,
                        index
                    });
                }
            }
        }
        /// <summary>
        /// Rotation of barricade
        /// </summary>
        public Quaternion Rotation => Quaternion.Euler((Data.angle_x * 2), (Data.angle_y * 2), (Data.angle_z * 2));

        // Barricade information
        /// <summary>
        /// Health of barricade
        /// </summary>
        public ushort Health => Barricade.health;
        /// <summary>
        /// State of barricade
        /// </summary>
        public byte[] State => Barricade.state;
        #endregion

        private UnturnedBarricade(BarricadeData data)
        {
            // Set the variables
            Data = data;

            // Run the code
            UnturnedServer.AddBarricade(this);
        }

        #region Static Functions
        /// <summary>
        /// Creates an unturned barricade instance or returns an existing one
        /// </summary>
        /// <param name="data">The barricade data</param>
        /// <returns>The unturned barricade instance</returns>
        internal static UnturnedBarricade Create(BarricadeData data)
        {
            UnturnedBarricade barricade = UnturnedServer.Barricades.FirstOrDefault(a => a.Data == data);

            return barricade ?? new UnturnedBarricade(data);
        }

        /// <summary>
        /// Finds a Barricade based on the unturned Barricade instance
        /// </summary>
        /// <param name="Barricade">The unturned Barricade instance</param>
        /// <returns>The instance of the custom Barricade class</returns>
        public static UnturnedBarricade FindBarricade(UBarricade Barricade) => UnturnedServer.Barricades.FirstOrDefault(a => a.Barricade == Barricade);

        /// <summary>
        /// Finds a Barricade based on the Barricade data
        /// </summary>
        /// <param name="data">The Barricade data</param>
        /// <returns>The unturned Barricade instance</returns>
        public static UnturnedBarricade FindBarricade(BarricadeData data) => UnturnedServer.Barricades.FirstOrDefault(a => a.Data == data);
        #endregion

        #region Public Functions
        /// <summary>
        /// Damage the barricade
        /// </summary>
        /// <param name="amount">The amount of damage to cause</param>
        public void Damage(float amount) => BarricadeManager.damage(Barricade.asset.barricade.transform, amount, 1, false);

        /// <summary>
        /// Repair the Barricade
        /// </summary>
        /// <param name="amount">The amount to repair it by</param>
        public void Repair(ushort amount) => Barricade.askRepair(amount);

        /// <summary>
        /// Set the health of the barricade
        /// </summary>
        /// <param name="health">The health you want the barricade to have</param>
        public void SetHealth(ushort health)
        {
            if (health == Health)
                return;

            if(Health < health)
                Repair((ushort)(health - Health));
            if (Health > health)
                Damage((ushort)(Health - health));
        }

        /*/// <summary>
        /// Duplicate the barricade
        /// </summary>
        public UnturnedBarricade Duplicate()
        {
            UnturnedBarricade Dupe = Create(new StructureData(new UBarricade((ushort)(UnturnedServer.Structures.Length + 1), Barricade.health, Barricade.state, Barricade.asset), Data.point, Data.angle_x, Data.angle_y, Data.angle_z, Data.owner, Data.group, Data.objActiveDate));

            return Dupe;
        }*/
        #endregion
    }
}
