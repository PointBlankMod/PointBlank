using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using PointBlank.Framework.Permissions.Ring;

namespace PointBlank.API.Services
{
    /// <summary>
    /// All the service events
    /// </summary>
    [RingPermission(SecurityAction.Demand, ring = RingPermissionRing.None)]
    public static class ServiceEvents
    {
        #region Handlers
        /// <summary>
        /// Called when the service is being started
        /// </summary>
        /// <param name="service">The service</param>
        public delegate void ServiceStartHandler(Service service);
        /// <summary>
        /// Called when the service is loaded/started
        /// </summary>
        /// <param name="service">The service</param>
        public delegate void ServiceLoadedHandler(Service service);

        /// <summary>
        /// Called when the service is being stopped
        /// </summary>
        /// <param name="service">The service</param>
        public delegate void ServiceStopHandler(Service service);
        /// <summary>
        /// Called when the service is unloaded/stopped
        /// </summary>
        /// <param name="service">THe service</param>
        public delegate void ServiceUnloadedHandler(Service service);
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
        internal static void RunServiceStart(Service service)
        {
            if (OnServiceStart == null)
                return;

            OnServiceStart(service);
        }

        internal static void RunServiceLoaded(Service service)
        {
            if (OnServiceLoaded == null)
                return;

            OnServiceLoaded(service);
        }

        internal static void RunServiceStop(Service service)
        {
            if (OnServiceStop == null)
                return;

            OnServiceStop(service);
        }

        internal static void RunServiceUnloaded(Service service)
        {
            if (OnServiceUnloaded == null)
                return;

            OnServiceUnloaded(service);
        }
        #endregion
    }
}
