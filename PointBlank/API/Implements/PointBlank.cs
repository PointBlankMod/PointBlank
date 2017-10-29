
using System.Collections.Generic;

namespace PointBlank.API.Implements
{
    public static class PointBlank
    {
        #region Metadata
        /// <summary>
        /// Returns the metadata value with assigned type
        /// </summary>
        /// <typeparam name="T">The type to assign the value to</typeparam>
        /// <param name="metadata">The metadata to search in</param>
        /// <param name="key">The key to search for</param>
        /// <returns>The value with assigned type</returns>
        public static T Get<T>(this Dictionary<string, object> metadata, string key) => (T)metadata[key];
        #endregion
    }
}
