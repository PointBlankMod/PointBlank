using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using PointBlank.Framework.Permissions.Ring;

namespace PointBlank.API.Plugins
{
    /// <summary>
    /// All the plugin events
    /// </summary>
    [RingPermission(SecurityAction.Demand, ring = RingPermissionRing.None)]
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
        public static event OnVoidDelegate OnPluginsLoaded;

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
        public static event OnVoidDelegate OnPluginsUnloaded;
        #endregion

        #region Functions
        internal static void RunPluginStart(Plugin plugin)
        {
            if (OnPluginStart == null)
                return;

            OnPluginStart(plugin);
        }
        internal static void RunPluginLoaded(Plugin plugin)
        {
            if (OnPluginLoaded == null)
                return;

            OnPluginLoaded(plugin);
        }
        internal static void RunPluginsLoaded()
        {
            if (OnPluginsLoaded == null)
                return;

            OnPluginsLoaded();
        }

        internal static void RunPluginStop(Plugin plugin)
        {
            if (OnPluginStop == null)
                return;

            OnPluginStop(plugin);
        }
        internal static void RunPluginUnloaded(Plugin plugin)
        {
            if (OnPluginUnloaded == null)
                return;

            OnPluginUnloaded(plugin);
        }
        internal static void RunPluginsUnloaded()
        {
            if (OnPluginsUnloaded == null)
                return;

            OnPluginsUnloaded();
        }
        #endregion
    }
}
