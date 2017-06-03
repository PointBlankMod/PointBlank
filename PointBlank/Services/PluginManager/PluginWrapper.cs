using System;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using PointBlank.API;
using PointBlank.API.Plugins;
using PointBlank.API.Collections;
using PointBlank.API.DataManagment;

namespace PointBlank.Services.PluginManager
{
    internal class PluginWrapper
    {
        #region Variables
        private Thread t;
        private DateTime LastUpdateCheck;
        #endregion

        #region Properties
        public string Location { get; private set; }
        public string Name { get; private set; }
        public bool Enabled { get; private set; }
        public string Version { get; private set; }

        public Assembly PluginAssembly { get; private set; }

        public Plugin PluginClass { get; private set; }

        public UniversalData UniConfigurationData { get; private set; }
        public XMLData ConfigurationData { get; private set; }
        public UniversalData UniTranslationData { get; private set; }
        public JsonData TranslationData { get; private set; }
        #endregion

        #region Plugin Data
        public TranslationList Translations { get; private set; }
        public ConfigurationList Configurations { get; private set; }
        #endregion

        public PluginWrapper(string pluginPath)
        {
            Location = pluginPath;
            Name = Path.GetFileNameWithoutExtension(pluginPath); // Get the file name
            Enabled = false; // Set enabled to false

            PluginAssembly = Assembly.Load(File.ReadAllBytes(pluginPath)); // Load the assembly

            UniConfigurationData = new UniversalData(PluginManager.ConfigurationPath + "\\" + Name); // Load the configuration data
            ConfigurationData = UniConfigurationData.GetData(EDataType.XML) as XMLData; // Get the XML
            UniTranslationData = new UniversalData(ServerInfo.TranslationsPath + "\\" + Name); // Load the translation data
            TranslationData = UniTranslationData.GetData(EDataType.JSON) as JsonData; // Get the JSON

            Translations = new TranslationList(); // Create the translation list
            Configurations = new ConfigurationList(); // Create the configuration list

            // Setup the thread
            t = new Thread(new ThreadStart(delegate ()
            {
                while (Enabled)
                {
                    if(LastUpdateCheck == null || (DateTime.Now - LastUpdateCheck).TotalSeconds >= PluginConfiguration.CheckUpdateTimeSeconds)
                    {
                        if (CheckUpdates())
                        {
                            if (PluginConfiguration.NotifyUpdates)
                                Notify();
                            if (PluginConfiguration.AutoUpdate)
                                Update();
                        }

                        LastUpdateCheck = DateTime.Now;
                    }
                }
            }));
        }

        #region Private Functions
        private void LoadConfiguration()
        {
            Configurations.AddRange(PluginClass.DefaultConfigurations); // Add the default configurations

            if (ConfigurationData.CreatedNew)
                return; // If it was just created don't bother loading

            foreach (XmlNode node in ConfigurationData.GetChildNodes("/Data"))
                Configurations[node.Name] = Convert.ChangeType(node.InnerText, Type.GetType(node.Attributes["Type"].Value)); // Add the configuration
        }

        private void SaveConfiguration()
        {
            foreach(KeyValuePair<string, object> config in Configurations)
            {
                if(ConfigurationData.CheckNode("/Data/" + config.Key))
                {
                    ConfigurationData.SetValue("/Data/" + config.Key, config.Value.ToString());
                    ConfigurationData.GetAttributes("/Data/" + config.Key)["Type"].InnerText = config.Value.GetType().ToString();
                }
                else
                {
                    XmlAttribute att = ConfigurationData.Document.CreateAttribute("Type");

                    ConfigurationData.AddNode("/Data", config.Key, config.Value.ToString());
                    ConfigurationData.GetAttributes("/Data/" + config.Key).Append(att);

                    att.Value = config.Value.GetType().ToString();
                }
            }
            UniConfigurationData.Save();
        }

        private void LoadTranslation()
        {
            Translations.AddRange(PluginClass.DefaultTranslations); // Add the default translations

            if (TranslationData.CreatedNew)
                return; // If it was just created don't bother loading

            foreach (KeyValuePair<string, JToken> kvp in TranslationData.Document)
                Translations[kvp.Key] = (string)kvp.Value; // Add the translation
        }

        private void SaveTranslation()
        {
            foreach(KeyValuePair<string, string> translation in Translations)
            {
                if (TranslationData.CheckKey(translation.Key))
                    TranslationData.Document[translation.Key] = translation.Value;
                else
                    TranslationData.Document.Add(translation.Key, translation.Value);
            }
            UniTranslationData.Save();
        }

        private bool CheckUpdates()
        {
            if (!PluginConfiguration.NotifyUpdates && !PluginConfiguration.AutoUpdate)
                return false;
            if (string.IsNullOrEmpty(PluginClass.VersionURL))
                return false;
            string bVersion = "";

            WebsiteData.GetData(PluginClass.VersionURL, out bVersion, false, Enviroment.WebsiteClient);

            return (bVersion != Version);
        }

        private void Update()
        {
            if (!PluginConfiguration.AutoUpdate)
                return;
            if (string.IsNullOrEmpty(PluginClass.BuildURL))
                return;

            Logging.LogImportant("Downloading " + Name + "...");
            WebsiteData.DownloadFile(PluginClass.BuildURL, Location, false, Enviroment.WebsiteClient);
            Logging.LogImportant(Name + " updated successfully! Please restart the server to finalize the update!");
        }

        private void Notify()
        {
            if (!PluginConfiguration.NotifyUpdates)
                return;

            Logging.LogWarning("New update for plugin: " + Name);
        }
        #endregion

        #region Public Functions
        public bool Load()
        {
            try
            {
                if (Enabled)
                    return true;

                Logging.Log("Starting " + Name + "...");
                Type _class = PluginAssembly.GetTypes().First(a => a.IsClass && typeof(Plugin).IsAssignableFrom(a)); // Get the first plugin class

                PluginClass = Enviroment.runtimeObjects["Plugins"].AddCodeObject(_class) as Plugin; // Instentate the plugin class
                Name = PluginClass.GetType().Name; // Change the name
                Version = PluginClass.Version;

                if (CheckUpdates())
                {
                    if (PluginConfiguration.NotifyUpdates)
                        Notify();
                    if (PluginConfiguration.AutoUpdate)
                        Update();
                }

                LoadConfiguration(); // Load the configuration
                LoadTranslation(); // Load the translation
                PluginEvents.RunPluginStart(PluginClass); // Run the start event
                PluginClass.Load(); // Run the load function
                PluginEvents.RunPluginLoaded(PluginClass); // Run the loaded event

                Enabled = true; // Set the enabled to true
                t.Start(); // Start the thread
                return true;
            }
            catch (Exception ex)
            {
                Logging.LogError("Error starting plugin: " + Name, ex);
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

                Logging.Log("Stopping " + Name + "...");

                SaveConfiguration(); // Save the configuration
                SaveTranslation(); // Save the translation
                PluginEvents.RunPluginStop(PluginClass); // Run the stop event
                PluginClass.Unload(); // Run the unload function
                PluginEvents.RunPluginUnloaded(PluginClass); // Run the unloaded event

                Enviroment.runtimeObjects["Plugins"].RemoveCodeObject(PluginClass.GetType().Name); // Remove the plugin from gameobject

                Enabled = false; // Set the enabled to false
                t.Abort(); // Abort the thread
                return true;
            }
            catch (Exception ex)
            {
                Logging.LogError("Error stopping plugin: " + Name, ex);
                return false;
            }
        }
        #endregion
    }
}
