using System;
using UnityEngine;

namespace PointBlank.API
{
    /// <summary>
    /// The server console functions for easier use of the console
    /// </summary>
    public static class PointBlankConsole
    {
        #region Variables
        private static ConsoleColor _savedColor = ConsoleColor.White;
        #endregion

        /// <summary>
        /// Writes a line in the console with custom color
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="color">The color to use</param>
        public static void WriteLine(object text, ConsoleColor color = ConsoleColor.White)
        {
            _savedColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            if (Console.CursorLeft != 0)
                ClearLine();
            Console.WriteLine(text);
			PointBlankConsoleEvents.RunConsoleLineWritten(text, color);
            Console.ForegroundColor = _savedColor;
        }

        /// <summary>
        /// Clears the console line
        /// </summary>
        public static void ClearLine()
        {
            Console.CursorLeft = 0;
            Console.Write(new string(' ', Console.BufferWidth));
            Console.CursorTop--;
            Console.CursorLeft = 0;
        }

        /// <summary>
        /// Converts the console color to unity3d color and returns it
        /// </summary>
        /// <param name="color">The console color to use for the conversion</param>
        /// <returns>The unity3d color</returns>
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
    }
}
