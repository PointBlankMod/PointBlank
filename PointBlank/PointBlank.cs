using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using PointBlank.API;
using UnityEngine;
using PointBlank.Framework.Objects;
using PointBlank.Framework;
using PointBlank.API.DataManagment;
using PointBlank.API.Console;

namespace PointBlank
{
    public class PointBlank
    {
        #region Properties
        public static PointBlank Instance { get; private set; } // Self instance
        public static bool Enabled { get; private set; } // Is PointBlank running
		#endregion

		#region Loader Functions
		public void Initialize()
        {
            if (!Environment.GetCommandLineArgs().Contains("-pointblank")) // Don't run if this isn't a server or if the -pointblank argument wasn't added
                return;
			if (Instance != null && Enabled) // Don't run if already running
                return;

            if (LinuxConsoleUtils.IsLinux)
            {
                LinuxConsoleInput.Init();
                LinuxConsoleOutput.Init();
            }

            PointBlankLogging.LogImportant("Loading " + PointBlankInfo.Name + " v" + PointBlankInfo.Version + "...");

            // Run required methods
            ApplyPatches();

            // Setup the runtime objects
            PBEnvironment.runtimeObjects.Add("Framework", new RuntimeObject(new GameObject("Framework")));
            PBEnvironment.runtimeObjects.Add("Extensions", new RuntimeObject(new GameObject("Extensions")));
            PBEnvironment.runtimeObjects.Add("Services", new RuntimeObject(new GameObject("Services")));
            PBEnvironment.runtimeObjects.Add("Plugins", new RuntimeObject(new GameObject("Plugins")));

            // Add the code objects
            PBEnvironment.runtimeObjects["Framework"].AddCodeObject<InterfaceManager>(); // Both the service manager and interface manager are important without them
            PBEnvironment.runtimeObjects["Framework"].AddCodeObject<ServiceManager>(); // the mod loader won't be able to function properly making it as useful as Rocket
            PBEnvironment.runtimeObjects["Framework"].AddCodeObject<ExtensionManager>(); // This one isn't as important but useful for extensions of the mod loader

            // Run the inits
            PBEnvironment.runtimeObjects["Framework"].GetCodeObject<InterfaceManager>().Load();
            PBEnvironment.runtimeObjects["Framework"].GetCodeObject<ServiceManager>().Load();
            PBEnvironment.runtimeObjects["Framework"].GetCodeObject<ExtensionManager>().Load();

            // Run required methods
            RunRequirements();

            // Initialize
            Instance = this;
            Enabled = true;

            PointBlankLogging.LogImportant("Loaded " + PointBlankInfo.Name + " v" + PointBlankInfo.Version + "!");
        }

        public void Shutdown()
        {
            if (!Environment.GetCommandLineArgs().Contains("-pointblank")) // Don't shutdown if this isn't a server or if the -pointblank argument wasn't added
                return;
            if (Instance == null || !Enabled) // Don't shutdown if it is not running
                return;

            PointBlankLogging.LogImportant("Shutting down " + PointBlankInfo.Name + " v" + PointBlankInfo.Version + "...");

            // Uninit
            Enabled = false;
            Instance = null;

            // Run the shutdowns
            PBEnvironment.runtimeObjects["Framework"].GetCodeObject<ExtensionManager>().Unload();
            PBEnvironment.runtimeObjects["Framework"].GetCodeObject<ServiceManager>().Unload();
            PBEnvironment.runtimeObjects["Framework"].GetCodeObject<InterfaceManager>().Unload();

            // Remove the runtime objects
            PBEnvironment.runtimeObjects["Framework"].RemoveCodeObject<InterfaceManager>();
            PBEnvironment.runtimeObjects["Framework"].RemoveCodeObject<ServiceManager>();
            PBEnvironment.runtimeObjects["Framework"].RemoveCodeObject<InterfaceManager>();

            // Remove the runtime objects
            PBEnvironment.runtimeObjects.Remove("Plugins");
            PBEnvironment.runtimeObjects.Remove("Services");
            PBEnvironment.runtimeObjects.Remove("Extensions");
            PBEnvironment.runtimeObjects.Remove("Framework");

            // Run the required functions
            RunRequirementsShutdown();

            PointBlankLogging.LogImportant("Shut down " + PointBlankInfo.Name + " v" + PointBlankInfo.Version + "!");
        }
        #endregion

        #region Functions
        private void ApplyPatches() => new I18N.West.CP1250();

        private void RunRequirements()
        {
            if (LinuxConsoleUtils.IsLinux)
                LinuxConsoleOutput.PostInit();
        }

        private void RunRequirementsShutdown()
        {
            PBEnvironment.Running = false;
            foreach(SQLData sql in PBEnvironment.SQLConnections.Where(a => a.Connected))
                sql.Disconnect();
        }
        #endregion

        #region Event Functions
        internal static bool ValidateCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors) => (errors == SslPolicyErrors.None);
        #endregion
    }
}
