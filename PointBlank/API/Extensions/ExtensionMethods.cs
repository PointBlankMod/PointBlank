using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;
using Steamworks;


namespace PointBlank.API.Extensions
{
    public static class ExtensionMethods
    {
        #region Player
        /// <summary>
        /// Extension Method To Get UnturnedPlayer 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>        
        public static UnturnedPlayer ToUnturnedPlayer(this SteamPlayer player) => UnturnedServer.GetPlayer(player);

        /// <summary>
        /// Extension Method To Get UnturnedPlayer 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>        
        public static UnturnedPlayer ToUnturnedPlayer(this SDG.Unturned.Player player) => UnturnedServer.GetPlayer(player);



        /// <summary>
        /// Extension Method To Get UnturnedPlayer 
        /// </summary>
        /// <param name="steamID"></param>
        /// <returns></returns>        
        public static UnturnedPlayer ToUnturnedPlayer(this CSteamID steamID) => UnturnedServer.GetPlayer(steamID);

        #endregion
    }
}