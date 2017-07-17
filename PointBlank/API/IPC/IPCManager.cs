using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPCM = PointBlank.Services.IPCManager.IPCManager;

namespace PointBlank.API.IPC
{
    /// <summary>
    /// Inter-process communication library
    /// </summary>
    public static class IPCManager
    {
        #region Properties
        /// <summary>
        /// List of all Inter-Process communication keys
        /// </summary>
        public static string[] IPCKeys => IPCM.IPC.Keys.ToArray();
        #endregion

        #region Functions
        /// <summary>
        /// Adds a key to the IPC
        /// </summary>
        /// <param name="key">The key to add</param>
        /// <param name="value">The default value of the key</param>
        public static void AddKey(string key, string value)
        {
            if (IPCM.IPC.ContainsKey(key))
                return;

            IPCM.IPC.Add(key, value);
            IPCEvents.RunKeyAdded(key);
        }
        /// <summary>
        /// Removes a key from the IPC
        /// </summary>
        /// <param name="key">The key to remove</param>
        public static void RemoveKey(string key)
        {
            if (!IPCM.IPC.ContainsKey(key))
                return;

            IPCM.IPC.Remove(key);
            IPCEvents.RunKeyRemoved(key);
        }

        /// <summary>
        /// Changes a key's value
        /// </summary>
        /// <param name="key">The key to change</param>
        /// <param name="value">The new value of the key</param>
        public static void SetValue(string key, string value)
        {
            if (!IPCM.IPC.ContainsKey(key))
                return;

            IPCM.IPC[key] = value;
            IPCEvents.RunKeyValueChanged(key, value);
        }
        /// <summary>
        /// Gets the value of an IPC key
        /// </summary>
        /// <param name="key">The key to get the value from</param>
        public static string GetValue(string key)
        {
            if (!IPCM.IPC.ContainsKey(key))
                return null;

            return IPCM.IPC[key];
        }
        #endregion
    }
}
