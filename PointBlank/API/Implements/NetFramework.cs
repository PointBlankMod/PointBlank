using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;

namespace PointBlank.API.Implements
{
    /// <summary>
    /// Contains .net framework functions that were removed or were added after version 3.5
    /// </summary>
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

        #region Reflection
        /// <summary>
        /// Gets the value and assigns the type of the value
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="field">The FieldInfo instance of the field</param>
        /// <param name="instance">The instance of the object(null if static)</param>
        /// <returns>The value with assigned type</returns>
        public static T GetValue<T>(this FieldInfo field, object instance) => (T)field.GetValue(instance);

        /// <summary>
        /// Runs a method and returns a value
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="method">The method to run</param>
        /// <param name="instance">The instance of the class(null if static)</param>
        /// <param name="parameters">The parameters of the method</param>
        /// <returns>The value the method returns</returns>
        public static T RunMethod<T>(this MethodInfo method, object instance, params object[] parameters) => 
            (T)method.Invoke(instance, parameters);
        /// <summary>
        /// Runs a method
        /// </summary>
        /// <param name="method">The method to run</param>
        /// <param name="instance">The instance of the class(null if static)</param>
        /// <param name="parameters">The parameters of the method</param>
        public static void RunMethod(this MethodInfo method, object instance, params object[] parameters) =>
            RunMethod<object>(method, instance, parameters);
        #endregion
    }

    public delegate void VoidHandler();
}
