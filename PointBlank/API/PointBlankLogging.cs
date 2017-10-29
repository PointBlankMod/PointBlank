using System;
using System.IO;
using System.Diagnostics;

namespace PointBlank.API
{
    /// <summary>
    /// Logging methods for PointBlank
    /// </summary>
    public static class PointBlankLogging
    {
        #region Info
        private static string LogDirectory = Directory.GetCurrentDirectory();
        #endregion

        static PointBlankLogging()
        {
            if (File.Exists(LogDirectory + "/PointBlankOld.log"))
                File.Delete(LogDirectory + "/PointBlankOld.log");
            if (File.Exists(LogDirectory + "/PointBlank.log"))
                File.Move(LogDirectory + "/PointBlank.log", LogDirectory + "/PointBlankOld.log");
        }

        /// <summary>
        /// Logs text into the logs file and console
        /// </summary>
        /// <param name="log">Object/Text to log</param>
        /// <param name="inConsole">Should the text be printed into the console</param>
        /// <param name="prefix">The logging prefix, ex. [LOG] or [ERROR]</param>
        public static void Log(object log, bool inConsole = true, ConsoleColor color = ConsoleColor.White, string prefix = "[LOG]")
        {
			string asm = GetAsm();

			log = prefix + " " + asm + " >> " + log;
            File.AppendAllText(LogDirectory + "/PointBlank.log", log + Environment.NewLine);
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

		/// <summary>
		/// Get the assembly name from the stack trace.
		/// </summary>
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
    }
}
