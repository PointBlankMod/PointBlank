using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using PointBlank.API.Collections;
using PointBlank.Framework.Permissions.Ring;
using PointBlank.Services.PluginManager;
using UnityEngine;

namespace PointBlank.API.Plugins
{
    /// <summary>
    /// Used to specify the entrypoint of the plugin
    /// </summary>
    [RingPermission(SecurityAction.Demand, ring = RingPermissionRing.Plugins)]
    public abstract class Plugin : MonoBehaviour
    {
        #region Properties
        /// <summary>
        /// The plugin instance
        /// </summary>
        public static Plugin Instance { get; internal set; }

        /// <summary>
        /// The translations for the plugin
        /// </summary>
        public static TranslationList Translations
        {
            get
            {
                return PluginManager.Plugins.First(a => a.PluginClass == Instance).Translations; // Get the plugin translations
            }
        }

        /// <summary>
        /// The configurations for the plugin
        /// </summary>
        public static ConfigurationList Configurations
        {
            get
            {
                return PluginManager.Plugins.First(a => a.PluginClass == Instance).Configurations; // Get the plugin configuration
            }
        }
        #endregion

        #region Abstract Properties
        /// <summary>
        /// The default translations for the plugin
        /// </summary>
        public abstract TranslationList DefaultTranslations { get; }

        /// <summary>
        /// The default configurations for the plugin
        /// </summary>
        public abstract ConfigurationList DefaultConfigurations { get; }

        /// <summary>
        /// The current version of the plugin
        /// </summary>
        public abstract string Version { get; }
        #endregion

        #region Virtual Properties
        /// <summary>
        /// The latest version of the plugin(for auto-update)(Leave null if you don't want a version check)
        /// </summary>
        public virtual string VersionURL { get { return null; } }

        /// <summary>
        /// The latest build of the plugin(for auto-update)(Leave null if you don't want an auto-update system)
        /// </summary>
        public virtual string BuildURL { get { return null; } }
        #endregion

        #region Abstract Functions
        /// <summary>
        /// Called when the plugin is loading
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// Called when the plugin is unloading
        /// </summary>
        public abstract void Unload();
        #endregion

        public Plugin()
        {
            Instance = this;
        }
    }
}
