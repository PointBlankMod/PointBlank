using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PointBlank.API.Logging
{
    /// <summary>
    /// The PointBlank console control API
    /// </summary>
    public static class PointBlankConsole
    {
        #region Variables
        private static ConsoleColor _SavedColor = ConsoleColor.White;
        private static TextReader _InputReader;

        private static string LinuxSTDOUTPath { get; }
        private static string LinuxSTDINPath { get; }
        #endregion

        #region Properties
        /// <summary>
        /// Is the operating system linux
        /// </summary>
        public static bool IsLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }

        /// <summary>
        /// The color scheme used for linux
        /// </summary>
        public static ELinuxConsoleColorScheme LinuxColorScheme { get; private set; } = ELinuxConsoleColorScheme.ANSI;
        #endregion

        #region Handlers
        /// <summary>
        /// Handles writing lines to the console
        /// </summary>
        /// <param name="text">The text that's being sent to the console</param>
        /// <param name="color">The color of the text being sent to the console</param>
        public delegate void WriteLineHandler(object text, ConsoleColor color);

        /// <summary>
        /// Handles inputs from the console
        /// </summary>
        /// <param name="text">The text written into the console</param>
        public delegate void InputHandler(string text);
        #endregion

        #region Events
        /// <summary>
        /// Called when a line is written to the console
        /// </summary>
        public static event WriteLineHandler OnWriteLine;

        /// <summary>
        /// Called when a user sends a command/text to the console
        /// </summary>
        public static event InputHandler OnInput;
        #endregion

        internal static void Initialize()
        {
            if (!IsLinux)
                return;

            Application.logMessageReceivedThreaded += OnLog;
            string path = LinuxSTDINPath.Replace('\\', '/');
            string[] splt = path.Split('/');
            string name = splt[splt.Length - 1];
            path = string.Join("/", splt, 0, splt.Length - 2);

            File.WriteAllText(LinuxSTDINPath, ConsoleColorToEscapeCode(ConsoleColor.Magenta));
            FileSystemWatcher fsWatcher = new FileSystemWatcher(path, name);
            fsWatcher.Changed += OnLinuxInput;
            _InputReader = new StreamReader(new FileStream(LinuxSTDINPath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite));
            fsWatcher.EnableRaisingEvents = true;
        }

        #region Conversions
        /// <summary>
        /// Converts the ConsoleColor type to UnityEngine color
        /// </summary>
        /// <param name="color">The ConsoleColor enum of a specific color</param>
        /// <returns>The UnityEngine version of the selected color</returns>
        public static Color ConsoleColorToColor(ConsoleColor color)
        {
            switch (color)
            {
                case ConsoleColor.Black:
                    return Color.black;
                case ConsoleColor.Blue:
                    return Color.blue;
                case ConsoleColor.Cyan:
                    return Color.cyan;
                case ConsoleColor.DarkBlue:
                    return new Color(0, 0, 139);
                case ConsoleColor.DarkCyan:
                    return new Color(0, 139, 139);
                case ConsoleColor.DarkGray:
                    return new Color(169, 169, 169);
                case ConsoleColor.DarkGreen:
                    return new Color(0, 100, 0);
                case ConsoleColor.DarkMagenta:
                    return new Color(139, 0, 139);
                case ConsoleColor.DarkRed:
                    return new Color(139, 0, 0);
                case ConsoleColor.DarkYellow:
                    return new Color(153, 153, 0);
                case ConsoleColor.Gray:
                    return Color.gray;
                case ConsoleColor.Green:
                    return Color.green;
                case ConsoleColor.Magenta:
                    return Color.magenta;
                case ConsoleColor.Red:
                    return Color.red;
                case ConsoleColor.White:
                    return Color.white;
                case ConsoleColor.Yellow:
                    return Color.yellow;
                default:
                    return Color.white;
            }
        }

        /// <summary>
        /// Converts the ConsoleColor type to an escape code for linux
        /// </summary>
        /// <param name="color">The ConsoleColor enum of a specific color</param>
        /// <returns>The escape code version of the selected color</returns>
        public static string ConsoleColorToEscapeCode(ConsoleColor color)
        {
            switch (LinuxColorScheme)
            {
                case ELinuxConsoleColorScheme.XTERM:
                    switch (color)
                    {
                        case ConsoleColor.Black:
                            return @"\033[30m";
                        case ConsoleColor.DarkRed:
                            return @"\033[31m";
                        case ConsoleColor.DarkGreen:
                            return @"\033[32m";
                        case ConsoleColor.DarkYellow:
                            return @"\033[33m";
                        case ConsoleColor.DarkBlue:
                            return @"\033[34m";
                        case ConsoleColor.DarkMagenta:
                            return @"\033[35m";
                        case ConsoleColor.DarkCyan:
                            return @"\033[36m";
                        case ConsoleColor.Gray:
                            return @"\033[30;1m";
                        case ConsoleColor.DarkGray:
                            return @"\033[30;1m";
                        case ConsoleColor.Red:
                            return @"\033[31;1m";
                        case ConsoleColor.Green:
                            return @"\033[32;1m";
                        case ConsoleColor.Yellow:
                            return @"\033[33;1m";
                        case ConsoleColor.Blue:
                            return @"\033[34;1m";
                        case ConsoleColor.Magenta:
                            return @"\033[35;1m";
                        case ConsoleColor.Cyan:
                            return @"\033[36;1m";
                        case ConsoleColor.White:
                            return @"\033[37;1m";
                    }
                    return "";
                case ELinuxConsoleColorScheme.ANSI:
                    switch (color)
                    {
                        case ConsoleColor.Black:
                            return @"\033[30m";
                        case ConsoleColor.DarkRed:
                            return @"\033[31m";
                        case ConsoleColor.DarkGreen:
                            return @"\033[32m";
                        case ConsoleColor.DarkYellow:
                            return @"\033[33m";
                        case ConsoleColor.DarkBlue:
                            return @"\033[34m";
                        case ConsoleColor.DarkMagenta:
                            return @"\033[35m";
                        case ConsoleColor.DarkCyan:
                            return @"\033[36m";
                        case ConsoleColor.Gray:
                            return @"\033[37m";
                        case ConsoleColor.DarkGray:
                            return @"\033[90m";
                        case ConsoleColor.Red:
                            return @"\033[91m";
                        case ConsoleColor.Green:
                            return @"\033[92m";
                        case ConsoleColor.Yellow:
                            return @"\033[93m";
                        case ConsoleColor.Blue:
                            return @"\033[94m";
                        case ConsoleColor.Magenta:
                            return @"\033[95m";
                        case ConsoleColor.Cyan:
                            return @"\033[96m";
                        case ConsoleColor.White:
                            return @"\033[97m";
                    }
                    return "";
                case ELinuxConsoleColorScheme.MONO:
                    return "";
                default:
                    return "";
            }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Writes a line in the console with custom color
        /// </summary>
        /// <param name="text">The text/object to write to the console</param>
        /// <param name="color">The color of the text</param>
        public static void WriteLine(object text, ConsoleColor color = ConsoleColor.White)
        {
            _SavedColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            if (Console.CursorLeft != 0)
                ClearLine();
            Console.WriteLine(text);
            if (IsLinux)
                LinuxWriteLine(text.ToString(), color);
            OnWriteLine(text, color);
            Console.ForegroundColor = _SavedColor;
        }

        /// <summary>
        /// Clears the current line in the console
        /// </summary>
        public static void ClearLine()
        {
            Console.CursorLeft = 0;
            Console.Write(new string(' ', Console.BufferWidth));
            Console.CursorTop--;
            Console.CursorLeft = 0;
        }
        #endregion

        #region Private Functions
        private static void LinuxWriteLine(string message, ConsoleColor color) =>
            File.AppendAllText(LinuxSTDOUTPath, ConsoleColorToEscapeCode(color) + message + Environment.NewLine);
        #endregion

        #region Event Functions
        static void OnLog(string text, string stack, LogType type)
        {
            ConsoleColor color = ConsoleColor.White;
            switch(type)
            {
                case LogType.Assert:
                    color = ConsoleColor.Cyan;
                    break;
                case LogType.Error:
                    color = ConsoleColor.Red;
                    break;
                case LogType.Exception:
                    color = ConsoleColor.DarkRed;
                    break;
                case LogType.Warning:
                    color = ConsoleColor.Yellow;
                    break;
            }
            LinuxWriteLine(text, color);
            if (!string.IsNullOrEmpty(stack))
                LinuxWriteLine(stack, color);
        }

        static void OnLinuxInput(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
                return;
            string newline = _InputReader.ReadToEnd();

            if (string.IsNullOrEmpty(newline))
                return;

            LinuxWriteLine("> " + newline, ConsoleColor.Magenta);
            OnInput(newline);
        }
        #endregion
    }
}
