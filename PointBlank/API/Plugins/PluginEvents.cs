using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Implements;

namespace PointBlank.API.Plugins
{
    /// <summary>
    /// All the plugin events
    /// </summary>
    public static class PluginEvents
    {
        #region Handlers
        /// <summary>
        /// The handler for any plugin events
        /// </summary>
        /// <param name="plugin">The plugin</param>
        public delegate void PluginEventHandler(Plugin plugin);
        #endregion

        #region Events
        /// <summary>
        /// Called when the plugin is being started
        /// </summary>
        public static event PluginEventHandler OnPluginStart;
        /// <summary>
        /// Called when the plugin has loaded
        /// </summary>
        public static event PluginEventHandler OnPluginLoaded;
        /// <summary>
        /// Called when all plugins have been loaded
        /// </summary>
        public static event VoidHandler OnPluginsLoaded;

        /// <summary>
        /// Called when the plugin is being stopped
        /// </summary>
        public static event PluginEventHandler OnPluginStop;
        /// <summary>
        /// Called when the plugin has unloaded
        /// </summary>
        public static event PluginEventHandler OnPluginUnloaded;
        /// <summary>
        /// Called when all plugins have been unloaded
        /// </summary>
        public static event VoidHandler OnPluginsUnloaded;
        #endregion

        #region Functions
        internal static void RunPluginStart(Plugin plugin) => OnPluginStart?.Invoke(plugin);

        internal static void RunPluginLoaded(Plugin plugin) => OnPluginLoaded?.Invoke(plugin);

        internal static void RunPluginsLoaded() => OnPluginsLoaded?.Invoke();

        internal static void RunPluginStop(Plugin plugin) => OnPluginStop?.Invoke(plugin);

        internal static void RunPluginUnloaded(Plugin plugin) => OnPluginUnloaded?.Invoke(plugin);

        internal static void RunPluginsUnloaded() => OnPluginsUnloaded?.Invoke();

        #endregion
    }
}
