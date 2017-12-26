using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Logging;
using PointBlank.API.Extension.Loader;
using PointBlank.API.PointBlankImplements;

namespace PointBlank.API.Extension
{
    /// <summary>
    /// ExtensionLoader manages loading, unloading and reloading of extensions for the mod loader
    /// </summary>
    public static class ExtensionLoader
    {
        #region Private Variables
        private static List<Type> _Blacklist = new List<Type>()
        {
            typeof(PointBlankExtension)
        };
        #endregion

        #region Public Properties
        /// <summary>
        /// A blacklist of types that the ExtensionLoader ignores when loading extension types
        /// This should only be used in extensions!
        /// </summary>
        public static List<Type> Blacklist
        {
            get
            {
                if (!Assembly.GetCallingAssembly().IsExtension())
                    return null;

                return _Blacklist;
            }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Loads the extension dll and processes the extension
        /// </summary>
        /// <param name="assembly">The assembly/dll to load</param>
        /// <returns>If the extension was loaded successfully</returns>
        public static bool LoadExtension(Assembly assembly)
        {
            if (!Assembly.GetCallingAssembly().IsExtension())
                return false;

            try
            {
                PointBlankExtensionAttribute att = (PointBlankExtensionAttribute)Attribute.GetCustomAttribute(assembly, typeof(PointBlankExtensionAttribute));
                PointBlankExtension ext = null;

                if (!att.RawExtension)
                {
                    Type t = assembly.GetTypes().FirstOrDefault(a => a != typeof(PointBlankExtension) && typeof(PointBlankExtension).IsAssignableFrom(a));

                    if(t != null)
                    {
                        ext = (PointBlankExtension)Activator.CreateInstance(t);

                        ext.Load();
                    }
                }
                if (att.LoadInternals)
                    InternalLoader.LoadAssembly(assembly);

                PointBlankEnvironment.ModLoaderExtensions.Add(assembly, ext);

                PointBlankLogging.Log("Loaded extension: " + assembly.GetName().Name, false);
                return true;
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Unable to load extension: " + assembly.GetName().Name, ex, false, false);
                return false;
            }
        }

        /// <summary>
        /// Unloads the extension
        /// </summary>
        /// <param name="assembly">The assembly/dll of the extension to unload</param>
        /// <returns>If the extension was unloaded successfully</returns>
        public static bool UnloadExtension(Assembly assembly)
        {
            if (!Assembly.GetCallingAssembly().IsExtension())
                return false;
            if (!PointBlankEnvironment.ModLoaderExtensions.ContainsKey(assembly))
                return false;

            try
            {
                PointBlankExtensionAttribute att = (PointBlankExtensionAttribute)Attribute.GetCustomAttribute(assembly, typeof(PointBlankExtensionAttribute));

                if (!att.RawExtension)
                    PointBlankEnvironment.ModLoaderExtensions[assembly].Unload();
                if (att.LoadInternals)
                    InternalLoader.UnloadAssembly(assembly);

                PointBlankEnvironment.ModLoaderExtensions.Remove(assembly);

                PointBlankLogging.Log("Unloaded extension: " + assembly.GetName().Name, false);
                return true;
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Unable to unload extension: " + assembly.GetName().Name, ex, false, false);
                return false;
            }
        }

        /// <summary>
        /// Reloads the extension
        /// </summary>
        /// <param name="assembly">The assembly/dll of the extension to reload</param>
        /// <returns>If the extension reloaded successfully</returns>
        public static bool ReloadExtension(Assembly assembly) =>
            UnloadExtension(assembly) && LoadExtension(assembly);
        #endregion
    }
}
