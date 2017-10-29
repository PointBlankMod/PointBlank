using PointBlank.API.Implements;

namespace PointBlank.API.Plugins
{
    /// <summary>
    /// All the plugin events
    /// </summary>
    public static class PointBlankPluginEvents
    {
        #region Handlers
        /// <summary>
        /// The handler for any plugin events
        /// </summary>
        /// <param name="plugin">The plugin</param>
        public delegate void PluginEventHandler(PointBlankPlugin plugin);
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
        internal static void RunPluginStart(PointBlankPlugin plugin) => OnPluginStart?.Invoke(plugin);
        internal static void RunPluginLoaded(PointBlankPlugin plugin) => OnPluginLoaded?.Invoke(plugin);
        internal static void RunPluginsLoaded() => OnPluginsLoaded?.Invoke();

        internal static void RunPluginStop(PointBlankPlugin plugin) => OnPluginStop?.Invoke(plugin);
        internal static void RunPluginUnloaded(PointBlankPlugin plugin) => OnPluginUnloaded?.Invoke(plugin);
        internal static void RunPluginsUnloaded() => OnPluginsUnloaded?.Invoke();
        #endregion
    }
}
