using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using PointBlank.Framework.Permissions.Ring;
using SDG.Unturned;

namespace PointBlank.API
{
    /// <summary>
    /// A storage of server information
    /// </summary>
    [RingPermission(SecurityAction.Demand, ring = RingPermissionRing.None)]
    public static class ServerInfo
    {
        /// <summary>
        /// Returns the server name
        /// </summary>
        public static string ServerName { get { return Provider.serverName; } }

        /// <summary>
        /// Returns the raw server password
        /// </summary>
        public static string ServerPassword { get { return Provider.serverPassword; } }

        /// <summary>
        /// Returns the hashed server password
        /// </summary>
        public static string ServerPasswordHash { get { return string.Join("", (string[])Provider.serverPasswordHash.Select(a => a.ToString())); } }

        /// <summary>
        /// Returns the hashed server password as bytes
        /// </summary>
        public static byte[] ServerPasswordHashBytes { get { return Provider.serverPasswordHash; } }

        /// <summary>
        /// Returns the server ID
        /// </summary>
        public static string ServerID { get { return Provider.serverID; } }

        /// <summary>
        /// Returns the server save location
        /// </summary>
        public static string ServerPath { get { return Directory.GetCurrentDirectory() + ServerSavedata.directory + "/" + Provider.serverID; } }

        /// <summary>
        /// Returns if the server is battleye secure or not
        /// </summary>
        public static bool IsBattleyeSecure { get { return Provider.configData.Server.BattlEye_Secure; } }

        /// <summary>
        /// Returns if the server is VAC secure or not
        /// </summary>
        public static bool IsVACSecure { get { return Provider.configData.Server.VAC_Secure; } }

        /// <summary>
        /// Returns the max players allowed on the server
        /// </summary>
        public static byte MaxPlayers { get { return Provider.maxPlayers; } }

        /// <summary>
        /// Returns the server game mode
        /// </summary>
        public static EGameMode GameMode { get { return Provider.mode; } }

        /// <summary>
        /// Returns the current map
        /// </summary>
        public static string Map { get { return Provider.map; } }

        /// <summary>
        /// Returns if the server is whitelisted
        /// </summary>
        public static bool IsWhitelisted { get { return Provider.isWhitelisted; } }

        /// <summary>
        /// Returns if the server is PvP
        /// </summary>
        public static bool IsPVP { get { return Provider.isPvP; } }

        /// <summary>
        /// Returns if the server has cheats
        /// </summary>
        public static bool HasCheats { get { return Provider.hasCheats; } }

        /// <summary>
        /// Returns if the server is gold
        /// </summary>
        public static bool IsGold { get { return Provider.isGold; } }

        /// <summary>
        /// Returns the queue size
        /// </summary>
        public static byte QueueSize { get { return Provider.queueSize; } }

        /// <summary>
        /// Returns the server level type
        /// </summary>
        public static ELevelType LevelType { get { return LevelManager.levelType; } }

        /// <summary>
        /// The path to the libraries directory
        /// </summary>
        public static string LibrariesPath { get { return ServerPath + "/Libraries"; } }

        /// <summary>
        /// The path to the plugins directory
        /// </summary>
        public static string PluginsPath { get { return ServerPath + "/Plugins"; } }

        /// <summary>
        /// The path to the configurations directory
        /// </summary>
        public static string ConfigurationsPath { get { return ServerPath + "/Configurations"; } }

        /// <summary>
        /// The path to the translations directory
        /// </summary>
        public static string TranslationsPath { get { return ServerPath + "/Translations"; } }

        /// <summary>
        /// The path to the data directory
        /// </summary>
        public static string DataPath { get { return ServerPath + "/Data"; } }
    }
}
