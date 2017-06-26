using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Structure;
using PointBlank.API.Unturned.Vehicle;
using PointBlank.API.Unturned.Barricade;
using PointBlank.API.Unturned.Item;
using Steamworks;
using UStructure = SDG.Unturned.Structure;
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
        private static HashSet<BotPlayer> _Bots = new HashSet<BotPlayer>();
        private static HashSet<UnturnedVehicle> _Vehicles = new HashSet<UnturnedVehicle>();
        private static HashSet<UnturnedStructure> _Structures = new HashSet<UnturnedStructure>();
        private static HashSet<UnturnedBarricade> _Barricades = new HashSet<UnturnedBarricade>();
        private static HashSet<UnturnedItem> _Items = new HashSet<UnturnedItem>();
        #endregion

        #region Properties
        /// <summary>
        /// The currently online players
        /// </summary>
        public static UnturnedPlayer[] Players => _Players.ToArray(); // Does this assign at runtime? bad if so
        /// <summary>
        /// The current existing bots
        /// </summary>
        public static BotPlayer[] Bots => _Bots.ToArray();
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
        public static UnturnedBarricade[] Barricades = _Barricades.ToArray();
        /// <summary>
        /// Items within the server
        /// </summary>
        public static UnturnedItem[] Items => _Items.ToArray();
        /// <summary>
        /// Current game time
        /// </summary>
        public static uint GameTime { get { return LightingManager.time; } set { LightingManager.time = value; } }
        /// <summary>
        /// Is it day time on the server
        /// </summary>
        public static bool IsDay => LightingManager.isDaytime;
        /// <summary>
        /// Is it currently full moon on the server
        /// </summary>
        public static bool IsFullMoon { get { return LightingManager.isFullMoon; } set { LightingManager.isFullMoon = value; } }
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
        public static UnturnedPlayer GetPlayer(ArenaPlayer player)
        {
            try
            {
                return Players.FirstOrDefault(a => a.SteamPlayer == player.steamPlayer);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
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
        #endregion
    }
}
