using System;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using PointBlank.Framework.Permissions.Ring;
using XFile = System.IO.File;

namespace PointBlank.API.DataManagment
{
    /// <summary>
    /// XML data managment
    /// </summary>
    [RingPermission(SecurityAction.Demand, ring = RingPermissionRing.None)]
    public class XMLData
    {
        #region Properties
        /// <summary>
        /// The XML file path
        /// </summary>
        public string File { get; private set; }
        /// <summary>
        /// Was the XML just created
        /// </summary>
        public bool CreatedNew { get; private set; }

        /// <summary>
        /// The XML document
        /// </summary>
        public XmlDocument Document { get; private set; }
        /// <summary>
        /// The XML document root node
        /// </summary>
        public XmlNode RootNode { get; private set; }
        #endregion

        /// <summary>
        /// XML data managment
        /// </summary>
        /// <param name="filepath">The path to the XML file</param>
        internal XMLData(string filepath)
        {
            File = filepath; // Set the file path
            CreatedNew = !XFile.Exists(File); // Check if we have to make the XML

            Reload(); // Run the reload
        }

        /// <summary>
        /// XML data managment
        /// </summary>
        /// <param name="doc">The document to copy</param>
        /// <param name="filepath">The path to the file</param>
        internal XMLData(XmlDocument doc, string filepath)
        {
            this.File = filepath; // Set the file path
            this.CreatedNew = !XFile.Exists(File); // Check if we have to make the XML
            this.Document = doc; // Set the document
            this.RootNode = Document.DocumentElement; // Set the root node
        }

        /// <summary>
        /// XML data managment
        /// </summary>
        internal XMLData()
        {
            this.CreatedNew = true; // Create new

            Reload(); // Run the reload
        }

        #region Static Functions
        /// <summary>
        /// Checks if the file is XML
        /// </summary>
        /// <param name="filepath">The path to the file to check</param>
        /// <returns>If the file is XML</returns>
        public static bool CheckFile(string filepath)
        {
            if (!XFile.Exists(filepath))
                return false;

            try
            {
                (new XmlDocument()).Load(filepath); // Try to load the file

                return true; // If no errors occured it is fine
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                return false; // An error occured
            }
        }

        /// <summary>
        /// Serializes a class instance to XML
        /// </summary>
        /// <param name="instance">The instance of the class</param>
        /// <returns>The XML</returns>
        public static string Serialize(object instance)
        {
            XmlSerializer serializer = new XmlSerializer(instance.GetType()); // Create the serializer

            using(StringWriter writer = new StringWriter()) // Create a temporary stream writer
            {
                serializer.Serialize(writer, instance); // Serialize
                return writer.ToString();
            }
        }

        /// <summary>
        /// Serializes a class to XML
        /// </summary>
        /// <typeparam name="T">The class</typeparam>
        /// <returns>The XML</returns>
        public static string Serialize<T>()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T)); // Create the serializer
            T instance = Activator.CreateInstance<T>(); // Instentate the class

            using (StringWriter writer = new StringWriter()) // Create a temporary stream writer
            {
                serializer.Serialize(writer, instance); // Serialize
                return writer.ToString();
            }
        }

        /// <summary>
        /// Deserializes XML into an instance
        /// </summary>
        /// <param name="xml">The XML</param>
        /// <param name="type">The file type</param>
        /// <returns>The instance of the object</returns>
        public static object Deserialize(string xml, Type type)
        {
            XmlSerializer serializer = new XmlSerializer(type); // Create the serializer
            object instance = null; // Create instance variable

            using (StringReader reader = new StringReader(xml)) // Create a temporary stream reader
                instance = serializer.Deserialize(reader); // Deserialize
                
            return instance; // Return the instance
        }

        /// <summary>
        /// Deserializes XML into an instance
        /// </summary>
        /// <typeparam name="T">The class to deserialize to</typeparam>
        /// <param name="xml">The XML</param>
        /// <returns>The instance of the class</returns>
        public static T Deserialize<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T)); // Create the serializer
            T instance; // Create instance variable

            using (StringReader reader = new StringReader(xml)) // Create a temporary stream reader
                instance = (T)serializer.Deserialize(reader); // Deserialize

            return instance; // Return the instance
        }

        /// <summary>
        /// Returns the path to the node
        /// </summary>
        /// <param name="node">The node to get the path to</param>
        /// <returns>The path to the node</returns>
        public static string GetPathToNode(XmlNode node)
        {
            string path = node.Name; // Set the start of the path

            while(node.ParentNode != null && node.ParentNode != node.OwnerDocument)
            {
                node = node.ParentNode; // Set the node to the parent
                path = node.Name + "/" + path; // Append the name
            }

            return "/" + path;
        }

        /// <summary>
        /// Converts a string node to an XML node
        /// </summary>
        /// <param name="node">The string node</param>
        /// <returns>The XML node</returns>
        public static XmlNode StringToNode(string node)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(node);

            return doc.DocumentElement;
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Reloads the XML file
        /// </summary>
        public void Reload()
        {
            Document = new XmlDocument(); // Make the new XML document

            if (!CreatedNew)
            {
                Document.Load(File); // Load the XML
                RootNode = Document.DocumentElement; // Get the root node
            }
            else
            {
                RootNode = Document.CreateElement("Data"); // Create the root node
                Document.AppendChild(RootNode); // Add the root node
            }
        }

        /// <summary>
        /// Saves the XML file
        /// </summary>
        internal void Save() => Document.Save(File);

        /// <summary>
        /// Merges 2 XmlDocuments into 1
        /// </summary>
        /// <param name="doc">The XML document to merge with</param>
        /// <param name="noDuplications">Removes all duplicated items</param>
        public void Merge(XmlDocument doc, bool noDuplications = true)
        {
            foreach(XmlNode node in doc.DocumentElement.ChildNodes)
            {
                if (noDuplications && CheckNode(GetPathToNode(node)))
                    continue;

                XmlNode import = Document.ImportNode(node, true); // Import the node

                RootNode.AppendChild(import); // Appeand to root
            }
        }

        /// <summary>
        /// Gets the value of a node
        /// </summary>
        /// <param name="nodepath">The path to the node</param>
        /// <returns>The value in the node</returns>
        public string GetValue(string nodepath) => GetNode(nodepath).InnerText;

        /// <summary>
        /// Gets the node
        /// </summary>
        /// <param name="nodepath">The path to the node</param>
        /// <returns>The node</returns>
        public XmlNode GetNode(string nodepath) => RootNode.SelectSingleNode(nodepath);

        /// <summary>
        /// Gets the values of the nodes with the same path
        /// </summary>
        /// <param name="nodepath">The path to the node</param>
        /// <returns>The values of the nodes with the same path</returns>
        public string[] GetValues(string nodepath)
        {
            List<string> values = new List<string>();

            foreach (XmlNode node in GetNodes(nodepath))
                values.Add(node.InnerText);

            return values.ToArray();
        }

        /// <summary>
        /// Gets the nodes with the same path
        /// </summary>
        /// <param name="nodepath">The path to the node</param>
        /// <returns>The list of the nodes with the same path</returns>
        public XmlNodeList GetNodes(string nodepath) => RootNode.SelectNodes(nodepath);

        /// <summary>
        /// Gets the values of all the child nodes
        /// </summary>
        /// <param name="nodepath">The path to the node</param>
        /// <returns>The values of all the child nodes</returns>
        public string[] GetChildValues(string nodepath)
        {
            List<string> values = new List<string>();

            foreach (XmlNode node in GetChildNodes(nodepath))
                values.Add(node.InnerText);

            return values.ToArray();
        }

        /// <summary>
        /// Gets all the child nodes of a node
        /// </summary>
        /// <param name="nodepath">The path to the node</param>
        /// <returns>The list of child nodes of the node</returns>
        public XmlNodeList GetChildNodes(string nodepath) => RootNode.SelectSingleNode(nodepath).ChildNodes;

        /// <summary>
        /// Gets the attribute value of an attribute on a node
        /// </summary>
        /// <param name="nodepath">The path to the node</param>
        /// <param name="attribute">The name of the attribute</param>
        /// <returns>The attribute value on the node</returns>
        public string GetAttributeValue(string nodepath, string attribute) => GetAttribute(nodepath, attribute).InnerText;

        /// <summary>
        /// Gets the attribute on a node
        /// </summary>
        /// <param name="nodepath">The path to the node</param>
        /// <param name="attribute">The name of the attribute</param>
        /// <returns>The attribute on the node</returns>
        public XmlAttribute GetAttribute(string nodepath, string attribute) => GetAttributes(nodepath)[attribute];

        /// <summary>
        /// Gets all the attributes on a node
        /// </summary>
        /// <param name="nodepath">The path to the node</param>
        /// <returns>The list of the attributes on the node</returns>
        public XmlAttributeCollection GetAttributes(string nodepath) => GetNode(nodepath).Attributes;

        /// <summary>
        /// Sets the value of a node
        /// </summary>
        /// <param name="nodepath">The path to the node</param>
        /// <param name="value">The value to store in the node</param>
        public void SetValue(string nodepath, string value) => RootNode.SelectSingleNode(nodepath).InnerText = value;

        /// <summary>
        /// Adds a node to the parent node and optionally sets the value of the node
        /// </summary>
        /// <param name="parentnodepath">The path to the parent node</param>
        /// <param name="nodename">The name of the node</param>
        /// <param name="value">The value of the node</param>
        public void AddNode(string parentnodepath, string nodename, string value = "")
        {
            XmlNode node = Document.CreateElement(nodename);

            node.InnerText = value;

            RootNode.SelectSingleNode(parentnodepath).AppendChild(node);
        }

        /// <summary>
        /// Removes a node
        /// </summary>
        /// <param name="nodepath">The path to the node</param>
        public void RemoveNode(string nodepath) => Document.RemoveChild(RootNode.SelectSingleNode(nodepath));

        /// <summary>
        /// Checks if a node exists
        /// </summary>
        /// <param name="nodepath">The path to the node</param>
        /// <returns>If the node exists</returns>
        public bool CheckNode(string nodepath) => (RootNode.SelectSingleNode(nodepath) != null);
        #endregion
    }
}
