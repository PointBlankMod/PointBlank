using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using PointBlank.Framework.Permissions.Ring;
using SDG.Unturned;
using UnityEngine;

namespace PointBlank.API
{
    /// <summary>
    /// Logging methods for PointBlank
    /// </summary>
    [RingPermission(SecurityAction.Demand, ring = RingPermissionRing.None)]
    public static class Logging
    {
        /// <summary>
        /// Logs text into the logs file and console
        /// </summary>
        /// <param name="log">Object/Text to log</param>
        /// <param name="inConsole">Should the text be printed into the console</param>
        public static void Log(object log, bool inConsole = true)
        {
            log = "[PointBlank] " + log;

            if(!inConsole || CommandWindow.output == null)
                Debug.Log(log);
            if (inConsole)
                CommandWindow.Log(log);
        }

        /// <summary>
        /// Logs an error into the logs file and console
        /// </summary>
        /// <param name="log">Object/Text to log</param>
        /// <param name="ex">Exception to log</param>
        /// <param name="exInConsole">Should the exception show in the console</param>
        /// <param name="inConsole">Should the text be printed into the console</param>
        public static void LogError(object log, Exception ex, bool exInConsole = false, bool inConsole = true)
        {
            log = "[PointBlank] ERROR: " + log;

            if (!inConsole || CommandWindow.output == null)
                Debug.LogError(log);
            if(!exInConsole || CommandWindow.output == null)
                Debug.LogException(ex);
            if (inConsole)
                CommandWindow.LogError(log);
            if (exInConsole)
                CommandWindow.LogError(ex.ToString());
        }

        /// <summary>
        /// Logs a warning into the logs file and console
        /// </summary>
        /// <param name="log">Object/Text to log</param>
        /// <param name="inConsole">Should the text be printed into the console</param>
        public static void LogWarning(object log, bool inConsole = true)
        {
            log = "[PointBlank] WARNING: " + log;

            if (!inConsole || CommandWindow.output == null)
                Debug.Log(log);
            if (inConsole)
                CommandWindow.LogWarning(log);
        }

        /// <summary>
        /// Logs important text into the logs file and console
        /// </summary>
        /// <param name="log">Object/Text to log</param>
        /// <param name="inConsole">Should the text be printed into the console</param>
        public static void LogImportant(object log, bool inConsole = true)
        {
            log = "[PointBlank] IMPORTANT: " + log;

            if (!inConsole || CommandWindow.output == null)
                Debug.Log(log);
            if (inConsole)
                CommandWindow.Log(log, ConsoleColor.Cyan);
        }
    }
}
