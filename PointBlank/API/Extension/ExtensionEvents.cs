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
<<<<<<< HEAD
        public static event VoidHandler OnAPITick;
=======
        public static event VoidHandler OnApiTick;
>>>>>>> master
        #endregion

        #region Functions
        internal static void RunFrameworkTick() => OnFrameworkTick?.Invoke();
<<<<<<< HEAD
        internal static void RunAPITick() => OnAPITick?.Invoke();
=======
        internal static void RunApiTick() => OnApiTick?.Invoke();
>>>>>>> master
        #endregion
    }
}
