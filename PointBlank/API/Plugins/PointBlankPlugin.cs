using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Collections;
using PM = PointBlank.Services.PluginManager.PluginManager;
using UnityEngine;

namespace PointBlank.API.Plugins
{
    /// <summary>
    /// Used to specify the entrypoint of the plugin
    /// </summary>
    public abstract class PointBlankPlugin : MonoBehaviour
    {
        #region Properties
        /// <summary>
        /// The plugin instance
        /// </summary>
        public static PointBlankPlugin Instance { get; internal set; }
        #endregion

        #region Abstract Properties
        /// <summary>
        /// The translations for the plugin
        /// </summary>
        public abstract TranslationList Translations { get; }

        /// <summary>
        /// The configurations for the plugin
        /// </summary>
        public abstract ConfigurationList Configurations { get; }

        /// <summary>
        /// The current version of the plugin
        /// </summary>
        public abstract string Version { get; }
        #endregion

        #region Virtual Properties
        /// <summary>
        /// The latest version of the plugin(for auto-update)(Leave null if you don't want a version check)
        /// </summary>
        public virtual string VersionURL => null;

        /// <summary>
        /// The latest build of the plugin(for auto-update)(Leave null if you don't want an auto-update system)
        /// </summary>
        public virtual string BuildURL => null;
        #endregion

        public PointBlankPlugin()
        {
            Instance = this;
        }

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

        #region Functions
        /// <summary>
        /// Easy translation with string formatting
        /// </summary>
        /// <param name="key">The translation key</param>
        /// <param name="data">The arguments for string formatting</param>
        /// <returns>The formatted message</returns>
        public string Translate(string key, params object[] data) => string.Format(Translations[key], data);

        /// <summary>
        /// Easy to use configuration value extractor
        /// </summary>
        /// <typeparam name="T">The configuration value type</typeparam>
        /// <param name="key">The key of the configuration value</param>
        /// <returns>The configuration value with specified type</returns>
        public T Configure<T>(string key) => (T)Configurations[key];
        #endregion
    }
}
