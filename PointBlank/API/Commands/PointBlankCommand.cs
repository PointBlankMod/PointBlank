using System.Linq;
using PointBlank.API.Plugins;
using PointBlank.API.Player;
using PointBlank.API.Permissions;
using PointBlank.Services.CommandManager;

namespace PointBlank.API.Commands
{
    /// <summary>
    /// Custom command class
    /// </summary>
    public abstract class PointBlankCommand
    {
        #region Properties
        /// <summary>
        /// The commands used to execute this command
        /// </summary>
        public string[] Commands => CommandManager.Commands.FirstOrDefault(a => a.CommandClass == this).Commands;

        /// <summary>
        /// The permissions needed to execute the command
        /// </summary>
        public PointBlankPermission Permission => CommandManager.Commands.FirstOrDefault(a => a.CommandClass == this).Permission;

        /// <summary>
        /// The cooldown needed to execute the command
        /// </summary>
        public int Cooldown => (Permission.Cooldown == null ? -1 : (int)Permission.Cooldown);

        /// <summary>
        /// Is the command enabled
        /// </summary>
        public bool Enabled => CommandManager.Commands.FirstOrDefault(a => a.CommandClass == this).Enabled;
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
        /// The name of the command
        /// </summary>
        public virtual string Name => GetType().Name;

        /// <summary>
        /// The minimum amount of parameters required for the command
        /// </summary>
        public virtual int MinimumParams => 0;

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
        /// <param name="args">The arguments the player inputted</param>
        /// <param name="executor">The player executing the command</param>
        public abstract void Execute(PointBlankPlayer executor, string[] args);
        #endregion

        #region Functions
        /// <summary>
        /// Translates a key and data to text depending on the translation
        /// </summary>
        /// <param name="key">The key of the translation</param>
        /// <param name="data">The data to modify the translation</param>
        /// <returns>The translated text</returns>
        public string Translate(string key, params object[] data) => PointBlankPlugin.GetInstance(this).Translate(key, data);

        /// <summary>
        /// Easy to use configuration value extractor
        /// </summary>
        /// <typeparam name="T">The configuration value type</typeparam>
        /// <param name="key">The key of the configuration value</param>
        /// <returns>The configuration value with specified type</returns>
        public T Configure<T>(string key) => PointBlankPlugin.GetInstance(this).Configure<T>(key);
        #endregion
    }
}
