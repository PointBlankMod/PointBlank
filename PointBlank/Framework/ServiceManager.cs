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
using PointBlank.Framework.Wrappers;

namespace PointBlank.Framework
{
    internal class ServiceManager : MonoBehaviour
    {
        #region Info
        public static readonly string ConfigurationPath = ServerInfo.ConfigurationsPath + "/Services";
        #endregion

        #region Properties
        public bool Initialized { get; private set; } = false; // Is the service manager initialized

        public UniversalData UniServicesData { get; private set; } // The universal data for services
        public JsonData ServicesData { get; private set; } // The services data
        #endregion

        #region Private Functions
        private void RunService(Type service, ServiceAttribute attribute) // Runs the service
        {
            try
            {
                if (attribute.Replace)
                    foreach (ServiceWrapper ser in Enviroment.services.Where(a => a.Value.ServiceAttribute.Name == attribute.Name).Select(a => a.Value))
                        ser.Stop(); // Stop all services with the same name
                else
                    if (Enviroment.services.Count(a => a.Key == (service.Name + "." + attribute.Name)) > 0)
                        return; // Make sure that the services with the same name aren't ran

                new ServiceWrapper(attribute.AutoStart, (Service)Activator.CreateInstance(service), attribute); // Create the service wrapper
            }
            catch (Exception ex)
            {
                Logging.LogError("Error starting service: " + attribute.Name, ex);
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
                Logging.LogError("Error stopping service: " + wrapper.ServiceAttribute.Name, ex);
            }
        }
        #endregion

        #region Public Functions
        public void LoadService(Type service) // Load the service using the type
        {
            ServiceAttribute attribute = (ServiceAttribute)Attribute.GetCustomAttribute(service, typeof(ServiceAttribute)); // Get the attribute

            if (!service.IsClass || !typeof(Service).IsAssignableFrom(service)) // If it isn't a service then return
                return;
            if (attribute == null) // If the attribute is null return
                return;

            if (ServicesData.CheckKey(attribute.Name))
            {
                if (((string)ServicesData.Document[attribute.Name]["AutoStart"]).ToLower() == "true")
                    attribute.AutoStart = true; // Set the autostart to true
                else
                    attribute.AutoStart = false; // Set the autostart to false
                if (((string)ServicesData.Document[attribute.Name]["Replace"]).ToLower() == "true")
                    attribute.Replace = true; // Set the replace to true
                else
                    attribute.Replace = false; // Set the replace to false
            }
            else
            {
                JObject jObj = new JObject(); // Create a jobject

                jObj.Add("AutoStart", (attribute.AutoStart ? "true" : "false")); // Add the autostart
                jObj.Add("Replace", (attribute.Replace ? "true" : "false")); //  Add the replace

                ServicesData.Document.Add(attribute.Name, jObj); // Add the jobject
                UniServicesData.Save();
            }

            RunService(service, attribute); // Run the service
        }

        public void UnloadService(string name) // Unload the service using the name
        {
            StopService(Enviroment.services[name]);
        }

        public void UnloadService(ServiceWrapper wrapper) // Unload the service using the wrapper
        {
            StopService(wrapper);
        }

        public void Init() // Initializes the service manager
        {
            if (Initialized) // Don't bother initializing if it already is initialized
                return;
            if (!Directory.Exists(ConfigurationPath))
                Directory.CreateDirectory(ConfigurationPath); // Create the services configuration

            UniServicesData = new UniversalData(ServerInfo.ConfigurationsPath + "/Services"); // Open the file
            ServicesData = UniServicesData.GetData(EDataType.JSON) as JsonData; // Get the JSON

            foreach (Type class_type in Assembly.GetExecutingAssembly().GetTypes()) // Load the local services
                LoadService(class_type);

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
            foreach(ServiceWrapper wrapper in Enviroment.services.Select(a => a.Value)) // Stop the services
                StopService(wrapper);

            // Set the variables
            Initialized = false;
        }
        #endregion

        #region Event Functions
        private void OnPluginLoaded(Plugin plugin)
        {
            foreach (Type class_type in plugin.GetType().Assembly.GetTypes())
                LoadService(class_type);
        }
        #endregion
    }
}
