using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using PointBlank.Framework.Permissions.Ring;

namespace PointBlank.API.Implements
{
    /// <summary>
    /// Contains .net framework functions that were removed or were added after version 3.5
    /// </summary>
    [RingPermission(SecurityAction.Demand, ring = RingPermissionRing.None)]
    public static class NetFramework
    {
        /// <summary>
        /// The .net 3.5 implementation of the HasFlag function found in .net 4.0+
        /// </summary>
        /// <param name="input">The flag to compare</param>
        /// <param name="matchTo">The flag to compare it to</param>
        /// <returns>If the input flag contains the matching flag</returns>
        public static bool HasFlag(this Enum input, Enum matchTo) => (Convert.ToUInt32(input) & Convert.ToUInt32(matchTo)) != 0;

        #region Lists
        /// <summary>
        /// Uses a for loop on any list
        /// </summary>
        /// <typeparam name="T">The list type</typeparam>
        /// <param name="list">The list the loop is running through</param>
        /// <param name="action">The list function that gets run</param>
        public static void For<T>(this IEnumerable<T> list, Action<int, T> action)
        {
            for(int i = 0; i < list.Count(); i++)
                action(i, list.ElementAt(i));
        }

        /// <summary>
        /// Uses a foreach loop on any list
        /// </summary>
        /// <typeparam name="T">The list type</typeparam>
        /// <param name="list">The list the loop is running through</param>
        /// <param name="action">The list function that gets run</param>
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (T obj in list)
                action(obj);
        }
        #endregion
    }

    public delegate void VoidHandler();
}
