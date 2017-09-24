using System.Linq;
using PointBlank.Services.IPCManager;

namespace PointBlank.API.IPC
{
    /// <summary>
    /// Inter-process communication library
    /// </summary>
    public static class PointBlankIpcManager
    {
        #region Properties
        /// <summary>
        /// List of all Inter-Process communication keys
        /// </summary>
        public static string[] IpcKeys => IpcManager.Ipc.Keys.ToArray();

        /// <summary>
        /// The current method used for IPC
        /// </summary>
        public static EipcType IpcType { get; set; } = EipcType.Console;
        #endregion

        #region Functions
        /// <summary>
        /// Adds a key to the IPC
        /// </summary>
        /// <param name="key">The key to add</param>
        /// <param name="value">The default value of the key</param>
        public static void AddKey(string key, string value)
        {
            if (IpcManager.Ipc.ContainsKey(key))
                return;

            IpcManager.Ipc.Add(key, value);
            IpcEvents.RunKeyAdded(key);
        }
        /// <summary>
        /// Removes a key from the IPC
        /// </summary>
        /// <param name="key">The key to remove</param>
        public static void RemoveKey(string key)
        {
            if (!IpcManager.Ipc.ContainsKey(key))
                return;

            IpcManager.Ipc.Remove(key);
            IpcEvents.RunKeyRemoved(key);
        }

        /// <summary>
        /// Changes a key's value
        /// </summary>
        /// <param name="key">The key to change</param>
        /// <param name="value">The new value of the key</param>
        public static void SetValue(string key, string value)
        {
            if (!IpcManager.Ipc.ContainsKey(key))
                return;

            IpcManager.Ipc[key] = value;
            IpcEvents.RunKeyValueChanged(key, value);
        }
        /// <summary>
        /// Gets the value of an IPC key
        /// </summary>
        /// <param name="key">The key to get the value from</param>
        public static string GetValue(string key)
        {
            if (!IpcManager.Ipc.ContainsKey(key))
                return null;

            return IpcManager.Ipc[key];
        }
        #endregion

        #region Event Functions
        /// <summary>
        /// This is an event! You call this every time something is printed into the console!
        /// WARNING: Do not hook to the console output as it will cause issues!
        /// </summary>
        /// <param name="text">The printed text</param>
        public static void HookOutput(string text) => IpcManager.OnOutput(text);
        #endregion
    }
}
