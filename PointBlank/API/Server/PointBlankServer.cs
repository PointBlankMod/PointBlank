namespace PointBlank.API.Server
{
    /// <summary>
    /// All information about the server
    /// </summary>
    public static class PointBlankServer
    {
        #region Information
        /// <summary>
        /// The location of the server
        /// </summary>
        public static string ServerLocation { get; set; }
        /// <summary>
        /// The path of the mod loader saving location for everything
        /// </summary>
        public static string ModLoaderPath => ServerLocation + "/PointBlank";

        /// <summary>
        /// The path to the libraries directory
        /// </summary>
        public static string LibrariesPath => ModLoaderPath + "/Libraries";
        /// <summary>
        /// The path to the plugins directory
        /// </summary>
        public static string PluginsPath => ModLoaderPath + "/Plugins";
        /// <summary>
        /// The path to the configurations directory
        /// </summary>
        public static string ConfigurationsPath => ModLoaderPath + "/Configurations";
        /// <summary>
        /// The path to the translations directory
        /// </summary>
        public static string TranslationsPath => ModLoaderPath + "/Translations";
        /// <summary>
        /// The path to the data directory
        /// </summary>
        public static string DataPath => ModLoaderPath + "/Data";
        #endregion

        /// <summary>
        /// Is the server running
        /// </summary>
        public static bool IsRunning { get; set; } = false;
    }
}
