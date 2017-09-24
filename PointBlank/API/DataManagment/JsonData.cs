using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PointBlank.API.DataManagment
{
    /// <summary>
    /// Json data managment
    /// </summary>
    public class JsonData
    {
        #region Properties
        /// <summary>
        /// Path to Json file
        /// </summary>
        public string Path { get; private set; }
        /// <summary>
        /// Json document
        /// </summary>
        public JObject Document { get; private set; }
        /// <summary>
        /// The XML document root node
        /// </summary>
        //public JArray RootNode { get; private set; }

        /// <summary>
        /// Was the Json just created
        /// </summary>
        public bool CreatedNew { get; private set; }
        #endregion

        /// <summary>
        /// Json data managment
        /// </summary>
        /// <param name="path">The path to Json file</param>
        internal JsonData(string path)
        {
            this.Path = path; // Set the path
            this.CreatedNew = !File.Exists(path); // Check if we have to create the file

            Reload(); // Run the reload
        }

        /// <summary>
        /// Json data managment
        /// </summary>
        /// <param name="doc">The Json document</param>
        internal JsonData(JObject doc, string path)
        {
            this.Path = path; // Set the path
            this.CreatedNew = !File.Exists(path); // Check if we have to create the file
            this.Document = doc; // Set the doc
        }

        #region Static Functions
        /// <summary>
        /// Checks if the file is Json
        /// </summary>
        /// <param name="filepath">The path to the file</param>
        /// <returns>If the file is Json</returns>
        public static bool CheckFile(string filepath)
        {
            if (!File.Exists(filepath))
                return false;

            try
            {
                JObject.Parse(File.ReadAllText(filepath)); // Try to parse

                return true; // Parse successful, file valid
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                return false; // Failed to validate
            }
        }

        /// <summary>
        /// Serializes a class instance into a Json
        /// </summary>
        /// <param name="instance">The instance of the class</param>
        /// <returns>The Json string</returns>
        public static string Serialize(object instance) => JsonConvert.SerializeObject(instance); // Serialize the object

        /// <summary>
        /// Serializes a class into a Json
        /// </summary>
        /// <typeparam name="T">The class to serialize</typeparam>
        /// <returns>The Json string</returns>
        public static string Serialize<T>()
        {
            return JsonConvert.SerializeObject(Activator.CreateInstance<T>()); // Instentate the class and serialize the instance
        }

        /// <summary>
        /// Deserializes Json into an instance of a class
        /// </summary>
        /// <param name="Json">The Json</param>
        /// <param name="type">The type to deserialize to</param>
        /// <returns>Instance of the deserialized file</returns>
        public static object Deserialize(string Json, Type type) => JsonConvert.DeserializeObject(Json, type); // Deserialize the file

        /// <summary>
        /// Deserialize Json into an instance of a class
        /// </summary>
        /// <typeparam name="T">The class</typeparam>
        /// <param name="Json">The Json</param>
        /// <returns>Instance of the class deserialized from Json</returns>
        public static T Deserialize<T>(string Json) => JsonConvert.DeserializeObject<T>(Json); // Deserialize the Json
        #endregion

        #region Public Functions
        /// <summary>
        /// Reloads the Json file
        /// </summary>
        public void Reload() => Document = CreatedNew ? new JObject() : JObject.Parse(File.ReadAllText(Path));

        /// <summary>
        /// Saves the Json file
        /// </summary>
        internal void Save() => File.WriteAllText(Path, Document.ToString(Formatting.Indented)); // Write the file

        /// <summary>
        /// Verifies the document and adds missing information specified in information
        /// </summary>
        /// <param name="information">The information to check from</param>
        public void Verify(Dictionary<string, JToken> information)
        {
            foreach(string key in information.Keys)
                if (!CheckKey(key))
                    Document.Add(key, information[key]);
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
        public bool CheckKey(string key) => (Document[key] != null && Document[key].Type != JTokenType.Null);
        #endregion
    }
}
