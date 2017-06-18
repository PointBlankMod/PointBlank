using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using PointBlank.Framework.Permissions.Ring;
using PointBlank.API.Unturned.Player;
using CM = PointBlank.Services.CommandManager.CommandManager;

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
        /// The command instance
        /// </summary>
        public static Command Instance { get; internal set; }

        /// <summary>
        /// The commands used to execute this command
        /// </summary>
        public string[] Commands
        {
            get
            {
                return CM.Commands.FirstOrDefault(a => a.Value.CommandClass == this).Value.Commands;
            }
        }

        /// <summary>
        /// The permissions needed to execute the command
        /// </summary>
        public string Permission
        {
            get
            {
                return CM.Commands.FirstOrDefault(a => a.Value.CommandClass == this).Value.Permission;
            }
        }

        /// <summary>
        /// The cooldown needed to execute the command
        /// </summary>
        public int Cooldown
        {
            get
            {
                return CM.Commands.FirstOrDefault(a => a.Value.CommandClass == this).Value.Cooldown;
            }
        }

        /// <summary>
        /// Is the command enabled
        /// </summary>
        public bool Enabled
        {
            get
            {
                return CM.Commands.FirstOrDefault(a => a.Value.CommandClass == this).Value.Enabled;
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
        public abstract string Help { get; }

        /// <summary>
        /// The translation key for the command usage message
        /// </summary>
        public abstract string Usage { get; }

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
        /// At what state is the command allowed to be executed
        /// </summary>
        public virtual EAllowedServerState AllowedServerState => EAllowedServerState.BOTH;

        /// <summary>
        /// Who can execute the command
        /// </summary>
        public virtual EAllowedCaller AllowedCaller => EAllowedCaller.BOTH;
        #endregion

        #region Abstract Functions
        /// <summary>
        /// Called when the player executes the command
        /// </summary>
        /// <param name="arguments">The arguments the player inputted</param>
        public abstract void Execute(UnturnedPlayer executor, string[] args);
        #endregion

        public Command()
        {
            Instance = this;
        }
    }
}
