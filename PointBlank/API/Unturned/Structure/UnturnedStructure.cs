using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UStructure = SDG.Unturned.Structure;
using UnityEngine;
using PointBlank.API.Unturned.Server;

namespace PointBlank.API.Unturned.Structure
{
    /// <summary>
    /// The unturned structure instance
    /// </summary>
    public class UnturnedStructure
    {
        #region Properties
        // Important information
        /// <summary>
        /// The structure data
        /// </summary>
        public StructureData Data { get; private set; }
        /// <summary>
        /// The structure object
        /// </summary>
        public UStructure Structure => Data.structure;

        // Structure data information
        /// <summary>
        /// The owner's steam64
        /// </summary>
        public ulong Owner => Data.owner;
        /// <summary>
        /// The group's steam64
        /// </summary>
        public ulong Group => Data.group;

        // Structure information
        /// <summary>
        /// The structure health
        /// </summary>
        public ushort Health => Structure.health;
        /// <summary>
        /// The structure ID
        /// </summary>
        public ushort ID => Data.structure.asset.id;
        /// <summary>
        /// The name of the structure
        /// </summary>
        public string Name => Data.structure.asset.itemName;
        /// <summary>
        /// The position of the structure
        /// </summary>
        public Vector3 Position => Structure.asset.structure.transform.position;
        /// <summary>
        /// The rotation of the structure
        /// </summary>
        public Quaternion Rotation => Structure.asset.structure.transform.rotation;
        #endregion

        /// <summary>
        /// The unturned structure instance
        /// </summary>
        /// <param name="data">The structure data</param>
        internal UnturnedStructure(StructureData data)
        {
            // Set the variables
            this.Data = data;

            // Run code
            UnturnedServer.AddStructure(this);
        }

        #region Static Functions
        /// <summary>
        /// Creates an unturned structure instance or returns an existing one
        /// </summary>
        /// <param name="data">The structure data</param>
        /// <returns>The unturned structure instance</returns>
        public static UnturnedStructure Create(StructureData data)
        {
            UnturnedStructure stru = UnturnedServer.Structures.FirstOrDefault(a => a.Data == data);

            if (stru != null)
                return stru;
            return new UnturnedStructure(data);
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Damage the structure
        /// </summary>
        /// <param name="amount">The amount of damage to cause</param>
        public void Damage(ushort amount)
        {
            Structure.askDamage(amount);
        }

        /// <summary>
        /// Repair the structure
        /// </summary>
        /// <param name="amount">The amount to repair it by</param>
        public void Repair(ushort amount)
        {
            Structure.askRepair(amount);
        }

        /// <summary>
        /// Set the health of the structure
        /// </summary>
        /// <param name="health">The health you want the structure to have</param>
        public void SetHealth(ushort health)
        {
            if (health == Health)
                return;

            if(Health < health)
                Repair((ushort)(health - Health));
            if (Health > health)
                Damage((ushort)(Health - health));
        }
        #endregion
    }
}
