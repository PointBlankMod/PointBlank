using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Plugins;
using PointBlank.API.Collections;
using PointBlank.API.Interfaces;
using PointBlank.API.DataManagment;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace PointBlank.Framework
{
    internal class InterfaceManager : MonoBehaviour
    {
        #region Properties
        public bool Initialized { get; private set; } = false;
        #endregion

        #region Private Functions
        private void LoadConfigurable(Type configurable)
        {
            MethodInfo miDirectory = configurable.GetMethod("get_ConfigurationDirectory", BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            MethodInfo miConfigurations = configurable.GetMethod("get_Configurations", BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            MethodInfo miDictionary = configurable.GetMethod("get_ConfigurationDictionary", BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            string path = null;
            ConfigurationList configurations = null;
            Dictionary<string, IConfigurable> dictionary = null;

            if (!(miDirectory.IsStatic && miConfigurations.IsStatic && miDictionary.IsStatic) && !(!miDirectory.IsStatic && !miDictionary.IsStatic && !miConfigurations.IsStatic))
                return;
            
            if (miDirectory.IsStatic && miConfigurations.IsStatic && miDictionary.IsStatic)
            {
                path = (string)miDirectory.Invoke(null, new object[0]);
                configurations = (ConfigurationList)miConfigurations.Invoke(null, new object[0]);
            }
            else if (!miDirectory.IsStatic && !miDictionary.IsStatic && !miConfigurations.IsStatic)
            {
                IConfigurable cfg = (IConfigurable)Activator.CreateInstance(configurable);
                path = cfg.ConfigurationDirectory;
                configurations = cfg.Configurations;
                dictionary = cfg.ConfigurationDictionary;

                if (dictionary != null)
                    dictionary.Add(configurable.Name, cfg);
            }
            else
                return;

            UniversalData UniData = new UniversalData(ServerInfo.ConfigurationsPath + "/" + path + "/" + configurable.Name);
            JsonData JSON = UniData.GetData(EDataType.JSON) as JsonData;

            if(UniData.CreatedNew)
            {
                foreach(KeyValuePair<string, object> kvp in configurations)
                {
                    if (JSON.CheckKey(kvp.Key))
                        JSON.Document[kvp.Key] = JToken.FromObject(kvp.Value);
                    else
                        JSON.Document.Add(kvp.Key, JToken.FromObject(kvp.Value));
                }
            }
            else
            {
                foreach(JProperty property in JSON.Document.Properties())
                {
                    if (configurations[property.Name] == null)
                        continue;

                    configurations[property.Name] = property.Value.ToObject(configurations[property.Name].GetType());
                }
            }
            UniData.Save();
        }

        private void LoadTranslatable(Type translatable)
        {

        }
        #endregion

        #region Public Functions
        public void LoadInterface(Type _interface)
        {
            if (!_interface.IsClass)
                return;

            if (typeof(IConfigurable).IsAssignableFrom(_interface))
                LoadConfigurable(_interface);
            if (typeof(ITranslatable).IsAssignableFrom(_interface))
                LoadTranslatable(_interface);
        }

        public void Init()
        {
            if (Initialized)
                return;

            if (!Directory.Exists(ServerInfo.ConfigurationsPath))
                Directory.CreateDirectory(ServerInfo.ConfigurationsPath); // Create configurations directory
            if (!Directory.Exists(ServerInfo.TranslationsPath))
                Directory.CreateDirectory(ServerInfo.TranslationsPath); // Create translations directory
            if (!Directory.Exists(ServerInfo.DataPath))
                Directory.CreateDirectory(ServerInfo.DataPath); // Create data directory

            foreach (Type _class in Assembly.GetExecutingAssembly().GetTypes())
                LoadInterface(_class);

            // Setup the events
            PluginEvents.OnPluginLoaded += new PluginEvents.PluginEventHandler(OnPluginLoaded);

            // Set the variables
            Initialized = true;
        }

        public void Shutdown()
        {
            if (!Initialized)
                return;

            // Set the variables
            Initialized = false;
        }
        #endregion

        #region Event Functions
        private void OnPluginLoaded(Plugin plugin)
        {
            foreach (Type _class in plugin.GetType().Assembly.GetTypes())
                LoadInterface(_class);
        }
        #endregion
    }
}
