using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Logging;

namespace PointBlank.API.Extension.Loader
{
    public static class InternalLoader
    {
        #region Public Functions
        public static bool LoadAssembly(Assembly assembly)
        {
            try
            {

                return true;
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Unable to load assembly: " + assembly.GetName().Name, ex, false, false);
                return false;
            }
        }

        public static bool LoadType(Type type)
        {
            if (!type.IsClass || !typeof(InternalObject).IsAssignableFrom(type))
                return false;
            if (type == typeof(InternalObject))
                return false;

            try
            {
                return true;
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Unable to load type: " + type.FullName, ex, false, false);
                return false;
            }
        }
        #endregion
    }
}
