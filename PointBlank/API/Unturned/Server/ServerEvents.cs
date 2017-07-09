using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using Steamworks;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Vehicle;
using PointBlank.API.Unturned.Structure;
using PointBlank.API.Unturned.Barricade;

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
        /// Used for handling vehicles
        /// </summary>
        /// <param name="vehicle">The vehicle instance</param>
        public delegate void VehicleStatusHandler(UnturnedVehicle vehicle);

        /// <summary>
        /// Used for handling structures
        /// </summary>
        /// <param name="structure">The structure instance</param>
        /// <param name="cancel">Should the event be canceled</param>
        public delegate void StructureStatusHandler(UnturnedStructure structure, ref bool cancel);

        /// <summary>
        /// Used for handling barricades
        /// </summary>
        /// <param name="barricade">The affected barricade</param>
        /// <param name="cancel">Should the event be canceled</param>
        public delegate void BarricadeStatusHandler(UnturnedBarricade barricade, ref bool cancel);

        /// <summary>
        /// Used for handling unturned packet sending
        /// </summary>
        /// <param name="steamID">The steamID to send to</param>
        /// <param name="type">The packet type to send</param>
        /// <param name="packet">The packet bytes to send</param>
        /// <param name="size">The size of the packet</param>
        /// <param name="channel">The channel to send it on</param>
        /// <param name="cancel">Should the packet be canceled</param>
        public delegate void PacketSentHandler(ref CSteamID steamID, ref ESteamPacket type, ref byte[] packet, ref int size, ref int channel, ref bool cancel);
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
        /// Called when the server is initialized
        /// </summary>
        public static event OnVoidDelegate OnServerInitialized;

        /// <summary>
        /// Called when a player connected to the server
        /// </summary>
        public static event PlayerConnectionHandler OnPlayerConnected;
        /// <summary>
        /// Called when a player disconnected from the server
        /// </summary>
        public static event PlayerConnectionHandler OnPlayerDisconnected;

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
        /// Called when a structure is removed
        /// </summary>
        public static event StructureStatusHandler OnStructureRemoved;

        /// <summary>
        /// Called when a barricade is created
        /// </summary>
        public static event BarricadeStatusHandler OnBarricadeCreated;
        /// <summary>
        /// Called when a barricade is removed
        /// </summary>
        public static event BarricadeStatusHandler OnBarricadeRemoved;

        /// <summary>
        /// Called when a packet is sent
        /// </summary>
        public static event PacketSentHandler OnPacketSent;
        #endregion

        #region Functions
        internal static void RunGameTick() => OnGameTick?.Invoke();

        internal static void RunThreadTick() => OnThreadTick?.Invoke();

        internal static void RunServerShutdown() => OnServerShutdown?.Invoke();

        internal static void RunServerInitialized() => OnServerInitialized?.Invoke();

        internal static void RunPlayerConnected(SteamPlayer player) => OnPlayerConnected?.Invoke(UnturnedPlayer.Create(player));

        internal static void RunPlayerDisconnected(SteamPlayer player) => OnPlayerDisconnected?.Invoke(UnturnedPlayer.Create(player));

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

        internal static void RunRainUpdated(ELightingRain status) => OnRainUpdated?.Invoke(status);

        internal static void RunVehicleCreated(InteractableVehicle vehicle) => OnVehicleCreated?.Invoke(UnturnedVehicle.Create(vehicle));

        internal static void RunVehicleRemoved(InteractableVehicle vehicle) => OnVehicleRemoved?.Invoke(UnturnedVehicle.Create(vehicle));

        internal static void RunStructureCreated(StructureData structure, ref bool cancel) => OnStructureCreated?.Invoke(UnturnedStructure.Create(structure), ref cancel);

        internal static void RunStructureRemoved(UnturnedStructure structure, ref bool cancel)
        {
            if (OnStructureRemoved == null)
                return;

            OnStructureRemoved(structure, ref cancel);
            if (!cancel)
                UnturnedServer.RemoveStructure(structure);
        }

        internal static void RunBarricadeCreated(BarricadeData barricade, ref bool cancel) => OnBarricadeCreated?.Invoke(UnturnedBarricade.Create(barricade), ref cancel);

        internal static void RunBarricadeRemoved(UnturnedBarricade barricade, ref bool cancel)
        {
            if (OnBarricadeRemoved == null)
                return;

            OnBarricadeRemoved(barricade, ref cancel);
            if (!cancel)
                UnturnedServer.RemoveBarricade(barricade);
        }

        internal static void RunPacketSent(ref CSteamID steamID, ref ESteamPacket type, ref byte[] packet, ref int size, ref int channel, ref bool cancel) => OnPacketSent?.Invoke(ref steamID, ref type, ref packet, ref size, ref channel, ref cancel);

        #endregion
    }
}
