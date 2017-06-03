using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Unturned.Structure
{
    /// <summary>
    /// All the structure events
    /// </summary>
    public static class StructureEvents
    {
        #region Handlers
        /// <summary>
        /// Used for handling structure damage events
        /// </summary>
        /// <param name="structure">The structure that got damaged</param>
        /// <param name="damage">The amount of damage caused</param>
        /// <param name="dead">Did the structure die</param>
        public delegate void StructureDamageHandler(UnturnedStructure structure, ushort damage, bool dead);
        /// <summary>
        /// Used for handling structure repair events
        /// </summary>
        /// <param name="structure">The structure instance</param>
        /// <param name="repair">The structure repair amount</param>
        public delegate void StructureRepairHandler(UnturnedStructure structure, ushort repair);

        /// <summary>
        /// Used for handling structure destroy events
        /// </summary>
        /// <param name="structure">The structure that gets destroyed</param>
        public delegate void StructureDestroyHandler(UnturnedStructure structure);
        #endregion

        #region Events
        /// <summary>
        /// Called when a structure gets damaged
        /// </summary>
        public static event StructureDamageHandler OnDamageStructure;
        /// <summary>
        /// Called when a structure gets repaired
        /// </summary>
        public static event StructureRepairHandler OnRepairStructure;

        /// <summary>
        /// Called when a structure gets destroyed
        /// </summary>
        public static event StructureDestroyHandler OnDestroyStructure;
        #endregion

        #region Functions
        internal static void RunDamageStructure(UnturnedStructure structure, ushort damage)
        {
            if (OnDamageStructure == null)
                return;

            OnDamageStructure(structure, damage, (damage > structure.Health));
            if (damage > structure.Health)
                RunDestroyStructure(structure);
        }
        internal static void RunRepairStructure(UnturnedStructure structure, ushort repair)
        {
            if (OnRepairStructure == null)
                return;

            OnRepairStructure(structure, repair);
        }

        internal static void RunDestroyStructure(UnturnedStructure structure)
        {
            if (OnDestroyStructure == null)
                return;

            OnDestroyStructure(structure);
        }
        #endregion
    }
}
