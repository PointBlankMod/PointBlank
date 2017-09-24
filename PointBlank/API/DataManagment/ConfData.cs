using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace PointBlank.API.DataManagment
{
#if DEBUG
    /// <summary>
    /// CONF data managment
    /// </summary>
    public class ConfData // This is still under development and is currently not to be used.
    {
        #region Properties
        /// <summary>
        /// The path to the file
        /// </summary>
        public string Path { get; private set; }
        /// <summary>
        /// CONF dictionary
        /// </summary>
        public Dictionary<int, object> ConFs { get; private set; }

        /// <summary>
        /// Was the CONF file just created
        /// </summary>
        public bool CreatedNew { get; private set; }
        #endregion

        /// <summary>
        /// CONF data managment
        /// </summary>
        /// <param name="path">The path to the file</param>
        internal ConfData(string path)
        {
            this.Path = path; // Set the path
            this.ConFs = new Dictionary<int, object>(); // Create the CONFs dictionary
            this.CreatedNew = !File.Exists(path); // Check if we have to create a new file

            Reload(); // Run the reload
        }

        #region Static Functions
        /// <summary>
        /// Checks if the file is CONF
        /// </summary>
        /// <param name="filepath">The path to the file</param>
        /// <returns>If the file is a CONF</returns>
        public static bool CheckFile(string filepath) => File.Exists(filepath) && File.ReadAllText(filepath).Contains("=");

        /// <summary>
        /// Serializes class instance to CONF file
        /// </summary>
        /// <param name="filepath">The path to CONF file</param>
        /// <param name="instance">The instance of the class</param>
        public static void Serialize(string filepath, object instance) => File.WriteAllLines(filepath, instance.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).Select(field => field.Name + "=" + field.GetValue(instance)).ToArray());

        /// <summary>
        /// Serializes class to CONF file
        /// </summary>
        /// <typeparam name="T">The class</typeparam>
        /// <param name="filepath">Path to the file</param>
        public static void Serialize<T>(string filepath) => Serialize(filepath, Activator.CreateInstance<T>()); // Create instance and serialize
        #endregion

        #region Public Functions
        /// <summary>
        /// Reloads the CONF file
        /// </summary>
        public void Reload()
        {
            if (CreatedNew)
                return; // If it is just created don't bother

            string[] lines = File.ReadAllLines(Path); // Read all the file lines

            for(int i = 0; i < lines.Length; i++)
            {
                switch (lines[i][0])
                {
                    case '\n':
                        continue; // Skip if empty
                    case '#':
                        ConFs.Add(i, lines[i].Substring(1, lines[i].Length - 1)); // It's a comment add it
                        continue;
                }

                if (!lines[i].Contains("=")) continue;
                string[] spl = lines[i].Split('='); // Split the CONF
                string value = ""; // Set the value

                for(int a = 1; a < spl.Length; a++)
                    value += spl[a] + ((a + 1) == spl.Length ? "" : "="); // Add to the value
                ConFs.Add(i, new Conf(value, spl[0])); // Add the CONF
                continue;
            }
        }

        /// <summary>
        /// Saves the CONF file
        /// </summary>
        public void Save()
        {
            int line = 0; // Set the line
            Dictionary<int, object> tmpConFs = new Dictionary<int, object>(); // Create temporary CONFs
            List<string> lines = new List<string>(); // Create the lines list

            while(ConFs.Count > 0) // Loop until both are empty
            {
                if (ConFs.ContainsKey(line))
                {
                    lines.Add(ConFs[line].ToString()); // Add to the list of lines
                    tmpConFs.Add(line, ConFs[line]); // Add to tmp CONFs
                    ConFs.Remove(line); // Remove from the original CONF

                    line++; // Add the line
                    continue; // Continue
                }

                lines.Add("\n"); // Add new line
                line++; // Add the line
            }

            ConFs = tmpConFs; // Set the CONFs
            File.WriteAllLines(Path, lines.ToArray()); // Write to file
        }

        /// <summary>
        /// Adds a comment to the file
        /// </summary>
        /// <param name="comment">The comment to add</param>
        public void AddComment(string comment) => ConFs.Add(ConFs.Keys.Max(), comment); // Add the comment

        /// <summary>
        /// Adds a comment to the line
        /// </summary>
        /// <param name="comment">The comment to add</param>
        /// <param name="line">The line position</param>
        public void AddComment(string comment, int line) => ConFs.Add(line, comment); // Add the comment

        /// <summary>
        /// Edits a comment on that line
        /// </summary>
        /// <param name="comment">The comment to do</param>
        /// <param name="line">The line to edit</param>
        public void EditComment(string comment, int line) => ConFs[line] = comment; // Change to the comment

        /// <summary>
        /// Remove the comment from the line
        /// </summary>
        /// <param name="line">The line to remove</param>
        public void RemoveConf(int line) => ConFs.Remove(line); // Remove the CONF

        /// <summary>
        /// Adds a CONF to the file
        /// </summary>
        /// <param name="key">The key of the CONF</param>
        /// <param name="value">The value of the CONF</param>
        public void AddConf(string key, string value) => ConFs.Add(ConFs.Keys.Max(), new Conf(value, key)); // Add the CONF

        /// <summary>
        /// Adds a CONF to the line
        /// </summary>
        /// <param name="key">The key of the CONF</param>
        /// <param name="value">The value of the CONF</param>
        /// <param name="line">The line to add the CONF to</param>
        public void AddConf(string key, string value, int line) => ConFs.Add(line, new Conf(value, key)); // Add the CONF

        /// <summary>
        /// Edits a CONF on that line
        /// </summary>
        /// <param name="key">The key of the CONF</param>
        /// <param name="value">The value of the CONF</param>
        /// <param name="line">The CONF line</param>
        public void EditConf(string key, string value, int line) => ConFs[line] = new Conf(value, key); // Edit the CONF
        #endregion
    }

    /// <summary>
    /// The CONF storage class
    /// </summary>
    public class Conf
    {
        #region Properties
        /// <summary>
        /// The value of the conf
        /// </summary>
        public string Value { get; private set; }
        /// <summary>
        /// The key of the CONF
        /// </summary>
        public string Key { get; private set; }
        #endregion

        /// <summary>
        /// The CONF storage class
        /// </summary>
        /// <param name="line">The line number</param>
        /// <param name="value">The value of the CONF</param>
        /// <param name="key">The key of the CONF</param>
        internal Conf(string value, string key)
        {
            this.Value = value;
            this.Key = key;
        }

        #region Functions
        public override string ToString() => Key + "=" + Value;
        #endregion
    }
#endif
}
