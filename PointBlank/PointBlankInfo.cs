using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace PointBlank
{
    /// <summary>
    /// All information about PointBlank can be found here
    /// </summary>
    public static class PointBlankInfo
    {
        #region Private Info
        private static Assembly pbAssembly = Assembly.GetExecutingAssembly();
        private static FileVersionInfo pbFileVersionInfo = FileVersionInfo.GetVersionInfo(pbAssembly.Location);
        #endregion

        #region Public Info
        /// <summary>
        /// The name of the program
        /// </summary>
        public static readonly string Name = pbFileVersionInfo.ProductName;
        /// <summary>
        /// The creator of the program
        /// </summary>
        public static readonly string Creator = pbFileVersionInfo.CompanyName;
        /// <summary>
        /// The version of the program
        /// </summary>
        public static readonly string Version = pbFileVersionInfo.ProductVersion;
        /// <summary>
        /// The description of the program
        /// </summary>
        public static readonly string Description = pbFileVersionInfo.FileDescription;
        /// <summary>
        /// Is the program in debug release
        /// </summary>
        public static readonly bool IsDebug = pbFileVersionInfo.IsDebug;
        /// <summary>
        /// The directory of pointblank
        /// </summary>
        public static readonly string Location = Path.GetDirectoryName(Uri.UnescapeDataString((new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path)));
        #endregion

        #region Extra Info
        /// <summary>
        /// The current log file path for PointBlank
        /// </summary>
        public static string Current_Log = Directory.GetCurrentDirectory() + "/PointBlank.log";

        /// <summary>
        /// The previous log file path for PointBlank
        /// </summary>
        public static string Previous_Log = Directory.GetCurrentDirectory() + "/PointBlank.old.log";
        #endregion
    }
}
