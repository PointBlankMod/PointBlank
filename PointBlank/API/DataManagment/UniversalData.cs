using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using PointBlank.Framework.Permissions.Ring;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PointBlank.API;
using PointBlank.Framework;

namespace PointBlank.API.DataManagment
{
    /// <summary>
    /// Universal data managment
    /// </summary>
    [RingPermission(SecurityAction.Demand, ring = RingPermissionRing.None)]
    public class UniversalData
    {
        #region Variables
        private XMLData _XML = null;
        private JsonData _JSON = null;
        #endregion

        #region Properties
        /// <summary>
        /// The path to the file(if linked to file)
        /// </summary>
        public string Path { get; private set; }
        /// <summary>
        /// The type of data that was inputted(SQL, XML, JSON)
        /// </summary>
        public EDataType DataType { get; private set; }

        /// <summary>
        /// The XML data(if the file is a XML)
        /// </summary>
        public XMLData XML
        {
            get
            {
                if (DataType != EDataType.XML)
                    return GetData(EDataType.XML) as XMLData;
                return _XML;
            }
        }
        /// <summary>
        /// The JSON data(if the file is a JSON)
        /// </summary>
        public JsonData JSON
        {
            get
            {
                if (DataType != EDataType.JSON)
                    return GetData(EDataType.JSON) as JsonData;
                return _JSON;
            }
        }
        /// <summary>
        /// The CONF data(if the file is a CONF)
        /// </summary>
        //public ConfData CONF { get; private set; }

        /// <summary>
        /// Check if a new file was created
        /// </summary>
        public bool CreatedNew { get; private set; }
        #endregion

        /// <summary>
        /// Universal data managment
        /// </summary>
        /// <param name="path">The path to the data file</param>
        public UniversalData(string path, EDataType DefaultDataType = EDataType.JSON)
        {
            this.Path = path + ".dat"; // Set the path
            this.CreatedNew = !File.Exists(Path); // Check if the file has to be created

            if (CreatedNew)
            {
                DataType = DefaultDataType; // Set the data type

                if(DefaultDataType == EDataType.JSON)
                {
                    _JSON = new JsonData(Path); // Create the JSON file
                }
                else if(DefaultDataType == EDataType.XML)
                {
                    _XML = new XMLData(Path); // Create the XML file
                }

                return; // No need to continue
            }

            if (XMLData.CheckFile(Path))
            {
                DataType = EDataType.XML; // An XML file
                _XML = new XMLData(Path);
            }
            else if (JsonData.CheckFile(Path))
            {
                DataType = EDataType.JSON; // A JSON file
                _JSON = new JsonData(Path);
            }
            else
            {
                DataType = EDataType.UNKNOWN; // Unknown type/corrupted file
            }
        }

        #region Public Functions
        /// <summary>
        /// Gets the data in the format of your choosing
        /// </summary>
        /// <param name="ExtractType">The format of the data</param>
        /// <returns>The data in the format you chose</returns>
        public object GetData(EDataType ExtractType)
        {
            if (DataType == EDataType.UNKNOWN || ExtractType == EDataType.UNKNOWN) // Unknown data type error
                return null;

            if(DataType == EDataType.JSON) // If the data type is JSON
            {
                if (ExtractType == EDataType.XML)
                    return JSONToXML(); // Convert the JSON to XML
                return JSON; // Return the JSON
            }
            if(DataType == EDataType.XML) // If the data type is XML
            {
                if (ExtractType == EDataType.JSON)
                    return XMLToJSON(); // Convert the XML to JSON
                return XML; // Return the XML
            }

            return null;
        }

        /// <summary>
        /// Saves the data depending on the format the server chose
        /// </summary>
        public void Save()
        {
            if(Configuration.SaveDataType == EDataType.JSON)
            {
                if (DataType == EDataType.XML || XML != null)
                    XMLToJSON(); // Convert to JSON

                JSON.Save(); // Save the JSON
            }
            else if(Configuration.SaveDataType == EDataType.XML)
            {
                if (DataType == EDataType.JSON || JSON != null)
                    JSONToXML(); // Convert to XML

                XML.Save(); // Save the XML
            }
        }
        #endregion

        #region Converter Functions
        private XMLData JSONToXML()
        {
            XMLData tmpXML = new XMLData(JsonConvert.DeserializeXmlNode(JSON.Document.ToString(), "Data"), Path); // Deserialize the JSON to XML

            if(XML == null)
            {
                _XML = tmpXML; // If the XML is null just set the tmpXML to it
                return XML;
            }

            XML.Merge(tmpXML.Document); // Merge the XMLs

            return XML;
        }

        private JsonData XMLToJSON()
        {
            string sJson = JsonConvert.SerializeXmlNode(XML.Document, Formatting.None, true); // Deserialize the XML to JSON
            JsonData tmpJSON = null;

            if (!string.IsNullOrEmpty(sJson) && sJson != "null")
                tmpJSON = new JsonData(JObject.Parse(sJson), Path); // Parse the JSON
            else
                tmpJSON = new JsonData(Path); // Create a new JSON
            if(JSON == null)
            {
                _JSON = tmpJSON; // If the JSON is null just set the tmpJSON to it
                return JSON;
            }

            JSON.Document.Merge(tmpJSON); // Merge both JSONs

            return JSON;
        }
        #endregion
    }
}
