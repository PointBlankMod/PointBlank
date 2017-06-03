using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Detour;
using SDG.Unturned;

namespace PointBlank.Framework.Overrides
{
    internal static class _Structure
    {
        [Detour(typeof(Structure), "askDamage", BindingFlags.Public | BindingFlags.Instance)]
        public static void askDamage(this Structure stru, ushort amount)
        {
        }
    }
}
