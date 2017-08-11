using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Plugins;
using PointBlank.API.Services;
using PointBlank.API.Commands;
using PointBlank.API.DataManagment;
using PointBlank.API.Player;
using Newtonsoft.Json.Linq;
using PointBlank.Services.PluginManager;
using PointBlank.Framework.Translations;
using PointBlank.API.Server;
using PointBlank.API.Extension;
using CMD = PointBlank.API.Commands.PointBlankCommand;

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

        #region Override Functions
        public override void Load()
        {
            // Setup variables
            Commands = new List<CommandWrapper>();
            UniConfig = new UniversalData(ConfigurationPath);
            JSONConfig = UniConfig.GetData(EDataType.JSON) as JsonData;

            // Setup events
            PointBlankPluginEvents.OnPluginLoaded += new PointBlankPluginEvents.PluginEventHandler(OnPluginLoaded); // Run code every time a plugin is loaded

            // Run the code
            LoadConfig();
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies().Where(a => Attribute.GetCustomAttribute(a, typeof(PointBlankExtensionAttribute)) != null))
                foreach (Type tClass in asm.GetTypes())
                    if (tClass.IsClass)
                            LoadCommand(tClass);
        }

        public override void Unload() => SaveConfig();
        #endregion

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
            if (!typeof(CMD).IsAssignableFrom(_class))
                return;
            if (_class == typeof(CMD))
                return;
            if (Commands.Count(a => a.GetType().Name == _class.Name && a.GetType().Assembly == _class.Assembly) > 0)
                return;

            try
            {
                string name = _class.Assembly.GetName().Name + "." + _class.Name;
                JObject objConfig = ((JArray)JSONConfig.Document["Commands"]).FirstOrDefault(a => (string)a["Name"] == name) as JObject;
                if(objConfig == null)
                {
                    objConfig = new JObject();
                    ((JArray)JSONConfig.Document["Commands"]).Add(objConfig);
                }
                CommandWrapper wrapper = new CommandWrapper(_class, objConfig);

                Commands.Add(wrapper);
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Error loading command: " + _class.Name, ex);
            }
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
            CommandWrapper wrapper = Commands.FirstOrDefault(a => a.Commands.FirstOrDefault(b => b.ToLower() == info[0].ToLower()) != null);
            string permission = "";

            if (wrapper == null || !wrapper.Enabled)
                return ECommandRunError.COMMAND_NOT_EXIST;
            permission = wrapper.Permission;
            if (info.Length > 1)
                for (int i = 1; i < info.Length; i++)
                    args.Add(info[i]);
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
            PluginWrapper wrapper = PluginManager.PluginManager.Plugins.First(a => a.PluginClass == plugin);

            foreach (Type tClass in wrapper.PluginAssembly.GetTypes())
                if (tClass.IsClass)
                    LoadCommand(tClass);
        }

        private void OnPluginUnloaded(PointBlankPlugin plugin)
        {
            PluginWrapper wrapper = PluginManager.PluginManager.Plugins.First(a => a.PluginClass == plugin);

            foreach (CommandWrapper wrap in Commands.Where(a => a.Class.DeclaringType.Assembly == wrapper.PluginAssembly))
                Commands.Remove(wrap);
            UniConfig.Save();
        }
        #endregion
    }
}
