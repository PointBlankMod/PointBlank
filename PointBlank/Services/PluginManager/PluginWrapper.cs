﻿using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using PointBlank.API;
using PointBlank.API.Plugins;
using PointBlank.API.DataManagment;
using PointBlank.API.Extension;

namespace PointBlank.Services.PluginManager
{
    internal class PluginWrapper
    {
        #region Variables
        private DateTime LastUpdateCheck;
        #endregion

        #region Properties
        public string Location { get; private set; }
        public string Name { get; private set; }
        public bool Enabled { get; private set; }
        public string Version { get; private set; }

        public Assembly PluginAssembly { get; private set; }

        public PointBlankPlugin PluginClass { get; private set; }

        public UniversalData UniConfigurationData { get; private set; }
        public JsonData ConfigurationData { get; private set; }
        public UniversalData UniTranslationData { get; private set; }
        public JsonData TranslationData { get; private set; }
        #endregion

        public PluginWrapper(string pluginPath)
        {
            Location = pluginPath;
            Name = Path.GetFileNameWithoutExtension(pluginPath); // Get the file name
            Enabled = false; // Set enabled to false

            PluginAssembly = Assembly.Load(File.ReadAllBytes(pluginPath)); // Load the assembly

            UniConfigurationData = new UniversalData(PluginManager.ConfigurationPath + "\\" + Name); // Load the configuration data
            ConfigurationData = UniConfigurationData.GetData(EDataType.JSON) as JsonData; // Get the JSON
            UniTranslationData = new UniversalData(PluginManager.TranslationPath + "\\" + Name); // Load the translation data
            TranslationData = UniTranslationData.GetData(EDataType.JSON) as JsonData; // Get the JSON
        }
        public PluginWrapper(Type plugin)
        {
            PluginAssembly = plugin.Assembly;
            Enabled = false;
            Location = Path.GetDirectoryName(PluginAssembly.Location);
            Name = PluginAssembly.GetName().Name;

            UniConfigurationData = new UniversalData(PluginManager.ConfigurationPath + "\\" + Name); // Load the configuration data
            ConfigurationData = UniConfigurationData.GetData(EDataType.JSON) as JsonData; // Get the JSON
            UniTranslationData = new UniversalData(PluginManager.TranslationPath + "\\" + Name); // Load the translation data
            TranslationData = UniTranslationData.GetData(EDataType.JSON) as JsonData; // Get the JSON
        }
        public PluginWrapper(PointBlankPlugin plugin)
        {
            PluginAssembly = plugin.GetType().Assembly;
            Enabled = false;
            Location = Path.GetDirectoryName(PluginAssembly.Location);
            Name = PluginAssembly.GetName().Name;
            PluginClass = plugin;

            UniConfigurationData = new UniversalData(PluginManager.ConfigurationPath + "\\" + Name); // Load the configuration data
            ConfigurationData = UniConfigurationData.GetData(EDataType.JSON) as JsonData; // Get the JSON
            UniTranslationData = new UniversalData(PluginManager.TranslationPath + "\\" + Name); // Load the translation data
            TranslationData = UniTranslationData.GetData(EDataType.JSON) as JsonData; // Get the JSON
        }

        #region Private Functions
        internal void LoadConfiguration()
        {
            if (ConfigurationData.CreatedNew)
            {
                SaveConfiguration();
                return;
            }

            foreach(JProperty obj in ConfigurationData.Document.Properties())
            {
                if (PluginClass.Configurations[obj.Name] == null)
                    continue;

                try
                {
                    PluginClass.Configurations[obj.Name] = obj.Value.ToObject(PluginClass.Configurations[obj.Name].GetType());
                }
                catch(Exception ex)
                {
                    PointBlankLogging.LogError("Failed to set the configuration " + obj.Name, ex, false, false);
                    ConfigurationData.Document[obj.Name] = JToken.FromObject(PluginClass.Configurations[obj.Name]);
                }
            }
        }

        internal void SaveConfiguration()
        {
            foreach(KeyValuePair<string, object> config in PluginClass.Configurations)
            {
                if (config.Value == null)
                    continue;
                if (ConfigurationData.CheckKey(config.Key))
                    ConfigurationData.Document[config.Key] = JToken.FromObject(config.Value);
                else
                    ConfigurationData.Document.Add(config.Key, JToken.FromObject(config.Value));
            }
            UniConfigurationData.Save();
        }

        internal void LoadTranslation()
        {
            if (TranslationData.CreatedNew)
            {
                SaveTranslation();
                return;
            }

            foreach (KeyValuePair<string, JToken> kvp in TranslationData.Document)
                PluginClass.Translations[kvp.Key] = (string)kvp.Value;
        }

        internal void SaveTranslation()
        {
            foreach(KeyValuePair<string, string> translation in PluginClass.Translations)
            {
                if (string.IsNullOrEmpty(translation.Value))
                    continue;
                if (TranslationData.CheckKey(translation.Key))
                    TranslationData.Document[translation.Key] = translation.Value;
                else
                    TranslationData.Document.Add(translation.Key, translation.Value);
            }
            UniTranslationData.Save();
        }

        private void ExecuteCheck()
        {
            if (LastUpdateCheck != null && !((DateTime.Now - LastUpdateCheck).TotalSeconds >=
                                                     PluginConfiguration.CheckUpdateTimeSeconds)) return;
            if (CheckUpdates())
            {
                if (PluginConfiguration.NotifyUpdates)
                    Notify();
                if (PluginConfiguration.AutoUpdate)
                    Update();
            }

            LastUpdateCheck = DateTime.Now;
        }

        private bool CheckUpdates()
        {
            if (!PluginConfiguration.NotifyUpdates && !PluginConfiguration.AutoUpdate)
                return false;
            if (string.IsNullOrEmpty(PluginClass.VersionURL))
                return false;
            string bVersion = "";

            WebsiteData.GetData(PluginClass.VersionURL, out bVersion);

            if (!int.TryParse(bVersion.Replace('.', '\0'), out int newVersion))
                return (bVersion != Version);
            if (!int.TryParse(Version.Replace('.', '\0'), out int curVersion))
                return (bVersion != Version);
            return (newVersion > curVersion);
        }

        private void Update()
        {
            if (!PluginConfiguration.AutoUpdate)
                return;
            if (string.IsNullOrEmpty(PluginClass.BuildURL))
                return;

            PointBlankLogging.LogImportant("Downloading " + Name + "...");
            WebsiteData.DownloadFile(PluginClass.BuildURL, Location);
            PointBlankLogging.LogImportant(Name + " updated successfully! Please restart the server to finalize the update!");
        }

        private void Notify()
        {
            if (!PluginConfiguration.NotifyUpdates)
                return;

            PointBlankLogging.LogWarning("New update for plugin: " + Name);
        }
        #endregion

        #region Public Functions
        public bool Load()
        {
            try
            {
                if (Enabled)
                    return true;

                PointBlankLogging.Log("Starting " + Name + "...");
                if(PluginClass == null)
                {
                    Type _class = PluginAssembly.GetTypes().FirstOrDefault(a => a.IsClass && typeof(PointBlankPlugin).IsAssignableFrom(a)); // Get the first plugin class

                    if (_class == null)
                    {
                        PluginManager.RemovePlugin(this);
                        return true;
                    }

                    PluginClass = Enviroment.runtimeObjects["Plugins"].AddCodeObject(_class) as PointBlankPlugin; // Instentate the plugin class
                }
                
                Name = PluginClass.GetType().Name; // Change the name
                Version = PluginClass.Version;

                if (CheckUpdates())
                {
                    if (PluginConfiguration.NotifyUpdates)
                        Notify();
                    if (PluginConfiguration.AutoUpdate)
                        Update();
                }
                LastUpdateCheck = DateTime.Now;

                LoadConfiguration(); // Load the configuration
                LoadTranslation(); // Load the translation
                PointBlankPluginEvents.RunPluginStart(PluginClass); // Run the start event
                PluginClass.Load(); // Run the load function
                PointBlankPluginEvents.RunPluginLoaded(PluginClass); // Run the loaded event

                Enabled = true; // Set the enabled to true
                ExtensionEvents.OnFrameworkTick += ExecuteCheck;
                return true;
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Error starting plugin: " + Name, ex);
                Unload();
                return false;
            }
        }

        public bool Unload()
        {
            try
            {
                if (!Enabled)
                    return true;

                PointBlankLogging.Log("Stopping " + Name + "...");

                SaveConfiguration(); // Save the configuration
                SaveTranslation(); // Save the translation
                PointBlankPluginEvents.RunPluginStop(PluginClass); // Run the stop event
                PluginClass.Unload(); // Run the unload function
                PointBlankPluginEvents.RunPluginUnloaded(PluginClass); // Run the unloaded event

                Enviroment.runtimeObjects["Plugins"].RemoveCodeObject(PluginClass.GetType().Name); // Remove the plugin from gameobject

                Enabled = false; // Set the enabled to false
                ExtensionEvents.OnFrameworkTick -= ExecuteCheck;
                return true;
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Error stopping plugin: " + Name, ex);
                return false;
            }
        }
        #endregion
    }
}
