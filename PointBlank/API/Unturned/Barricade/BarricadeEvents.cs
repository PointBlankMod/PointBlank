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
        /// <param name="cancel">Should the event be canceled</param>
        public delegate void BarricadeDamageHandler(UnturnedBarricade barricade, ref ushort damage, bool dead, ref bool cancel);
        /// <summary>
        /// Handles all barricade repair events
        /// </summary>
        /// <param name="barricade">The affected barricade</param>
        /// <param name="repair">The amount the barricade was repaired</param>
        /// <param name="cancel">Should the event be canceled</param>
        public delegate void BarricadeRepairHandler(UnturnedBarricade barricade, ref ushort repair, ref bool cancel);

        /// <summary>
        /// Handles the barricade destroy events
        /// </summary>
        /// <param name="barricade">The affected barricade</param>
        /// <param name="cancel">Should the event be canceled</param>
        public delegate void BarricadeDestroyHandler(UnturnedBarricade barricade, ref bool cancel);
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
        internal static void RunBarricadeDamage(UnturnedBarricade barricade, ref ushort damage, ref bool cancel)
        {
            if (OnBarricadeDamage == null)
                return;

            OnBarricadeDamage(barricade, ref damage, (damage > barricade.Health), ref cancel);
            if (damage > barricade.Health && !cancel)
                RunBarricadeDestroy(barricade, ref cancel);
        }
        internal static void RunBarricadeRepair(UnturnedBarricade barricade, ref ushort repair, ref bool cancel) => OnBarricadeRepair?.Invoke(barricade, ref repair, ref cancel);

        internal static void RunBarricadeDestroy(UnturnedBarricade barricade, ref bool cancel) => OnBarricadeDestroy?.Invoke(barricade, ref cancel);

        internal static void RunBarricadeSalvage(UnturnedBarricade barricade, ref bool cancel) => OnBarricadeSalvage?.Invoke(barricade, ref cancel);

        #endregion
    }
}