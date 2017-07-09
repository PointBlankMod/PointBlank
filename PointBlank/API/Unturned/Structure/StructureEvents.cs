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
        /// <param name="cancel">Should the event be canceled</param>
        public delegate void StructureDamageHandler(UnturnedStructure structure, ref ushort damage, bool dead, ref bool cancel);
        /// <summary>
        /// Used for handling structure repair events
        /// </summary>
        /// <param name="structure">The structure instance</param>
        /// <param name="repair">The structure repair amount</param>
        /// <param name="cancel">Should the event be canceled</param>
        public delegate void StructureRepairHandler(UnturnedStructure structure, ref ushort repair, ref bool cancel);

        /// <summary>
        /// Used for handling structure destroy events
        /// </summary>
        /// <param name="structure">The structure that gets destroyed</param>
        /// <param name="cancel">Should the event be canceled</param>
        public delegate void StructureDestroyHandler(UnturnedStructure structure, ref bool cancel);
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
        /// <summary>
        /// Called when a structure gets salvaged
        /// </summary>
        public static event StructureDestroyHandler OnSalvageStructure;
        #endregion

        #region Functions
        internal static void RunDamageStructure(UnturnedStructure structure, ref ushort damage, ref bool cancel)
        {
            if (OnDamageStructure == null)
                return;

            OnDamageStructure(structure, ref damage, (damage > structure.Health), ref cancel);
            if (damage > structure.Health && !cancel)
                RunDestroyStructure(structure, ref cancel);
        }
        internal static void RunRepairStructure(UnturnedStructure structure, ref ushort repair, ref bool cancel) => OnRepairStructure?.Invoke(structure, ref repair, ref cancel);

        internal static void RunDestroyStructure(UnturnedStructure structure, ref bool cancel) => OnDestroyStructure?.Invoke(structure, ref cancel);

        internal static void RunSalvageStructure(UnturnedStructure structure, ref bool cancel) => OnSalvageStructure?.Invoke(structure, ref cancel);

        #endregion
    }
}
