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
        private static Dictionary<Configurable, UniversalData> _savedConfigs = new Dictionary<Configurable, UniversalData>();
        private static Dictionary<Translatable, UniversalData> _savedTranslations = new Dictionary<Translatable, UniversalData>();
        #endregion

        #region Properties
        public bool Initialized { get; private set; } = false;
        #endregion

        #region Private Functions
        private void LoadConfigurable(Type configurable)
        {
            string path = null;
            ConfigurationList configurations = null;
            Dictionary<Type, Configurable> dictionary = null;
            Configurable cfg = (Configurable)Activator.CreateInstance(configurable);

            path = cfg.ConfigurationDirectory;
            configurations = cfg.Configurations;
            dictionary = cfg.ConfigurationDictionary;
            if (dictionary != null)
                dictionary.Add(configurable, cfg);

            UniversalData uniData;
            if (_savedConfigs.ContainsKey(cfg))
                uniData = _savedConfigs[cfg];
            else
                uniData = new UniversalData(PointBlankServer.ConfigurationsPath + "/" + (string.IsNullOrEmpty(path) ? "" : path + "/") + configurable.Name);
            JsonData Json = uniData.GetData(EDataType.Json) as JsonData;

            if (!_savedConfigs.ContainsKey(cfg))
                _savedConfigs.Add(cfg, uniData);
            if(uniData.CreatedNew)
            {
                foreach(KeyValuePair<string, object> kvp in configurations)
                {
                    if (Json.CheckKey(kvp.Key))
                        Json.Document[kvp.Key] = JToken.FromObject(kvp.Value);
                    else
                        Json.Document.Add(kvp.Key, JToken.FromObject(kvp.Value));
                }
            }
            else
            {
                foreach(JProperty property in Json.Document.Properties())
                {
                    if (configurations[property.Name] == null)
                        continue;

                    configurations[property.Name] = property.Value.ToObject(configurations[property.Name].GetType());
                }
            }
            uniData.Save();
        }

        private void SaveConfigurable(Type configurable)
        {
            foreach(Configurable cfg in _savedConfigs.Keys)
            {
                if(cfg.GetType() == configurable)
                {
                    foreach(KeyValuePair<string, object> conf in cfg.Configurations)
                    {
                        JsonData Json  = _savedConfigs[cfg].GetData(EDataType.Json) as JsonData;

                        if (Json.Document[conf.Key] != null)
                            Json.Document[conf.Key] = JToken.FromObject(conf.Value);
                        else
                            Json.Document.Add(conf.Key, JToken.FromObject(conf.Value));
                    }
                    _savedConfigs[cfg].Save();
                    break;
                }
            }
        }

        private void LoadTranslatable(Type translatable)
        {
            string path = null;
            TranslationList translations = null;
            Dictionary<Type, Translatable> dictionary = null;
            Translatable translater = (Translatable)Activator.CreateInstance(translatable);

            path = translater.TranslationDirectory;
            translations = translater.Translations;
            dictionary = translater.TranslationDictionary;
            if (dictionary != null)
                dictionary.Add(translatable, translater);

            UniversalData uniData;
            if (_savedTranslations.ContainsKey(translater))
                uniData = _savedTranslations[translater];
            else
                uniData = new UniversalData(PointBlankServer.TranslationsPath + "/" + (string.IsNullOrEmpty(path) ? "" : path + "/") + translatable.Name);
            JsonData Json = uniData.GetData(EDataType.Json) as JsonData;

            if (!_savedTranslations.ContainsKey(translater))
                _savedTranslations.Add(translater, uniData);
            if (uniData.CreatedNew)
            {
                foreach (KeyValuePair<string, string> kvp in translations)
                {
                    if (Json.CheckKey(kvp.Key))
                        Json.Document[kvp.Key] = kvp.Value;
                    else
                        Json.Document.Add(kvp.Key, kvp.Value);
                }
            }
            else
            {
                foreach (JProperty property in Json.Document.Properties())
                {
                    if (translations[property.Name] == null)
                        continue;

                    translations[property.Name] = (string)property.Value;
                }
            }
            uniData.Save();
        }

        private void SaveTranslatable(Type translatable)
        {
            foreach (Translatable translator in _savedTranslations.Keys)
            {
                if (translator.GetType() == translatable)
                {
                    foreach (KeyValuePair<string, string> translation in translator.Translations)
                    {
                        JsonData Json = _savedTranslations[translator].GetData(EDataType.Json) as JsonData;

                        if (Json.Document[translation.Key] != null)
                            Json.Document[translation.Key] = translation.Value;
                        else
                            Json.Document.Add(translation.Key, translation.Value);
                    }
                    _savedTranslations[translator].Save();
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
                foreach (Type classType in asm.GetTypes())
                    LoadInterface(classType);

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
<<<<<<< HEAD
                foreach (Type class_type in asm.GetTypes())
                    SaveInterface(class_type);
=======
                foreach (Type classType in asm.GetTypes())
                    SaveInterface(classType);
>>>>>>> master

            // Set the variables
            Initialized = false;
        }

        #region Public Functions
        public void LoadInterface(Type _interface)
        {
            if (!_interface.IsClass)
                return;

<<<<<<< HEAD
            if (typeof(IConfigurable).IsAssignableFrom(_interface) && _interface != typeof(IConfigurable))
                LoadConfigurable(_interface);
            if (typeof(ITranslatable).IsAssignableFrom(_interface) && _interface != typeof(ITranslatable))
=======
            if (typeof(Configurable).IsAssignableFrom(_interface) && _interface != typeof(Configurable))
                LoadConfigurable(_interface);
            if (typeof(Translatable).IsAssignableFrom(_interface) && _interface != typeof(Translatable))
>>>>>>> master
                LoadTranslatable(_interface);
        }

        public void SaveInterface(Type _interface)
        {
            if (!_interface.IsClass)
                return;

<<<<<<< HEAD
            if (typeof(IConfigurable).IsAssignableFrom(_interface) && _interface != typeof(IConfigurable))
                SaveConfigurable(_interface);
            if (typeof(ITranslatable).IsAssignableFrom(_interface) && _interface != typeof(ITranslatable))
=======
            if (typeof(Configurable).IsAssignableFrom(_interface) && _interface != typeof(Configurable))
                SaveConfigurable(_interface);
            if (typeof(Translatable).IsAssignableFrom(_interface) && _interface != typeof(Translatable))
>>>>>>> master
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
