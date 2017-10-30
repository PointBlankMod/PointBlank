using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Server;

namespace PointBlank.API.Console
{
    internal static class LinuxConsoleInput
    {
        #region Variables
        private static TextReader _InputReader;
        #endregion

        public static void Init()
        {
            File.WriteAllText(PointBlankServer.ServerLocation + "/Console.STDIN", LinuxConsoleUtils.ConsoleColorToEscapeCode(ConsoleColor.Magenta));
            FileSystemWatcher fsWatcher = new FileSystemWatcher(PointBlankServer.ServerLocation, "Console.STDIN");
            fsWatcher.Changed += OnInput;
            _InputReader = new StreamReader(new FileStream(PointBlankServer.ServerLocation + "/Console.STDIN", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite));
            fsWatcher.EnableRaisingEvents = true;
        }

        #region Event Functions
        static void OnInput(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
                return;
            string newline = _InputReader.ReadToEnd();
            if (string.IsNullOrEmpty(newline))
                return;

            LinuxConsoleOutput.WriteLine("> " + newline, ConsoleColor.Magenta);
            PointBlankConsoleEvents.RunConsoleInput(newline);
        }
        #endregion
    }
}
