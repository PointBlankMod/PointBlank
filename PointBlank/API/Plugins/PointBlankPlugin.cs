using System;
using System.Linq;
using PointBlank.API.Collections;
using PointBlank.Services.PluginManager;
using UnityEngine;

namespace PointBlank.API.Plugins
{
    /// <summary>
    /// Used to specify the entrypoint of the plugin
    /// </summary>
    public abstract class PointBlankPlugin : MonoBehaviour
    {
        #region Variables
        private TranslationList _Translations = null;
        private ConfigurationList _Configurations = null;
        #endregion

        #region Properties
        /// <summary>
        /// The translations for the plugin
        /// </summary>
        public TranslationList Translations
        {
            get
            {
                if (_Translations == null)
                    _Translations = DefaultTranslations;
                return _Translations;
            }
        }

        /// <summary>
        /// The configurations for the plugin
        /// </summary>
        public ConfigurationList Configurations
        {
            get
            {
                if (_Configurations == null)
                    _Configurations = DefaultConfigurations;
                return _Configurations;
            }
        }
        #endregion

        #region Abstract Properties
        /// <summary>
        /// The current version of the plugin
        /// </summary>
        public abstract string Version { get; }
        #endregion

        #region Virtual Properties
        /// <summary>
        /// The translations for the plugin
        /// </summary>
        public virtual TranslationList DefaultTranslations => new TranslationList();

        /// <summary>
        /// The configurations for the plugin
        /// </summary>
        public virtual ConfigurationList DefaultConfigurations => new ConfigurationList();

        /// <summary>
        /// The latest version of the plugin(for auto-update)(Leave null if you don't want a version check)
        /// </summary>
        public virtual string VersionURL => null;

        /// <summary>
        /// The latest build of the plugin(for auto-update)(Leave null if you don't want an auto-update system)
        /// </summary>
        public virtual string BuildURL => null;
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

        #region Functions
        /// <summary>
        /// Easy translation with string formatting
        /// </summary>
        /// <param name="key">The translation key</param>
        /// <param name="data">The arguments for string formatting</param>
        /// <returns>The formatted message</returns>
        public string Translate(string key, params object[] data) => (Translations.ContainsKey(key) ? string.Format(Translations[key], data) : "#" + key);

        /// <summary>
        /// Easy to use configuration value extractor
        /// </summary>
        /// <typeparam name="T">The configuration value type</typeparam>
        /// <param name="key">The key of the configuration value</param>
        /// <returns>The configuration value with specified type</returns>
        public T Configure<T>(string key) => (T)Configurations[key];

        /// <summary>
        /// Gets the plugin instance based on any instance of any class inside the plugin dll
        /// </summary>
        /// <param name="pluginObject">The instance of any class inside the plugin dll</param>
        /// <returns>The plugin instance</returns>
        public static PointBlankPlugin GetInstance(object pluginObject)
        {
            PluginWrapper wrapper = PluginManager.Plugins.FirstOrDefault(a => a.PluginAssembly == pluginObject.GetType().Assembly);

            return (wrapper == null ? null : wrapper.PluginClass);
        }
        /// <summary>
        /// Gets the plugin instance based on any type of any class inside the plugin dll
        /// </summary>
        /// <param name="type">The type of any class inside the plugin dll</param>
        /// <returns>The plugin instance</returns>
        public static PointBlankPlugin GetInstance(Type type)
        {
            PluginWrapper wrapper = PluginManager.Plugins.FirstOrDefault(a => a.PluginAssembly == type.Assembly);

            return (wrapper == null ? null : wrapper.PluginClass);
        }
        /// <summary>
        /// Gets the plugin instance based on the plugin class provided
        /// </summary>
        /// <typeparam name="T">The plugin class</typeparam>
        /// <returns>The plugin instance</returns>
        public static T GetInstance<T>() where T : PointBlankPlugin
        {
            PluginWrapper wrapper = PluginManager.Plugins.FirstOrDefault(a => a.PluginClass.GetType() == typeof(T));

            return (wrapper == null ? null : (T)wrapper.PluginClass);
        }
        #endregion
    }
}
