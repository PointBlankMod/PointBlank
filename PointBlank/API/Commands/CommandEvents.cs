using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Unturned.Player;

namespace PointBlank.API.Commands
{
    /// <summary>
    /// Class for command events
    /// </summary>
    public static class CommandEvents
    {
        #region Handlers
        /// <summary>
        /// Handler for command enable/disable
        /// </summary>
        /// <param name="command">The affected command</param>
        public delegate void CommandStatusChangedHandler(PointBlankCommand command);

        /// <summary>
        /// Handler for command calls
        /// </summary>
        /// <param name="command">The affected command</param>
        /// <param name="args">The arguments passed</param>
        /// <param name="allowExecute">Should the command be executed</param>
        /// <param name="executor">The user that executed the command(null if it was the server)</param>
        public delegate void CommandCalledHandler(PointBlankCommand command, string[] args, UnturnedPlayer executor, ref bool allowExecute);
        #endregion

        #region Events
        /// <summary>
        /// Called when a command is enabled
        /// </summary>
        public static event CommandStatusChangedHandler OnCommandEnabled;
        /// <summary>
        /// Called when a command is disabled
        /// </summary>
        public static event CommandStatusChangedHandler OnCommandDisabled;

        /// <summary>
        /// Called when a command is executed
        /// </summary>
        public static event CommandCalledHandler OnCommandExecuted;
        #endregion

        #region Functions
        internal static void RunCommandEnable(PointBlankCommand command)
        {
            if (OnCommandEnabled == null)
                return;

            OnCommandEnabled(command);
        }
        internal static void RunCommandDisable(PointBlankCommand command)
        {
            if (OnCommandDisabled == null)
                return;

            OnCommandDisabled(command);
        }

        internal static void RunCommandExecute(PointBlankCommand command, string[] args, UnturnedPlayer executor, ref bool allowExecute)
        {
            if (OnCommandExecuted == null)
                return;

            OnCommandExecuted(command, args, executor, ref allowExecute);
        }
        #endregion
    }
}
