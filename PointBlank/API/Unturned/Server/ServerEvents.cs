using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Vehicle;
using PointBlank.API.Unturned.Structure;

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

        /// <summary>
        /// Used for handling vehicles
        /// </summary>
        /// <param name="vehicle">The vehicle instance</param>
        public delegate void VehicleStatusHandler(UnturnedVehicle vehicle);

        /// <summary>
        /// Used for handling structures
        /// </summary>
        /// <param name="structure">The structure instance</param>
        public delegate void StructureStatusHandler(UnturnedStructure structure);
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
        /// Called when the server is shutting down
        /// </summary>
        public static event OnVoidDelegate OnServerShutdown;

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

        /// <summary>
        /// Called when a vehicle is created
        /// </summary>
        public static event VehicleStatusHandler OnVehicleCreated;
        /// <summary>
        /// Called when a vehicle is removed
        /// </summary>
        public static event VehicleStatusHandler OnVehicleRemoved;

        /// <summary>
        /// Called when a structure is created
        /// </summary>
        public static event StructureStatusHandler OnStructureCreated;
        /// <summary>
        /// Called wehn a structure is removed
        /// </summary>
        public static event StructureStatusHandler OnStructureRemoved;
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

        internal static void RunServerShutdown()
        {
            if (OnServerShutdown == null)
                return;

            OnServerShutdown();
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

        internal static void RunVehicleCreated(InteractableVehicle vehicle)
        {
            if (OnVehicleCreated == null)
                return;

            OnVehicleCreated(UnturnedVehicle.Create(vehicle));
        }
        internal static void RunVehicleRemoved(InteractableVehicle vehicle)
        {
            if (OnVehicleRemoved == null)
                return;

            OnVehicleRemoved(UnturnedVehicle.Create(vehicle));
        }

        internal static void RunStructureCreated(StructureData structure)
        {
            if (OnStructureCreated == null)
                return;

            OnStructureCreated(UnturnedStructure.Create(structure));
        }
        internal static void RunStructureRemoved(UnturnedStructure structure)
        {
            if (OnStructureRemoved == null)
                return;

            OnStructureRemoved(structure);
        }
        #endregion
    }
}
