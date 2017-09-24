using System;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using PointBlank.API;
using UnityEngine;
using PointBlank.Framework.Objects;
using PointBlank.Framework;
using PointBlank.API.DataManagment;

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

            PointBlankLogging.LogImportant("Loading " + PointBlankInfo.Name + " v" + PointBlankInfo.Version + "...");

            // Run required methods
            ApplyPatches();

            // Setup the runtime objects
            PointBlankEnvironment.RuntimeObjects.Add("Framework", new RuntimeObject(new GameObject("Framework")));
            PointBlankEnvironment.RuntimeObjects.Add("Extensions", new RuntimeObject(new GameObject("Extensions")));
            PointBlankEnvironment.RuntimeObjects.Add("Services", new RuntimeObject(new GameObject("Services")));
            PointBlankEnvironment.RuntimeObjects.Add("Plugins", new RuntimeObject(new GameObject("Plugins")));

            // Add the code objects
            PointBlankEnvironment.RuntimeObjects["Framework"].AddCodeObject<InterfaceManager>(); // Both the service manager and interface manager are important without them
            PointBlankEnvironment.RuntimeObjects["Framework"].AddCodeObject<ServiceManager>(); // the mod loader won't be able to function properly making it as useful as Rocket
            PointBlankEnvironment.RuntimeObjects["Framework"].AddCodeObject<ExtensionManager>(); // This one isn't as important but useful for extensions of the mod loader

            // Run the inits
            PointBlankEnvironment.RuntimeObjects["Framework"].GetCodeObject<InterfaceManager>().Load();
            PointBlankEnvironment.RuntimeObjects["Framework"].GetCodeObject<ServiceManager>().Load();
            PointBlankEnvironment.RuntimeObjects["Framework"].GetCodeObject<ExtensionManager>().Load();

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
            PointBlankEnvironment.RuntimeObjects["Framework"].GetCodeObject<ExtensionManager>().Unload();
            PointBlankEnvironment.RuntimeObjects["Framework"].GetCodeObject<ServiceManager>().Unload();
            PointBlankEnvironment.RuntimeObjects["Framework"].GetCodeObject<InterfaceManager>().Unload();

            // Remove the runtime objects
            PointBlankEnvironment.RuntimeObjects["Framework"].RemoveCodeObject<InterfaceManager>();
            PointBlankEnvironment.RuntimeObjects["Framework"].RemoveCodeObject<ServiceManager>();
            PointBlankEnvironment.RuntimeObjects["Framework"].RemoveCodeObject<InterfaceManager>();

            // Remove the runtime objects
            PointBlankEnvironment.RuntimeObjects.Remove("Plugins");
            PointBlankEnvironment.RuntimeObjects.Remove("Services");
            PointBlankEnvironment.RuntimeObjects.Remove("Extensions");
            PointBlankEnvironment.RuntimeObjects.Remove("Framework");

            // Run the required functions
            RunRequirementsShutdown();

            PointBlankLogging.LogImportant("Shut down " + PointBlankInfo.Name + " v" + PointBlankInfo.Version + "!");
        }
        #endregion

        #region Functions
        private void ApplyPatches() => new I18N.West.CP1250();

        private void RunRequirements()
        {
            // Need to add something here
        }

        private void RunRequirementsShutdown()
        {
            PointBlankEnvironment.Running = false;
            foreach(SqlData sql in PointBlankEnvironment.SqlConnections.Where(a => a.Connected))
                sql.Disconnect();
        }
        #endregion

        #region Event Functions
        internal static bool ValidateCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors) => (errors == SslPolicyErrors.None);
        #endregion
    }
}
