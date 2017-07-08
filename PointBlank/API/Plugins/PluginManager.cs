using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.Services.PluginManager;
using PM = PointBlank.Services.PluginManager.PluginManager;

namespace PointBlank.API.Plugins
{
    /// <summary>
    /// Functions for managing plugins
    /// </summary>
    public static class PluginManager
    {
        #region Properties
        public static Plugin[] LoadedPlugins => PM.Plugins.Select(a => a.PluginClass).ToArray();
        #endregion

        #region Functions
        public static void Reload()
        {

        }

        public static void Reload(Plugin plugin)
        {

        }
        #endregion
    }
}
