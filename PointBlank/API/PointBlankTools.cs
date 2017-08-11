using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using PointBlank.Framework.Permissions.Ring;
using UnityEngine;

namespace PointBlank.API
{
    /// <summary>
    /// Contains methods that you need but aren't available by default
    /// </summary>
    [RingPermission(SecurityAction.Demand, ring = RingPermissionRing.None)]
    public static class PointBlankTools
    {
        #region Public Functions
        /// <summary>
        /// Generates a random string
        /// </summary>
        /// <param name="length">The length of the string</param>
        /// <returns>A random string</returns>
        public static string RandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");

            return path;
        }

        /// <summary>
        /// Emulates a foreach loop using the for loop. This is useful as the foreach loop is slower than a for loop
        /// </summary>
        /// <typeparam name="T">The type of objects inside the array</typeparam>
        /// <param name="array">The array of objects to loop through</param>
        /// <param name="functions">The function to run</param>
        public static void ForeachLoop<T>(T[] array, Action<int, T> functions)
        {
            for(int i = 0; i < array.Length; i++)
                functions(i, array[i]);
        }

        /// <summary>
        /// Emulates a foreach loop using the for loop. This is useful as the foreach loop is slower than a for loop
        /// </summary>
        /// <typeparam name="T">The type of objects inside the list</typeparam>
        /// <param name="list">The list of objects to loop through</param>
        /// <param name="functions">The function to run</param>
        public static void ForeachLoop<T>(List<T> list, Action<int, T> functions)
        {
            for (int i = 0; i < list.Count; i++)
                functions(i, list[i]);
        }

        /// <summary>
        /// Gets the byte array of the string without using encoders
        /// </summary>
        /// <param name="str">The string to convert</param>
        /// <returns>Byte array of the converted string</returns>
        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];

            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);

            return bytes;
        }

        /// <summary>
        /// Gets the string of the byte array without using encoders
        /// </summary>
        /// <param name="bytes">The byte array to convert</param>
        /// <returns>The converted string</returns>
        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];

            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);

            return new string(chars);
        }

        /// <summary>
        /// Gets the string of the byte array with a specific length without using encoders
        /// </summary>
        /// <param name="bytes">The byte array to convert</param>
        /// <param name="length">The length of the string</param>
        /// <returns>The converted string</returns>
        public static string GetString(byte[] bytes, int length)
        {
            char[] chars = new char[length / sizeof(char)];

            Buffer.BlockCopy(bytes, 0, chars, 0, length);

            return new string(chars);
        }

        /// <summary>
        /// Converts a float to a byte
        /// </summary>
        /// <param name="f">The float</param>
        /// <returns>The output byte</returns>
        public static byte ToByte(float f)
        {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }

        /// <summary>
        /// Converts the unix time to date time
        /// </summary>
        /// <param name="unix">The unix time to convert</param>
        /// <returns>The date time from the unix time</returns>
        public static DateTime FromUnixTime(long unix)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unix).ToLocalTime();
        }
        #endregion
    }
}
