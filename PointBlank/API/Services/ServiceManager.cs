using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using PointBlank.Framework.Permissions.Ring;

namespace PointBlank.API.Services
{
    /// <summary>
    /// The service manager class allows you to manage services at ease
    /// </summary>
    [RingPermission(SecurityAction.Demand, ring = RingPermissionRing.None)]
    public static class ServiceManager
    {
        #region Properties
        /// <summary>
        /// Returns a list of the current running services
        /// </summary>
        public static string[] RunningServices => Enviroment.services.Where(a => a.Value.Enabled).Select(a => a.Key) as string[];

        /// <summary>
        /// Returns a list of the current stopped services
        /// </summary>
        public static string[] StoppedServices => Enviroment.services.Where(a => !a.Value.Enabled).Select(a => a.Key) as string[];

        /// <summary>
        /// Returns a list of all the current services
        /// </summary>
        public static string[] AllServices => Enviroment.services.Select(a => a.Key) as string[];
        #endregion

        #region Functions
        public static void Reload()
        {

        }

        public static void Reload(string serviceName)
        {

        }

        /// <summary>
        /// Attempts to stop a running service
        /// </summary>
        /// <param name="serviceName">The target service name</param>
        /// <returns>If the service was successfully stopped</returns>
        public static bool StopService(string serviceName) => Enviroment.services[serviceName].Stop();

        /// <summary>
        /// Attempts to start a stopped service
        /// </summary>
        /// <param name="serviceName">The target service name</param>
        /// <returns>If the service was successfully started</returns>
        public static bool StartService(string serviceName) => Enviroment.services[serviceName].Start();
        #endregion
    }
}
