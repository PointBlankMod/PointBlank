using System;
using System.Linq;
using PointBlank.Services.CommandManager;

namespace PointBlank.API.Commands
{
    /// <summary>
    /// All functions for managing commands
    /// </summary>
    public static class PointBlankCommandManager
    {
        #region Properties
        /// <summary>
        /// List of commands
        /// </summary>
        public static PointBlankCommand[] Commands => CommandManager.Commands.Select(a => a.CommandClass).ToArray();
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
            CommandWrapper wrapper = CommandManager.Commands.FirstOrDefault(a => a.CommandClass == command);

            wrapper?.Enable();
        }

        /// <summary>
        /// Disables a command
        /// </summary>
        /// <param name="command">The command to disable</param>
        public static void DisableCommand(PointBlankCommand command)
        {
            CommandWrapper wrapper = CommandManager.Commands.FirstOrDefault(a => a.CommandClass == command);

            wrapper?.Disable();
        }

        /// <summary>
        /// Loads a command into the commands list
        /// </summary>
        /// <param name="_class">The command class type</param>
        public static void LoadCommand(Type _class)
        {
            CommandManager cmd = (CommandManager)PBEnvironment.services["CommandManager.CommandManager"].ServiceClass;

            cmd.LoadCommand(_class);
        }
        /// <summary>
        /// Loads a command into the commands list
        /// </summary>
        /// <param name="command">The command to load</param>
        public static void LoadCommand(PointBlankCommand command)
        {
            CommandManager cmd = (CommandManager)PBEnvironment.services["CommandManager.CommandManager"].ServiceClass;

            cmd.LoadCommand(command);
        }

        /// <summary>
        /// Unloads a command from the commands list
        /// </summary>
        /// <param name="_class">The command class type to unload</param>
        public static void UnloadCommand(Type _class)
        {
            CommandManager cmd = (CommandManager)PBEnvironment.services["CommandManager.CommandManager"].ServiceClass;

            cmd.UnloadCommand(_class);
        }
        /// <summary>
        /// Unloads a command from the commands list
        /// </summary>
        /// <param name="command">The command to unload</param>
        public static void UnloadCommand(PointBlankCommand command)
        {
            CommandManager cmd = (CommandManager)PBEnvironment.services["CommandManager.CommandManager"].ServiceClass;

            cmd.UnloadCommand(command);
        }

        /// <summary>
        /// Emulates command execution
        /// </summary>
        /// <param name="command">The command to execute</param>
        public static ECommandRunError ExecuteCommand(string command, Player.PointBlankPlayer executor)
        {
            CommandManager cmd = (CommandManager)PBEnvironment.services["CommandManager.CommandManager"].ServiceClass;

            return cmd.ExecuteCommand(command, executor);
        }
        #endregion
    }
}
