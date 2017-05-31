using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using PointBlank.Framework.Permissions.Ring;
using UnityEngine;

namespace PointBlank.API.Services
{
    /// <summary>
    /// Used to specify a service based class
    /// </summary>
    [RingPermission(SecurityAction.Demand, ring = RingPermissionRing.Extensions)]
    public abstract class Service : MonoBehaviour
    {
        #region Abstract Functions
        /// <summary>
        /// Called when the service is loading
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// Called when the service is unloading
        /// </summary>
        public abstract void Unload();
        #endregion
    }
}
