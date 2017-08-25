namespace PointBlank.API.Services
{
    /// <summary>
    /// All the service events
    /// </summary>
    public static class PointBlankServiceEvents
    {
        #region Handlers
        /// <summary>
        /// Called when the service is being started
        /// </summary>
        /// <param name="service">The service</param>
        public delegate void ServiceStartHandler(PointBlankService service);
        /// <summary>
        /// Called when the service is loaded/started
        /// </summary>
        /// <param name="service">The service</param>
        public delegate void ServiceLoadedHandler(PointBlankService service);

        /// <summary>
        /// Called when the service is being stopped
        /// </summary>
        /// <param name="service">The service</param>
        public delegate void ServiceStopHandler(PointBlankService service);
        /// <summary>
        /// Called when the service is unloaded/stopped
        /// </summary>
        /// <param name="service">THe service</param>
        public delegate void ServiceUnloadedHandler(PointBlankService service);
        #endregion

        #region Events
        /// <summary>
        /// Called when the service is being started
        /// </summary>
        public static event ServiceStartHandler OnServiceStart;
        /// <summary>
        /// Called when the service is loaded/started
        /// </summary>
        public static event ServiceLoadedHandler OnServiceLoaded;

        /// <summary>
        /// Called when the service is being stopped
        /// </summary>
        public static event ServiceStopHandler OnServiceStop;
        /// <summary>
        /// Called when the service is unloaded/stopped
        /// </summary>
        public static event ServiceUnloadedHandler OnServiceUnloaded;
        #endregion

        #region Functions
        internal static void RunServiceStart(PointBlankService service) => OnServiceStart?.Invoke(service);

        internal static void RunServiceLoaded(PointBlankService service) => OnServiceLoaded?.Invoke(service);

        internal static void RunServiceStop(PointBlankService service) => OnServiceStop?.Invoke(service);

        internal static void RunServiceUnloaded(PointBlankService service) => OnServiceUnloaded?.Invoke(service);

        #endregion
    }
}
