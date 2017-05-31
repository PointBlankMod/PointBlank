using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using PointBlank.Framework.Permissions.Ring;

namespace PointBlank.API.Services
{
    /// <summary>
    /// Used to specify a service
    /// </summary>
    [RingPermission(SecurityAction.Demand, ring = RingPermissionRing.Extensions)]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ServiceAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// The name of the service
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Will the service autostart
        /// </summary>
        public bool AutoStart { get; internal set; }

        /// <summary>
        /// Will any services with the same name be replaced
        /// </summary>
        public bool Replace { get; internal set; }
        #endregion

        /// <summary>
        /// Used to specify a service
        /// </summary>
        /// <param name="name">The name of the service</param>
        /// <param name="autostart">Should the service autostart</param>
        /// <param name="replace">Replace any existing services with the same name</param>
        public ServiceAttribute(string name, bool autostart, bool replace = false)
        {
            this.Name = name;
            this.AutoStart = autostart;
            this.Replace = replace;
        }
    }
}
