using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Newtonsoft.Json.Linq;
using PointBlank.API;
using PointBlank.API.Services;
using PointBlank.API.Plugins;
using PointBlank.API.DataManagment;
using PointBlank.API.Server;
using PointBlank.API.Extension;
using PointBlank.Framework.Wrappers;

namespace PointBlank.Framework
{
    internal class ServiceManager : MonoBehaviour
    {
        #region Info
        public static readonly string ConfigurationPath = Server.ConfigurationsPath + "/Services";
        #endregion

        #region Variables
        private List<Service> _tempServices = new List<Service>();
        private List<ServiceWrapper> _tempWrappers = new List<ServiceWrapper>();
        #endregion

        #region Properties
        public bool Initialized { get; private set; } = false; // Is the service manager initialized

        public UniversalData UniServicesData { get; private set; } // The universal data for services
        public JsonData ServicesData { get; private set; } // The services data
        #endregion

        #region Private Functions
        private void RunService(Service service) // Runs the service
        {
            try
            {
                if (service.Replace)
                    foreach (ServiceWrapper wrapper in Enviroment.services.Where(a => a.Value.ServiceClass.FullName == service.FullName && a.Value.ServiceClass.Replacable).Select(a => a.Value))
                        wrapper.Stop(); // Stop all services with the same name
                else
                    if (Enviroment.services.Count(a => a.Key == service.FullName) > 0)
                        return; // Make sure that the services with the same name aren't ran

                new ServiceWrapper(service); // Create the service wrapper
            }
            catch (Exception ex)
            {
                Logging.LogError("Error starting service: " + service.Name, ex);
            }
        }

        private void StopService(ServiceWrapper wrapper) // Stops the service
        {
            try
            {
                wrapper.Stop(); // Stop the service
            }
            catch (Exception ex)
            {
                Logging.LogError("Error stopping service: " + wrapper.ServiceClass.Name, ex);
            }
        }
        #endregion

        #region Public Functions
        public void LoadService(Type service) // Load the service using the type
        {
            if (!service.IsClass || !typeof(Service).IsAssignableFrom(service)) // If it isn't a service then return
                return;
            if (service == typeof(Service)) // Prevents the actual service API from being loaded
                return;
            Service ser = (Service)Activator.CreateInstance(service);

            if (ServicesData.CheckKey(ser.Name))
            {
                ser.AutoStart = ((string)ServicesData.Document[ser.Name]["AutoStart"]).ToLower() == "true";
                ser.Replace = ((string)ServicesData.Document[ser.Name]["Replace"]).ToLower() == "true";
            }
            else
            {
                JObject jObj = new JObject
                {
                    {"AutoStart", (ser.AutoStart ? "true" : "false")},
                    {"Replace", (ser.Replace ? "true" : "false")}
                };

                ServicesData.Document.Add(ser.Name, jObj);
                UniServicesData.Save();
            }

            _tempServices.Add(ser);
        }

        public void UnloadService(string name) => StopService(Enviroment.services[name]); // Unload the service using the name

        public void UnloadService(ServiceWrapper wrapper) => StopService(wrapper); // Unload the service using the wrapper

        public void Init() // Initializes the service manager
        {
            if (Initialized) // Don't bother initializing if it already is initialized
                return;
            if (!Directory.Exists(ConfigurationPath))
                Directory.CreateDirectory(ConfigurationPath); // Create the services configuration

            UniServicesData = new UniversalData(Server.ConfigurationsPath + "/Services"); // Open the file
            ServicesData = UniServicesData.GetData(EDataType.JSON) as JsonData; // Get the JSON

            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies().Where(a => Attribute.GetCustomAttribute(a, typeof(ExtensionAttribute)) != null))
                foreach (Type class_type in asm.GetTypes())
                    LoadService(class_type);
            foreach (Service ser in _tempServices.OrderBy(a => a.LaunchIndex))
                RunService(ser);
            _tempServices.Clear();

            // Setup the events
            PluginEvents.OnPluginLoaded += new PluginEvents.PluginEventHandler(OnPluginLoaded);

            // Set the variables
            Initialized = true;
        }

        public void Shutdown() // The service manager is getting shut down
        {
            if (!Initialized) // Don't bother shutting down if the service manager isn't initialized
                return;

            UniServicesData.Save(); // Save the services data
            foreach (ServiceWrapper wrapper in Enviroment.services.Select(a => a.Value)) // Stop the services
                _tempWrappers.Add(wrapper);
            foreach (ServiceWrapper wrapper in _tempWrappers.OrderByDescending(a => a.ServiceClass.LaunchIndex))
                StopService(wrapper);
            _tempWrappers.Clear();

            // Set the variables
            Initialized = false;
        }
        #endregion

        #region Event Functions
        private void OnPluginLoaded(Plugin plugin)
        {
            foreach (Type class_type in plugin.GetType().Assembly.GetTypes())
                LoadService(class_type);
            foreach (Service ser in _tempServices.OrderBy(a => a.LaunchIndex))
                RunService(ser);
            _tempServices.Clear();
        }
        #endregion
    }
}
