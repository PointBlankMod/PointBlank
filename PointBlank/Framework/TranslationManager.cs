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
                CommandTranslations.Base_InvalidPlayer = "Invalid player or player ID!";

                CommandTranslations.Admin_Help = "This adds the specified player to the list of users allowed to use admin commands in the chat";
                CommandTranslations.Admin_Set = "{0} has been set as admin!";
                CommandTranslations.Admin_Usage = " <player/id>";

                CommandTranslations.Admins_Help = "This shows a list of the current admins.";
                CommandTranslations.Admins_Hidden = "Admins are hidden for this server.";
                CommandTranslations.Admins_List = "Admins: ";

                CommandTranslations.Airdrop_Help = "This forces a dropship to fly over and perform an airdrop.";
                CommandTranslations.Airdrop_Success = "Airdrop spawned successfully!";

                CommandTranslations.Ban_Help = "This adds the specified player to the list of users not allowed to join the server.";
                CommandTranslations.Ban_Usage = " <player> [duration] [reason]";
                CommandTranslations.Ban_Reason = "Undefined";
                CommandTranslations.Ban_Success = "{0} has been banned successfully!";

                CommandTranslations.Bans_Help = "This shows a list of the current bans.";
                CommandTranslations.Bans_List = "Bans: ";

                CommandTranslations.Bind_Help = "This binds a specific internal IP to the socket.";
                CommandTranslations.Bind_InvalidIP = "The IP specified is invalid!";
                CommandTranslations.Bind_Usage = " <IP>";

                CommandTranslations.Camera_Help = "This assigns the perspective of the server.";
                CommandTranslations.Camera_Usage = " <first/third/both/vehicle>";
                CommandTranslations.Camera_SetTo = "Allowed camera mode set to {0}.";

                CommandTranslations.Chatrate_Help = "This assigns a minimum time between chat messages.";
                CommandTranslations.Chatrate_Usage = " <rate>";
                CommandTranslations.Chatrate_Invalid = "The rate specified is invalid!";
                CommandTranslations.Chatrate_SetTo = "Server chat rate set to {0}.";
                CommandTranslations.Chatrate_TooHigh = "The chat rate cannot be set higher than {0}.";
                CommandTranslations.Chatrate_TooLow = "The chat rate cannot be set lower than {0}.";

                CommandTranslations.Cheats_Help = "This allows the use of cheats like spawning items, vehicles and experience.";
                CommandTranslations.Cheats_Enabled = "Cheats are now enabled for this server.";

                Save();
            }
            else
            {
                CommandTranslations.Base_InvalidPlayer = (string)JCommand.Document["Base_InvalidPlayer"];

                CommandTranslations.Admin_Help = (string)JCommand.Document["Admin_Help"];
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
            SetCommand("Base_InvalidPlayer", CommandTranslations.Base_InvalidPlayer);

            SetCommand("Admin_Help", CommandTranslations.Admin_Help);
            SetCommand("Admin_Set", CommandTranslations.Admin_Set);
            SetCommand("Admin_Usage", CommandTranslations.Admin_Usage);

            UniCommand.Save();
        }
        #endregion
    }
}
