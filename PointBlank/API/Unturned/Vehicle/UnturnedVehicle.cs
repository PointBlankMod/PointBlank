using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using PointBlank.API.Unturned.Server;

namespace PointBlank.API.Unturned.Vehicle
{
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
        #endregion

        public UnturnedVehicle(InteractableVehicle vehicle)
        {
            // Set the variables
            this.Vehicle = vehicle;

            // Run code
            UnturnedServer.AddVehicle(this);
        }
    }
}
