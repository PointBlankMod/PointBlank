using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Console
{
    internal static class LinuxConsoleUtils
    {
        public static bool IsLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }

        public static string ConsoleColorToEscapeCode(ConsoleColor color)
        {
            switch (LinuxConsoleOutput.ColorScheme)
            {
                case EConsoleColorScheme.XTERM:
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
                case EConsoleColorScheme.ANSI:
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
                case EConsoleColorScheme.MONO:
                    return "";
                default:
                    return "";
            }
        }
    }
}
