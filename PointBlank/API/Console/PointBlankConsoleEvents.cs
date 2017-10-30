using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Console
{
	public static class PointBlankConsoleEvents
	{
		#region Handlers
		/// <summary>
		/// Handles when a line is written to the console
		/// </summary>
		/// <param name="text">The text sent to the console</param>
		/// <param name="color">The color used by the console</param>
		public delegate void ConsoleLineWriteHandler(object text, ConsoleColor color);

        /// <summary>
        /// Handles console input
        /// </summary>
        /// <param name="text">The input text</param>
        public delegate void ConsoleInputHandler(string text);
		#endregion

		#region Events
		/// <summary>
		/// Called a line is written to the console
		/// </summary>
		public static event ConsoleLineWriteHandler OnConsoleLineWritten;

        /// <summary>
        /// Called when a text is inputted into the console
        /// </summary>
        public static event ConsoleInputHandler OnConsoleInput;
		#endregion

		#region Functions
		internal static void RunConsoleLineWritten(object text, ConsoleColor color) => OnConsoleLineWritten?.Invoke(text, color);

        internal static void RunConsoleInput(string text) => OnConsoleInput?.Invoke(text);
		#endregion
	}
}
