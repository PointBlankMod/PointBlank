using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Structure;

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

        #region Methods
        /// <summary>
        /// Updates Structures within the server
        /// </summary>
        public static void UpdateStructures()
        {
            List<UnturnedStructure> Structures = new List<UnturnedStructure>();
            for (int i = 0; i < Regions.WORLD_SIZE; i++)
            {
                for (int o = 0; o < Regions.WORLD_SIZE; o++)
                {
                    StructureRegion region = StructureManager.regions[i, o];
                    StructureData[] data = region.structures.ToArray();
                    for (int z = 0; z < data.Length; z++)
                        Structures.Add(new UnturnedStructure(data[z]));
                }
            }
            _Structures = Structures;
        }
        /// <summary>
        /// Updates Items within the server
        /// </summary>
        public static void UpdateItems()
        {
            List<Item> Items = new List<Item>();
            for (int i = 0; i < Regions.WORLD_SIZE; i++)
            {
                for (int o = 0; o < Regions.WORLD_SIZE; o++)
                {
                    ItemRegion region = ItemManager.regions[i, o];
                    ItemData[] data = region.items.ToArray();
                    for (int z = 0; z < data.Length; z++)
                        Items.Add(data[z].item);
                }
            }
            _Items = Items;
        }
        #endregion
    }
}
