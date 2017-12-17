using System;
using System.IO;
using System.Collections.Generic;
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
            File.Create(PointBlankInfo.Current_Log);
            PointBlankConsole.Initialize();
        }

        #region Private Functions
        private static string GetAsm()
        {
            StackTrace stack = new StackTrace(false);
            string asm = "";

            try
            {
                asm = stack.FrameCount > 0 ? stack.GetFrame(1).GetMethod().DeclaringType.Assembly.GetName().Name : "Not Found";

                if (stack.FrameCount > 1 && (asm == "PointBlank" || asm == "Assembly-CSharp" || asm == "UnityEngine"))
                    asm = stack.GetFrame(2).GetMethod().DeclaringType.Assembly.GetName().Name;
                if (asm == "Assembly-CSharp" || asm == "UnityEngine")
                    asm = "Game";
            }
            catch (Exception ex)
            {
                asm = "Mono";
            }
            return asm;
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Logs an object/text to the PointBlank log file and console
        /// </summary>
        /// <param name="log">Object/Text to place into the file/console</param>
        /// <param name="inConsole">Should it be printed into the console</param>
        /// <param name="color">The color of the logged object/text(console only)</param>
        /// <param name="prefix">The prefix of the log</param>
        public static void Log(object log, bool inConsole = true, ConsoleColor color = ConsoleColor.White, string prefix = "[LOG]")
        {
            string asm = GetAsm();

            log = prefix + " " + asm + " >> " + log;
            File.AppendAllText(PointBlankInfo.Current_Log, log + Environment.NewLine);
            if (inConsole)
                PointBlankConsole.WriteLine(log, color);
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
            Log(log, inConsole, ConsoleColor.Red, "[ERROR]");
            Log(ex, exInConsole, ConsoleColor.DarkRed);
        }

        /// <summary>
        /// Logs a warning into the logs file and console
        /// </summary>
        /// <param name="log">Object/Text to log</param>
        /// <param name="inConsole">Should the text be printed into the console</param>
        public static void LogWarning(object log, bool inConsole = true) =>
            Log(log, inConsole, ConsoleColor.Yellow, "[WARNING]");

        /// <summary>
        /// Logs important text into the logs file and console
        /// </summary>
        /// <param name="log">Object/Text to log</param>
        /// <param name="inConsole">Should the text be printed into the console</param>
        public static void LogImportant(object log, bool inConsole = true) =>
            Log(log, inConsole, ConsoleColor.Cyan, "[IMPORTANT]");
        #endregion
    }
}
