using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
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
        protected Dictionary<string, string> Translations { get; private set; }

        /// <summary>
        /// Gets/Sets the translation text using the key provided
        /// </summary>
        /// <param name="key">The key to find the translation</param>
        /// <returns>The translation text</returns>
        public string this[string key]
        {
            get => Translations[key];
            set => Translations[key] = value;
        }

        /// <summary>
        /// Gets/Sets the translation KeyValuePair using the index provided
        /// </summary>
        /// <param name="index">The index to find the translation</param>
        /// <returns>The translation KeyValuePair</returns>
        public KeyValuePair<string, string> this[int index]
        {
            get => Translations.ElementAt(index);
            set => Translations[Translations.ElementAt(index).Key] = value.Value;
        }

        /// <summary>
        /// Returns the amount of items in the translation list
        /// </summary>
        public int Count => Translations.Count;

        public object SyncRoot => throw new NotImplementedException();

        public bool IsSynchronized => throw new NotImplementedException();
        #endregion

        /// <summary>
        /// Custom collection for tranlsations
        /// </summary>
        public TranslationList()
        {
            StackTrace stack = new StackTrace(false);
            if (stack.FrameCount <= 0)
            {
                Translations = new Dictionary<string, string>();
                return;
            }
            MethodBase jumpMethod = stack.GetFrame(1).GetMethod();

            if (!Memory.TranslationCollection.ContainsKey(jumpMethod))
            {
                Translations = new Dictionary<string, string>();
                Memory.TranslationCollection.Add(jumpMethod, this);
                return;
            }
            Translations = Memory.TranslationCollection[jumpMethod].Translations;
        }

        #region Collection Functions
        /// <summary>
        /// Adds a translation entry using the key and value
        /// </summary>
        /// <param name="key">Key to save it as</param>
        /// <param name="value">Value of the text</param>
        public void Add(string key, string value)
        {
            if (!Translations.ContainsKey(key))
                Translations.Add(key, value);
        }
        /// <summary>
        /// Adds a translation entry using the KeyValuePair
        /// </summary>
        /// <param name="kvp">The KeyValuePair to extract the data from</param>
        public void Add(KeyValuePair<string, string> kvp) => this.Add(kvp.Key, kvp.Value);

        /// <summary>
        /// Removes a translation entry using the key
        /// </summary>
        /// <param name="key">Key of translation entry</param>
        public void Remove(string key)
        {
            if (Translations.ContainsKey(key))
                Translations.Remove(key);
        }
        /// <summary>
        /// Removes a translation entry using the index
        /// </summary>
        /// <param name="index">Index of the entry</param>
        public void RemoveAt(int index) => this.Remove(this[index].Key);

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
        /// Does the translation key exist
        /// </summary>
        /// <param name="key">The key of the translation</param>
        /// <returns>If the translation key exists</returns>
        public bool ContainsKey(string key) => Translations.ContainsKey(key);

        /// <summary>
        /// Gets the enumerator and returns it
        /// </summary>
        /// <returns>Enumerator for translation list</returns>
        IEnumerator IEnumerable.GetEnumerator() => Translations.GetEnumerator();

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

        /// <summary>
        /// Translates text based on the key and values
        /// </summary>
        /// <param name="key">The key to find the text</param>
        /// <param name="values">The values to insert into the text</param>
        /// <returns>The translated text</returns>
        public string Translate(string key, params object[] values) => string.Format(this[key], values);
        #endregion

        #region SubClasses
        private static class Memory
        {
            public static Dictionary<MethodBase, TranslationList> TranslationCollection = new Dictionary<MethodBase, TranslationList>();
        }
        #endregion
    }
}
