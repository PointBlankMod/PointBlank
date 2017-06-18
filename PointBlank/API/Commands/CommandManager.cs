using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CM = PointBlank.Services.CommandManager.CommandManager;
using PointBlank.Services.CommandManager;

namespace PointBlank.API.Commands
{
    /// <summary>
    /// All functions for managing commands
    /// </summary>
    public static class CommandManager
    {
        #region Properties
        /// <summary>
        /// List of commands
        /// </summary>
        public static PointBlankCommand[] Commands => CM.Commands.Values.Select(a => a.CommandClass).ToArray();
        #endregion

        #region Functions
        /// <summary>
        /// Gets a command using a class
        /// </summary>
        /// <typeparam name="T">The class to get the command with</typeparam>
        /// <returns>The command</returns>
        public static T GetCommand<T>() where T : PointBlankCommand
        {
            CommandWrapper wrapper = CM.Commands.Values.FirstOrDefault(a => a.Class == typeof(T));

            if (wrapper == null)
                return null;
            return (T)wrapper.CommandClass;
        }

        /// <summary>
        /// Gets a command based on type
        /// </summary>
        /// <param name="Class">The type to find the command by</param>
        /// <returns>The command</returns>
        public static PointBlankCommand GetCommand(Type Class)
        {
            CommandWrapper wrapper = CM.Commands.Values.FirstOrDefault(a => a.Class == Class);

            if (wrapper == null)
                return null;
            return wrapper.CommandClass;
        }

        /// <summary>
        /// Enables a command
        /// </summary>
        /// <param name="command">The command to enable</param>
        public static void EnableCommand(PointBlankCommand command)
        {
            CommandWrapper wrapper = CM.Commands.Values.FirstOrDefault(a => a.CommandClass == command);

            if (wrapper == null)
                return;
            wrapper.Enable();
        }

        /// <summary>
        /// Disables a command
        /// </summary>
        /// <param name="command">The command to disable</param>
        public static void DisableCommand(PointBlankCommand command)
        {
            CommandWrapper wrapper = CM.Commands.Values.FirstOrDefault(a => a.CommandClass == command);

            if (wrapper == null)
                return;
            wrapper.Disable();
        }
        #endregion
    }
}
