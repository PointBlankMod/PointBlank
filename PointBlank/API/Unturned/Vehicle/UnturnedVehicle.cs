using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
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
        /// Is the vehicle locked
        /// </summary>
        public bool IsLocked
        {
            get
            {
                return Vehicle.isLocked;
            }
            set
            {
                VehicleManager.instance.channel.send("tellVehicleLock", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    InstanceID,
                    LockedOwner,
                    LockedGroup,
                    value
                });
            }
        }
        /// <summary>
        /// The player who locked the vehicle
        /// </summary>
        public CSteamID LockedOwner
        {
            get
            {
                return Vehicle.lockedOwner;
            }
            set
            {
                VehicleManager.instance.channel.send("tellVehicleLock", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    InstanceID,
                    value,
                    LockedGroup,
                    IsLocked
                });
            }
        }
        /// <summary>
        /// The group who locked the vehicle
        /// </summary>
        public CSteamID LockedGroup
        {
            get
            {
                return Vehicle.lockedGroup;
            }
            set
            {
                VehicleManager.instance.channel.send("tellVehicleLock", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    InstanceID,
                    LockedOwner,
                    value,
                    IsLocked
                });
            }
        }
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
        public static UnturnedVehicle Create(InteractableVehicle vehicle)
        {
            UnturnedVehicle veh = UnturnedServer.Vehicles.FirstOrDefault(a => a.Vehicle == vehicle);

            if (veh != null)
                return veh;
            return new UnturnedVehicle(vehicle);
        }
        #endregion
    }
}
