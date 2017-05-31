using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Plugins;
using PointBlank.API.Services;
using PointBlank.API.Detour;
using PointBlank.Services.PluginManager;
using PM = PointBlank.Services.PluginManager.PluginManager;

namespace PointBlank.Services.DetourManager
{
    [Service("DetourManager", true)]
    internal class DetourManager : Service
    {
        #region Variables
        private static Dictionary<DetourAttribute, DetourWrapper> _Detours = new Dictionary<DetourAttribute, DetourWrapper>(); // Dictionary of detours
        #endregion

        #region Properties
        public static Dictionary<DetourAttribute, DetourWrapper> Detours { get { return _Detours; } } // The public detours

        public bool Initialized { get; private set; } = false; // Is the detour manager initialized
        #endregion

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
                Logging.LogError("Error detouring: " + method.Name, ex);
            }
        }
        #endregion

        #region Override Functions
        public override void Load()
        {
            if (Initialized)
                return;
            Logging.Log("Loading detours...");

            // Set the events
            PluginEvents.OnPluginLoaded += new PluginEvents.PluginEventHandler(OnPluginLoaded);
            PluginEvents.OnPluginUnloaded += new PluginEvents.PluginEventHandler(OnPluginUnloaded);

            // Main code
            foreach(Type tClass in Assembly.GetExecutingAssembly().GetTypes())
                if(tClass.IsClass)
                    foreach(MethodInfo method in tClass.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
                        LoadDetour(method);

            // Set the variables
            Initialized = true;
            Logging.Log("Detours loaded!");
        }

        public override void Unload()
        {
            if (!Initialized)
                return;
            Logging.Log("Unloading detours...");

            // Main code
            foreach (KeyValuePair<DetourAttribute, DetourWrapper> kvp in Detours.Where(a => a.Value.Local == false))
            {
                kvp.Value.Revert();
                Detours.Remove(kvp.Key);
            }

            // Set the variables
            Initialized = false;
            Logging.Log("Detours unloaded!");
        }
        #endregion

        #region Event Functions
        private void OnPluginLoaded(Plugin plugin)
        {
            PluginWrapper wrapper = PM.Plugins.First(a => a.PluginClass == plugin);

            foreach(Type tClass in wrapper.PluginAssembly.GetTypes())
                if (tClass.IsClass)
                    foreach (MethodInfo method in tClass.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
                        LoadDetour(method);
        }

        private void OnPluginUnloaded(Plugin plugin)
        {
            PluginWrapper wrapper = PM.Plugins.First(a => a.PluginClass == plugin);

            foreach(KeyValuePair<DetourAttribute, DetourWrapper> kvp in Detours.Where(a => a.Key.Method.DeclaringType.Assembly == wrapper.PluginAssembly && !a.Value.Local))
            {
                kvp.Value.Revert();
                Detours.Remove(kvp.Key);
            }
        }
        #endregion
    }
}
