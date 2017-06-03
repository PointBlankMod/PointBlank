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
        /// <summary>
        /// Is the structure a bed
        /// </summary>
        public bool IsBed => (Data.structure.GetType() == typeof(InteractableBed));
        /// <summary>
        /// Damage structure 
        /// </summary>
        public void askDamage(float StructureDamage)
        {
            DamageTool.explode(Data.point, 1f, EDeathCause.ANIMAL, CSteamID.Nil, 0, 0, 0, 0, StructureDamage, 0, 0, 0, EExplosionDamageType.CONVENTIONAL, 1f, false);
        }
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
    }
}
