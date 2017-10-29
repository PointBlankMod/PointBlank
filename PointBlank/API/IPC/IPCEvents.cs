namespace PointBlank.API.IPC
{
    /// <summary>
    /// Events for the inter-process communication library
    /// </summary>
    public static class IPCEvents
    {
        #region Handlers
        /// <summary>
        /// Handles all key list based events
        /// </summary>
        /// <param name="key">The affected key</param>
        public delegate void KeyListChangedHandler(string key);

        /// <summary>
        /// Handles all key value changes
        /// </summary>
        /// <param name="key">The affected key</param>
        /// <param name="value">The new value of the key</param>
        public delegate void KeyValueChangedHandler(string key, string value);
        #endregion

        #region Events
        /// <summary>
        /// Called when a key is added
        /// </summary>
        public static event KeyListChangedHandler OnKeyAdded;
        /// <summary>
        /// Called when a key is removed
        /// </summary>
        public static event KeyListChangedHandler OnKeyRemoved;

        /// <summary>
        /// Called when a key value is changed
        /// </summary>
        public static event KeyValueChangedHandler OnKeyValueChanged;
        #endregion

        #region Functions
        internal static void RunKeyAdded(string key) => OnKeyAdded?.Invoke(key);
        internal static void RunKeyRemoved(string key) => OnKeyRemoved?.Invoke(key);

        internal static void RunKeyValueChanged(string key, string value) => OnKeyValueChanged?.Invoke(key, value);
        #endregion
    }
}
