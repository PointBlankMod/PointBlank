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
        /// <summary>
        /// The Barricade Data
        /// </summary>
        public BarricadeData Data { get; private set; }

        /// <summary>
        /// The Barricade
        /// </summary>
        public UBarricade Barricade => Data.barricade;

        /// <summary>
        /// Location of Barricade
        /// </summary>
        public Vector3 Position => Barricade.asset.barricade.transform.position;

        /// <summary>
        /// SteamID of owner
        /// </summary>
        public ulong Owner => Data.owner;

        /// <summary>
        /// ID of Group possessing ownership of barricade
        /// </summary>
        public ulong Group => Data.group;

        /// <summary>
        /// ID of barricade
        /// </summary>
        public ushort ID => Barricade.asset.id;

        /// <summary>
        /// Health of barricade
        /// </summary>
        public ushort Health => Barricade.health;

        /// <summary>
        /// Rotation of barricade
        /// </summary>
        public Quaternion Rotation => Barricade.asset.barricade.transform.rotation;

        /// <summary>
        /// ID of barricade asset
        /// </summary>
        public ushort AssetID => Barricade.asset.id;

        /// <summary>
        /// State of barricade
        /// </summary>
        public byte[] State => Barricade.state;
        #endregion

        private UnturnedBarricade(BarricadeData data)
        {
            Data = data;


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

            if (barricade != null)
                return barricade;
            return new UnturnedBarricade(data);
        }

        /// <summary>
        /// Finds a Barricade based on the unturned Barricade instance
        /// </summary>
        /// <param name="Barricade">The unturned Barricade instance</param>
        /// <returns>The instance of the custom Barricade class</returns>
        public static UnturnedBarricade FindBarricade(UBarricade Barricade)
        {
            return UnturnedServer.Barricades.FirstOrDefault(a => a.Barricade == Barricade);
        }

        /// <summary>
        /// Finds a Barricade based on the Barricade data
        /// </summary>
        /// <param name="data">The Barricade data</param>
        /// <returns>The unturned Barricade instance</returns>
        public static UnturnedBarricade FindBarricade(BarricadeData data)
        {
            return UnturnedServer.Barricades.FirstOrDefault(a => a.Data == data);
        }
        #endregion

        #region Public Functions

        /// <summary>
        /// Damage the barricade
        /// </summary>
        /// <param name="amount">The amount of damage to cause</param>
        public void Damage(float amount)
        {
            BarricadeManager.damage(Barricade.asset.barricade.transform, amount, 1, false);
        }

        /// <summary>
        /// Repair the Barricade
        /// </summary>
        /// <param name="amount">The amount to repair it by</param>
        public void Repair(ushort amount)
        {
            Barricade.askRepair(amount);
        }

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

        /// <summary>
        /// Duplicate the barricade
        /// </summary>
        public UnturnedBarricade Duplicate()
        {
            UnturnedBarricade Dupe = new UnturnedBarricade(new StructureData(new UBarricade((ushort)(UnturnedServer.Structures.Length + 1), Barricade.health, Barricade.state Barricade.asset), Data.point, Data.angle_x, Data.angle_y, Data.angle_z, Data.owner, Data.group, Data.objActiveDate));
            UnturnedServer.AddBarricade(Dupe);
            return Dupe;
        }
        #endregion
    }
}
