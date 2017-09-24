﻿using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using PointBlank.API;
using PointBlank.API.Server;
using PointBlank.API.Plugins;
using PointBlank.API.Services;
using PointBlank.API.DataManagment;
using PointBlank.Framework;

namespace PointBlank.Services.PluginManager
{
    internal class PluginManager : PointBlankService
    {
        #region Variables
        private static List<PluginWrapper> _plugins = new List<PluginWrapper>(); // List of plugins
        private static List<Assembly> _libraries = new List<Assembly>(); // List of libraries
        #endregion

        #region Properties
        public static string ConfigurationPath => PointBlankServer.ConfigurationsPath + "/Plugins";
        public static string TranslationPath => PointBlankServer.TranslationsPath + "/Plugins";

        public static PluginWrapper[] Plugins => _plugins.ToArray(); // Returns the plugins

        public static Assembly[] Libraries => _libraries.ToArray(); // Returns the libraries

        public UniversalData UniConfig { get; private set; } // The universal config data
        public JsonData JsonConfig { get; private set; } // The Json config data

        public override int LaunchIndex => 3;
        #endregion

        public override void Load()
        {
            if (!Directory.Exists(PointBlankServer.LibrariesPath))
                Directory.CreateDirectory(PointBlankServer.LibrariesPath); // Create libraries directory
            if (!Directory.Exists(PointBlankServer.PluginsPath))
                Directory.CreateDirectory(PointBlankServer.PluginsPath); // Create plugins directory
            if (!Directory.Exists(ConfigurationPath))
                Directory.CreateDirectory(ConfigurationPath); // Create plugins directory
            if (!Directory.Exists(TranslationPath))
                Directory.CreateDirectory(TranslationPath); // Create plugins directory

            // Setup the config
            UniConfig = new UniversalData(ServiceManager.ConfigurationPath + "\\PluginManager");
<<<<<<< HEAD
            JSONConfig = UniConfig.GetData(EDataType.JSON) as JsonData;
=======
            JsonConfig = UniConfig.GetData(EDataType.Json) as JsonData;
>>>>>>> master
            LoadConfig();

            foreach (string library in Directory.GetFiles(PointBlankServer.LibrariesPath, "*.dll")) // Get all the dll files in libraries directory
                _libraries.Add(Assembly.Load(File.ReadAllBytes(library))); // Load and add the library
            foreach (string plugin in Directory.GetFiles(PointBlankServer.PluginsPath, "*.dll")) // Get all the dll files in plugins directory
            {
                try
                {
                    if (!LoadPlugin(plugin))
                        break;
                }
                catch (Exception ex)
                {
                    PointBlankLogging.LogError("Error initializing plugin!", ex);
                    if (!PluginConfiguration.ContinueOnError)
                        break;
                }
            }
            PointBlankPluginEvents.RunPluginsLoaded();
        }

        public override void Unload()
        {
            foreach (PluginWrapper wrapper in _plugins)
                wrapper.Unload(); // Unload the wrapper
            PointBlankPluginEvents.RunPluginsUnloaded();
            SaveConfig();
        }

        #region Static Functions
        public static bool LoadPlugin(string plugin)
        {
            PluginWrapper wrapper = new PluginWrapper(plugin); // Create the plugin wrapper

            _plugins.Add(wrapper); // Add the wrapper
            if (!wrapper.Load() && !PluginConfiguration.ContinueOnError)
                return false;
            return true;
        }
        public static bool LoadPlugin(Type plugin)
        {
            PluginWrapper wrapper = new PluginWrapper(plugin); // Create the plugin wrapper

            _plugins.Add(wrapper); // Add the wrapper
            if (!wrapper.Load() && !PluginConfiguration.ContinueOnError)
                return false;
            return true;
        }
        public static bool LoadPlugin(PointBlankPlugin plugin)
        {
            PluginWrapper wrapper = new PluginWrapper(plugin); // Create the plugin wrapper

            _plugins.Add(wrapper); // Add the wrapper
            if (!wrapper.Load() && !PluginConfiguration.ContinueOnError)
                return false;
            return true;
        }

        public static void RemovePlugin(PluginWrapper wrapper) => _plugins.Remove(wrapper);
        #endregion

        #region Public Functions
        public PluginWrapper GetWrapper(PointBlankPlugin plugin) => Plugins.First(a => a.PluginClass == plugin);
        #endregion

        #region Private Functions
        internal void LoadConfig()
        {
            if (UniConfig.CreatedNew)
            {
                PluginConfiguration.AutoUpdate = false;
                PluginConfiguration.ContinueOnError = true;
                PluginConfiguration.NotifyUpdates = true;
                PluginConfiguration.CheckUpdateTimeSeconds = 1800;

                JsonConfig.Document.Add("AutoUpdate", (PluginConfiguration.AutoUpdate ? "true" : "false"));
                JsonConfig.Document.Add("ContinueOnError", (PluginConfiguration.ContinueOnError ? "true" : "false"));
                JsonConfig.Document.Add("NotifyUpdates", (PluginConfiguration.NotifyUpdates ? "true" : "false"));
                JsonConfig.Document.Add("CheckUpdateTimeSeconds", PluginConfiguration.CheckUpdateTimeSeconds.ToString());
                UniConfig.Save();
            }
            else
            {
                JsonConfig.Verify(new Dictionary<string, Newtonsoft.Json.Linq.JToken>()
                {
                    { "AutoUpdate", "false" },
                    { "ContinueOnError", "true" },
                    { "NotifyUpdates", "true" },
                    { "CheckUpdateTimeSeconds", "1800" }
                });

                PluginConfiguration.AutoUpdate = ((string)JsonConfig.Document["AutoUpdate"] == "true");
                PluginConfiguration.ContinueOnError = ((string)JsonConfig.Document["ContinueOnError"] == "true");
                PluginConfiguration.NotifyUpdates = ((string)JsonConfig.Document["NotifyUpdates"] == "true");
                PluginConfiguration.CheckUpdateTimeSeconds = int.Parse((string)JsonConfig.Document["CheckUpdateTimeSeconds"]);
            }
        }

        internal void SaveConfig() => UniConfig.Save();
        #endregion
    }
}
