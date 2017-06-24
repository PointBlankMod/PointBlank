using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
        public delegate void BarricadeDamageHandler(UnturnedBarricade barricade, ref ushort damage, bool dead, ref bool cancel);
        /// <summary>
        /// Handles all barricade repair events
        /// </summary>
        /// <param name="barricade">The affected barricade</param>
        /// <param name="repair">The amount the barricade was repaired</param>
        public delegate void BarricadeRepairHandler(UnturnedBarricade barricade, ref ushort repair, ref bool cancel);

        /// <summary>
        /// Handles the barricade destroy events
        /// </summary>
        /// <param name="barricade">The affected barricade</param>
        public delegate void BarricadeDestroyHandler(UnturnedBarricade barricade);
        

        /// <summary>
        /// Handles the barricade salvage events
        /// </summary>
        /// <param name="barricade">The affected barricade</param>
        public delegate void BarricadeSalvageHandler(UnturnedBarricade barricade, ref bool cancel);
        

        /// <summary>
        /// Handles the barricade create events
        /// </summary>
        /// <param name="barricade">The affected barricade</param>
        public delegate void BarricadeCreateHandler(SDG.Unturned.ItemBarricadeAsset barricade, Vector3 point, ulong owner, ref ulong group, ref bool cancel);

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
        public static event BarricadeSalvageHandler OnBarricadeSalvage;

        public static event BarricadeCreateHandler OnBarricadeCreate;


        #endregion

        #region Functions


        internal static void RunBarricadeCreate(SDG.Unturned.ItemBarricadeAsset barricade, Vector3 point, ulong owner, ref ulong group, ref bool cancel)
        {
            if(OnBarricadeCreate == null)
                return;

            OnBarricadeCreate(barricade, point, owner, ref group, ref cancel);
        }

        internal static void RunBarricadeDamage(UnturnedBarricade barricade, ref ushort damage, ref bool cancel)
        {
            if (OnBarricadeDamage == null)
                return;

            OnBarricadeDamage(barricade, ref damage, (damage > barricade.Health), ref cancel);
            if (damage > barricade.Health)
                RunBarricadeDestroy(barricade);
        }
        internal static void RunBarricadeRepair(UnturnedBarricade barricade, ref ushort repair, ref bool cancel)
        {
            if (OnBarricadeRepair == null)
                return;
            
            OnBarricadeRepair(barricade, ref repair, ref cancel);
        }

        //not sure to add cancel or not
        internal static void RunBarricadeDestroy(UnturnedBarricade barricade)
        {
            if (OnBarricadeDestroy == null)
                return;

            OnBarricadeDestroy(barricade);
        }
        internal static void RunBarricadeSalvage(UnturnedBarricade barricade, ref bool cancel)
        {
            if (OnBarricadeSalvage == null)
                return;

            OnBarricadeSalvage(barricade, ref cancel);
        }
        #endregion
    }
}