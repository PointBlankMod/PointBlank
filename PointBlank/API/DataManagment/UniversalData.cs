using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PT = System.IO.Path;

namespace PointBlank.API.DataManagment
{
    /// <summary>
    /// Universal data managment
    /// </summary>
    public class UniversalData
    {
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
        public XMLData XML { get; private set; }
        /// <summary>
        /// The JSON data(if the file is a JSON)
        /// </summary>
        public JsonData JSON { get; private set; }
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

            if (!Directory.Exists(PT.GetDirectoryName(Path)))
                Directory.CreateDirectory(PT.GetDirectoryName(Path));
            if (CreatedNew)
            {
                DataType = DefaultDataType; // Set the data type

                switch (DefaultDataType)
                {
                    case EDataType.JSON:
                        JSON = new JsonData(Path); // Create the JSON file
                        break;
                    case EDataType.XML:
                        XML = new XMLData(Path); // Create the XML file
                        break;
                }

                return; // No need to continue
            }

            if (XMLData.CheckFile(Path))
            {
                DataType = EDataType.XML; // An XML file
                XML = new XMLData(Path);
            }
            else if (JsonData.CheckFile(Path))
            {
                DataType = EDataType.JSON; // A JSON file
                JSON = new JsonData(Path);
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

            switch (DataType)
            {
                case EDataType.JSON:
                    if (ExtractType == EDataType.XML)
                        return JSONToXML(); // Convert the JSON to XML
                    return JSON; // Return the JSON
                case EDataType.XML:
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
            /*switch ((EDataType)Enviroment.FrameworkConfig[typeof(PointBlank)].Configurations["ConfigFormat"])
            {
                case EDataType.JSON:
                    if (DataType == EDataType.XML || XML != null)
                        XMLToJSON(); // Convert to JSON

                    JSON.Save(); // Save the JSON
                    break;
                case EDataType.XML:
                    if (DataType == EDataType.JSON || JSON != null)
                        JSONToXML(); // Convert to XML

                    XML.Save(); // Save the XML
                    break;
            }*/
            JSON.Save();
        }
        #endregion

        #region Converter Functions
        private XMLData JSONToXML()
        {
            XMLData tmpXML = new XMLData(JsonConvert.DeserializeXmlNode(JSON.Document.ToString(), "Data"), Path); // Deserialize the JSON to XML

            if(XML == null)
            {
                XML = tmpXML; // If the XML is null just set the tmpXML to it
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
                JSON = tmpJSON; // If the JSON is null just set the tmpJSON to it
                return JSON;
            }

            JSON.Document.Merge(tmpJSON); // Merge both JSONs

            return JSON;
        }
        #endregion
    }
}
