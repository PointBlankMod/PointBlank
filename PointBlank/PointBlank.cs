using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using PointBlank.API.Logging;

namespace PointBlank
{
    public static class PointBlank
    {
        #region Public Functions
        /// <summary>
        /// Loads the PointBlank plugin loader and sets up all required systems
        /// </summary>
        public static void LoadPointBlank()
        {
            // Run checks
            if (!Environment.GetCommandLineArgs().Contains("-pointblank"))
                return;
            if (PointBlankEnvironment.Enabled)
                return;

            // Initialize system
            PointBlankLogging.Initialize();
            new I18N.West.CP1250();

            PointBlankLogging.LogImportant("Loading " + PointBlankInfo.Name + " v" + PointBlankInfo.Version + "...");
        }

        /// <summary>
        /// Unloads the PointBlank plugin loader and disables all systems
        /// </summary>
        public static void UnloadPointBlank()
        {
            // Run checks
            if (!Environment.GetCommandLineArgs().Contains("-pointblank"))
                return;
            if (!PointBlankEnvironment.Enabled)
                return;
        }

        /// <summary>
        /// Reloads the PointBlank plugin loader and resets all systems including configurations and translations
        /// </summary>
        public static void ReloadPointBlank()
        {
            // Run checks
            if (!Environment.GetCommandLineArgs().Contains("-pointblank"))
                return;
            if (!PointBlankEnvironment.Enabled)
                return;
        }
        #endregion

        #region Private Functions
        #endregion
    }
}
