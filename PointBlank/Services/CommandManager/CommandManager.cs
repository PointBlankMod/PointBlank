using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using PointBlank.API;
using PointBlank.API.Plugins;
using PointBlank.API.Services;
using PointBlank.API.Commands;
using PointBlank.API.DataManagment;
using PointBlank.API.Player;
using Newtonsoft.Json.Linq;
using PointBlank.Framework.Translations;
using PointBlank.API.Server;
using PointBlank.API.Extension;

namespace PointBlank.Services.CommandManager
{
    internal class CommandManager : PointBlankService
    {
        #region Info
        public static readonly string ConfigurationPath = PointBlankServer.ConfigurationsPath + "//Commands";
        #endregion

        #region Properties
        public static List<CommandWrapper> Commands { get; private set; }

        public UniversalData UniConfig { get; private set; }
        public JsonData JSONConfig { get; private set; }

        public override int LaunchIndex => 2;
        #endregion

        public override void Load()
        {
            // Setup variables
            Commands = new List<CommandWrapper>();
            UniConfig = new UniversalData(ConfigurationPath);
            JSONConfig = UniConfig.GetData(EDataType.JSON) as JsonData;

            // Setup events
            PointBlankPluginEvents.OnPluginLoaded += OnPluginLoaded;
            PointBlankPluginEvents.OnPluginUnloaded -= OnPluginUnloaded;

            // Run the code
            LoadConfig();
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies().Where(a => Attribute.GetCustomAttribute(a, typeof(PointBlankExtensionAttribute)) != null))
                foreach (Type tClass in asm.GetTypes())
                    if (tClass.IsClass)
                            LoadCommand(tClass);
        }

        public override void Unload()
        {
            // Unload events
            PointBlankPluginEvents.OnPluginLoaded -= OnPluginLoaded;
            PointBlankPluginEvents.OnPluginUnloaded -= OnPluginUnloaded;

            // Run the code
            SaveConfig();
        }

        #region Private Functions
        private void LoadConfig()
        {
            if (!UniConfig.CreatedNew)
                return;

            JSONConfig.Document.Add("Commands", new JArray());
            SaveConfig();
        }

        private void SaveConfig()
        {
            foreach(CommandWrapper wrapper in Commands)
                wrapper.Save();

            UniConfig.Save();
        }
        #endregion

        #region Public Functions
        public void LoadCommand(Type _class)
        {
            if (!typeof(PointBlankCommand).IsAssignableFrom(_class))
                return;
            if (_class == typeof(PointBlankCommand))
                return;

            try
            {
                CommandWrapper wrapper = new CommandWrapper(_class);

                if (Commands.Count(a => a.CommandClass.Name == wrapper.CommandClass.Name && a.Class.Assembly == _class.Assembly) > 0)
                    return;
                Commands.Add(wrapper);
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Error loading command: " + _class.Name, ex);
            }
        }
        public void LoadCommand(PointBlankCommand command)
        {
            Type _class = command.GetType();

            try
            {
                CommandWrapper wrapper = new CommandWrapper(command);

                if (Commands.Count(a => a.CommandClass.Name == command.Name && a.Class.Assembly == _class.Assembly) > 0)
                    return;
                Commands.Add(wrapper);
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Error loading command: " + _class.Name, ex);
            }
        }

        public void UnloadCommand(Type _class)
        {
            CommandWrapper wrapper = Commands.FirstOrDefault(a => a.Class == _class);

            if (wrapper == null)
                return;
            wrapper.Save();
            Commands.Remove(wrapper);
        }
        public void UnloadCommand(PointBlankCommand command)
        {
            CommandWrapper wrapper = Commands.FirstOrDefault(a => a.CommandClass == command);

            if (wrapper == null)
                return;
            wrapper.Save();
            Commands.Remove(wrapper);
        }

        public string[] ParseCommand(string command)
        {
            string[] sCommand = new string[0];
            List<string> ret = new List<string>();
            string arg = "";
            bool one = false;

            if (command.StartsWith("@") || command.StartsWith("/"))
                command = command.Remove(0, 1);
            sCommand = command.Split(' ');
            if (sCommand.Length <= 0)
                return new string[0];

            if (sCommand.Length <= 1)
                return sCommand.ToArray();
            ret.Add(sCommand[0]);
            for (int i = 1; i < sCommand.Length; i++)
            {
                if (sCommand[i].StartsWith("\"") || one)
                {
                    bool nocomplete = false;

                    if (sCommand[i].StartsWith("\""))
                    {
                        one = true;
                        arg = sCommand[i];
                        nocomplete = true;
                    }
                    if (sCommand[i].EndsWith("\""))
                        one = false;
                    if (!nocomplete)
                        arg = arg + " " + sCommand[i];
                    if (!one)
                    {
                        arg = arg.Remove(0, 1);
                        arg = arg.Remove(arg.Length - 1);
                        ret.Add(arg);
                        arg = "";
                    }

                    continue;
                }
                ret.Add(sCommand[i]);
            }
            return ret.ToArray();
        }

        public ECommandRunError ExecuteCommand(string text, PointBlankPlayer executor)
        {
            string[] info = ParseCommand(text);
            List<string> args = new List<string>();
            CommandWrapper wrapper = Commands.FirstOrDefault(a => a.Commands.FirstOrDefault(b => b.ToLower() == info[0].ToLower()) != null && a.Enabled);
            string permission = "";

            if (info.Length > 1)
                for (int i = 1; i < info.Length; i++)
                    args.Add(info[i]);
            bool allowExecute = true;

            PointBlankCommandEvents.RunCommandParse(info[0], args.ToArray(), executor, ref allowExecute);
            if (!allowExecute)
                return ECommandRunError.NO_EXECUTE;
            if (wrapper == null)
            {
                PointBlankPlayer.SendMessage(executor, Enviroment.ServiceTranslations[typeof(ServiceTranslations)].Translations["CommandManager_Invalid"], ConsoleColor.Red);
                return ECommandRunError.COMMAND_NOT_EXIST;
            }
            permission = wrapper.Permission;
            if (args.Count > 0)
                permission += "." + string.Join(".", args.ToArray());
            if (!PointBlankPlayer.IsServer(executor) && !executor.HasPermission(permission))
            {
                PointBlankPlayer.SendMessage(executor, Enviroment.ServiceTranslations[typeof(ServiceTranslations)].Translations["CommandManager_NotEnoughPermissions"], ConsoleColor.Red);
                return ECommandRunError.NO_PERMISSION;
            }

            return wrapper.Execute(executor, args.ToArray());
        }
        #endregion

        #region Event Functions
        private void OnPluginLoaded(PointBlankPlugin plugin)
        {
            foreach (Type tClass in plugin.GetType().Assembly.GetTypes())
                if (tClass.IsClass)
                    LoadCommand(tClass);
        }

        private void OnPluginUnloaded(PointBlankPlugin plugin)
        {
            foreach (CommandWrapper wrap in Commands.Where(a => a.Class.DeclaringType.Assembly == plugin.GetType().Assembly))
                Commands.Remove(wrap);
            UniConfig.Save();
        }
        #endregion
    }
}
