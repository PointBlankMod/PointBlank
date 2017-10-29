using System;
using System.Reflection;
using System.Linq;
using PointBlank.Services.PluginManager;

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
        public static PointBlankPlugin[] LoadedPlugins => PluginManager.Plugins.Select(a => a.PluginClass).ToArray();
        /// <summary>
        /// Array of all the loaded libraries
        /// </summary>
        public static Assembly[] LoadedLibraries => PluginManager.Libraries;
        #endregion

        #region Functions
        /// <summary>
        /// Returns the plugin name of a specific plugin
        /// </summary>
        /// <param name="plugin">The plugin to get the name of</param>
        /// <returns>The name of the plugin</returns>
        public static string GetPluginName(PointBlankPlugin plugin) => PluginManager.Plugins.FirstOrDefault(a => a.PluginClass == plugin).Name;

        /// <summary>
        /// Reloads the plugin manager
        /// </summary>
        public static void Reload()
        {
<<<<<<< HEAD
            PluginManager manager = (PluginManager)Enviroment.services["PluginManager.PluginManager"].ServiceClass;
=======
            PluginManager manager = (PluginManager)PointBlankEnvironment.Services["PluginManager.PluginManager"].ServiceClass;
>>>>>>> master

            manager.LoadConfig();
            foreach(PluginWrapper wrapper in PluginManager.Plugins)
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
            PluginWrapper wrapper = PluginManager.Plugins.FirstOrDefault(a => a.PluginClass == plugin);

            if(wrapper != null)
            {
                wrapper.Unload();
                wrapper.Load();
            }
        }

        /// <summary>
        /// Dynamically loads a plugin
        /// </summary>
        /// <param name="plugin">The plugin to load</param>
        public static void LoadPlugin(Type plugin) => PluginManager.LoadPlugin(plugin);
        /// <summary>
        /// Dynamically loads a plugin
        /// </summary>
        /// <param name="plugin">The plugin to load</param>
        public static void LoadPlugin(PointBlankPlugin plugin) => PluginManager.LoadPlugin(plugin);
        #endregion
    }
}
