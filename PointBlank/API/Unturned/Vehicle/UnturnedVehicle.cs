using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UnityEngine;
using Steamworks;
using PointBlank.API.Unturned.Server;

namespace PointBlank.API.Unturned.Vehicle
{
    /// <summary>
    /// The unturned vehicle class
    /// </summary>
    public class UnturnedVehicle
    {
        #region Properties
        // Important information
        /// <summary>
        /// The interactablevehicle instance
        /// </summary>
        public InteractableVehicle Vehicle { get; private set; }
        /// <summary>
        /// The vehicle asset
        /// </summary>
        public VehicleAsset Asset => Vehicle.asset;
        /// <summary>
        /// The ID of the vehicle
        /// </summary>
        public uint InstanceID => Vehicle.instanceID;

        // Vehicle information
        /// <summary>
        /// Can the vehicle lights be turned on
        /// </summary>
        public bool CanTurnOnLights => Vehicle.canTurnOnLights;
        /// <summary>
        /// Can use the vehicle horn
        /// </summary>
        public bool CanUseHorn => Vehicle.canUseHorn;
        /// <summary>
        /// Does the vehicle have a battery
        /// </summary>
        public bool HasBattery => Vehicle.hasBattery;
        /// <summary>
        /// Is the battery full
        /// </summary>
        public bool IsBatteryFull => Vehicle.isBatteryFull;
        /// <summary>
        /// Can the battery be replaced
        /// </summary>
        public bool IsBatteryReplacable => Vehicle.isBatteryReplaceable;
        /// <summary>
        /// Is the vehicle dead
        /// </summary>
        public bool IsDead => Vehicle.isDead;
        /// <summary>
        /// Is there a player driving the vehicle
        /// </summary>
        public bool IsDriven => Vehicle.isDriven;
        /// <summary>
        /// Is the vehicle drowned
        /// </summary>
        public bool IsDrowned => Vehicle.isDrowned;
        /// <summary>
        /// Is the engine on
        /// </summary>
        public bool IsEngineOn => Vehicle.isEngineOn;
        /// <summary>
        /// Is the vehicle destroyed
        /// </summary>
        public bool IsDestroyed => Vehicle.isGoingToRespawn;
        /// <summary>
        /// Can the vehicle be refilled
        /// </summary>
        public bool IsRefillable => Vehicle.isRefillable;
        /// <summary>
        /// Does the vehicle have full health
        /// </summary>
        public bool HasFullHealth => Vehicle.isRepaired;
        /// <summary>
        /// Is the vehicle underwater
        /// </summary>
        public bool IsUnderwater => Vehicle.isUnderwater;
        /// <summary>
        /// Is the vehicle locked
        /// </summary>
        public bool IsLocked
        {
            get => Vehicle.isLocked;
            set => VehicleManager.instance.channel.send("tellVehicleLock", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    InstanceID,
                    LockedOwner,
                    LockedGroup,
                    value
                });
        }
        /// <summary>
        /// The player who locked the vehicle
        /// </summary>
        public CSteamID LockedOwner
        {
            get => Vehicle.lockedOwner;
            set => VehicleManager.instance.channel.send("tellVehicleLock", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    InstanceID,
                    value,
                    LockedGroup,
                    IsLocked
                });
        }
        /// <summary>
        /// The group who locked the vehicle
        /// </summary>
        public CSteamID LockedGroup
        {
            get => Vehicle.lockedGroup;
            set => VehicleManager.instance.channel.send("tellVehicleLock", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    InstanceID,
                    LockedOwner,
                    value,
                    IsLocked
                });
        }
        /// <summary>
        /// The current battery charge
        /// </summary>
        public ushort Battery
        {
            get => Vehicle.batteryCharge;
            set => Vehicle.batteryCharge = value;
        }
        /// <summary>
        /// Are the headlights turned on
        /// </summary>
        public bool HeadlightsOn
        {
            get => Vehicle.headlightsOn;
            set => VehicleManager.instance.channel.send("tellVehicleHeadlights", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    InstanceID,
                    value
                });
        }
        /// <summary>
        /// Are the sirens turned on
        /// </summary>
        public bool SirensOn
        {
            get => Vehicle.sirensOn;
            set => VehicleManager.instance.channel.send("tellVehicleSirens", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    InstanceID,
                    value
                });
        }
        /// <summary>
        /// The vehicle fuel
        /// </summary>
        public ushort Fuel
        {
            get => Vehicle.fuel;
            set => VehicleManager.instance.channel.send("tellVehicleFuel", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    InstanceID,
                    value
                });
        }
        /// <summary>
        /// The vehicle health
        /// </summary>
        public ushort Health
        {
            get => Vehicle.health;
            set => VehicleManager.instance.channel.send("tellVehicleHealth", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    InstanceID,
                    value
                });
        }

        // Asset information
        /// <summary>
        /// Maximum fuel the vehicle can have
        /// </summary>
        public ushort MaxFuel => Asset.fuelMax;
        /// <summary>
        /// Minimum fuel the vehicle can have
        /// </summary>
        public ushort MinFuel => Asset.fuelMin;
        /// <summary>
        /// Can the tires be damaged
        /// </summary>
        public bool CanTiresBeDamaged => Asset.canTiresBeDamaged;
        /// <summary>
        /// The vehicle engine
        /// </summary>
        public EEngine Engine => Asset.engine;
        /// <summary>
        /// Does the vehicle have a crawler
        /// </summary>
        public bool HasCrawler => Asset.hasCrawler;
        /// <summary>
        /// Does the vehicle have headlights
        /// </summary>
        public bool HasHeadlights => Asset.hasHeadlights;
        /// <summary>
        /// Does the vehicle have sirens
        /// </summary>
        public bool HasSirens => Asset.hasSirens;
        /// <summary>
        /// Does the vehicle have sleds
        /// </summary>
        public bool HasSleds => Asset.hasSleds;
        /// <summary>
        /// Does the vehicle have traction
        /// </summary>
        public bool HasTraction => Asset.hasTraction;
        /// <summary>
        /// Maximum health the vehicle can have
        /// </summary>
        public ushort MaxHealth => Asset.healthMax;
        /// <summary>
        /// Minimum health the vehicle can have
        /// </summary>
        public ushort MinHealth => Asset.healthMin;
        /// <summary>
        /// Can the vehicle be destroyed
        /// </summary>
        public bool IsVulnerable => !Asset.isVulnerable;
        /// <summary>
        /// Maximum speed the vehicle can have
        /// </summary>
        public float MaxSpeed => Asset.speedMax;
        /// <summary>
        /// Minimum speed the vehicle can have
        /// </summary>
        public float MinSpeed => Asset.speedMin;
        #endregion

        private UnturnedVehicle(InteractableVehicle vehicle)
        {
            // Set the variables
            this.Vehicle = vehicle;

            // Run code
            UnturnedServer.AddVehicle(this);
        }

        #region Static Functions
        /// <summary>
        /// Creates a new unturned vehicle instance or returns an existing one
        /// </summary>
        /// <param name="vehicle">The vehicle instance</param>
        /// <returns>The unturned vehicle instance</returns>
        internal static UnturnedVehicle Create(InteractableVehicle vehicle)
        {
            UnturnedVehicle veh = UnturnedServer.Vehicles.FirstOrDefault(a => a.Vehicle == vehicle);

            return veh ?? new UnturnedVehicle(vehicle);
        }

        /// <summary>
        /// Spawns a new vehicle and returns the unturned vehicle instance
        /// </summary>
        /// <param name="id">The ID of the vehicle</param>
        /// <param name="position">The position to spawn it at</param>
        /// <param name="rotation">The rotation of the vehicle</param>
        /// <returns>The unturned vehicle instance</returns>
        public static UnturnedVehicle Create(ushort id, Vector3 position, Quaternion rotation)
        {
            VehicleManager.spawnVehicle(id, position, rotation);
            InteractableVehicle iv = VehicleManager.vehicles[VehicleManager.vehicles.Count - 1];

            return new UnturnedVehicle(iv);
        }
        #endregion
    }
}
