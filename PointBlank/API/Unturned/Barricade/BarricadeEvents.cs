using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Unturned.Barricade
{
    /// <summary>
    /// Contains all barricade related events
    /// </summary>
    public static class BarricadeEvents
    {
        #region Handlers
        /// <summary>
        /// Handles all barricade damage events
        /// </summary>
        /// <param name="barricade">The affected barricade</param>
        /// <param name="damage">The amount of damage caused</param>
        /// <param name="dead">Did the barricade die</param>
        public delegate void BarricadeDamageHandler(UnturnedBarricade barricade, ushort damage, bool dead);
        /// <summary>
        /// Handles all barricade repair events
        /// </summary>
        /// <param name="barricade">The affected barricade</param>
        /// <param name="repair">The amount the barricade was repaired</param>
        public delegate void BarricadeRepairHandler(UnturnedBarricade barricade, ushort repair);

        /// <summary>
        /// Handles the barricade destroy events
        /// </summary>
        /// <param name="barricade">The affected barricade</param>
        public delegate void BarricadeDestroyHandler(UnturnedBarricade barricade);
        #endregion

        #region Events
        /// <summary>
        /// Called when a barricade is damaged
        /// </summary>
        public static event BarricadeDamageHandler OnBarricadeDamage;
        /// <summary>
        /// Called when a barricade is repaired
        /// </summary>
        public static event BarricadeRepairHandler OnBarricadeRepair;

        /// <summary>
        /// Called when a barricade is destroyed
        /// </summary>
        public static event BarricadeDestroyHandler OnBarricadeDestroy;
        /// <summary>
        /// Called when a barricade is salvaged
        /// </summary>
        public static event BarricadeDestroyHandler OnBarricadeSalvage;
        #endregion

        #region Functions
        internal static void RunBarricadeDamage(UnturnedBarricade barricade, ushort damage)
        {
            if (OnBarricadeDamage == null)
                return;

            OnBarricadeDamage(barricade, damage, (damage > barricade.Health));
            if (damage > barricade.Health)
                RunBarricadeDestroy(barricade);
        }
        internal static void RunBarricadeRepair(UnturnedBarricade barricade, ushort repair)
        {
            if (OnBarricadeRepair == null)
                return;

            OnBarricadeRepair(barricade, repair);
        }

        internal static void RunBarricadeDestroy(UnturnedBarricade barricade)
        {
            if (OnBarricadeDestroy == null)
                return;

            OnBarricadeDestroy(barricade);
        }
        internal static void RunBarricadeSalvage(UnturnedBarricade barricade)
        {
            if (OnBarricadeSalvage == null)
                return;

            OnBarricadeSalvage(barricade);
        }
        #endregion
    }
}