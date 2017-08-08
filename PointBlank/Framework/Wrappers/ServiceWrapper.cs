using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Services;
using PointBlank.API;

namespace PointBlank.Framework.Wrappers
{
    internal class ServiceWrapper
    {
        #region Properties
        public bool Enabled { get; private set; } // Is the service running
        public Service ServiceClass { get; private set; } // The service class
        #endregion

        public ServiceWrapper(Service ServiceClass)
        {
            // Setup variables
            this.ServiceClass = ServiceClass;

            // Setup data
            Enviroment.services.Add(ServiceClass.GetType().Name + "." + ServiceClass.Name, this); // Add the service

            // Run functions
            if (ServiceClass.AutoStart)
                Start();
        }

        #region Functions
        public bool Start()
        {
            if (Enabled) // Don't run if it is already running
                return true;

            Logging.Log("Starting service: " + ServiceClass.Name);

            // Call the important functions
            try
            {
                ServiceEvents.RunServiceStart(ServiceClass); // Run the pre-run event
                ServiceClass.Load(); // Run the code
                ServiceEvents.RunServiceLoaded(ServiceClass); // Run the post-run event
            }
            catch (Exception ex)
            {
                Logging.LogError("Error when starting service: " + ServiceClass.Name, ex);
            }

            // Setup data
            Enviroment.runtimeObjects["Services"].AddCodeObject(ServiceClass.GetType()); // Add the code object

            // Set the variables
            Enabled = true;

            Logging.Log("Started service: " + ServiceClass.Name);
            return true;
        }

        public bool Stop()
        {
            if (!Enabled) // Don't stop if it isn't running
                return true;

            Logging.Log("Stopping service: " + ServiceClass.Name);

            // Call the important functions
            try
            {
                ServiceEvents.RunServiceStop(ServiceClass); // Run the pre-stop event
                ServiceClass.Unload(); // Run the code
                ServiceEvents.RunServiceUnloaded(ServiceClass); // Run the post-stop event
            }
            catch (Exception ex)
            {
                Logging.LogError("Error when stopping service: " + ServiceClass.Name, ex);
            }

            // Stop data
            Enviroment.runtimeObjects["Services"].RemoveCodeObject(ServiceClass.GetType().Name); // Remove the code object

            // Set the variables
            Enabled = false;

            Logging.Log("Stopped service: " + ServiceClass.Name);
            return true;
        }
        #endregion
    }
}
