using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using PointBlank.Framework.Permissions.Ring;

namespace PointBlank.API.Commands
{
    /// <summary>
    /// Custom command class
    /// </summary>
    [RingPermission(SecurityAction.Demand, ring = RingPermissionRing.None)]
    public abstract class Command
    {
        #region Properties
        /// <summary>
        /// The commands used to execute this command
        /// </summary>
        public string[] Commands
        {
            get
            {
                return new string[0];
            }
        }
        #endregion

        #region Abstract Properties
        /// <summary>
        /// If the player types any of the commands into the console it will run this command
        /// </summary>
        public abstract string[] DefaultCommands { get; }

        /// <summary>
        /// Displayed if the player wants more info on the command
        /// </summary>
        public abstract string Help { get; }
        #endregion

        #region Abstract Functions
        /// <summary>
        /// Called when the player executes the command
        /// </summary>
        /// <param name="arguments">The arguments the player inputted</param>
        public abstract void Execute(string[] arguments);
        #endregion
    }
}
