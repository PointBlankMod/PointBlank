using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using PointBlank.API;
using PointBlank.API.Plugins;
using PointBlank.API.Services;
using PointBlank.API.Commands;
using PointBlank.API.DataManagment;
using PointBlank.API.Unturned.Player;
using Newtonsoft.Json.Linq;
using PointBlank.Services.PluginManager;
using UnityEngine;
using CMD = PointBlank.API.Commands.Command;

namespace PointBlank.Services.CommandManager
{
    [Service("CommandManager", true)]
    internal class CommandManager : Service
    {
        #region Info
        public static readonly string ConfigurationPath = ServerInfo.ConfigurationsPath + "//Commands";
        #endregion

        #region Properties
        public static Dictionary<CommandAttribute, CommandWrapper> Commands { get; private set; }

        public UniversalData UniConfig { get; private set; }
        public JsonData JSONConfig { get; private set; }
        #endregion

        #region Override Functions
        public override void Load()
        {
            // Setup variables
            Commands = new Dictionary<CommandAttribute, CommandWrapper>();
            UniConfig = new UniversalData(ConfigurationPath);
            JSONConfig = UniConfig.GetData(EDataType.JSON) as JsonData;

            // Setup events
            PluginEvents.OnPluginLoaded += new PluginEvents.PluginEventHandler(OnPluginLoaded); // Run code every time a plugin is loaded
            CommandWindow.onCommandWindowInputted += new CommandWindowInputted(OnConsoleCommand);
            ChatManager.onCheckPermissions += new CheckPermissions(OnUnturnedCommand);

            // Run the code
            LoadConfig();
            foreach (Type tClass in Assembly.GetExecutingAssembly().GetTypes())
                if (tClass.IsClass)
                    LoadCommand(tClass);
        }

        public override void Unload()
        {
            SaveConfig();
        }
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
            foreach(CommandWrapper wrapper in Commands.Values)
                wrapper.Save();

            UniConfig.Save();
        }
        #endregion

        #region Public Functions
        public void LoadCommand(Type _class)
        {
            CommandAttribute attribute = (CommandAttribute)Attribute.GetCustomAttribute(_class, typeof(CommandAttribute));

            if (attribute == null)
                return;
            if (!typeof(CMD).IsAssignableFrom(_class))
                return;
            if (Commands.Count(a => a.Key.Name == attribute.Name && a.Key.GetType().Assembly == attribute.GetType().Assembly) > 0)
                return;

            try
            {
                string name = attribute.GetType().Assembly.GetName().Name + "." + attribute.Name;
                JObject objConfig = ((JArray)JSONConfig.Document["Commands"]).FirstOrDefault(a => (string)a["Name"] == name) as JObject;
                if(objConfig == null)
                {
                    objConfig = new JObject();
                    ((JArray)JSONConfig.Document["Commands"]).Add(objConfig);
                }
                CommandWrapper wrapper = new CommandWrapper(_class, attribute, objConfig);

                Commands.Add(attribute, wrapper);
            }
            catch (Exception ex)
            {
                Logging.LogError("Error loading command: " + attribute.Name, ex);
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
        #endregion

        #region Event Functions
        private void OnPluginLoaded(Plugin plugin)
        {
            PluginWrapper wrapper = PluginManager.PluginManager.Plugins.First(a => a.PluginClass == plugin);

            foreach (Type tClass in wrapper.PluginAssembly.GetTypes())
                if (tClass.IsClass)
                    LoadCommand(tClass);
        }

        private void OnPluginUnloaded(Plugin plugin)
        {
            PluginWrapper wrapper = PluginManager.PluginManager.Plugins.First(a => a.PluginClass == plugin);

            foreach (KeyValuePair<CommandAttribute, CommandWrapper> kvp in Commands.Where(a => a.Value.Class.DeclaringType.Assembly == wrapper.PluginAssembly))
                Commands.Remove(kvp.Key);
            UniConfig.Save();
        }

        private void OnConsoleCommand(string text, ref bool shouldExecute)
        {
            shouldExecute = false;
            if (text.StartsWith("/") || text.StartsWith("@"))
                text = text.Remove(0, 1);

            string[] info = ParseCommand(text);
            List<string> args = new List<string>();
            CommandWrapper wrapper = Commands.Select(a => a.Value).FirstOrDefault(a => a.Commands.Contains(info[0]));

            if(wrapper == null || !wrapper.Enabled)
            {
                CommandWindow.Log("Invalid command! Do help for the list of commands", ConsoleColor.Red);
                return;
            }
            if (info.Length > 1)
                for (int i = 1; i < info.Length; i++)
                    args.Add(info[i]);
            wrapper.Execute(null, args.ToArray());
        }

        private void OnUnturnedCommand(SteamPlayer player, string text, ref bool shouldExecuteCommand, ref bool shouldList)
        {
            shouldExecuteCommand = false;

            if(text.StartsWith("/") || text.StartsWith("@"))
            {
                shouldList = false;

                string[] info = ParseCommand(text);
                CommandWrapper wrapper = Commands.Select(a => a.Value).FirstOrDefault(a => a.Commands.Contains(info[0]));
                UnturnedPlayer ply = UnturnedPlayer.Get(player);
                List<string> args = new List<string>();

                if(wrapper == null || !wrapper.Enabled)
                {
                    ply.SendMessage("Invalid command! Do /help for the list of commands", Color.red);
                    return;
                }
                if (!ply.HasPermission(wrapper.Permission))
                {
                    ply.SendMessage("Not enough permissions!", Color.red);
                    return;
                }
                if (info.Length > 1)
                    for (int i = 1; i < info.Length; i++)
                        args.Add(info[i]);
                wrapper.Execute(ply, args.ToArray());
            }
        }
        #endregion
    }
}
