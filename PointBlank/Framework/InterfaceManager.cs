using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using PointBlank.API;
using PointBlank.API.Plugins;
using PointBlank.API.Collections;
using PointBlank.API.Interfaces;
using PointBlank.API.DataManagment;
using PointBlank.API.Server;
using PointBlank.API.Extension;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace PointBlank.Framework
{
    internal class InterfaceManager : MonoBehaviour, ILoadable
    {
        #region Variables
        private static Dictionary<IConfigurable, UniversalData> _SavedConfigs = new Dictionary<IConfigurable, UniversalData>();
        private static Dictionary<ITranslatable, UniversalData> _SavedTranslations = new Dictionary<ITranslatable, UniversalData>();
        #endregion

        #region Properties
        public bool Initialized { get; private set; } = false;
        #endregion

        #region Private Functions
        private void LoadConfigurable(Type configurable)
        {
            string path = null;
            ConfigurationList configurations = null;
            Dictionary<Type, IConfigurable> dictionary = null;
            IConfigurable cfg = (IConfigurable)Activator.CreateInstance(configurable);

            path = cfg.ConfigurationDirectory;
            configurations = cfg.Configurations;
            dictionary = cfg.ConfigurationDictionary;
            if (dictionary != null)
                dictionary.Add(configurable, cfg);

            UniversalData UniData;
            if (_SavedConfigs.ContainsKey(cfg))
                UniData = _SavedConfigs[cfg];
            else
                UniData = new UniversalData(PointBlankServer.ConfigurationsPath + "/" + (string.IsNullOrEmpty(path) ? "" : path + "/") + configurable.Name);
            JsonData JSON = UniData.GetData(EDataType.JSON) as JsonData;

            if (!_SavedConfigs.ContainsKey(cfg))
                _SavedConfigs.Add(cfg, UniData);
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

        private void SaveConfigurable(Type configurable)
        {
            foreach(IConfigurable cfg in _SavedConfigs.Keys)
            {
                if(cfg.GetType() == configurable)
                {
                    foreach(KeyValuePair<string, object> conf in cfg.Configurations)
                    {
                        JsonData JSON  = _SavedConfigs[cfg].GetData(EDataType.JSON) as JsonData;

                        if (JSON.Document[conf.Key] != null)
                            JSON.Document[conf.Key] = JToken.FromObject(conf.Value);
                        else
                            JSON.Document.Add(conf.Key, JToken.FromObject(conf.Value));
                    }
                    _SavedConfigs[cfg].Save();
                    break;
                }
            }
        }

        private void LoadTranslatable(Type translatable)
        {
            string path = null;
            TranslationList translations = null;
            Dictionary<Type, ITranslatable> dictionary = null;
            ITranslatable translater = (ITranslatable)Activator.CreateInstance(translatable);

            path = translater.TranslationDirectory;
            translations = translater.Translations;
            dictionary = translater.TranslationDictionary;
            if (dictionary != null)
                dictionary.Add(translatable, translater);

            UniversalData UniData;
            if (_SavedTranslations.ContainsKey(translater))
                UniData = _SavedTranslations[translater];
            else
                UniData = new UniversalData(PointBlankServer.TranslationsPath + "/" + (string.IsNullOrEmpty(path) ? "" : path + "/") + translatable.Name);
            JsonData JSON = UniData.GetData(EDataType.JSON) as JsonData;

            if (!_SavedTranslations.ContainsKey(translater))
                _SavedTranslations.Add(translater, UniData);
            if (UniData.CreatedNew)
            {
                foreach (KeyValuePair<string, string> kvp in translations)
                {
                    if (JSON.CheckKey(kvp.Key))
                        JSON.Document[kvp.Key] = kvp.Value;
                    else
                        JSON.Document.Add(kvp.Key, kvp.Value);
                }
            }
            else
            {
                foreach (JProperty property in JSON.Document.Properties())
                {
                    if (translations[property.Name] == null)
                        continue;

                    translations[property.Name] = (string)property.Value;
                }
            }
            UniData.Save();
        }

        private void SaveTranslatable(Type translatable)
        {
            foreach (ITranslatable translator in _SavedTranslations.Keys)
            {
                if (translator.GetType() == translatable)
                {
                    foreach (KeyValuePair<string, string> translation in translator.Translations)
                    {
                        JsonData JSON = _SavedTranslations[translator].GetData(EDataType.JSON) as JsonData;

                        if (JSON.Document[translation.Key] != null)
                            JSON.Document[translation.Key] = translation.Value;
                        else
                            JSON.Document.Add(translation.Key, translation.Value);
                    }
                    _SavedTranslations[translator].Save();
                    break;
                }
            }
        }
        #endregion

        public void Load()
        {
            if (Initialized)
                return;

            if (!Directory.Exists(PointBlankServer.ConfigurationsPath))
                Directory.CreateDirectory(PointBlankServer.ConfigurationsPath); // Create configurations directory
            if (!Directory.Exists(PointBlankServer.TranslationsPath))
                Directory.CreateDirectory(PointBlankServer.TranslationsPath); // Create translations directory
            if (!Directory.Exists(PointBlankServer.DataPath))
                Directory.CreateDirectory(PointBlankServer.DataPath); // Create data directory

            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies().Where(a => Attribute.GetCustomAttribute(a, typeof(PointBlankExtensionAttribute)) != null))
                foreach (Type class_type in asm.GetTypes())
                    LoadInterface(class_type);

            // Setup the events
            PointBlankPluginEvents.OnPluginLoaded += new PointBlankPluginEvents.PluginEventHandler(OnPluginLoaded);
            PointBlankPluginEvents.OnPluginUnloaded += new PointBlankPluginEvents.PluginEventHandler(OnPluginUnloaded);

            // Set the variables
            Initialized = true;
        }

        public void Unload()
        {
            if (!Initialized)
                return;

            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies().Where(a => Attribute.GetCustomAttribute(a, typeof(PointBlankExtensionAttribute)) != null))
                foreach (Type class_type in asm.GetTypes())
                    SaveInterface(class_type);

            // Set the variables
            Initialized = false;
        }

        #region Public Functions
        public void LoadInterface(Type _interface)
        {
            if (!_interface.IsClass)
                return;

            if (typeof(IConfigurable).IsAssignableFrom(_interface) && _interface != typeof(IConfigurable))
                LoadConfigurable(_interface);
            if (typeof(ITranslatable).IsAssignableFrom(_interface) && _interface != typeof(ITranslatable))
                LoadTranslatable(_interface);
        }

        public void SaveInterface(Type _interface)
        {
            if (!_interface.IsClass)
                return;

            if (typeof(IConfigurable).IsAssignableFrom(_interface) && _interface != typeof(IConfigurable))
                SaveConfigurable(_interface);
            if (typeof(ITranslatable).IsAssignableFrom(_interface) && _interface != typeof(ITranslatable))
                SaveTranslatable(_interface);
        }
        #endregion

        #region Event Functions
        private void OnPluginLoaded(PointBlankPlugin plugin)
        {
            foreach (Type _class in plugin.GetType().Assembly.GetTypes())
                LoadInterface(_class);
        }

        private void OnPluginUnloaded(PointBlankPlugin plugin)
        {
            foreach (Type _class in plugin.GetType().Assembly.GetTypes())
                SaveInterface(_class);
        }
        #endregion
    }
}
