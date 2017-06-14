using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using PointBlank.Framework.Permissions.Ring;

namespace PointBlank.API.Commands
{
    /// <summary>
    /// Create a custom command
    /// </summary>
    [RingPermission(SecurityAction.Demand, ring = RingPermissionRing.None)]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// The command name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The minimum amount of paramaters that can be used
        /// </summary>
        public int MinParams { get; private set; }
        #endregion

        /// <summary>
        /// Create a custom command
        /// </summary>
        /// <param name="name">The name of the command</param>
        /// <param name="minparams">The minimum number of paramaters/arguments needed for the command</param>
        public CommandAttribute(string name, int minparams)
        {
            this.Name = name; // Set the name
            this.MinParams = minparams; // Set the minimum params
            this.Description = description; // Set the command description
        }
    }
}
