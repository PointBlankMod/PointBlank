using PointBlank.API.Plugins;
using UnityEngine;

namespace PointBlank.API.Player
{
    /// <summary>
    /// The attachable component for the player
    /// </summary>
    public class PointBlankPlayerComponent : MonoBehaviour
    {
        #region Mono Functions
        void Awake()
        {
            DontDestroyOnLoad(this);
        }
        #endregion

        #region Virtual Functions
        /// <summary>
        /// Called for loading the component
        /// </summary>
        public virtual void Load() { }

        /// <summary>
        /// Called for unloading the component
        /// </summary>
        public virtual void Unload() { }
        #endregion

        #region Functions
        /// <summary>
        /// Translates a key and data to text depending on the translation
        /// </summary>
        /// <param name="key">The key of the translation</param>
        /// <param name="data">The data to modify the translation</param>
        /// <returns>The translated text</returns>
        public string Translate(string key, params object[] data) => PointBlankPlugin.GetInstance(this).Translate(key, data);

        /// <summary>
        /// Easy to use configuration value extractor
        /// </summary>
        /// <typeparam name="T">The configuration value type</typeparam>
        /// <param name="key">The key of the configuration value</param>
        /// <returns>The configuration value with specified type</returns>
        public T Configure<T>(string key) => PointBlankPlugin.GetInstance(this).Configure<T>(key);
        #endregion
    }
}
