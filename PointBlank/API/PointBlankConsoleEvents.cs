using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API
{
	public static class PointBlankConsoleEvents
	{
<<<<<<< HEAD
		#region Handlers
		/// <summary>
		/// Handles when a line is written to the console
		/// </summary>
		/// <param name="text">The text sent to the console</param>
		/// <param name="color">The color used by the console</param>
		public delegate void ConsoleLineWriteHandler(object text, ConsoleColor color);
		#endregion
=======
		#region Handlers
		/// <summary>
		/// Handles when a line is written to the console
		/// </summary>
		/// <param name="text">The text sent to the console</param>
		/// <param name="color">The color used by the console</param>
		public delegate void ConsoleLineWriteHandler(object text, ConsoleColor color);
		#endregion
>>>>>>> master

		#region Events

		/// <summary>
		/// Called a line is written to the console
		/// </summary>
		public static event ConsoleLineWriteHandler OnConsoleLineWritten;
<<<<<<< HEAD
		#endregion
=======
		#endregion
>>>>>>> master

		#region Functions

		internal static void RunConsoleLineWritten(object text, ConsoleColor color) => OnConsoleLineWritten?.Invoke(text, color);

		#endregion
	}
}
