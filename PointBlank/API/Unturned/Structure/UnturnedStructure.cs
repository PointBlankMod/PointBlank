using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UnityEngine;

namespace PointBlank.API.Unturned.Structure
{
    public class UnturnedStructure
    {
        private UnturnedStructure(){}
        public UnturnedStructure(StructureData data)
        {
            Data = data;
        }
        #region Properties
        public StructureData Data { get; private set;}

        public SDG.Unturned.Structure Structure => Data.structure;

        public Vector3 Position => Structure.asset.structure.transform.position;

        public Quaternion Rotation => Structure.asset.structure.transform.rotation;

        public ushort Health => Structure.health;

        public ulong Owner => Data.owner;

        public ulong Group => Data.group;

        public String Name => Data.structure.asset.itemName;

        public ushort ID => Data.structure.asset.id;
        #endregion
    }
}