using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Detour
{
    /// <summary>
    /// Used to detour a specific function
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DetourAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// The class that contains the method
        /// </summary>
        public Type Class { get; private set; }
        /// <summary>
        /// The name of the method
        /// </summary>
        public string MethodName { get; private set; }
        /// <summary>
        /// The methodinfo of the method
        /// </summary>
        public MethodInfo Method { get; private set; }
        /// <summary>
        /// The flags to find the method
        /// </summary>
        public BindingFlags Flags { get; private set; }
        /// <summary>
        /// Was the method found
        /// </summary>
        public bool MethodFound { get; private set; }
        #endregion

        /// <summary>
        /// Used to detour a specific function
        /// </summary>
        /// <param name="tClass">The class containing the method</param>
        /// <param name="method">The name of the method to replace</param>
        /// <param name="flags">The flags of the method to replace</param>
        public DetourAttribute(Type tClass, string method, BindingFlags flags)
        {
            // Set the variables
            Class = tClass;
            MethodName = method;
            Flags = flags;

            try
            {
                Method = Class.GetMethod(MethodName, Flags);
                MethodFound = true;
            }
            catch (Exception ex)
            {
                Logging.LogError("Method not found! " + MethodName + " in class " + Class.FullName, ex, false, false);
                MethodFound = false;
            }
        }
    }
}
