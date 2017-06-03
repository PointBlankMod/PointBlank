using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using PointBlank.API.Unturned.Player;

namespace PointBlank.API.Unturned.Server
{
    /// <summary>
    /// All the server based events
    /// </summary>
    public static class ServerEvents
    {
        #region Handlers
        /// <summary>
        /// Used for handling player connections
        /// </summary>
        /// <param name="player">The player that connected/disconnected</param>
        public delegate void PlayerConnectionHandler(UnturnedPlayer player);
        /// <summary>
        /// Used for handling bots
        /// </summary>
        /// <param name="bot">The bot instance</param>
        public delegate void BotStatusHandler(BotPlayer bot);
        #endregion

        #region Events
        /// <summary>
        /// Called every game tick
        /// </summary>
        public static event OnVoidDelegate OnGameTick;
        /// <summary>
        /// Called every thread tick
        /// </summary>
        public static event OnVoidDelegate OnThreadTick;

        /// <summary>
        /// Called when a player connected to the server
        /// </summary>
        public static event PlayerConnectionHandler OnPlayerConnected;
        /// <summary>
        /// Called when a player disconnected from the server
        /// </summary>
        public static event PlayerConnectionHandler OnPlayerDisconnected;

        /// <summary>
        /// Called when a bot is created
        /// </summary>
        public static event BotStatusHandler OnBotCreated;
        /// <summary>
        /// Called when a bot is removed
        /// </summary>
        public static event BotStatusHandler OnBotRemoved;

        /// <summary>
        /// Called when the time is officially day
        /// </summary>
        public static event OnVoidDelegate OnTimeDay;
        /// <summary>
        /// Called when the time is officially night
        /// </summary>
        public static event OnVoidDelegate OnTimeNight;

        /// <summary>
        /// Called when the full moon is started
        /// </summary>
        public static event OnVoidDelegate OnFullMoonStarted;
        /// <summary>
        /// Called when the full moon is ended
        /// </summary>
        public static event OnVoidDelegate OnFullMoonEnded;

        /// <summary>
        /// Called when the rain/snow is updated
        /// </summary>
        public static event RainUpdated OnRainUpdated;
        #endregion

        #region Functions
        internal static void RunGameTick()
        {
            if (OnGameTick == null)
                return;

            OnGameTick();
        }
        internal static void RunThreadTick()
        {
            if (OnThreadTick == null)
                return;

            OnThreadTick();
        }

        internal static void RunPlayerConnected(SteamPlayer player)
        {
            if (OnPlayerConnected == null)
                return;

            OnPlayerConnected(UnturnedPlayer.Create(player));
        }
        internal static void RunPlayerDisconnected(SteamPlayer player)
        {
            if (OnPlayerDisconnected == null)
                return;

            OnPlayerDisconnected(UnturnedPlayer.Create(player));
        }

        internal static void RunBotCreated(BotPlayer bot)
        {
            if (OnBotCreated == null)
                return;

            OnBotCreated(bot);
        }
        internal static void RunBotRemoved(BotPlayer bot)
        {
            if (OnBotRemoved == null)
                return;

            OnBotRemoved(bot);
        }

        internal static void RunDayNight(bool isDay)
        {
            if (isDay && OnTimeDay == null)
                return;
            if (!isDay && OnTimeNight == null)
                return;

            if (isDay)
                OnTimeDay();
            else
                OnTimeNight();
        }

        internal static void RunFullMoon(bool isFullMoon)
        {
            if (isFullMoon && OnFullMoonStarted == null)
                return;
            if (!isFullMoon && OnFullMoonEnded == null)
                return;

            if (isFullMoon)
                OnFullMoonStarted();
            else
                OnFullMoonEnded();
        }

        internal static void RunRainUpdated(ELightingRain status)
        {
            if (OnRainUpdated == null)
                return;

            OnRainUpdated(status);
        }
        #endregion
    }
}
