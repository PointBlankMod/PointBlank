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

        /// <summary>
        /// The current method used for IPC
        /// </summary>
        public static EIPCType IPCType { get; set; } = EIPCType.CONSOLE;
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

        #region Event Functions
        /// <summary>
        /// This is an event! You call this every time something is printed into the console!
        /// WARNING: Do not hook to the console output as it will cause issues!
        /// </summary>
        /// <param name="text">The printed text</param>
        public static void HookOutput(string text) => IPCM.OnOutput(text);
        #endregion
    }
}
