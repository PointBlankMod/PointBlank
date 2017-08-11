using System;
using System.Reflection;
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
    public static class PointBlankPluginManager
    {
        #region Properties
        /// <summary>
        /// Array of all the loaded plugins
        /// </summary>
        public static PointBlankPlugin[] LoadedPlugins => PM.Plugins.Select(a => a.PluginClass).ToArray();
        /// <summary>
        /// Array of all the loaded libraries
        /// </summary>
        public static Assembly[] LoadedLibraries => PM.Libraries;
        #endregion

        #region Functions
        /// <summary>
        /// Returns the plugin name of a specific plugin
        /// </summary>
        /// <param name="plugin">The plugin to get the name of</param>
        /// <returns>The name of the plugin</returns>
        public static string GetPluginName(PointBlankPlugin plugin) => PM.Plugins.FirstOrDefault(a => a.PluginClass == plugin).Name;

        /// <summary>
        /// Reloads the plugin manager
        /// </summary>
        public static void Reload()
        {
            PM manager = (PM)Enviroment.services["PluginManager.PluginManager"].ServiceClass;

            manager.LoadConfig();
            foreach(PluginWrapper wrapper in PM.Plugins)
            {
                wrapper.LoadConfiguration();
                wrapper.LoadTranslation();
            }
        }

        /// <summary>
        /// Reloads a specific plugin
        /// </summary>
        /// <param name="plugin">The plugin to reload</param>
        public static void Reload(PointBlankPlugin plugin)
        {
            PluginWrapper wrapper = PM.Plugins.FirstOrDefault(a => a.PluginClass == plugin);

            if(wrapper != null)
            {
                wrapper.Unload();
                wrapper.Load();
            }
        }
        #endregion
    }
}
