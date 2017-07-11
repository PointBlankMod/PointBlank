using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Net.Security;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Text;
using SDG.Framework.Modules;
using PointBlank.API;
using SDG.Unturned;
using UnityEngine;
using PointBlank.Framework.Objects;
using PointBlank.Framework;
using PointBlank.API.DataManagment;
using PointBlank.API.Interfaces;
using PointBlank.Services.PluginManager;
using Newtonsoft.Json.Linq;
using Steamworks;
using PointBlank.API.Collections;

namespace PointBlank
{
    internal class PointBlank : IModuleNexus
    {
        #region Properties
        public static PointBlank Instance { get; private set; } // Self instance
        public static bool Enabled { get; private set; } // Is PointBlank running

        public string ConfigurationDirectory => null;
        public ConfigurationList Configurations => new ConfigurationList()
        {
            { "ConfigFormat", EDataType.JSON }
        };
        public Dictionary<Type, IConfigurable> ConfigurationDictionary => Enviroment.FrameworkConfig;
        #endregion

        #region Nexus Interface
        public void initialize()
        {
            if ((!Provider.isServer && !Dedicator.isDedicated) || !Environment.GetCommandLineArgs().Contains("-pointblank")) // Don't run if this isn't a server or if the -pointblank argument wasn't added
                return;
            if (Instance != null && Enabled) // Don't run if already running
                return;

            Logging.LogImportant("Loading " + PointBlankInfo.Name + " v" + PointBlankInfo.Version + "...");

            // Run required methods
            ApplyPatches();

            // Setup the runtime objects
            Enviroment.runtimeObjects.Add("Framework", new RuntimeObject(new GameObject("Framework")));
            Enviroment.runtimeObjects.Add("Extensions", new RuntimeObject(new GameObject("Extensions")));
            Enviroment.runtimeObjects.Add("Services", new RuntimeObject(new GameObject("Services")));
            Enviroment.runtimeObjects.Add("Plugins", new RuntimeObject(new GameObject("Plugins")));

            // Add the code objects
            Enviroment.runtimeObjects["Framework"].AddCodeObject<InterfaceManager>(); // Both the service manager and interface manager are important without them
            Enviroment.runtimeObjects["Framework"].AddCodeObject<ServiceManager>(); // the modloader won't be able to function properly making it as usefull as Rocket

            // Run the inits
            Enviroment.runtimeObjects["Framework"].GetCodeObject<InterfaceManager>().Init();
            Enviroment.runtimeObjects["Framework"].GetCodeObject<ServiceManager>().Init();

            // Run required methods
            RunRequirements();

            // Initialize
            Instance = this;
            Enabled = true;
#if !DEBUG
            Console.Clear();
#endif
            Dedicator.commandWindow.title = PointBlankInfo.Name + " v" + PointBlankInfo.Version;

            Logging.LogImportant("Loaded " + PointBlankInfo.Name + " v" + PointBlankInfo.Version + "!");
        }

        public void shutdown()
        {
            if ((!Provider.isServer && !Dedicator.isDedicated) || !Environment.GetCommandLineArgs().Contains("-pointblank")) // Don't shutdown if this isn't a server or if the -pointblank argument wasn't added
                return;
            if (Instance == null || !Enabled) // Don't shutdown if it is not running
                return;

            Logging.LogImportant("Shutting down " + PointBlankInfo.Name + " v" + PointBlankInfo.Version + "...");

            // Uninit
            Enabled = false;
            Instance = null;

            // Run the shutdowns
            Enviroment.runtimeObjects["Framework"].GetCodeObject<ServiceManager>().Shutdown();
            Enviroment.runtimeObjects["Framework"].GetCodeObject<InterfaceManager>().Shutdown();

            // Remove the runtime objects
            Enviroment.runtimeObjects["Framework"].RemoveCodeObject<ServiceManager>();
            Enviroment.runtimeObjects["Framework"].RemoveCodeObject<InterfaceManager>();

            // Remove the runtime objects
            Enviroment.runtimeObjects.Remove("Plugins");
            Enviroment.runtimeObjects.Remove("Services");
            Enviroment.runtimeObjects.Remove("Extensions");
            Enviroment.runtimeObjects.Remove("Framework");

            // Run the required functions
            RunRequirementsShutdown();

            Logging.LogImportant("Shut down " + PointBlankInfo.Name + " v" + PointBlankInfo.Version + "!");
        }
        #endregion

        #region Functions
        private void ApplyPatches() => new I18N.West.CP1250();

        private void RunRequirements()
        {
            TranslationManager.Load();
        }

        private void RunRequirementsShutdown()
        {
            foreach(SQLData sql in Enviroment.SQLConnections.Where(a => a.Connected))
                sql.Disconnect();

            TranslationManager.Save();
        }
        #endregion

        #region Event Functions
        internal static bool ValidateCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors) => (errors == SslPolicyErrors.None);
        #endregion
    }
}
