using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using PointBlank.Framework.Permissions.Ring;

namespace PointBlank.API.Collections
{
    /// <summary>
    /// Custom collection for tranlsations
    /// </summary>
    [RingPermission(SecurityAction.Demand, ring = RingPermissionRing.None)]
    public class TranslationList : ICollection
    {
        #region Properties
        /// <summary>
        /// The list of transaltions
        /// </summary>
        protected Dictionary<string, string> translations { get; private set; }

        /// <summary>
        /// Gets/Sets the translation text using the key provided
        /// </summary>
        /// <param name="key">The key to find the translation</param>
        /// <returns>The translation text</returns>
        public string this[string key]
        {
            get => translations[key];
            set => translations[key] = value;
        }

        /// <summary>
        /// Gets/Sets the translation KeyValuePair using the index provided
        /// </summary>
        /// <param name="index">The index to find the translation</param>
        /// <returns>The translation KeyValuePair</returns>
        public KeyValuePair<string, string> this[int index]
        {
            get => translations.ElementAt(index);
            set => translations[translations.ElementAt(index).Key] = value.Value;
        }

        /// <summary>
        /// Returns the amount of items in the translation list
        /// </summary>
        public int Count => translations.Count;

        public object SyncRoot => throw new NotImplementedException();

        public bool IsSynchronized => throw new NotImplementedException();
        #endregion

        /// <summary>
        /// Custom collection for tranlsations
        /// </summary>
        public TranslationList()
        {
            // Initialize the variables
            translations = new Dictionary<string, string>();
        }

        #region Collection Functions
        /// <summary>
        /// Adds a translation entry using the key and value
        /// </summary>
        /// <param name="key">Key to save it as</param>
        /// <param name="value">Value of the text</param>
        public void Add(string key, string value) => translations.Add(key, value);

        /// <summary>
        /// Adds a translation entry using the KeyValuePair
        /// </summary>
        /// <param name="kvp">The KeyValuePair to extract the data from</param>
        public void Add(KeyValuePair<string, string> kvp) => translations.Add(kvp.Key, kvp.Value);

        /// <summary>
        /// Removes a translation entry using the key
        /// </summary>
        /// <param name="key">Key of translation entry</param>
        public void Remove(string key) => translations.Remove(key);

        /// <summary>
        /// Removes a translation entry using the index
        /// </summary>
        /// <param name="index">Index of the entry</param>
        public void RemoveAt(int index) => translations.Remove(this[index].Key)

        /// <summary>
        /// Adds a range of translations using the KeyValuePair list
        /// </summary>
        /// <param name="list">The list of translations to add</param>
        public void AddRange(IEnumerable<KeyValuePair<string, string>> list)
        {
            foreach (KeyValuePair<string, string> x in list)
                this.Add(x.Key, x.Value);
        }

        /// <summary>
        /// Adds a range of translations using the Translation list
        /// </summary>
        /// <param name="list">The list of translations to add</param>
        public void AddRange(TranslationList list)
        {
            for (int i = 0; i < list.Count; i++)
                this.Add(list[i]);
        }

        /// <summary>
        /// Gets the enumerator and returns it
        /// </summary>
        /// <returns>Enumerator for translation list</returns>
        IEnumerator IEnumerable.GetEnumerator() => translations.GetEnumerator();

        /// <summary>
        /// Copies the KeyValuePairs to the array
        /// </summary>
        /// <param name="array">Copy destination</param>
        /// <param name="index">Start from index</param>
        public void CopyTo(Array array, int index)
        {
            for (int i = 0; i < (this.Count - index); i++)
                array.SetValue(this[index + i], i);
        }
        #endregion
    }
}
