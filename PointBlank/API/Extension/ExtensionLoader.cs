using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Logging;

namespace PointBlank.API.Extension
{
    public static class ExtensionLoader
    {
        #region Public Properties
        public static List<Type> Blacklist { get; private set; }
        #endregion

        static ExtensionLoader()
        {
            // Add properties/variables
            Blacklist = new List<Type>
            {
                typeof(PointBlankExtension)
            };
        }

        #region Public Functions
        public static bool LoadExtension(Assembly assembly)
        {
            try
            {
                foreach (Type t in assembly.GetTypes())
                    LoadType(t);

                PointBlankLogging.Log("Loaded assembly: " + assembly.GetName().Name, false);
                return true;
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Unable to load assembly: " + assembly.GetName().Name, ex, false, false);
                return false;
            }
        }

        public static bool UnloadExtension(Assembly assembly)
        {
            try
            {
                foreach (Type t in assembly.GetTypes())
                    UnloadType(t);

                PointBlankLogging.Log("Unloaded assembly: " + assembly.GetName().Name, false);
                return true;
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Unable to unload assembly: " + assembly.GetName().Name, ex, false, false);
                return false;
            }
        }

        public static bool ReloadExtension(Assembly assembly) =>
            UnloadAssembly(assembly) && LoadAssembly(assembly);
        #endregion
    }
}
