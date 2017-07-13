using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using SDG.Unturned;
using PointBlank.Services.APIManager;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Structure;
using PointBlank.API.Unturned.Vehicle;
using PointBlank.API.Unturned.Barricade;
using PointBlank.API.Unturned.Item;
using Steamworks;
using UPlayer = SDG.Unturned.Player;

namespace PointBlank.API.Unturned.Server
{
    /// <summary>
    /// Functions for managing the unturned server
    /// </summary>
    public static class UnturnedServer
    {
        #region Variables
        private static HashSet<UnturnedPlayer> _Players = new HashSet<UnturnedPlayer>();
        private static HashSet<UnturnedVehicle> _Vehicles = new HashSet<UnturnedVehicle>();
        private static HashSet<UnturnedStructure> _Structures = new HashSet<UnturnedStructure>();
        private static HashSet<UnturnedBarricade> _Barricades = new HashSet<UnturnedBarricade>();
        private static HashSet<UnturnedItem> _Items = new HashSet<UnturnedItem>();
        #endregion

        #region Properties
        /// <summary>
        /// The currently online players
        /// </summary>
        public static UnturnedPlayer[] Players => _Players.ToArray(); 
        /// <summary>
        /// All vehicles on the server
        /// </summary>
        public static UnturnedVehicle[] Vehicles => _Vehicles.ToArray();
        /// <summary>
        /// Structures within the server
        /// </summary>
        public static UnturnedStructure[] Structures => _Structures.ToArray();
        /// <summary>
        /// Barricades within the server
        /// </summary>
        public static UnturnedBarricade[] Barricades => _Barricades.ToArray();
        /// <summary>
        /// Items within the server
        /// </summary>
        public static UnturnedItem[] Items => _Items.ToArray();
        /// <summary>
        /// Current game time
        /// </summary>
        public static uint GameTime
        {
            get => LightingManager.time;
            set => LightingManager.time = value;
        }
        /// <summary>
        /// Is it day time on the server
        /// </summary>
        public static bool IsDay => LightingManager.isDaytime;
        /// <summary>
        /// Is it currently full moon on the server
        /// </summary>
        public static bool IsFullMoon
        {
            get => LightingManager.isFullMoon;
            set => LightingManager.isFullMoon = value;
        }
        /// <summary>
        /// Is it currently raining/snowing
        /// </summary>
        public static bool IsRaining => LightingManager.hasRain;
        #endregion

        #region Functions
        internal static UnturnedPlayer AddPlayer(UnturnedPlayer player)
        {
            UnturnedPlayer ply = Players.FirstOrDefault(a => a.SteamPlayer == player.SteamPlayer);

            if (ply != null)
                return ply;

            _Players.Add(player);
            return player;
        }
        internal static bool RemovePlayer(UnturnedPlayer player)
        {
            UnturnedPlayer ply = Players.FirstOrDefault(a => a.SteamPlayer == player.SteamPlayer);

            if (ply == null)
                return false;

            _Players.Remove(ply);
            return true;
        }

        internal static UnturnedStructure AddStructure(UnturnedStructure structure)
        {
            UnturnedStructure stru = Structures.FirstOrDefault(a => a.Data == structure.Data);

            if (stru != null)
                return stru;

            _Structures.Add(structure);
            return structure;
        }
        internal static bool RemoveStructure(UnturnedStructure structure)
        {
            UnturnedStructure stru = Structures.FirstOrDefault(a => a.Data == structure.Data);

            if (stru == null)
                return false;

            _Structures.Remove(stru);
            return true;
        }

        internal static UnturnedBarricade AddBarricade(UnturnedBarricade Barricade)
        {
            UnturnedBarricade barricade = Barricades.FirstOrDefault(a => a.Data == Barricade.Data);

            if (barricade != null)
                return barricade;

            _Barricades.Add(Barricade);
            return Barricade;
        }
        internal static bool RemoveBarricade(UnturnedBarricade Barricade)
        {
            UnturnedBarricade barricade = Barricades.FirstOrDefault(a => a.Data == Barricade.Data);

            if (barricade == null)
                return false;

            _Barricades.Remove(barricade);
            return true;
        }

        internal static UnturnedVehicle AddVehicle(UnturnedVehicle vehicle)
        {
            UnturnedVehicle veh = Vehicles.FirstOrDefault(a => a.Vehicle == vehicle.Vehicle);

            if (veh != null)
                return veh;

            _Vehicles.Add(vehicle);
            return vehicle;
        }
        internal static bool RemoveVehicle(UnturnedVehicle vehicle)
        {
            UnturnedVehicle veh = Vehicles.FirstOrDefault(a => a.Vehicle == vehicle.Vehicle);

            if (veh == null)
                return false;

            _Vehicles.Remove(vehicle);
            return true;
        }

        internal static UnturnedItem AddItem(UnturnedItem item)
        {
            UnturnedItem itm = Items.FirstOrDefault(a => a.Item == item.Item);

            if (itm != null)
                return itm;

            _Items.Add(item);
            return item;
        }
        internal static bool RemoveItem(UnturnedItem item)
        {
            UnturnedItem itm = Items.FirstOrDefault(a => a.Item == item.Item);

            if (itm == null)
                return false;

            _Items.Remove(itm);
            return true;
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Checks if the UnturnedPlayer is the server player
        /// </summary>
        /// <param name="player">The unturned player instance to check</param>
        /// <returns>If the UnturnedPlayer instance is the server</returns>
        public static bool IsServer(UnturnedPlayer player) => (player == null);
        /// <summary>
        /// Checks if the player is still in the server and returns the result
        /// </summary>
        /// <param name="player">The player to look for</param>
        /// <returns>If the player is still in the server or not</returns>
        public static bool IsInServer(UnturnedPlayer player) => Players.Contains(player);

        /// <summary>
        /// Gets the unturned player instance based on steam player instance
        /// </summary>
        /// <param name="player">The steam player instance</param>
        /// <returns>The unturned player instance</returns>
        public static UnturnedPlayer GetPlayer(SteamPlayer player) => Players.FirstOrDefault(a => a.SteamPlayer == player);
        /// <summary>
        /// Gets the unturned player instance based on player instance
        /// </summary>
        /// <param name="player">The player instance</param>
        /// <returns>The unturned player instace</returns>
        public static UnturnedPlayer GetPlayer(UPlayer player) => Players.FirstOrDefault(a => a.Player == player);
        /// <summary>
        /// Gets the unturned player instance based on arena player instance
        /// </summary>
        /// <param name="player">The unturned player instance</param>
        /// <returns>The unturned player instance</returns>
        public static UnturnedPlayer GetPlayer(ArenaPlayer player) => Players.FirstOrDefault(a => a.SteamPlayer == player.steamPlayer);
        /// <summary>
        /// Gets the unturned player instance based on steam player id instance
        /// </summary>
        /// <param name="playerID">The steam player id instance</param>
        /// <returns>The unturned player instance</returns>
        public static UnturnedPlayer GetPlayer(SteamPlayerID playerID) => Players.FirstOrDefault(a => a.SteamPlayerID == playerID);
        /// <summary>
        /// Gets the unturned player instance based on steam id instance
        /// </summary>
        /// <param name="steamID">The steam id instance</param>
        /// <returns>The unturned player instance</returns>
        public static UnturnedPlayer GetPlayer(CSteamID steamID) => Players.FirstOrDefault(a => a.SteamID == steamID);
        /// <summary>
        /// Gets the unturned player instance based on steam64 ID
        /// </summary>
        /// <param name="steam64">The steam64 ID</param>
        /// <returns>The unturned player instance</returns>
        public static UnturnedPlayer GetPlayer(ulong steam64) => Players.FirstOrDefault(a => a.SteamID.m_SteamID == steam64);

        /// <summary>
        /// Changes the map
        /// </summary>
        /// <param name="mapName">The name of the map to change to</param>
        /// <returns>Whether the map was successfully changed or not</returns>
        public static bool ChangeMap(string mapName)
        {
            if (!Level.exists(mapName))
                return false;

            for (int i = 0; i < Players.Length; i++)
                Players[i].Kick("Map is changing.");

            Provider.map = mapName;
            Level.load(Level.getLevel(Provider.map));
            typeof(Provider).GetMethod("loadGameMode", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[0]);
            SteamGameServer.SetMapName(Provider.map);
            SteamGameServer.SetGameTags(string.Concat(new object[]
            {
                (!Provider.isPvP) ? "PVE" : "PVP",
                ",GAMEMODE:",
                Provider.gameMode.GetType().Name,
                ',',
                (!Provider.hasCheats) ? "STAEHC" : "CHEATS",
                ',',
                Provider.mode.ToString(),
                ",",
                Provider.cameraMode.ToString(),
                ",",
                (Provider.serverWorkshopFileIDs.Count <= 0) ? "KROW" : "WORK",
                ",",
                (!Provider.isGold) ? "YLNODLOG" : "GOLDONLY",
                ",",
                (!Provider.configData.Server.BattlEye_Secure) ? "BATTLEYE_OFF" : "BATTLEYE_ON"
            }));
            if (Provider.serverWorkshopFileIDs.Count > 0)
            {
                string text = string.Empty;
                for (int l = 0; l < Provider.serverWorkshopFileIDs.Count; l++)
                {
                    if (text.Length > 0)
                    {
                        text += ',';
                    }
                    text += Provider.serverWorkshopFileIDs[l];
                }
                int num4 = (text.Length - 1) / 120 + 1;
                int num5 = 0;
                SteamGameServer.SetKeyValue("Browser_Workshop_Count", num4.ToString());
                for (int m = 0; m < text.Length; m += 120)
                {
                    int num6 = 120;
                    if (m + num6 > text.Length)
                    {
                        num6 = text.Length - m;
                    }
                    string pValue2 = text.Substring(m, num6);
                    SteamGameServer.SetKeyValue("Browser_Workshop_Line_" + num5, pValue2);
                    num5++;
                }
            }
            string text2 = string.Empty;
            Type type = Provider.modeConfigData.GetType();
            FieldInfo[] fields = type.GetFields();
            for (int n = 0; n < fields.Length; n++)
            {
                FieldInfo fieldInfo = fields[n];
                object value = fieldInfo.GetValue(Provider.modeConfigData);
                Type type2 = value.GetType();
                FieldInfo[] fields2 = type2.GetFields();
                for (int num7 = 0; num7 < fields2.Length; num7++)
                {
                    FieldInfo fieldInfo2 = fields2[num7];
                    object value2 = fieldInfo2.GetValue(value);
                    if (text2.Length > 0)
                    {
                        text2 += ',';
                    }
                    if (value2 is bool)
                    {
                        text2 += ((!(bool)value2) ? "F" : "T");
                    }
                    else
                    {
                        text2 += value2;
                    }
                }
            }
            int num8 = (text2.Length - 1) / 120 + 1;
            int num9 = 0;

            SteamGameServer.SetKeyValue("Browser_Config_Count", num8.ToString());
            for (int num10 = 0; num10 < text2.Length; num10 += 120)
            {
                int num11 = 120;

                if (num10 + num11 > text2.Length)
                    num11 = text2.Length - num10;
                string pValue3 = text2.Substring(num10, num11);

                SteamGameServer.SetKeyValue("Browser_Config_Line_" + num9, pValue3);
                num9++;
            }

            return true;
        }

        /// <summary>
        /// Reloads the player configs
        /// </summary>
        public static void ReloadPlayers()
        {
            InfoManager info = (InfoManager)Enviroment.services["InfoManager.InfoManager"].ServiceClass;

            foreach(UnturnedPlayer player in Players)
                info.OnPlayerJoin(player);
        }
        #endregion
    }
}