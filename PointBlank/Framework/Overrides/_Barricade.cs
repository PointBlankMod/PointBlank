using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Barricade;
using SDG.Unturned;

namespace PointBlank.Framework.Overrides
{
    internal static class _Barricade
    {
        [Detour(typeof(Barricade), "askDamage", BindingFlags.Instance | BindingFlags.Public)]
        public static void askDamage(this Barricade barricade, ushort amount)
        {
            // Set the variables
            bool cancel = false;

            // Run the events
            BarricadeEvents.RunBarricadeDamage(UnturnedBarricade.FindBarricade(barricade), ref amount, ref cancel);

            // Run the original function
            if (!cancel)
                DetourManager.CallOriginal(typeof(Barricade).GetMethod("askDamage", BindingFlags.Instance | BindingFlags.Public), barricade, amount);
        }

        [Detour(typeof(Barricade), "askRepair", BindingFlags.Instance | BindingFlags.Public)]
        public static void askRepair(this Barricade barricade, ushort amount)
        {
            // Set the variables
            bool cancel = false;

            // Run the events
            BarricadeEvents.RunBarricadeRepair(UnturnedBarricade.FindBarricade(barricade), ref amount, ref cancel);

            // Run the original function
            if (!cancel)
                DetourManager.CallOriginal(typeof(Barricade).GetMethod("askRepair", BindingFlags.Instance | BindingFlags.Public), barricade, amount);
        }
    }
}
