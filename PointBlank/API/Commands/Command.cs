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

        /// <summary>
        /// The permissions needed to execute the command
        /// </summary>
        public string Permission
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// The cooldown needed to execute the command
        /// </summary>
        public int Cooldown
        {
            get
            {
                return -1;
            }
        }
        #endregion

        #region Abstract Properties
        /// <summary>
        /// If the player types any of the commands into the console it will run this command
        /// </summary>
        public abstract string[] DefaultCommands { get; }

        /// <summary>
        /// The translation key for the command help message
        /// </summary>
        public abstract string HelpTranslationKey { get; }

        /// <summary>
        /// The translation key for the command usage message
        /// </summary>
        public abstract string UsageTranslationKey { get; }

        /// <summary>
        /// The permission needed to run the command
        /// </summary>
        public abstract string DefaultPermission { get; }
        #endregion

        #region Virtual Properties
        /// <summary>
        /// The default cooldown(-1 to not override cooldown)
        /// </summary>
        public virtual int DefaultCooldown
        {
            get
            {
                return -1;
            }
        }

        /// <summary>
        /// Can the command be executed while the server is running
        /// </summary>
        public virtual bool AllowRuntime
        {
            get
            {
                return true;
            }
        }
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
