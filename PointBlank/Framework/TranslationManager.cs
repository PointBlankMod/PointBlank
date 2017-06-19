using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.Framework.Translations;
using PointBlank.API;
using PointBlank.API.DataManagment;

namespace PointBlank.Framework
{
    internal static class TranslationManager
    {
        #region Info
        public static readonly string CommandPath = ServerInfo.TranslationsPath + "/Commands";
        public static readonly string PointBlankPath = ServerInfo.TranslationsPath + "/PointBlank";
        public static readonly string ServicePath = ServerInfo.TranslationsPath + "/Services";
        #endregion

        #region Properties
        public static UniversalData UniCommand { get; private set; }
        public static UniversalData UniPointBlank { get; private set; }
        public static UniversalData UniService { get; private set; }

        public static JsonData JCommand { get; private set; }
        public static JsonData JPointBlank { get; private set; }
        public static JsonData JService { get; private set; }
        #endregion

        static TranslationManager()
        {
            // Set universal data
            UniCommand = new UniversalData(CommandPath);
            UniPointBlank = new UniversalData(PointBlankPath);
            UniService = new UniversalData(ServicePath);

            // Set the json data
            JCommand = UniCommand.GetData(EDataType.JSON) as JsonData;
            JPointBlank = UniPointBlank.GetData(EDataType.JSON) as JsonData;
            JService = UniService.GetData(EDataType.JSON) as JsonData;
        }

        #region Private Functions
        private static void SetCommand(string key, string val)
        {
            if (JCommand.Document[key] != null)
                JCommand.Document[key] = val;
            else
                JCommand.Document.Add(key, val);
        }
        #endregion

        #region Functions
        public static void Load()
        {
            if (UniCommand.CreatedNew)
            {
                CommandTranslations.Admin_Help = "This adds the specified player to the list of users allowed to use admin commands in the chat";
                CommandTranslations.Admin_InvalidPlayer = "Invalid player or player ID!";
                CommandTranslations.Admin_Set = "{0} has been set as admin!";
                CommandTranslations.Admin_Usage = " <player/id>";

                Save();
            }
            else
            {
                CommandTranslations.Admin_Help = (string)JCommand.Document["Admin_Help"];
                CommandTranslations.Admin_InvalidPlayer = (string)JCommand.Document["Admin_InvalidPlayer"];
                CommandTranslations.Admin_Set = (string)JCommand.Document["Admin_Set"];
                CommandTranslations.Admin_Usage = (string)JCommand.Document["Admin_Usage"];
            }
        }

        public static void Reload()
        {
            Load();
        }

        public static void Save()
        {
            SetCommand("Admin_Help", CommandTranslations.Admin_Help);
            SetCommand("Admin_InvalidPlayer", CommandTranslations.Admin_InvalidPlayer);
            SetCommand("Admin_Set", CommandTranslations.Admin_Set);
            SetCommand("Admin_Usage", CommandTranslations.Admin_Usage);

            UniCommand.Save();
        }
        #endregion
    }
}
