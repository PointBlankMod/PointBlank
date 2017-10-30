using System;
using System.IO;
using System.Linq;
using UnityEngine;
using PointBlank.API.Server;

namespace PointBlank.API.Console
{
    internal static class LinuxConsoleOutput
    {
        #region Properties
        public static EConsoleColorScheme ColorScheme { get; private set; }
        #endregion

        public static void Init()
        {
            string colorscheme = Environment.GetCommandLineArgs().FirstOrDefault(a => a.Contains("-colorscheme="));

            if(colorscheme != null)
            {
                switch ((colorscheme.Split('=')[1]).ToUpperInvariant())
                {
                    case "XTERM":
                        ColorScheme = EConsoleColorScheme.XTERM;
                        break;
                    case "MONO":
                        ColorScheme = EConsoleColorScheme.MONO;
                        break;
                    default:
                        ColorScheme = EConsoleColorScheme.ANSI;
                        break;
                }
            }
            else
            {
                ColorScheme = EConsoleColorScheme.ANSI;
            }

            Application.logMessageReceivedThreaded += OnLog;
        }

        public static void PostInit()
        {
            PointBlankConsoleEvents.OnConsoleLineWritten += OnLineWritten;
            WriteLine("Linux PointBlank Console wrapper has been initialized!", ConsoleColor.Blue);
        }

        #region Functions
        public static void WriteLine(string message, ConsoleColor color) =>
            File.AppendAllText(PointBlankServer.ServerLocation + "/Console.STDOUT", LinuxConsoleUtils.ConsoleColorToEscapeCode(color) + message + Environment.NewLine);
        #endregion

        #region Event Functions
        static void OnLog(string text, string stack, LogType type)
        {
            ConsoleColor color = ConsoleColor.White;
            switch (type)
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
            WriteLine(text, color);
            if (!string.IsNullOrEmpty(stack))
                WriteLine(stack, color);
        }

        static void OnLineWritten(object o, ConsoleColor color) =>
            WriteLine(o.ToString(), color);
        #endregion
    }
}
