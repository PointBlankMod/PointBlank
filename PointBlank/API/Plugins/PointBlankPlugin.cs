using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
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
        #region Properties
        /// <summary>
        /// The plugin instance
        /// </summary>
        public static PointBlankPlugin Instance
        {
            get
            {
                StackTrace stack = new StackTrace(false);
                Assembly asm = null;
                int count = 1;

                if(stack.FrameCount <= 0)
                    return null;
                while((asm == null || asm.Location == typeof(PointBlankPlugin).Assembly.Location) && count < stack.FrameCount)
                {
                    asm = stack.GetFrame(count).GetMethod().DeclaringType.Assembly;

                    if (asm.Location == typeof(PointBlankPlugin).Assembly.Location)
                        count++;
                }

                PluginWrapper wrapper = PluginManager.Plugins.FirstOrDefault(a => a.PluginAssembly.Location == asm.Location);
                return (wrapper == null ? null : wrapper.PluginClass);
            }
        }
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
        #endregion
    }
}
