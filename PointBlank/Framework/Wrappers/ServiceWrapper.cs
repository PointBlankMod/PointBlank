using System;
using PointBlank.API.Services;
using PointBlank.API;

namespace PointBlank.Framework.Wrappers
{
    internal class ServiceWrapper
    {
        #region Properties
        public bool Enabled { get; private set; } // Is the service running
        public PointBlankService ServiceClass { get; private set; } // The service class
        #endregion

        public ServiceWrapper(PointBlankService ServiceClass)
        {
            // Setup variables
            this.ServiceClass = ServiceClass;

            // Setup data
            PBEnvironment.services.Add(ServiceClass.GetType().Name + "." + ServiceClass.Name, this); // Add the service

            // Run functions
            if (ServiceClass.AutoStart)
                Start();
        }

        #region Functions
        public bool Start()
        {
            if (Enabled) // Don't run if it is already running
                return true;

            PointBlankLogging.Log("Starting service: " + ServiceClass.Name);

            // Call the important functions
            try
            {
                PointBlankServiceEvents.RunServiceStart(ServiceClass); // Run the pre-run event
                ServiceClass.Load(); // Run the code
                PointBlankServiceEvents.RunServiceLoaded(ServiceClass); // Run the post-run event
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Error when starting service: " + ServiceClass.Name, ex);
            }

            // Set the variables
            Enabled = true;

            PointBlankLogging.Log("Started service: " + ServiceClass.Name);
            return true;
        }

        public bool Stop()
        {
            if (!Enabled) // Don't stop if it isn't running
                return true;

            PointBlankLogging.Log("Stopping service: " + ServiceClass.Name);

            // Call the important functions
            try
            {
                PointBlankServiceEvents.RunServiceStop(ServiceClass); // Run the pre-stop event
                ServiceClass.Unload(); // Run the code
                PointBlankServiceEvents.RunServiceUnloaded(ServiceClass); // Run the post-stop event
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Error when stopping service: " + ServiceClass.Name, ex);
            }

            // Set the variables
            Enabled = false;

            PointBlankLogging.Log("Stopped service: " + ServiceClass.Name);
            return true;
        }
        #endregion
    }
}
