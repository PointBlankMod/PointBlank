using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using PointBlank.Framework.Permissions.Ring;

namespace PointBlank.API.DataManagment
{
    /// <summary>
    /// CONF data managment
    /// </summary>
    [RingPermission(SecurityAction.Demand, ring = RingPermissionRing.None)]
    internal class ConfData // This is still under development and is currently not to be used.
    {
        #region Properties
        /// <summary>
        /// The path to the file
        /// </summary>
        public string Path { get; private set; }
        /// <summary>
        /// CONF dictionary
        /// </summary>
        public Dictionary<int, object> CONFs { get; private set; }

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
            this.CONFs = new Dictionary<int, object>(); // Create the CONFs dictionary
            this.CreatedNew = !File.Exists(path); // Check if we have to create a new file

            Reload(); // Run the reload
        }

        #region Static Functions
        /// <summary>
        /// Checks if the file is CONF
        /// </summary>
        /// <param name="filepath">The path to the file</param>
        /// <returns>If the file is a CONF</returns>
        public static bool CheckFile(string filepath)
        {
            if (!File.Exists(filepath))
                return false;
            if (File.ReadAllText(filepath).Contains("="))
                return true;

            return false;
        }

        /// <summary>
        /// Serializes class instance to CONF file
        /// </summary>
        /// <param name="filepath">The path to CONF file</param>
        /// <param name="instance">The instance of the class</param>
        public static void Serialize(string filepath, object instance)
        {
            List<string> lines = new List<string>(); // The lines to be written

            foreach(FieldInfo field in instance.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)) // Get the fields
                lines.Add(field.Name + "=" + field.GetValue(instance)); // Add the line

            File.WriteAllLines(filepath, lines.ToArray()); // Write the file
        }

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
                if (lines[i][0] == '\n') // Is it empty?
                    continue; // Skip if empty

                if(lines[i][0] == '#') // Is it a comment?
                {
                    CONFs.Add(i, lines[i].Substring(1, lines[i].Length - 1)); // It's a comment add it
                    continue;
                }

                if (lines[i].Contains("=")) // Is it a CONF?
                {
                    string[] spl = lines[i].Split('='); // Split the CONF
                    string Value = ""; // Set the value

                    for(int a = 1; a < spl.Length; a++)
                        Value += spl[a] + ((a + 1) == spl.Length ? "" : "="); // Add to the value
                    CONFs.Add(i, new CONF(Value, spl[0])); // Add the CONF
                    continue;
                }
            }
        }

        /// <summary>
        /// Saves the CONF file
        /// </summary>
        public void Save()
        {
            int line = 0; // Set the line
            Dictionary<int, object> tmpCONFs = new Dictionary<int, object>(); // Create temporary CONFs
            List<string> lines = new List<string>(); // Create the lines list

            while(CONFs.Count > 0) // Loop until both are empty
            {
                if (CONFs.ContainsKey(line))
                {
                    lines.Add(CONFs[line].ToString()); // Add to the list of lines
                    tmpCONFs.Add(line, CONFs[line]); // Add to tmp CONFs
                    CONFs.Remove(line); // Remove from the original CONF

                    line++; // Add the line
                    continue; // Continue
                }

                lines.Add("\n"); // Add new line
                line++; // Add the line
            }

            CONFs = tmpCONFs; // Set the CONFs
            File.WriteAllLines(Path, lines.ToArray()); // Write to file
        }

        /// <summary>
        /// Adds a comment to the file
        /// </summary>
        /// <param name="comment">The comment to add</param>
        public void AddComment(string comment) => CONFs.Add(CONFs.Keys.Max(), comment); // Add the comment

        /// <summary>
        /// Adds a comment to the line
        /// </summary>
        /// <param name="comment">The comment to add</param>
        /// <param name="line">The line position</param>
        public void AddComment(string comment, int line) => CONFs.Add(line, comment); // Add the comment

        /// <summary>
        /// Edits a comment on that line
        /// </summary>
        /// <param name="comment">The comment to do</param>
        /// <param name="line">The line to edit</param>
        public void EditComment(string comment, int line) => CONFs[line] = comment; // Change to the comment

        /// <summary>
        /// Remove the comment from the line
        /// </summary>
        /// <param name="line">The line to remove</param>
        public void RemoveCONF(int line) => CONFs.Remove(line); // Remove the CONF

        /// <summary>
        /// Adds a CONF to the file
        /// </summary>
        /// <param name="key">The key of the CONF</param>
        /// <param name="value">The value of the CONF</param>
        public void AddCONF(string key, string value) => CONFs.Add(CONFs.Keys.Max(), new CONF(value, key)); // Add the CONF

        /// <summary>
        /// Adds a CONF to the line
        /// </summary>
        /// <param name="key">The key of the CONF</param>
        /// <param name="value">The value of the CONF</param>
        /// <param name="line">The line to add the CONF to</param>
        public void AddCONF(string key, string value, int line) => CONFs.Add(line, new CONF(value, key)); // Add the CONF

        /// <summary>
        /// Edits a CONF on that line
        /// </summary>
        /// <param name="key">The key of the CONF</param>
        /// <param name="value">The value of the CONF</param>
        /// <param name="line">The CONF line</param>
        public void EditCONF(string key, string value, int line) => CONFs[line] = new CONF(value, key); // Edit the CONF
        #endregion
    }

    /// <summary>
    /// The CONF storage class
    /// </summary>
    [RingPermission(SecurityAction.Demand, ring = RingPermissionRing.None)]
    public class CONF
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
        internal CONF(string value, string key)
        {
            this.Value = value;
            this.Key = key;
        }

        #region Functions
        public override string ToString() => Key + "=" + Value;
        #endregion
    }
}
