using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace PointBlank.API.Logging
{
    public static class PointBlankLogging
    {
        internal static void Initialize()
        {
            if (File.Exists(PointBlankInfo.Previous_Log))
                File.Delete(PointBlankInfo.Previous_Log);
            if (File.Exists(PointBlankInfo.Current_Log))
                File.Move(PointBlankInfo.Current_Log, PointBlankInfo.Previous_Log);
            PointBlankConsole.Initialize();
        }

        #region Private Functions
        private static void Log(object log, string asm, bool inConsole = true, ConsoleColor color = ConsoleColor.White, string prefix = "[LOG]")
        {
            log = prefix + " " + asm + " >> " + log;
            File.AppendAllText(PointBlankInfo.Current_Log, log + Environment.NewLine);
            if (inConsole)
                PointBlankConsole.WriteLine(log, color);
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Logs an object/text to the PointBlank log file and console
        /// </summary>
        /// <param name="log">Object/Text to place into the file/console</param>
        /// <param name="inConsole">Should it be printed into the console</param>
        public static void Log(object log, bool inConsole = true)
        {
            Log(log.ToString(), Assembly.GetCallingAssembly().GetName().Name, inConsole, ConsoleColor.White);
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
            Log(log, Assembly.GetCallingAssembly().GetName().Name, inConsole, ConsoleColor.Red, "[ERROR]");
            Log(ex, Assembly.GetCallingAssembly().GetName().Name, exInConsole, ConsoleColor.DarkRed);
        }

        /// <summary>
        /// Logs a warning into the logs file and console
        /// </summary>
        /// <param name="log">Object/Text to log</param>
        /// <param name="inConsole">Should the text be printed into the console</param>
        public static void LogWarning(object log, bool inConsole = true) =>
            Log(log, Assembly.GetCallingAssembly().GetName().Name, inConsole, ConsoleColor.Yellow, "[WARNING]");

        /// <summary>
        /// Logs important text into the logs file and console
        /// </summary>
        /// <param name="log">Object/Text to log</param>
        /// <param name="inConsole">Should the text be printed into the console</param>
        public static void LogImportant(object log, bool inConsole = true) =>
            Log(log, Assembly.GetCallingAssembly().GetName().Name, inConsole, ConsoleColor.Cyan, "[IMPORTANT]");
        #endregion
    }
}
