using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PointBlank.Framework.Permissions.Ring;

namespace PointBlank.API.DataManagment
{
    /// <summary>
    /// JSON data managment
    /// </summary>
    [RingPermission(SecurityAction.Demand, ring = RingPermissionRing.None)]
    public class JsonData
    {
        #region Properties
        /// <summary>
        /// Path to JSON file
        /// </summary>
        public string Path { get; private set; }
        /// <summary>
        /// JSON document
        /// </summary>
        public JObject Document { get; private set; }
        /// <summary>
        /// The XML document root node
        /// </summary>
        //public JArray RootNode { get; private set; }

        /// <summary>
        /// Was the JSON just created
        /// </summary>
        public bool CreatedNew { get; private set; }
        #endregion

        /// <summary>
        /// JSON data managment
        /// </summary>
        /// <param name="path">The path to json file</param>
        internal JsonData(string path)
        {
            this.Path = path; // Set the path
            this.CreatedNew = !File.Exists(path); // Check if we have to create the file

            Reload(); // Run the reload
        }

        /// <summary>
        /// JSON data managment
        /// </summary>
        /// <param name="doc">The json document</param>
        internal JsonData(JObject doc, string path)
        {
            this.Path = path; // Set the path
            this.CreatedNew = !File.Exists(path); // Check if we have to create the file
            this.Document = doc; // Set the doc
        }

        #region Static Functions
        /// <summary>
        /// Checks if the file is JSON
        /// </summary>
        /// <param name="filepath">The path to the file</param>
        /// <returns>If the file is JSON</returns>
        public static bool CheckFile(string filepath)
        {
            if (!File.Exists(filepath))
                return false;

            try
            {
                JObject.Parse(File.ReadAllText(filepath)); // Try to parse

                return true; // Parse successful, file valid
            }
            catch (Exception ex)
            {
                return false; // Failed to validate
            }
        }

        /// <summary>
        /// Serializes a class instance into a JSON file
        /// </summary>
        /// <param name="filepath">The path to the file</param>
        /// <param name="instance">The instance of the class</param>
        public static void Serialize(string filepath, object instance)
        {
            File.WriteAllText(filepath, JsonConvert.SerializeObject(instance)); // Serialize the object
        }

        /// <summary>
        /// Serializes a class into a JSON file
        /// </summary>
        /// <typeparam name="T">The class to serialize</typeparam>
        /// <param name="filepath">The path to the file</param>
        public static void Serialize<T>(string filepath)
        {
            File.WriteAllText(filepath, JsonConvert.SerializeObject(Activator.CreateInstance<T>())); // Instentate the class and serialize the instance
        }

        /// <summary>
        /// Deserializes a JSON file into an instance of a class
        /// </summary>
        /// <param name="filepath">The path to the file</param>
        /// <param name="type">The type to deserialize to</param>
        /// <returns>Instance of the deserialized file</returns>
        public static object Deserialize(string filepath, Type type)
        {
            return JsonConvert.DeserializeObject(File.ReadAllText(filepath), type); // Deserialize the file
        }

        /// <summary>
        /// Deserialize a JSON file into an instance of a class
        /// </summary>
        /// <typeparam name="T">The class</typeparam>
        /// <param name="filepath">The file</param>
        /// <returns>Instance of the class deserialized from JSON</returns>
        public static T Deserialize<T>(string filepath)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filepath)); // Deserialize the JSON file
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Reloads the JSON file
        /// </summary>
        public void Reload()
        {
            if (CreatedNew)
                Document = new JObject(); // Create a new JObject
            else
                Document = JObject.Parse(File.ReadAllText(Path)); // Parse the JSON file
        }

        /// <summary>
        /// Saves the JSON file
        /// </summary>
        internal void Save()
        {
            File.WriteAllText(Path, Document.ToString(Formatting.Indented)); // Write the file
        }

        /// <summary>
        /// Adds a name + value to an array
        /// </summary>
        /// <param name="array">The array to add to</param>
        /// <param name="name">The name/key</param>
        /// <param name="value">The value</param>
        public void AddToArray(JArray array, string name, object value)
        {
            array.Add(new JObject(
                new JProperty(name, value)
            ));
        }

        /// <summary>
        /// Checks if the key exists in the document
        /// </summary>
        /// <param name="key">The key you want to check</param>
        /// <returns>If it exists in the document</returns>
        public bool CheckKey(string key)
        {
            return (Document[key] != null && Document[key].Type != JTokenType.Null);
        }
        #endregion
    }
}
