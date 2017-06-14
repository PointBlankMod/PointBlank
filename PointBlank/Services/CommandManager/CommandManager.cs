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
            JSONConfig = UniConfig.JSON;

            // Setup events
            PluginEvents.OnPluginLoaded += new PluginEvents.PluginEventHandler(OnPluginLoaded); // Run code every time a plugin is loaded

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
            UniConfig.Save();
        }
        #endregion

        #region Public Functions
        public void LoadCommand(Type _class)
        {
            CommandAttribute attribute = (CommandAttribute)Attribute.GetCustomAttribute(_class, typeof(CommandAttribute));

            if (attribute == null)
                return;

            try
            {
                JObject objConfig = ((JArray)JSONConfig.Document["Commands"]).FirstOrDefault(a => (string)a["Name"] == attribute.Name) as JObject;
                if(objConfig == null)
                {
                    objConfig = new JObject();
                    ((JArray)JSONConfig.Document["Commands"]).Add(objConfig);
                }
                CommandWrapper wrapper = new CommandWrapper(_class, attribute, objConfig);

                wrapper.Enable();

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

            sCommand = command.Remove(0).Split(' ');
            if (sCommand.Length <= 0)
                return new string[0];

            for(int i = 1; i < sCommand.Length; i++)
            {
                if (sCommand[i].StartsWith("\"") || one)
                {
                    string push = sCommand[i];

                    if (sCommand[i].StartsWith("\""))
                    {
                        one = true;
                        push = push.Remove(0);
                    }
                    if (sCommand[i].EndsWith("\""))
                    {
                        one = false;
                        push = push.Remove(push.Length - 1);
                        arg = arg + " " + push;
                        ret.Add(arg);
                        arg = "";
                        continue;
                    }
                    arg = arg + (arg.Length > 0 ? " " : "") + push;

                    continue;
                }
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

        private void OnUnturnedCommand(SteamPlayer player, string text, ref bool shouldExecuteCommand, ref bool shouldList)
        {
            shouldExecuteCommand = false;

            if(text.StartsWith("/") || text.StartsWith("@"))
            {
                string[] info = ParseCommand(text);
                CommandWrapper wrapper = Commands.FirstOrDefault(a => a.Value.Commands.Contains(info[0])).Value;
                UnturnedPlayer ply = UnturnedPlayer.Get(player);
                List<string> args = new List<string>();

                if(wrapper == null || !wrapper.Enabled)
                {
                    ply.SendMessage("Invalid command! Do /help for the list of commands", Color.red);
                    return;
                }
                for(int i = 1; i < info.Length; i++)
                    args.Add(info[i]);
                wrapper.Execute(ply, args.ToArray());

                shouldList = false;
            }
        }
        #endregion
    }
}
