using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PointBlank.API.Services
{
    /// <summary>
    /// Used to specify a service based class
    /// </summary>
    public abstract class PointBlankService : MonoBehaviour
    {
        #region Properties
        /// <summary>
        /// Can the service be replaced
        /// </summary>
        public bool Replacable => !Replace;

        /// <summary>
        /// The full name of the service
        /// </summary>
        public string FullName => this.GetType().Name + "." + Name;
        #endregion

        #region Virtual Properties
        /// <summary>
        /// The name of the service
        /// </summary>
        public virtual string Name => this.GetType().Name;

        /// <summary>
        /// Should the service be started on load
        /// </summary>
        public virtual bool AutoStart { get; set; } = true;

        /// <summary>
        /// Does the service replace an existing service
        /// </summary>
        public virtual bool Replace { get; set; } = false;

        /// <summary>
        /// The launch index specifies when the service is launched(the higher it is the slower it will launch)
        /// </summary>
        public virtual int LaunchIndex => 0;
        #endregion

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
