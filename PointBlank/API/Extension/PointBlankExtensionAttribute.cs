using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Extension
{
    /// <summary>
    /// Adding this attribute to your assembly makes the assembly an extension
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class PointBlankExtensionAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// Should the InternalObjects be loaded when the Extension is loaded
        /// Disable this only if you plan on modifying the Internals of PointBlank
        /// </summary>
        public bool LoadInternals { get; set; } = true;

        /// <summary>
        /// Setting this to true will prevent other systems from loading or executing any part of the Extension
        /// This only runs Internals(if enabled) and nothing else this also includes not running the Extension itself
        /// Enable this only if you plan on modifying the Internals of PointBlank
        /// </summary>
        public bool RawExtension { get; set; } = false;
        #endregion
    }
}
