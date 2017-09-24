using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

namespace PointBlank.API.Collections
{
    /// <summary>
    /// Custom collection for configurations
    /// </summary>
    public class ConfigurationList : ICollection
    {
        #region Properties
        /// <summary>
        /// The list of the configurations
        /// </summary>
        public Dictionary<string, object> Configurations { get; protected set; }

        /// <summary>
        /// Gets/Sets the configuration object with the configuration name provided
        /// </summary>
        /// <param name="configurationName">The configuration name</param>
        /// <returns>The configuration object</returns>
        public object this[string configurationName]
        {
            get => !Configurations.ContainsKey(configurationName) ? null : Configurations[configurationName];
            set
            {
                if (Configurations.ContainsKey(configurationName))
                    Configurations[configurationName] = value;
                else
                    Configurations.Add(configurationName, value);
            }
        }

        /// <summary>
        /// Gets/Sets the configuration KeyValuePair using the index provided
        /// </summary>
        /// <param name="index">The index of the configuration</param>
        /// <returns>The KeyValuePair of the configuration</returns>
        public KeyValuePair<string, object> this[int index]
        {
            get => Configurations.ElementAt(index);
            set => Configurations[Configurations.ElementAt(index).Key] = value.Value;
        }

        /// <summary>
        /// Returns the amount of items in the configuration list
        /// </summary>
        public int Count => Configurations.Count;

        public object SyncRoot => throw new NotImplementedException();

        public bool IsSynchronized => throw new NotImplementedException();
        #endregion

        /// <summary>
        /// Custom collection for configurations
        /// </summary>
        public ConfigurationList() =>
            Configurations = new Dictionary<string, object>();

        #region Collection Functions
        /// <summary>
        /// Adds a configuration entry using the configuration name and configuration object
        /// </summary>
        /// <param name="configurationName">Configuration name</param>
        /// <param name="configuration">Configuration object</param>
        public void Add(string configurationName, object configuration)
        {
            if (!Configurations.ContainsKey(configurationName))
                Configurations.Add(configurationName, configuration);
        }
        /// <summary>
        /// Adds a configuration entry using the KeyValuePair
        /// </summary>
        /// <param name="kvp">KeyValuePair of the entry</param>
        public void Add(KeyValuePair<string, object> kvp) => this.Add(kvp.Key, kvp.Value);

        /// <summary>
        /// Removes a configuration entry using the configuration name
        /// </summary>
        /// <param name="configurationName">Configuration name</param>
        public void Remove(string configurationName)
        {
            if (Configurations.ContainsKey(configurationName))
                Configurations.Remove(configurationName);
        }
        /// <summary>
        /// Removes a configuration entry using the index
        /// </summary>
        /// <param name="index">Index of the entry</param>
        public void RemoveAt(int index) => this.Remove(this[index].Key);

        /// <summary>
        /// Adds a range of configurations using the KeyValuePair list
        /// </summary>
        /// <param name="list">KeyValuePair list</param>
        public void AddRange(IEnumerable<KeyValuePair<string, object>> list)
        {
            foreach(KeyValuePair<string, object> x in list)
                this.Add(x.Key, x.Value);
        }
        /// <summary>
        /// Adds a range of configurations using the configuration list
        /// </summary>
        /// <param name="list">Configuration list</param>
        public void AddRange(ConfigurationList list)
        {
            for (int i = 0; i < list.Count; i++)
                this.Add(list[i]);
        }

        /// <summary>
        /// Does the configuration key exist
        /// </summary>
        /// <param name="key">The configuration key</param>
        /// <returns>If the configuration key exists</returns>
        public bool ContainsKey(string key) => Configurations.ContainsKey(key);

        /// <summary>
        /// Gets the enumerator and returns it
        /// </summary>
        /// <returns>List enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator() => Configurations.GetEnumerator();

        /// <summary>
        /// Copies the KeyValuePairs into the array
        /// </summary>
        /// <param name="array">The target array</param>
        /// <param name="index">The start index</param>
        public void CopyTo(Array array, int index)
        {
            for (int i = 0; i < (this.Count - index); i++)
                array.SetValue(this[index + i], i);
        }

        /// <summary>
        /// Clears the list
        /// </summary>
        public void Clear() => Configurations.Clear();
        #endregion
    }
}
