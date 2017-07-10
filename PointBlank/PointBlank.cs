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
using PointBlank.Services.PluginManager;
using Newtonsoft.Json.Linq;
using Steamworks;

namespace PointBlank
{
    internal class PointBlank : IModuleNexus
    {
        #region Properties
        public static PointBlank Instance { get; private set; } // Self instance
        public static bool Enabled { get; private set; } // Is PointBlank running

        private static UniversalData LoaderData { get; set; } // The configuration data
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
            Enviroment.runtimeObjects["Framework"].AddCodeObject<ServiceManager>();
            Enviroment.runtimeObjects["Framework"].AddCodeObject<InterfaceManager>();

            // Run the inits
            Enviroment.runtimeObjects["Framework"].GetCodeObject<ServiceManager>().Init();
            Enviroment.runtimeObjects["Framework"].GetCodeObject<InterfaceManager>().Init();

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
            LoaderData = new UniversalData(ServerInfo.ConfigurationsPath + "\\PointBlank");
            JsonData data = LoaderData.GetData(EDataType.JSON) as JsonData;

            if (!LoaderData.CreatedNew)
            {
                data.Verify(new Dictionary<string, JToken>()
                {
                    { "ConfigFormat", "JSON" }
                });
                switch (((string)data.Document["ConfigFormat"]).ToLower())
                {
                    case "xml":
                        Configuration.SaveDataType = EDataType.XML;
                        break;
                    case "json":
                        Configuration.SaveDataType = EDataType.JSON;
                        break;
                    default:
                        Configuration.SaveDataType = EDataType.UNKNOWN;
                        break;
                }
            }
            else
            {
                data.Document.Add("ConfigFormat", "JSON");

                Configuration.SaveDataType = EDataType.JSON;
                LoaderData.Save();
            }
            TranslationManager.Load();
        }

        private void RunRequirementsShutdown()
        {
            foreach(SQLData sql in Enviroment.SQLConnections.Where(a => a.Connected))
                sql.Disconnect();

            LoaderData.Save();
            TranslationManager.Save();
        }
        #endregion

        #region Event Functions
        internal static bool ValidateCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors) => (errors == SslPolicyErrors.None);
        #endregion
    }
}
