using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Structure;
using UStructure = SDG.Unturned.Structure;

namespace PointBlank.API.Unturned.Server
{
    /// <summary>
    /// Functions for managing the unturned server
    /// </summary>
    public static class UnturnedServer
    {
        #region Variables
        private static List<UnturnedPlayer> _Players = new List<UnturnedPlayer>();
        private static List<UnturnedStructure> _Structures = new List<UnturnedStructure>();
        private static List<Item> _Items = new List<Item>();
        #endregion

        #region Properties
        /// <summary>
        /// The currently online players
        /// </summary>
        public static UnturnedPlayer[] Players => _Players.ToArray();
        /// <summary>
        /// Structures within the server
        /// </summary>
        public static UnturnedStructure[] Structures => _Structures.ToArray();
        /// <summary>
        /// Items within the server
        /// </summary>
        public static Item[] Items => _Items.ToArray();
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
        /// <summary>
        /// Add a player to the server
        /// </summary>
        /// <param name="player">The player instance</param>
        /// <returns>The player instance</returns>
        internal static UnturnedPlayer AddPlayer(UnturnedPlayer player)
        {
            UnturnedPlayer ply = Players.FirstOrDefault(a => a.SteamPlayer == player.SteamPlayer);

            if (ply != null)
                return ply;

            _Players.Add(player);
            return player;
        }

        /// <summary>
        /// Removes a player from the server
        /// </summary>
        /// <param name="player">The player instance</param>
        /// <returns>If the player was removed successfully</returns>
        internal static bool RemovePlayer(UnturnedPlayer player)
        {
            UnturnedPlayer ply = Players.FirstOrDefault(a => a.SteamPlayer == player.SteamPlayer);

            if (ply == null)
                return false;

            _Players.Remove(ply);
            return true;
        }

        /// <summary>
        /// Adds a structure to the server
        /// </summary>
        /// <param name="structure">The structure instance to add</param>
        /// <returns>The added structure instance</returns>
        internal static UnturnedStructure AddStructure(UnturnedStructure structure)
        {
            UnturnedStructure stru = Structures.FirstOrDefault(a => a.Data == structure.Data);

            if (stru != null)
                return stru;

            _Structures.Add(structure);
            return structure;
        }

        /// <summary>
        /// Removes a structure from the server
        /// </summary>
        /// <param name="structure">The structure instance to remove</param>
        /// <returns>If the structure was removed successfully</returns>
        internal static bool RemoveStructure(UnturnedStructure structure)
        {
            UnturnedStructure stru = Structures.FirstOrDefault(a => a.Data == structure.Data);

            if (stru == null)
                return false;

            _Structures.Remove(stru);
            return true;
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Finds a structure based on the unturned structure instance
        /// </summary>
        /// <param name="structure">The unturned structure instance</param>
        /// <returns>The instance of the custom structure class</returns>
        public static UnturnedStructure FindStructure(UStructure structure)
        {
            return Structures.FirstOrDefault(a => a.Structure == structure);
        }

        /// <summary>
        /// Finds a structure based on the structure data
        /// </summary>
        /// <param name="data">The structure data</param>
        /// <returns>The unturned structure instance</returns>
        public static UnturnedStructure FindStructure(StructureData data)
        {
            return Structures.FirstOrDefault(a => a.Data == data);
        }
        #endregion
    }
}
