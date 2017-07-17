using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Commands
{
    /// <summary>
    /// All functions for managing commands
    /// </summary>
    public static class CommandManager
    {
        #region Variables
        private static Dictionary<PointBlankCommandAttribute, PointBlankCommand> _Commands = new Dictionary<PointBlankCommandAttribute, PointBlankCommand>();
        #endregion

        #region Properties
        /// <summary>
        /// List of commands
        /// </summary>
        public static PointBlankCommand[] Commands => _Commands.Values.ToArray();
        #endregion

        #region Functions
        /// <summary>
        /// Gets a command using a class
        /// </summary>
        /// <typeparam name="T">The class to get the command with</typeparam>
        /// <returns>The command</returns>
        public static T GetCommand<T>() where T : PointBlankCommand => (T)Commands.FirstOrDefault(a => a.GetType() == typeof(T));

        /// <summary>
        /// Gets a command based on type
        /// </summary>
        /// <param name="Class">The type to find the command by</param>
        /// <returns>The command</returns>
        public static PointBlankCommand GetCommand(Type Class) => Commands.FirstOrDefault(a => a.GetType() == Class);

        /// <summary>
        /// Enables a command
        /// </summary>
        /// <param name="command">The command to enable</param>
        public static void EnableCommand(PointBlankCommand command)
        {
            CommandWrapper wrapper = CM.Commands.Values.FirstOrDefault(a => a.CommandClass == command);

            wrapper?.Enable();
        }

        /// <summary>
        /// Disables a command
        /// </summary>
        /// <param name="command">The command to disable</param>
        public static void DisableCommand(PointBlankCommand command)
        {
            CommandWrapper wrapper = CM.Commands.Values.FirstOrDefault(a => a.CommandClass == command);

            wrapper?.Disable();
        }
        #endregion
    }
}
