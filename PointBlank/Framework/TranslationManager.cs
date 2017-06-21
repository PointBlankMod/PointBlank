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
                CommandTranslations.Base_NoArenaTime = "Cannot change time in arena!";
                CommandTranslations.Base_NoHordeTime = "Cannot change time in horde!";

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

                CommandTranslations.Cycle_Help = "This assigns the length of the day/night cycle in seconds.";
                CommandTranslations.Cycle_Usage = " <cycle>";
                CommandTranslations.Cycle_Invalid = "Invalid number of cycles!";
                CommandTranslations.Cycle_SetTo = "Server cycle set to {0}.";

                CommandTranslations.Day_Help = "This assigns the current time to day.";
                CommandTranslations.Day_Set = "Successfully set time to day!";

                CommandTranslations.Debug_Help = "This provides information on the state of the server.";
                CommandTranslations.Debug_Title = "Debug Information";
                CommandTranslations.Debug_SteamID = "SteamID: {0}";
                CommandTranslations.Debug_IP = "IP: {0}";
                CommandTranslations.Debug_Port = "Port: {0}";
                CommandTranslations.Debug_BytesSent = "Bytes sent: {0}B";
                CommandTranslations.Debug_BytesReceived = "Bytes received: {0}B";
                CommandTranslations.Debug_ABytesSent = "Average bytes sent: {0}B";
                CommandTranslations.Debug_ABytesReceived = "Average bytes received: {0}B";
                CommandTranslations.Debug_PacketsSent = "Packets sent: {0}";
                CommandTranslations.Debug_PacketsReceived = "Packets received: {0}";
                CommandTranslations.Debug_APacketsSent = "Average packets sent: {0}";
                CommandTranslations.Debug_APacketsReceived = "Average packets received: {0}";
                CommandTranslations.Debug_UPS = "Updates per second: {0}";
                CommandTranslations.Debug_TPS = "Ticks per second: {0}";
                CommandTranslations.Debug_Zombies = "Zombie count: {0}";
                CommandTranslations.Debug_Animals = "Animal count: {0}";

                CommandTranslations.Experience_Help = "This gives a player some experience.";
                CommandTranslations.Experience_Usage = " <amount> [player]";
                CommandTranslations.Experience_Invalid = "Invalid number of experience!";
                CommandTranslations.Experience_Give = "Gave {0} {1} experience!";

                CommandTranslations.Filter_Enable = "Succesfully enabled name filter!";
                CommandTranslations.Filter_Help = "This filters out players with non-English-alphanumeric names.";

                Save();
            }
            else
            {
                CommandTranslations.Base_InvalidPlayer = (string)JCommand.Document["Base_InvalidPlayer"];
                CommandTranslations.Base_InvalidPlayer = (string)JCommand.Document["Base_InvalidPlayer"];
                CommandTranslations.Base_InvalidPlayer = (string)JCommand.Document["Base_InvalidPlayer"];

                CommandTranslations.Admin_Help = (string)JCommand.Document["Admin_Help"];
                CommandTranslations.Admin_Set = (string)JCommand.Document["Admin_Set"];
                CommandTranslations.Admin_Usage = (string)JCommand.Document["Admin_Usage"];

                CommandTranslations.Admins_Help = (string)JCommand.Document["Admins_Help"];
                CommandTranslations.Admins_Hidden = (string)JCommand.Document["Admins_Hidden"];
                CommandTranslations.Admins_List = (string)JCommand.Document["Admins_List"];

                CommandTranslations.Airdrop_Help = (string)JCommand.Document["Airdrop_Help"];
                CommandTranslations.Airdrop_Success = (string)JCommand.Document["Airdrop_Success"];

                CommandTranslations.Ban_Help = (string)JCommand.Document["Ban_Help"];
                CommandTranslations.Ban_Usage = (string)JCommand.Document["Ban_Usage"];
                CommandTranslations.Ban_Reason = (string)JCommand.Document["Ban_Reason"];
                CommandTranslations.Ban_Success = (string)JCommand.Document["Ban_Success"];

                CommandTranslations.Bans_Help = (string)JCommand.Document["Bans_Help"];
                CommandTranslations.Bans_List = (string)JCommand.Document["Bans_List"];

                CommandTranslations.Bind_Help = (string)JCommand.Document["Bind_Help"];
                CommandTranslations.Bind_InvalidIP = (string)JCommand.Document["Bind_InvalidIP"];
                CommandTranslations.Bind_Usage = (string)JCommand.Document["Bind_Usage"];

                CommandTranslations.Camera_Help = (string)JCommand.Document["Camera_Help"];
                CommandTranslations.Camera_Usage = (string)JCommand.Document["Camera_Usage"];
                CommandTranslations.Camera_SetTo = (string)JCommand.Document["Camera_SetTo"];

                CommandTranslations.Chatrate_Help = (string)JCommand.Document["Chatrate_Help"];
                CommandTranslations.Chatrate_Usage = (string)JCommand.Document["Chatrate_Usage"];
                CommandTranslations.Chatrate_Invalid = (string)JCommand.Document["Chatrate_Invalid"];
                CommandTranslations.Chatrate_SetTo = (string)JCommand.Document["Chatrate_SetTo"];
                CommandTranslations.Chatrate_TooHigh = (string)JCommand.Document["Chatrate_TooHigh"];
                CommandTranslations.Chatrate_TooLow = (string)JCommand.Document["Chatrate_TooLow"];

                CommandTranslations.Cheats_Help = (string)JCommand.Document["Cheats_Help"];
                CommandTranslations.Cheats_Enabled = (string)JCommand.Document["Cheats_Enabled"];

                CommandTranslations.Cycle_Help = (string)JCommand.Document["Cycle_Help"];
                CommandTranslations.Cycle_Usage = (string)JCommand.Document["Cycle_Usage"];
                CommandTranslations.Cycle_Invalid = (string)JCommand.Document["Cycle_Invalid"];
                CommandTranslations.Cycle_SetTo = (string)JCommand.Document["Cycle_SetTo"];

                CommandTranslations.Day_Help = (string)JCommand.Document["Day_Help"];
                CommandTranslations.Day_Set = (string)JCommand.Document["Day_Set"];

                CommandTranslations.Debug_Help = (string)JCommand.Document["Debug_Help"];
                CommandTranslations.Debug_Title = (string)JCommand.Document["Debug_Title"];
                CommandTranslations.Debug_SteamID = (string)JCommand.Document["Debug_SteamID"];
                CommandTranslations.Debug_IP = (string)JCommand.Document["Debug_IP"];
                CommandTranslations.Debug_Port = (string)JCommand.Document["Debug_Port"];
                CommandTranslations.Debug_BytesSent = (string)JCommand.Document["Debug_BytesSent"];
                CommandTranslations.Debug_BytesReceived = (string)JCommand.Document["Debug_BytesReceived"];
                CommandTranslations.Debug_ABytesSent = (string)JCommand.Document["Debug_ABytesSent"];
                CommandTranslations.Debug_ABytesReceived = (string)JCommand.Document["Debug_ABytesReceived"];
                CommandTranslations.Debug_PacketsSent = (string)JCommand.Document["Debug_PacketsSent"];
                CommandTranslations.Debug_PacketsReceived = (string)JCommand.Document["Debug_PacketsReceived"];
                CommandTranslations.Debug_APacketsSent = (string)JCommand.Document["Debug_APacketsSent"];
                CommandTranslations.Debug_APacketsReceived = (string)JCommand.Document["Debug_APacketsReceived"];
                CommandTranslations.Debug_UPS = (string)JCommand.Document["Debug_UPS"];
                CommandTranslations.Debug_TPS = (string)JCommand.Document["Debug_TPS"];
                CommandTranslations.Debug_Zombies = (string)JCommand.Document["Debug_Zombies"];
                CommandTranslations.Debug_Animals = (string)JCommand.Document["Debug_Animals"];

                CommandTranslations.Experience_Help = (string)JCommand.Document["Experience_Help"];
                CommandTranslations.Experience_Usage = (string)JCommand.Document["Experience_Usage"];
                CommandTranslations.Experience_Invalid = (string)JCommand.Document["Experience_Invalid"];
                CommandTranslations.Experience_Give = (string)JCommand.Document["Experience_Give"];

                CommandTranslations.Filter_Enable = (string)JCommand.Document["Filter_Enable"];
                CommandTranslations.Filter_Help = (string)JCommand.Document["Filter_Help"];
            }
        }

        public static void Reload()
        {
            Load();
        }

        public static void Save()
        {
            SetCommand("Base_InvalidPlayer", CommandTranslations.Base_InvalidPlayer);
            SetCommand("Base_InvalidPlayer", CommandTranslations.Base_InvalidPlayer);
            SetCommand("Base_InvalidPlayer", CommandTranslations.Base_InvalidPlayer);

            SetCommand("Admin_Help", CommandTranslations.Admin_Help);
            SetCommand("Admin_Set", CommandTranslations.Admin_Set);
            SetCommand("Admin_Usage", CommandTranslations.Admin_Usage);

            SetCommand("Admins_Help", CommandTranslations.Admins_Help);
            SetCommand("Admins_Hidden", CommandTranslations.Admins_Hidden);
            SetCommand("Admins_List", CommandTranslations.Admins_List);

            SetCommand("Airdrop_Help", CommandTranslations.Airdrop_Help);
            SetCommand("Airdrop_Success", CommandTranslations.Airdrop_Success);

            SetCommand("Ban_Help", CommandTranslations.Ban_Help);
            SetCommand("Ban_Usage", CommandTranslations.Ban_Usage);
            SetCommand("Ban_Reason", CommandTranslations.Ban_Reason);
            SetCommand("Ban_Success", CommandTranslations.Ban_Success);

            SetCommand("Bans_Help", CommandTranslations.Bans_Help);
            SetCommand("Bans_List", CommandTranslations.Bans_List);

            SetCommand("Bind_Help", CommandTranslations.Bind_Help);
            SetCommand("Bind_InvalidIP", CommandTranslations.Bind_InvalidIP);
            SetCommand("Bind_Usage", CommandTranslations.Bind_Usage);

            SetCommand("Camera_Help", CommandTranslations.Camera_Help);
            SetCommand("Camera_Usage", CommandTranslations.Camera_Usage);
            SetCommand("Camera_SetTo", CommandTranslations.Camera_SetTo);

            SetCommand("Chatrate_Help", CommandTranslations.Chatrate_Help);
            SetCommand("Chatrate_Usage", CommandTranslations.Chatrate_Usage);
            SetCommand("Chatrate_Invalid", CommandTranslations.Chatrate_Invalid);
            SetCommand("Chatrate_SetTo", CommandTranslations.Chatrate_SetTo);
            SetCommand("Chatrate_TooHigh", CommandTranslations.Chatrate_TooHigh);
            SetCommand("Chatrate_TooLow", CommandTranslations.Chatrate_TooLow);

            SetCommand("Cheats_Help", CommandTranslations.Cheats_Help);
            SetCommand("Cheats_Enabled", CommandTranslations.Cheats_Enabled);

            SetCommand("Cycle_Help", CommandTranslations.Cycle_Help);
            SetCommand("Cycle_Usage", CommandTranslations.Cycle_Usage);
            SetCommand("Cycle_Invalid", CommandTranslations.Cycle_Invalid);
            SetCommand("Cycle_SetTo", CommandTranslations.Cycle_SetTo);

            SetCommand("Day_Help", CommandTranslations.Day_Help);
            SetCommand("Day_Set", CommandTranslations.Day_Set);

            SetCommand("Debug_Help", CommandTranslations.Debug_Help);
            SetCommand("Debug_Title", CommandTranslations.Debug_Title);
            SetCommand("Debug_SteamID", CommandTranslations.Debug_SteamID);
            SetCommand("Debug_IP", CommandTranslations.Debug_IP);
            SetCommand("Debug_Port", CommandTranslations.Debug_Port);
            SetCommand("Debug_BytesSent", CommandTranslations.Debug_BytesSent);
            SetCommand("Debug_BytesReceived", CommandTranslations.Debug_BytesReceived);
            SetCommand("Debug_ABytesSent", CommandTranslations.Debug_ABytesSent);
            SetCommand("Debug_ABytesReceived", CommandTranslations.Debug_ABytesReceived);
            SetCommand("Debug_PacketsSent", CommandTranslations.Debug_PacketsSent);
            SetCommand("Debug_PacketsReceived", CommandTranslations.Debug_PacketsReceived);
            SetCommand("Debug_APacketsSent", CommandTranslations.Debug_APacketsSent);
            SetCommand("Debug_APacketsReceived", CommandTranslations.Debug_APacketsReceived);
            SetCommand("Debug_UPS", CommandTranslations.Debug_UPS);
            SetCommand("Debug_TPS", CommandTranslations.Debug_TPS);
            SetCommand("Debug_Zombies", CommandTranslations.Debug_Zombies);
            SetCommand("Debug_Animals", CommandTranslations.Debug_Animals);

            SetCommand("Experience_Help", CommandTranslations.Experience_Help);
            SetCommand("Experience_Usage", CommandTranslations.Experience_Usage);
            SetCommand("Experience_Invalid", CommandTranslations.Experience_Invalid);
            SetCommand("Experience_Give", CommandTranslations.Experience_Give);

            SetCommand("Filter_Enable", CommandTranslations.Filter_Enable);
            SetCommand("Filter_Help", CommandTranslations.Filter_Help);

            UniCommand.Save();
        }
        #endregion
    }
}
