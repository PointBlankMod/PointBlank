using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Services
{
    /// <summary>
    /// The service manager class allows you to manage services at ease
    /// </summary>
    public static class PointBlankServiceManager
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

        /// <summary>
        /// Attempts to get the service by name and return the class
        /// </summary>
        /// <param name="serviceName">The service name to query for</param>
        /// <returns>The service class/instance</returns>
        public static PointBlankService GetService(string serviceName) => Enviroment.services[serviceName].ServiceClass;
        #endregion
    }
}
