using PointBlank.API.Implements;

namespace PointBlank.API.Extension
{
    /// <summary>
    /// Used for extension hooking
    /// </summary>
    public static class ExtensionEvents
    {
        #region Handlers
        #endregion

        #region Events
        /// <summary>
        /// Called when a framework tick happens
        /// </summary>
        public static event VoidHandler OnFrameworkTick;
        /// <summary>
        /// Called when an API tick happens
        /// </summary>
        public static event VoidHandler OnAPITick;
        #endregion

        #region Functions
        internal static void RunFrameworkTick() => OnFrameworkTick?.Invoke();
        internal static void RunAPITick() => OnAPITick?.Invoke();
        #endregion
    }
}
