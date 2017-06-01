using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UStructure = SDG.Unturned.Structure;
using UnityEngine;

namespace PointBlank.API.Unturned.Structure
{
    /// <summary>
    /// The unturned structure instance
    /// </summary>
    public class UnturnedStructure
    {
        #region Properties
        // Important information
        public StructureData Data { get; private set; }
        public UStructure Structure => Data.structure;

        // Structure data information
        public ulong Owner => Data.owner;
        public ulong Group => Data.group;

        // Structure information
        public ushort Health => Structure.health;
        public ushort ID => Data.structure.asset.id;
        public string Name => Data.structure.asset.itemName;
        public Vector3 Position => Structure.asset.structure.transform.position;
        public Quaternion Rotation => Structure.asset.structure.transform.rotation;
        public bool IsBed => (Data.structure.GetType() == typeof(InteractableBed));
        #endregion

        /// <summary>
        /// The unturned structure instance
        /// </summary>
        /// <param name="data">The structure data</param>
        public UnturnedStructure(StructureData data)
        {
            this.Data = data;
        }
    }
}
