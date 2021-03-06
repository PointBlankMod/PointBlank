﻿using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Plugins;
using PointBlank.API.Services;
using PointBlank.API.Detour;
using PointBlank.API.Extension;

namespace PointBlank.Services.DetourManager
{
    internal class DetourManager : PointBlankService
    {
        #region Variables
        private static Dictionary<DetourAttribute, DetourWrapper> _Detours = new Dictionary<DetourAttribute, DetourWrapper>(); // Dictionary of detours
        #endregion

        #region Properties
        public static Dictionary<DetourAttribute, DetourWrapper> Detours => _Detours; // The public detours

        public bool Initialized { get; private set; } = false; // Is the detour manager initialized

        public override int LaunchIndex => 0;
        #endregion

        public override void Load()
        {
            if (Initialized)
                return;

            // Set the events
            PointBlankPluginEvents.OnPluginLoaded += OnPluginLoaded;
            PointBlankPluginEvents.OnPluginUnloaded += OnPluginUnloaded;

            // Main code
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies().Where(a => Attribute.GetCustomAttribute(a, typeof(PointBlankExtensionAttribute)) != null))
                foreach (Type tClass in asm.GetTypes())
                    if (tClass.IsClass)
                        foreach(MethodInfo method in tClass.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
                            LoadDetour(method);

            // Set the variables
            Initialized = true;
        }

        public override void Unload()
        {
            if (!Initialized)
                return;

            // Unload the events
            PointBlankPluginEvents.OnPluginLoaded -= OnPluginLoaded;
            PointBlankPluginEvents.OnPluginUnloaded -= OnPluginUnloaded;

            // Main code
            while(Detours.Count > 0)
            {
                DetourAttribute att = Detours.Keys.ElementAt(0);

                Detours[att].Revert();
                Detours.Remove(att);
            }

            // Set the variables
            Initialized = false;
        }

        #region Public Functions
        public void LoadDetour(MethodInfo method)
        {
            // Setup variables
            DetourAttribute attribute = (DetourAttribute)Attribute.GetCustomAttribute(method, typeof(DetourAttribute));

            // Do checks
            if (attribute == null)
                return;
            if (!attribute.MethodFound)
                return;
            if (Detours.Count(a => a.Key.Method == attribute.Method) > 0)
                return;

            try
            {
                DetourWrapper wrapper = new DetourWrapper(attribute.Method, method, attribute);

                wrapper.Detour();

                Detours.Add(attribute, wrapper);
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Error detouring: " + method.Name, ex);
            }
        }
        #endregion

        #region Event Functions
        private void OnPluginLoaded(PointBlankPlugin plugin)
        {
            foreach(Type tClass in plugin.GetType().Assembly.GetTypes())
                if (tClass.IsClass)
                    foreach (MethodInfo method in tClass.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
                        LoadDetour(method);
        }

        private void OnPluginUnloaded(PointBlankPlugin plugin)
        {
            foreach(KeyValuePair<DetourAttribute, DetourWrapper> kvp in Detours.Where(a => a.Key.Method.DeclaringType.Assembly == plugin.GetType().Assembly && !a.Value.Local))
            {
                kvp.Value.Revert();
                Detours.Remove(kvp.Key);
            }
        }
        #endregion
    }
}
