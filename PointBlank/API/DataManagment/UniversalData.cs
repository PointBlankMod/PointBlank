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
        /// The type of data that was inputted(SQL, XML, Json)
        /// </summary>
        public EDataType DataType { get; private set; }

        /// <summary>
        /// The XML data(if the file is a XML)
        /// </summary>
        public XmlData Xml { get; private set; }
        /// <summary>
        /// The Json data(if the file is a Json)
        /// </summary>
        public JsonData Json { get; private set; }
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
        public UniversalData(string path, EDataType defaultDataType = EDataType.Json)
        {
            this.Path = path + ".dat"; // Set the path
            this.CreatedNew = !File.Exists(Path); // Check if the file has to be created

            if (!Directory.Exists(PT.GetDirectoryName(Path)))
                Directory.CreateDirectory(PT.GetDirectoryName(Path));
            if (CreatedNew)
            {
                DataType = defaultDataType; // Set the data type

                switch (defaultDataType)
                {
                    case EDataType.Json:
                        Json = new JsonData(Path); // Create the Json file
                        break;
                    case EDataType.Xml:
                        Xml = new XmlData(Path); // Create the XML file
                        break;
                }

                return; // No need to continue
            }

            if (XmlData.CheckFile(Path))
            {
                DataType = EDataType.Xml; // An XML file
                Xml = new XmlData(Path);
            }
            else if (JsonData.CheckFile(Path))
            {
                DataType = EDataType.Json; // A Json file
                Json = new JsonData(Path);
            }
            else
            {
                DataType = EDataType.Unknown; // Unknown type/corrupted file
            }
        }

        #region Public Functions
        /// <summary>
        /// Gets the data in the format of your choosing
        /// </summary>
        /// <param name="extractType">The format of the data</param>
        /// <returns>The data in the format you chose</returns>
        public object GetData(EDataType extractType)
        {
            if (DataType == EDataType.Unknown || extractType == EDataType.Unknown) // Unknown data type error
                return null;

            switch (DataType)
            {
                case EDataType.Json:
                    if (extractType == EDataType.Xml)
                        return JsontoXml(); // Convert the Json to XML
                    return Json; // Return the Json
                case EDataType.Xml:
                    if (extractType == EDataType.Json)
                        return XmltoJson(); // Convert the XML to Json
                    return Xml; // Return the XML
            }

            return null;
        }

        /// <summary>
        /// Saves the data depending on the format the server chose
        /// </summary>
        public void Save()
        {
            /*switch ((EDataType)PointBlankEnvironment.FrameworkConfig[typeof(PointBlank)].Configurations["ConfigFormat"])
            {
                case EDataType.Json:
                    if (DataType == EDataType.XML || XML != null)
                        XMLToJson(); // Convert to Json

                    Json.Save(); // Save the Json
                    break;
                case EDataType.XML:
                    if (DataType == EDataType.Json || Json != null)
                        JsonToXML(); // Convert to XML

                    XML.Save(); // Save the XML
                    break;
            }*/
            Json.Save();
        }
        #endregion

        #region Converter Functions
        private XmlData JsontoXml()
        {
            XmlData tmpXml = new XmlData(JsonConvert.DeserializeXmlNode(Json.Document.ToString(), "Data"), Path); // Deserialize the Json to XML

            if(Xml == null)
            {
                Xml = tmpXml; // If the XML is null just set the tmpXML to it
                return Xml;
            }

            Xml.Merge(tmpXml.Document); // Merge the XMLs

            return Xml;
        }

        private JsonData XmltoJson()
        {
            string sJson = JsonConvert.SerializeXmlNode(Xml.Document, Formatting.None, true); // Deserialize the XML to Json
            JsonData tmpJson = null;

            if (!string.IsNullOrEmpty(sJson) && sJson != "null")
                tmpJson = new JsonData(JObject.Parse(sJson), Path); // Parse the Json
            else
                tmpJson = new JsonData(Path); // Create a new Json
            if(Json == null)
            {
                Json = tmpJson; // If the Json is null just set the tmpJson to it
                return Json;
            }

            Json.Document.Merge(tmpJson); // Merge both Jsons

            return Json;
        }
        #endregion
    }
}
