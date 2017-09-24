using System;
using System.Reflection;
using System.Diagnostics;
using System.Linq;
using PointBlank.Services.DetourManager;

namespace PointBlank.API.Detour
{
    /// <summary>
    /// Contains function for managing detours
    /// </summary>
    public static class PointBlankDetourManager
    {
        #region Public Functions
        /// <summary>
        /// Calls the original method that was detoured
        /// </summary>
        /// <param name="method">The original method</param>
        /// <param name="instance">The instance for the method(null if static)</param>
        /// <param name="args">The arguments for the method</param>
        /// <returns>The value that the original function returns</returns>
        public static object CallOriginal(MethodInfo method, object instance = null, params object[] args)
        {
            // Set the variables
            DetourWrapper wrapper = DetourManager.Detours.First(a => a.Value.Original == method).Value;

            // Do the checks
            if (wrapper == null)
                throw new Exception("The detour specified was not found!");

            return wrapper.CallOriginal(args, instance);
        }

        /// <summary>
        /// Calls the original method that was detoured
        /// </summary>
        /// <param name="instance">The instance for the method(null if static)</param>
        /// <param name="args">The arguments for the method</param>
        /// <returns>The value tahat the original function returns</returns>
        public static object CallOriginal(object instance = null, params object[] args)
        {
            StackTrace trace = new StackTrace(false);

            if (trace.FrameCount < 1)
                throw new Exception("Invalid trace back to the original method! Please provide the methodinfo instead!");

            MethodBase modded = trace.GetFrame(1).GetMethod();
            MethodInfo original = null;

            if (!Attribute.IsDefined(modded, typeof(DetourAttribute)))
                modded = trace.GetFrame(2).GetMethod();
            DetourAttribute att = (DetourAttribute)Attribute.GetCustomAttribute(modded, typeof(DetourAttribute));

            if (att == null)
                throw new Exception("This method can only be called from an overwritten method!");
            if (!att.MethodFound)
                throw new Exception("The original method was never found!");
            original = att.Method;

            DetourWrapper wrapper = DetourManager.Detours.First(a => a.Value.Original == original).Value;

            if (wrapper == null)
                throw new Exception("The detour specified was not found!");

            return wrapper.CallOriginal(args, instance);
        }

        /// <summary>
        /// Enables the detour of a method(WARNING: The method needs to have been detoured atleast once!)
        /// </summary>
        /// <param name="method">The original method that was detoured</param>
        /// <returns>If the detour was enabled successfully</returns>
        public static bool EnableDetour(MethodInfo method)
        {
            // Set the variables
            DetourWrapper wrapper = DetourManager.Detours.First(a => a.Value.Original == method).Value;

            // Do the checks
            if (wrapper == null)
                return false;

            return wrapper.Detour();
        }

        /// <summary>
        /// Disables the detour of a method(WARNING: The method needs to have been detoured atleast once!)
        /// </summary>
        /// <param name="method">The original method that was detoured</param>
        /// <returns>If the detour was disabled successfully</returns>
        public static bool DisableDetour(MethodInfo method)
        {
            // Set the variables
            DetourWrapper wrapper = DetourManager.Detours.First(a => a.Value.Original == method).Value;

            // Do the checks
            if (wrapper == null)
                return false;

            return wrapper.Revert();
        }
        #endregion
    }
}
