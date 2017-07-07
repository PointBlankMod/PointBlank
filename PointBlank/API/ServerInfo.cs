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
        public static string ServerName => Provider.serverName; 

        /// <summary>
        /// Returns the raw server password
        /// </summary>
        public static string ServerPassword => Provider.serverPassword; 

        /// <summary>
        /// Returns the hashed server password
        /// </summary>
        public static string ServerPasswordHash => string.Join("", (string[])Provider.serverPasswordHash.Select(a => a.ToString())); 

        /// <summary>
        /// Returns the hashed server password as bytes
        /// </summary>
        public static byte[] ServerPasswordHashBytes => Provider.serverPasswordHash; 

        /// <summary>
        /// Returns the server ID
        /// </summary>
        public static string ServerID => Provider.serverID; 

        /// <summary>
        /// Returns the server save location
        /// </summary>
        public static string ServerPath => Directory.GetCurrentDirectory() + ServerSavedata.directory + "/" + Provider.serverID; 

        /// <summary>
        /// Returns if the server is battleye secure or not
        /// </summary>
        public static bool IsBattleyeSecure => Provider.configData.Server.BattlEye_Secure; 

        /// <summary>
        /// Returns if the server is VAC secure or not
        /// </summary>
        public static bool IsVACSecure => Provider.configData.Server.VAC_Secure; 

        /// <summary>
        /// Returns the max players allowed on the server
        /// </summary>
        public static byte MaxPlayers => Provider.maxPlayers; 

        /// <summary>
        /// Returns the server game mode
        /// </summary>
        public static EGameMode GameMode => Provider.mode; 

        /// <summary>
        /// Returns the current map
        /// </summary>
        public static string Map => Provider.map; 

        /// <summary>
        /// Returns if the server is whitelisted
        /// </summary>
        public static bool IsWhitelisted => Provider.isWhitelisted; 

        /// <summary>
        /// Returns if the server is PvP
        /// </summary>
        public static bool IsPVP => Provider.isPvP; 

        /// <summary>
        /// Returns if the server has cheats
        /// </summary>
        public static bool HasCheats => Provider.hasCheats; 

        /// <summary>
        /// Returns if the server is gold
        /// </summary>
        public static bool IsGold => Provider.isGold; 

        /// <summary>
        /// Returns the queue size
        /// </summary>
        public static byte QueueSize => Provider.queueSize; 

        /// <summary>
        /// Returns the server level type
        /// </summary>
        public static ELevelType LevelType => LevelManager.levelType; 

        /// <summary>
        /// The path to the libraries directory
        /// </summary>
        public static string LibrariesPath => ServerPath + "/PointBlank/Libraries"; 

        /// <summary>
        /// The path to the plugins directory
        /// </summary>
        public static string PluginsPath => ServerPath + "/PointBlank/Plugins"; 

        /// <summary>
        /// The path to the configurations directory
        /// </summary>
        public static string ConfigurationsPath => ServerPath + "/PointBlank/Configurations"; 

        /// <summary>
        /// The path to the translations directory
        /// </summary>
        public static string TranslationsPath => ServerPath + "/PointBlank/Translations"; 

        /// <summary>
        /// The path to the data directory
        /// </summary>
        public static string DataPath => ServerPath + "/PointBlank/Data"; 
    }
}
