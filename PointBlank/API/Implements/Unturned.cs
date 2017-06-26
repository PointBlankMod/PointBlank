using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using PointBlank.API.Unturned.Player;
using Steamworks;

namespace PointBlank.API.Implements
{
    /// <summary>
    /// The extension class for unturned
    /// </summary>
    public static class Unturned
    {
        /// <summary>
        /// Tries to convert the steam player to unturned player instance
        /// </summary>
        /// <param name="player">The steam player instance</param>
        /// <returns>The unturned player instance gotten from the steam player</returns>
        public static UnturnedPlayer ToUnturnedPlayer(this SteamPlayer player) => UnturnedPlayer.Get(player);
        /// <summary>
        /// Tries to convert the player to unturned player instance
        /// </summary>
        /// <param name="player">The player instance</param>
        /// <returns>The unturned player instance gotten from the player instance</returns>
        public static UnturnedPlayer ToUnturnedPlayer(this Player player) => UnturnedPlayer.Get(player);
        /// <summary>
        /// Tries to convert the arena player to unturned player instance
        /// </summary>
        /// <param name="player">The arena player instance</param>
        /// <returns>The unturned player instance gotten from the arena player instance</returns>
        public static UnturnedPlayer ToUnturnedPlayer(this ArenaPlayer player) => UnturnedPlayer.Get(player);
        /// <summary>
        /// Tries to convert the steam id to unturned player instance
        /// </summary>
        /// <param name="player">The steam id instance</param>
        /// <returns>The unturned player instance gotten from the steam id instance</returns>
        public static UnturnedPlayer ToUnturnedPlayer(this CSteamID player) => UnturnedPlayer.Get(player);
    }
}
