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
        #endregion

        #region Properties
        public static UniversalData UniCommand { get; private set; }

        public static JsonData JCommand { get; private set; }
        #endregion

        static TranslationManager()
        {
            // Set universal data
            UniCommand = new UniversalData(CommandPath);

            // Set the json data
            JCommand = UniCommand.GetData(EDataType.JSON) as JsonData;
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
            #region Command Translations
            if (UniCommand.CreatedNew)
            {
                CommandTranslations.Base_InvalidPlayer = "Invalid player or player ID!";
                CommandTranslations.Base_NoArenaTime = "Cannot change time in arena!";
                CommandTranslations.Base_NoHordeTime = "Cannot change time in horde!";
                CommandTranslations.Base_CommandInvalid = "The specified command is invalid!";

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

                CommandTranslations.Flag_Help = "This sets a player's flag.";
                CommandTranslations.Flag_Usage = " <flag> <value> [player]";
                CommandTranslations.Flag_InvalidFlag = "Selected flag ID is invalid!";
                CommandTranslations.Flag_InvalidValue = "Selected flag value is invalid!";
                CommandTranslations.Flag_Set = "The flag for {0} has been set!";

                CommandTranslations.GameMode_Help = "This assigns the game mode of the server.";
                CommandTranslations.GameMode_Usage = " <game mode>";
                CommandTranslations.GameMode_Set = "Server game mode set to {0}!";

                CommandTranslations.Gold_Help = "This marks the server as joinable only by Gold members.";
                CommandTranslations.Gold_Set = "Server has been set to gold only!";

                CommandTranslations.Help_Help = "Displayes all enabled commands or information on a specific command";
                CommandTranslations.Help_Usage = " [command]";

                CommandTranslations.HideAdmins_Help = "This allows the recording of cinematic footage without the admin labels visible to players.";
                CommandTranslations.HideAdmins_Set = "Admin labels are now hidden!";

                CommandTranslations.Item_Help = "This gives the player an item.";
                CommandTranslations.Item_Usage = " <item> [amount] [player]";
                CommandTranslations.Item_Invalid = "Failed to find the specified item!";
                CommandTranslations.Item_Fail = "Failed to give the specified item to player!";
                CommandTranslations.Item_Give = "{0} has been given to {1}";

                CommandTranslations.Kick_Help = "This disconnects the specified player from the server.";
                CommandTranslations.Kick_Usage = " <player> [reason]";
                CommandTranslations.Kick_Reason = "Undefined";
                CommandTranslations.Kick_Kicked = "{0} has been kicked from the server!";

                CommandTranslations.Kill_Help = "This kills the specified player in-game.";
                CommandTranslations.Kill_Usage = " [player]";
                CommandTranslations.Kill_Killed = "{0} has been killed!";

                CommandTranslations.Log_Help = "This assigns the console log options.";
                CommandTranslations.Log_Usage = " <chat(y/n)> <join/leave(y/n)> <deaths(y/n)> <anticheat(y/n)>";
                CommandTranslations.Log_Set = "Console logging settings have been set!";

                CommandTranslations.Map_Help = "This sets the map that the server loads on startup.";
                CommandTranslations.Map_Usage = " <map>";
                CommandTranslations.Map_Invalid = "The specified map is invalid!";
                CommandTranslations.Map_Set = "Server map has been set to {0}";

                CommandTranslations.MaxPlayers_Help = "This sets the maximum number of connections the server is willing to accept.";
                CommandTranslations.MaxPlayers_Usage = " <max players>";
                CommandTranslations.MaxPlayers_Invalid = "Invalid number of players!";
                CommandTranslations.MaxPlayers_TooHigh = "Max player count cannot be higher than {0}!";
                CommandTranslations.MaxPlayers_TooLow = "Max player count cannot be lower than {0}!";
                CommandTranslations.MaxPlayers_Set = "Max players set to {0}!";

                CommandTranslations.Mode_Help = "This assigns the difficulty of the server.";
                CommandTranslations.Mode_Usage = " <difficulty(easy/normal/hard)>";
                CommandTranslations.Mode_Invalid = "The selected difficulty is invalid!";
                CommandTranslations.Mode_Set = "The server difficulty has been set to {0}!";

                CommandTranslations.Modules_Help = "This shows a list of the loaded modules.";
                CommandTranslations.Modules_Title = "Module list";
                CommandTranslations.Modules_Name = "Name: {0}";
                CommandTranslations.Modules_Version = "Version: {0}";

                CommandTranslations.Name_Help = "This assigns the name of the server on the server list.";
                CommandTranslations.Name_Usage = " <name>";
                CommandTranslations.Name_TooLong = "The server name cannot be longer than {0}!";
                CommandTranslations.Name_TooShort = "The server name cannot be shorter than {0}!";
                CommandTranslations.Name_Set = "The server name has been set to {0}";

                CommandTranslations.Night_Help = "This assigns the current time to night.";
                CommandTranslations.Night_Set = "The server time has been changed to night!";

                CommandTranslations.Owner_Help = "This sets the owner of the server.";
                CommandTranslations.Owner_Usage = " <owner>";
                CommandTranslations.Owner_Set = "The server owner has been set to {0}!";

                CommandTranslations.Password_Help = "This assigns the codeword required for entry to the server.";
                CommandTranslations.Password_Usage = " <password>";
                CommandTranslations.Password_Removed = "The password has been removed from the server!";
                CommandTranslations.Password_Set = "The server password has been set to {0}!";

                CommandTranslations.Permit_Help = "This adds the specified player to the list of users allowed to join the server.";
                CommandTranslations.Permit_Usage = " <player> <tag>";
                CommandTranslations.Permit_Added = "{0} has been added to the server whitelist!";

                CommandTranslations.Permits_Help = "This shows a list of the current players allowed to join the server.";
                CommandTranslations.Permits_List = "Permits: {0}";

                CommandTranslations.Port_Help = "This assigns the port of the server. Port + 1 and port + 2 are also used, so remember to open them on the router as well.";
                CommandTranslations.Port_Usage = " <port>";
                CommandTranslations.Port_Invalid = "The specified port is invalid!";
                CommandTranslations.Port_Set = "The server port has been set to {0}!";

                CommandTranslations.PvE_Help = "This allows player versus environment combat.";
                CommandTranslations.PvE_Set = "The server has been now set to PvE!";

                CommandTranslations.Quest_Help = "This gives a player a flag.";
                CommandTranslations.Quest_Usage = " <quest> [player]";
                CommandTranslations.Quest_Invalid = "The specified quest ID is invalid!";
                CommandTranslations.Quest_Added = "Quest {0} has been added to {1}!";

                CommandTranslations.Queue_Help = "This sets the maximum number of queued connections the server is willing to hold on to.";
                CommandTranslations.Queue_Usage = " <queue>";
                CommandTranslations.Queue_Invalid = "The specified queue is invalid!";
                CommandTranslations.Queue_TooHigh = "The queue number cannot be higher than {0}!";
                CommandTranslations.Queue_Set = "The server queue has been set to {0}!";

                CommandTranslations.Reputation_Help = "This gives a player some reputation.";
                CommandTranslations.Reputation_Usage = " <reputation> [player]";
                CommandTranslations.Reputation_Invalid = "The specified amount of reputation is invalid!";
                CommandTranslations.Reputation_Give = "{0} has been given {1} reputation!";

                CommandTranslations.ResetConfig_Help = "This resets the config file to the default values.";
                CommandTranslations.ResetConfig_Reset = "The server configuration has been reset!";

                CommandTranslations.Save_Help = "This forces a proper save of the server state.";
                CommandTranslations.Save_Save = "The server state has been successfully saved!";

                CommandTranslations.Say_Help = "This broadcasts a message to all of the connected clients.";
                CommandTranslations.Say_Usage = " <message> [red] [green] [blue]";

                CommandTranslations.Shutdown_Help = "This properly saves the server state, disconnects the clients and closes the program.";
                CommandTranslations.Shutdown_Usage = " [seconds until shutdown]";

                CommandTranslations.Slay_Help = "This kills the specified player in-game and permanently bans them.";
                CommandTranslations.Slay_Usage = " <player> [reason]";
                CommandTranslations.Slay_Slay = "{0} has been slayed!";

                CommandTranslations.Spy_Help = "This requests a screenshot from the target player and saves it on the caller's computer as Spy.jpg.";
                CommandTranslations.Spy_Usage = " <player>";
                CommandTranslations.Spy_Spy = "Successfully spyed {0}";

                CommandTranslations.Storm_Help = "This starts or stops the current rain cycle.";
                CommandTranslations.Storm_Change = "Weather has been chaged successfully!";

                CommandTranslations.Sync_Help = "This allows players to share savedata between your servers.";
                CommandTranslations.Sync_Sync = "The server sync has been enabled!";

                CommandTranslations.Teleport_Help = "This teleports the first player to the second or a location.";
                CommandTranslations.Teleport_Usage = " <target/node> [player]";
                CommandTranslations.Teleport_Invalid = "The specified teleport location is invalid!";
                CommandTranslations.Teleport_Teleport = "{0} has been teleported to {1}";

                CommandTranslations.Time_Help = "This assigns the current time in seconds of the day/night cycle.";
                CommandTranslations.Time_Usage = " <time>";
                CommandTranslations.Time_Invalid = "The specified time is invalid!";
                CommandTranslations.Time_Set = "The server time has been set to {0}";

                CommandTranslations.Timeout_Help = "This assigns a maximum ping threshhold to the server before a client is kicked.";
                CommandTranslations.Timeout_Usage = " <timeout>";
                CommandTranslations.Timeout_Invalid = "The specified timeout is invalid!";
                CommandTranslations.Timeout_TooHigh = "The timeout cannot be higher than {0}";
                CommandTranslations.Timeout_TooLow = "The timeout cannot be lower than {0}";
                CommandTranslations.Timeout_Set = "The server timeout has been set to {0}";

                CommandTranslations.Unadmin_Help = "This removes the specified player from the list of users allowed to use admin commands in the chat.";
                CommandTranslations.Unadmin_Usage = " <player>";
                CommandTranslations.Unadmin_Unadmin = "{0} is no longer admin!";

                CommandTranslations.Unban_Help = "This removes the specified player from the list of users not allowed to join the server.";
                CommandTranslations.Unban_Usage = " <player>";
                CommandTranslations.Unban_NotBanned = "{0} is not on the ban list!";
                CommandTranslations.Unban_Unban = "{0} is no longer banned!";

                CommandTranslations.Unpermit_Help = "This removes the specified player from the list of users allowed to join the server.";
                CommandTranslations.Unpermit_Usage = " <player>";
                CommandTranslations.Unpermit_NotWhitelisted = "{0} is not on the whitelist!";
                CommandTranslations.Unpermit_Unpermit = "{0} is no longer whitelisted!";

                CommandTranslations.Vehicle_Help = "This gives a player a vehicle.";
                CommandTranslations.Vehicle_Usage = " <vehicle> [player]";
                CommandTranslations.Vehicle_Invalid = "The specified vehicle is invalid!";
                CommandTranslations.Vehicle_Fail = "Failed to spawn a {0}";
                CommandTranslations.Vehicle_Spawn = "{0} has been spawned!";

                CommandTranslations.Votify_Help = "This configures voting for the server.";
                CommandTranslations.Votify_Usage = " <allowed> <pass cooldown> <fail cooldown> <duration> <percentage> <player count>";
                CommandTranslations.Votify_Pass = "Invalid cooldown for a passed vote!";
                CommandTranslations.Votify_Fail = "Invalid cooldown for a failed vote!";
                CommandTranslations.Votify_Duration = "Invalid vote duration!";
                CommandTranslations.Votify_Percentage = "Invalid vote percentage!";
                CommandTranslations.Votify_Count = "Invalid player count for vote!";
                CommandTranslations.Votify_Set = "The server vote settings have been set!";

                CommandTranslations.Welcome_Help = "This sets a welcome message shown to clients as they connect.";
                CommandTranslations.Welcome_Usage = " <message>";
                CommandTranslations.Welcome_Set = "The server welcome message has been set!";

                CommandTranslations.Whitelisted_Help = "This makes only permited players allowed to join the server.";
                CommandTranslations.Whitelisted_Set = "The server is now in whitelist only mode!";

                Save();
            }
            else
            {
                CommandTranslations.Base_InvalidPlayer = (string)JCommand.Document["Base_InvalidPlayer"];
                CommandTranslations.Base_NoArenaTime = (string)JCommand.Document["Base_NoArenaTime"];
                CommandTranslations.Base_NoHordeTime = (string)JCommand.Document["Base_NoHordeTime"];
                CommandTranslations.Base_CommandInvalid = (string)JCommand.Document["Base_CommandInvalid"];

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

                CommandTranslations.Flag_Help = (string)JCommand.Document["Flag_Help"];
                CommandTranslations.Flag_Usage = (string)JCommand.Document["Flag_Usage"];
                CommandTranslations.Flag_InvalidFlag = (string)JCommand.Document["Flag_InvalidFlag"];
                CommandTranslations.Flag_InvalidValue = (string)JCommand.Document["Flag_InvalidValue"];
                CommandTranslations.Flag_Set = (string)JCommand.Document["Flag_Set"];

                CommandTranslations.GameMode_Help = (string)JCommand.Document["GameMode_Help"];
                CommandTranslations.GameMode_Usage = (string)JCommand.Document["GameMode_Usage"];
                CommandTranslations.GameMode_Set = (string)JCommand.Document["GameMode_Set"];

                CommandTranslations.Gold_Help = (string)JCommand.Document["Gold_Help"];
                CommandTranslations.Gold_Set = (string)JCommand.Document["Gold_Set"];

                CommandTranslations.Help_Help = (string)JCommand.Document["Help_Help"];
                CommandTranslations.Help_Usage = (string)JCommand.Document["Help_Usage"];

                CommandTranslations.HideAdmins_Help = (string)JCommand.Document["HideAdmins_Help"];
                CommandTranslations.HideAdmins_Set = (string)JCommand.Document["HideAdmins_Set"];

                CommandTranslations.Item_Help = (string)JCommand.Document["Item_Help"];
                CommandTranslations.Item_Usage = (string)JCommand.Document["Item_Usage"];
                CommandTranslations.Item_Invalid = (string)JCommand.Document["Item_Invalid"];
                CommandTranslations.Item_Fail = (string)JCommand.Document["Item_Fail"];
                CommandTranslations.Item_Give = (string)JCommand.Document["Item_Give"];

                CommandTranslations.Kick_Help = (string)JCommand.Document["Kick_Help"];
                CommandTranslations.Kick_Usage = (string)JCommand.Document["Kick_Usage"];
                CommandTranslations.Kick_Reason = (string)JCommand.Document["Kick_Reason"];
                CommandTranslations.Kick_Kicked = (string)JCommand.Document["Kick_Kicked"];

                CommandTranslations.Kill_Help = (string)JCommand.Document["Kill_Help"];
                CommandTranslations.Kill_Usage = (string)JCommand.Document["Kill_Usage"];
                CommandTranslations.Kill_Killed = (string)JCommand.Document["Kill_Killed"];

                CommandTranslations.Log_Help = (string)JCommand.Document["Log_Help"];
                CommandTranslations.Log_Usage = (string)JCommand.Document["Log_Usage"];
                CommandTranslations.Log_Set = (string)JCommand.Document["Log_Set"];

                CommandTranslations.Map_Help = (string)JCommand.Document["Map_Help"];
                CommandTranslations.Map_Usage = (string)JCommand.Document["Map_Usage"];
                CommandTranslations.Map_Invalid = (string)JCommand.Document["Map_Invalid"];
                CommandTranslations.Map_Set = (string)JCommand.Document["Map_Set"];

                CommandTranslations.MaxPlayers_Help = (string)JCommand.Document["MaxPlayers_Help"];
                CommandTranslations.MaxPlayers_Usage = (string)JCommand.Document["MaxPlayers_Usage"];
                CommandTranslations.MaxPlayers_Invalid = (string)JCommand.Document["MaxPlayers_Invalid"];
                CommandTranslations.MaxPlayers_TooHigh = (string)JCommand.Document["MaxPlayers_TooHigh"];
                CommandTranslations.MaxPlayers_TooLow = (string)JCommand.Document["MaxPlayers_TooLow"];
                CommandTranslations.MaxPlayers_Set = (string)JCommand.Document["MaxPlayers_Set"];

                CommandTranslations.Mode_Help = (string)JCommand.Document["Mode_Help"];
                CommandTranslations.Mode_Usage = (string)JCommand.Document["Mode_Usage"];
                CommandTranslations.Mode_Invalid = (string)JCommand.Document["Mode_Invalid"];
                CommandTranslations.Mode_Set = (string)JCommand.Document["Mode_Set"];

                CommandTranslations.Modules_Help = (string)JCommand.Document["Modules_Help"];
                CommandTranslations.Modules_Title = (string)JCommand.Document["Modules_Title"];
                CommandTranslations.Modules_Name = (string)JCommand.Document["Modules_Name"];
                CommandTranslations.Modules_Version = (string)JCommand.Document["Modules_Version"];

                CommandTranslations.Name_Help = (string)JCommand.Document["Name_Help"];
                CommandTranslations.Name_Usage = (string)JCommand.Document["Name_Usage"];
                CommandTranslations.Name_TooLong = (string)JCommand.Document["Name_TooLong"];
                CommandTranslations.Name_TooShort = (string)JCommand.Document["Name_TooShort"];
                CommandTranslations.Name_Set = (string)JCommand.Document["Name_Set"];

                CommandTranslations.Night_Help = (string)JCommand.Document["Night_Help"];
                CommandTranslations.Night_Set = (string)JCommand.Document["Night_Set"];

                CommandTranslations.Owner_Help = (string)JCommand.Document["Owner_Help"];
                CommandTranslations.Owner_Usage = (string)JCommand.Document["Owner_Usage"];
                CommandTranslations.Owner_Set = (string)JCommand.Document["Owner_Set"];

                CommandTranslations.Password_Help = (string)JCommand.Document["Password_Help"];
                CommandTranslations.Password_Usage = (string)JCommand.Document["Password_Usage"];
                CommandTranslations.Password_Removed = (string)JCommand.Document["Password_Removed"];
                CommandTranslations.Password_Set = (string)JCommand.Document["Password_Set"];

                CommandTranslations.Permit_Help = (string)JCommand.Document["Permit_Help"];
                CommandTranslations.Permit_Usage = (string)JCommand.Document["Permit_Usage"];
                CommandTranslations.Permit_Added = (string)JCommand.Document["Permit_Added"];

                CommandTranslations.Permits_Help = (string)JCommand.Document["Permits_Help"];
                CommandTranslations.Permits_List = (string)JCommand.Document["Permits_List"];

                CommandTranslations.Port_Help = (string)JCommand.Document["Port_Help"];
                CommandTranslations.Port_Usage = (string)JCommand.Document["Port_Usage"];
                CommandTranslations.Port_Invalid = (string)JCommand.Document["Port_Invalid"];
                CommandTranslations.Port_Set = (string)JCommand.Document["Port_Set"];

                CommandTranslations.PvE_Help = (string)JCommand.Document["PvE_Help"];
                CommandTranslations.PvE_Set = (string)JCommand.Document["PvE_Set"];

                CommandTranslations.Quest_Help = (string)JCommand.Document["Quest_Help"];
                CommandTranslations.Quest_Usage = (string)JCommand.Document["Quest_Usage"];
                CommandTranslations.Quest_Invalid = (string)JCommand.Document["Quest_Invalid"];
                CommandTranslations.Quest_Added = (string)JCommand.Document["Quest_Added"];

                CommandTranslations.Queue_Help = (string)JCommand.Document["Queue_Help"];
                CommandTranslations.Queue_Usage = (string)JCommand.Document["Queue_Usage"];
                CommandTranslations.Queue_Invalid = (string)JCommand.Document["Queue_Invalid"];
                CommandTranslations.Queue_TooHigh = (string)JCommand.Document["Queue_TooHigh"];
                CommandTranslations.Queue_Set = (string)JCommand.Document["Queue_Set"];

                CommandTranslations.Reputation_Help = (string)JCommand.Document["Reputation_Help"];
                CommandTranslations.Reputation_Usage = (string)JCommand.Document["Reputation_Usage"];
                CommandTranslations.Reputation_Invalid = (string)JCommand.Document["Reputation_Invalid"];
                CommandTranslations.Reputation_Give = (string)JCommand.Document["Reputation_Give"];

                CommandTranslations.ResetConfig_Help = (string)JCommand.Document["ResetConfig_Help"];
                CommandTranslations.ResetConfig_Reset = (string)JCommand.Document["ResetConfig_Reset"];

                CommandTranslations.Save_Help = (string)JCommand.Document["Save_Help"];
                CommandTranslations.Save_Save = (string)JCommand.Document["Save_Save"];

                CommandTranslations.Say_Help = (string)JCommand.Document["Say_Help"];
                CommandTranslations.Say_Usage = (string)JCommand.Document["Say_Usage"];

                CommandTranslations.Shutdown_Help = (string)JCommand.Document["Shutdown_Help"];
                CommandTranslations.Shutdown_Usage = (string)JCommand.Document["Shutdown_Usage"];

                CommandTranslations.Slay_Help = (string)JCommand.Document["Slay_Help"];
                CommandTranslations.Slay_Usage = (string)JCommand.Document["Slay_Usage"];
                CommandTranslations.Slay_Slay = (string)JCommand.Document["Slay_Slay"];

                CommandTranslations.Spy_Help = (string)JCommand.Document["Spy_Help"];
                CommandTranslations.Spy_Usage = (string)JCommand.Document["Spy_Usage"];
                CommandTranslations.Spy_Spy = (string)JCommand.Document["Spy_Spy"];

                CommandTranslations.Storm_Help = (string)JCommand.Document["Storm_Help"];
                CommandTranslations.Storm_Change = (string)JCommand.Document["Storm_Change"];

                CommandTranslations.Sync_Help = (string)JCommand.Document["Sync_Help"];
                CommandTranslations.Sync_Sync = (string)JCommand.Document["Sync_Sync"];

                CommandTranslations.Teleport_Help = (string)JCommand.Document["Teleport_Help"];
                CommandTranslations.Teleport_Usage = (string)JCommand.Document["Teleport_Usage"];
                CommandTranslations.Teleport_Invalid = (string)JCommand.Document["Teleport_Invalid"];
                CommandTranslations.Teleport_Teleport = (string)JCommand.Document["Teleport_Teleport"];

                CommandTranslations.Time_Help = (string)JCommand.Document["Time_Help"];
                CommandTranslations.Time_Usage = (string)JCommand.Document["Time_Usage"];
                CommandTranslations.Time_Invalid = (string)JCommand.Document["Time_Invalid"];
                CommandTranslations.Time_Set = (string)JCommand.Document["Time_Set"];

                CommandTranslations.Timeout_Help = (string)JCommand.Document["Timeout_Help"];
                CommandTranslations.Timeout_Usage = (string)JCommand.Document["Timeout_Usage"];
                CommandTranslations.Timeout_Invalid = (string)JCommand.Document["Timeout_Invalid"];
                CommandTranslations.Timeout_TooHigh = (string)JCommand.Document["Timeout_TooHigh"];
                CommandTranslations.Timeout_TooLow = (string)JCommand.Document["Timeout_TooLow"];
                CommandTranslations.Timeout_Set = (string)JCommand.Document["Timeout_Set"];

                CommandTranslations.Unadmin_Help = (string)JCommand.Document["Unadmin_Help"];
                CommandTranslations.Unadmin_Usage = (string)JCommand.Document["Unadmin_Usage"];
                CommandTranslations.Unadmin_Unadmin = (string)JCommand.Document["Unadmin_Unadmin"];

                CommandTranslations.Unban_Help = (string)JCommand.Document["Unban_Help"];
                CommandTranslations.Unban_Usage = (string)JCommand.Document["Unban_Usage"];
                CommandTranslations.Unban_NotBanned = (string)JCommand.Document["Unban_NotBanned"];
                CommandTranslations.Unban_Unban = (string)JCommand.Document["Unban_Unban"];

                CommandTranslations.Unpermit_Help = (string)JCommand.Document["Unpermit_Help"];
                CommandTranslations.Unpermit_Usage = (string)JCommand.Document["Unpermit_Usage"];
                CommandTranslations.Unpermit_NotWhitelisted = (string)JCommand.Document["Unpermit_NotWhitelisted"];
                CommandTranslations.Unpermit_Unpermit = (string)JCommand.Document["Unpermit_Unpermit"];

                CommandTranslations.Vehicle_Help = (string)JCommand.Document["Vehicle_Help"];
                CommandTranslations.Vehicle_Usage = (string)JCommand.Document["Vehicle_Usage"];
                CommandTranslations.Vehicle_Invalid = (string)JCommand.Document["Vehicle_Invalid"];
                CommandTranslations.Vehicle_Fail = (string)JCommand.Document["Vehicle_Fail"];
                CommandTranslations.Vehicle_Spawn = (string)JCommand.Document["Vehicle_Spawn"];

                CommandTranslations.Votify_Help = (string)JCommand.Document["Votify_Help"];
                CommandTranslations.Votify_Usage = (string)JCommand.Document["Votify_Usage"];
                CommandTranslations.Votify_Pass = (string)JCommand.Document["Votify_Pass"];
                CommandTranslations.Votify_Fail = (string)JCommand.Document["Votify_Fail"];
                CommandTranslations.Votify_Duration = (string)JCommand.Document["Votify_Duration"];
                CommandTranslations.Votify_Percentage = (string)JCommand.Document["Votify_Percentage"];
                CommandTranslations.Votify_Count = (string)JCommand.Document["Votify_Count"];
                CommandTranslations.Votify_Set = (string)JCommand.Document["Votify_Set"];

                CommandTranslations.Welcome_Help = (string)JCommand.Document["Welcome_Help"];
                CommandTranslations.Welcome_Usage = (string)JCommand.Document["Welcome_Usage"];
                CommandTranslations.Welcome_Set = (string)JCommand.Document["Welcome_Set"];

                CommandTranslations.Whitelisted_Help = (string)JCommand.Document["Whitelisted_Help"];
                CommandTranslations.Whitelisted_Set = (string)JCommand.Document["Whitelisted_Set"];
            }
            #endregion

            #region PointBlank Command Translations
            if (UniCommand.CreatedNew)
            {
                CommandTranslations.Base_NotEnoughArgs = "Not enough arguments specified!";
                CommandTranslations.Base_InvalidGroup = "The specified group is invalid!";

                CommandTranslations.Group_Help = "This allows you to get information on groups or modify them.";
                CommandTranslations.Group_Usage = " {0}";
                CommandTranslations.Group_NotFound = "The specified group has not been found!";
                CommandTranslations.Group_Exists = "The specified group ID is already in use!";
                CommandTranslations.Group_Usage_Add = " {0} <ID> <Name>";
                CommandTranslations.Group_Usage_IDs = " {0}";
                CommandTranslations.Group_Usage_List = " {0}";
                CommandTranslations.Group_Usage_Permissions = " {0} <ID>";
                CommandTranslations.Group_Usage_Remove = " {0} <ID>";
                CommandTranslations.Group_Added = "The group has been added!";
                CommandTranslations.Group_Removed = "The group has been removed!";
                CommandTranslations.Group_Commands_Add = "Add";
                CommandTranslations.Group_Commands_Help = "Help";
                CommandTranslations.Group_Commands_IDs = "IDs";
                CommandTranslations.Group_Commands_List = "List";
                CommandTranslations.Group_Commands_Permissions = "Permissions";
                CommandTranslations.Group_Commands_Remove = "Remove";

                CommandTranslations.Player_Help = "This allows you to get information on the player or modify it.";
                CommandTranslations.Player_Usage = " {0}";
                CommandTranslations.Player_Group = " {0} <player>";
                CommandTranslations.Player_Group_Modify = " {0} <player> {1} <group ID>";
                CommandTranslations.Player_Group_Added = "The group has been added to player!";
                CommandTranslations.Player_Group_Removed = "The group has been removed from the player!";
                CommandTranslations.Player_Permissions = " {0} <player>";
                CommandTranslations.Player_Group_Invalid = "The specified group ID is invalid!";
                CommandTranslations.Player_Commands_Groups = "Groups";
                CommandTranslations.Player_Commands_Groups_Add = "Add";
                CommandTranslations.Player_Commands_Groups_Remove = "Remove";
                CommandTranslations.Player_Commands_Help = "Help";
                CommandTranslations.Player_Commands_Permissions = "Permissions";

                CommandTranslations.Usage_Help = "This provides information on how to use the specified command.";
                CommandTranslations.Usage_Usage = " <command>";

                CommandTranslations.Permissions_Help = "This allows you to view or modify permissions of groups or players.";
                CommandTranslations.Permissions_Usage = " {0}";
                CommandTranslations.Permissions_Commands_Help = "Help";
                CommandTranslations.Permissions_Group = " {0} <group ID>";
                CommandTranslations.Permissions_Group_Modify = " {0} <group ID> {1} <permission>";
                CommandTranslations.Permissions_Player = " {0} <player>";
                CommandTranslations.Permissions_Player_Modify = " {0} <player> {1} <permission>";
                CommandTranslations.Permissions_Commands_Group = "Group";
                CommandTranslations.Permissions_Commands_Player = "Player";
                CommandTranslations.Permissions_Add_Success = "{0} has been successfully added to {1}!";
                CommandTranslations.Permissions_Remove_Success = "{0} has been successfully removed from {1}!";
                CommandTranslations.Permissions_Commands_Add = "Add";
                CommandTranslations.Permissions_Commands_Remove = "Remove";
            }
            else
            {
                CommandTranslations.Base_NotEnoughArgs = (string)JCommand.Document["Base_NotEnoughArgs"];
                CommandTranslations.Base_InvalidGroup = (string)JCommand.Document["Base_InvalidGroup"];

                CommandTranslations.Group_Help = (string)JCommand.Document["Group_Help"];
                CommandTranslations.Group_Usage = (string)JCommand.Document["Group_Usage"];
                CommandTranslations.Group_NotFound = (string)JCommand.Document["Group_NotFound"];
                CommandTranslations.Group_Exists = (string)JCommand.Document["Group_Exists"];
                CommandTranslations.Group_Usage_Add = (string)JCommand.Document["Group_Usage_Add"];
                CommandTranslations.Group_Usage_IDs = (string)JCommand.Document["Group_Usage_IDs"];
                CommandTranslations.Group_Usage_List = (string)JCommand.Document["Group_Usage_List"];
                CommandTranslations.Group_Usage_Permissions = (string)JCommand.Document["Group_Usage_Permissions"];
                CommandTranslations.Group_Usage_Remove = (string)JCommand.Document["Group_Usage_Remove"];
                CommandTranslations.Group_Added = (string)JCommand.Document["Group_Added"];
                CommandTranslations.Group_Removed = (string)JCommand.Document["Group_Removed"];
                CommandTranslations.Group_Commands_Add = (string)JCommand.Document["Group_Commands_Add"];
                CommandTranslations.Group_Commands_Help = (string)JCommand.Document["Group_Commands_Help"];
                CommandTranslations.Group_Commands_IDs = (string)JCommand.Document["Group_Commands_IDs"];
                CommandTranslations.Group_Commands_List = (string)JCommand.Document["Group_Commands_List"];
                CommandTranslations.Group_Commands_Permissions = (string)JCommand.Document["Group_Commands_Permissions"];
                CommandTranslations.Group_Commands_Remove = (string)JCommand.Document["Group_Commands_Remove"];

                CommandTranslations.Player_Help = (string)JCommand.Document["Player_Help"];
                CommandTranslations.Player_Usage = (string)JCommand.Document["Player_Usage"];
                CommandTranslations.Player_Group = (string)JCommand.Document["Player_Group"];
                CommandTranslations.Player_Group_Modify = (string)JCommand.Document["Player_Group_Modify"];
                CommandTranslations.Player_Group_Added = (string)JCommand.Document["Player_Group_Added"];
                CommandTranslations.Player_Group_Removed = (string)JCommand.Document["Player_Group_Removed"];
                CommandTranslations.Player_Permissions = (string)JCommand.Document["Player_Permissions"];
                CommandTranslations.Player_Group_Invalid = (string)JCommand.Document["Player_Group_Invalid"];
                CommandTranslations.Player_Commands_Groups = (string)JCommand.Document["Player_Commands_Groups"];
                CommandTranslations.Player_Commands_Groups_Add = (string)JCommand.Document["Player_Commands_Groups_Add"];
                CommandTranslations.Player_Commands_Groups_Remove = (string)JCommand.Document["Player_Commands_Groups_Remove"];
                CommandTranslations.Player_Commands_Help = (string)JCommand.Document["Player_Commands_Help"];
                CommandTranslations.Player_Commands_Permissions = (string)JCommand.Document["Player_Commands_Permissions"];

                CommandTranslations.Usage_Help = (string)JCommand.Document["Usage_Help"];
                CommandTranslations.Usage_Usage = (string)JCommand.Document["Usage_Usage"];

                CommandTranslations.Permissions_Help = (string)JCommand.Document["Permissions_Help"];
                CommandTranslations.Permissions_Usage = (string)JCommand.Document["Permissions_Usage"];
                CommandTranslations.Permissions_Commands_Help = (string)JCommand.Document["Permissions_Commands_Help"];
                CommandTranslations.Permissions_Group = (string)JCommand.Document["Permissions_Group"];
                CommandTranslations.Permissions_Group_Modify = (string)JCommand.Document["Permissions_Group_Modify"];
                CommandTranslations.Permissions_Player = (string)JCommand.Document["Permissions_Player"];
                CommandTranslations.Permissions_Player_Modify = (string)JCommand.Document["Permissions_Player_Modify"];
                CommandTranslations.Permissions_Add_Success = (string)JCommand.Document["Permissions_Add_Success"];
                CommandTranslations.Permissions_Remove_Success = (string)JCommand.Document["Permissions_Remove_Success"];
                CommandTranslations.Permissions_Commands_Add = (string)JCommand.Document["Permissions_Commands_Add"];
                CommandTranslations.Permissions_Commands_Remove = (string)JCommand.Document["Permissions_Commands_Remove"];
                CommandTranslations.Permissions_Commands_Group = (string)JCommand.Document["Permissions_Commands_Group"];
                CommandTranslations.Permissions_Commands_Player = (string)JCommand.Document["Permissions_Commands_Player"];
            }
            #endregion
        }

        public static void Reload() => Load();

        public static void Save()
        {
            #region Command Translations
            SetCommand("Base_InvalidPlayer", CommandTranslations.Base_InvalidPlayer);
            SetCommand("Base_NoArenaTime", CommandTranslations.Base_NoArenaTime);
            SetCommand("Base_NoHordeTime", CommandTranslations.Base_NoHordeTime);
            SetCommand("Base_CommandInvalid", CommandTranslations.Base_CommandInvalid);

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

            SetCommand("Flag_Help", CommandTranslations.Flag_Help);
            SetCommand("Flag_Usage", CommandTranslations.Flag_Usage);
            SetCommand("Flag_InvalidFlag", CommandTranslations.Flag_InvalidFlag);
            SetCommand("Flag_InvalidValue", CommandTranslations.Flag_InvalidValue);
            SetCommand("Flag_Set", CommandTranslations.Flag_Set);

            SetCommand("GameMode_Help", CommandTranslations.GameMode_Help);
            SetCommand("GameMode_Usage", CommandTranslations.GameMode_Usage);
            SetCommand("GameMode_Set", CommandTranslations.GameMode_Set);

            SetCommand("Gold_Help", CommandTranslations.Gold_Help);
            SetCommand("Gold_Set", CommandTranslations.Gold_Set);

            SetCommand("Help_Help", CommandTranslations.Help_Help);
            SetCommand("Help_Usage", CommandTranslations.Help_Usage);

            SetCommand("HideAdmins_Help", CommandTranslations.HideAdmins_Help);
            SetCommand("HideAdmins_Set", CommandTranslations.HideAdmins_Set);

            SetCommand("Item_Help", CommandTranslations.Item_Help);
            SetCommand("Item_Usage", CommandTranslations.Item_Usage);
            SetCommand("Item_Invalid", CommandTranslations.Item_Invalid);
            SetCommand("Item_Fail", CommandTranslations.Item_Fail);
            SetCommand("Item_Give", CommandTranslations.Item_Give);

            SetCommand("Kick_Help", CommandTranslations.Kick_Help);
            SetCommand("Kick_Usage", CommandTranslations.Kick_Usage);
            SetCommand("Kick_Reason", CommandTranslations.Kick_Reason);
            SetCommand("Kick_Kicked", CommandTranslations.Kick_Kicked);

            SetCommand("Kill_Help", CommandTranslations.Kill_Help);
            SetCommand("Kill_Usage", CommandTranslations.Kill_Usage);
            SetCommand("Kill_Killed", CommandTranslations.Kill_Killed);

            SetCommand("Log_Help", CommandTranslations.Log_Help);
            SetCommand("Log_Usage", CommandTranslations.Log_Usage);
            SetCommand("Log_Set", CommandTranslations.Log_Set);

            SetCommand("Map_Help", CommandTranslations.Map_Help);
            SetCommand("Map_Usage", CommandTranslations.Map_Usage);
            SetCommand("Map_Invalid", CommandTranslations.Map_Invalid);
            SetCommand("Map_Set", CommandTranslations.Map_Set);

            SetCommand("MaxPlayers_Help", CommandTranslations.MaxPlayers_Help);
            SetCommand("MaxPlayers_Usage", CommandTranslations.MaxPlayers_Usage);
            SetCommand("MaxPlayers_Invalid", CommandTranslations.MaxPlayers_Invalid);
            SetCommand("MaxPlayers_TooHigh", CommandTranslations.MaxPlayers_TooHigh);
            SetCommand("MaxPlayers_TooLow", CommandTranslations.MaxPlayers_TooLow);
            SetCommand("MaxPlayers_Set", CommandTranslations.MaxPlayers_Set);

            SetCommand("Mode_Help", CommandTranslations.Mode_Help);
            SetCommand("Mode_Usage", CommandTranslations.Mode_Usage);
            SetCommand("Mode_Invalid", CommandTranslations.Mode_Invalid);
            SetCommand("Mode_Set", CommandTranslations.Mode_Set);

            SetCommand("Modules_Help", CommandTranslations.Modules_Help);
            SetCommand("Modules_Title", CommandTranslations.Modules_Title);
            SetCommand("Modules_Name", CommandTranslations.Modules_Name);
            SetCommand("Modules_Version", CommandTranslations.Modules_Version);

            SetCommand("Name_Help", CommandTranslations.Name_Help);
            SetCommand("Name_Usage", CommandTranslations.Name_Usage);
            SetCommand("Name_TooLong", CommandTranslations.Name_TooLong);
            SetCommand("Name_TooShort", CommandTranslations.Name_TooShort);
            SetCommand("Name_Set", CommandTranslations.Name_Set);

            SetCommand("Night_Help", CommandTranslations.Night_Help);
            SetCommand("Night_Set", CommandTranslations.Night_Set);

            SetCommand("Owner_Help", CommandTranslations.Owner_Help);
            SetCommand("Owner_Usage", CommandTranslations.Owner_Usage);
            SetCommand("Owner_Set", CommandTranslations.Owner_Set);

            SetCommand("Password_Help", CommandTranslations.Password_Help);
            SetCommand("Password_Usage", CommandTranslations.Password_Usage);
            SetCommand("Password_Removed", CommandTranslations.Password_Removed);
            SetCommand("Password_Set", CommandTranslations.Password_Set);

            SetCommand("Permit_Help", CommandTranslations.Permit_Help);
            SetCommand("Permit_Usage", CommandTranslations.Permit_Usage);
            SetCommand("Permit_Added", CommandTranslations.Permit_Added);

            SetCommand("Permits_Help", CommandTranslations.Permits_Help);
            SetCommand("Permits_List", CommandTranslations.Permits_List);

            SetCommand("Port_Help", CommandTranslations.Port_Help);
            SetCommand("Port_Usage", CommandTranslations.Port_Usage);
            SetCommand("Port_Invalid", CommandTranslations.Port_Invalid);
            SetCommand("Port_Set", CommandTranslations.Port_Set);

            SetCommand("PvE_Help", CommandTranslations.PvE_Help);
            SetCommand("PvE_Set", CommandTranslations.PvE_Set);

            SetCommand("Quest_Help", CommandTranslations.Quest_Help);
            SetCommand("Quest_Usage", CommandTranslations.Quest_Usage);
            SetCommand("Quest_Invalid", CommandTranslations.Quest_Invalid);
            SetCommand("Quest_Added", CommandTranslations.Quest_Added);

            SetCommand("Queue_Help", CommandTranslations.Queue_Help);
            SetCommand("Queue_Usage", CommandTranslations.Queue_Usage);
            SetCommand("Queue_Invalid", CommandTranslations.Queue_Invalid);
            SetCommand("Queue_TooHigh", CommandTranslations.Queue_TooHigh);
            SetCommand("Queue_Set", CommandTranslations.Queue_Set);

            SetCommand("Reputation_Help", CommandTranslations.Reputation_Help);
            SetCommand("Reputation_Usage", CommandTranslations.Reputation_Usage);
            SetCommand("Reputation_Invalid", CommandTranslations.Reputation_Invalid);
            SetCommand("Reputation_Give", CommandTranslations.Reputation_Give);

            SetCommand("ResetConfig_Help", CommandTranslations.ResetConfig_Help);
            SetCommand("ResetConfig_Reset", CommandTranslations.ResetConfig_Reset);

            SetCommand("Save_Help", CommandTranslations.Save_Help);
            SetCommand("Save_Save", CommandTranslations.Save_Save);

            SetCommand("Say_Help", CommandTranslations.Say_Help);
            SetCommand("Say_Usage", CommandTranslations.Say_Usage);

            SetCommand("Shutdown_Help", CommandTranslations.Shutdown_Help);
            SetCommand("Shutdown_Usage", CommandTranslations.Shutdown_Usage);

            SetCommand("Slay_Help", CommandTranslations.Slay_Help);
            SetCommand("Slay_Usage", CommandTranslations.Slay_Usage);
            SetCommand("Slay_Slay", CommandTranslations.Slay_Slay);

            SetCommand("Spy_Help", CommandTranslations.Spy_Help);
            SetCommand("Spy_Usage", CommandTranslations.Spy_Usage);
            SetCommand("Spy_Spy", CommandTranslations.Spy_Spy);

            SetCommand("Storm_Help", CommandTranslations.Storm_Help);
            SetCommand("Storm_Change", CommandTranslations.Storm_Change);

            SetCommand("Sync_Help", CommandTranslations.Sync_Help);
            SetCommand("Sync_Sync", CommandTranslations.Sync_Sync);

            SetCommand("Teleport_Help", CommandTranslations.Teleport_Help);
            SetCommand("Teleport_Usage", CommandTranslations.Teleport_Usage);
            SetCommand("Teleport_Invalid", CommandTranslations.Teleport_Invalid);
            SetCommand("Teleport_Teleport", CommandTranslations.Teleport_Teleport);

            SetCommand("Time_Help", CommandTranslations.Time_Help);
            SetCommand("Time_Usage", CommandTranslations.Time_Usage);
            SetCommand("Time_Invalid", CommandTranslations.Time_Invalid);
            SetCommand("Time_Set", CommandTranslations.Time_Set);

            SetCommand("Timeout_Help", CommandTranslations.Timeout_Help);
            SetCommand("Timeout_Usage", CommandTranslations.Timeout_Usage);
            SetCommand("Timeout_Invalid", CommandTranslations.Timeout_Invalid);
            SetCommand("Timeout_TooHigh", CommandTranslations.Timeout_TooHigh);
            SetCommand("Timeout_TooLow", CommandTranslations.Timeout_TooLow);
            SetCommand("Timeout_Set", CommandTranslations.Timeout_Set);

            SetCommand("Unadmin_Help", CommandTranslations.Unadmin_Help);
            SetCommand("Unadmin_Usage", CommandTranslations.Unadmin_Usage);
            SetCommand("Unadmin_Unadmin", CommandTranslations.Unadmin_Unadmin);

            SetCommand("Unban_Help", CommandTranslations.Unban_Help);
            SetCommand("Unban_Usage", CommandTranslations.Unban_Usage);
            SetCommand("Unban_NotBanned", CommandTranslations.Unban_NotBanned);
            SetCommand("Unban_Unban", CommandTranslations.Unban_Unban);

            SetCommand("Unpermit_Help", CommandTranslations.Unpermit_Help);
            SetCommand("Unpermit_Usage", CommandTranslations.Unpermit_Usage);
            SetCommand("Unpermit_NotWhitelisted", CommandTranslations.Unpermit_NotWhitelisted);
            SetCommand("Unpermit_Unpermit", CommandTranslations.Unpermit_Unpermit);

            SetCommand("Vehicle_Help", CommandTranslations.Vehicle_Help);
            SetCommand("Vehicle_Usage", CommandTranslations.Vehicle_Usage);
            SetCommand("Vehicle_Invalid", CommandTranslations.Vehicle_Invalid);
            SetCommand("Vehicle_Fail", CommandTranslations.Vehicle_Fail);
            SetCommand("Vehicle_Spawn", CommandTranslations.Vehicle_Spawn);

            SetCommand("Votify_Help", CommandTranslations.Votify_Help);
            SetCommand("Votify_Usage", CommandTranslations.Votify_Usage);
            SetCommand("Votify_Pass", CommandTranslations.Votify_Pass);
            SetCommand("Votify_Fail", CommandTranslations.Votify_Fail);
            SetCommand("Votify_Duration", CommandTranslations.Votify_Duration);
            SetCommand("Votify_Percentage", CommandTranslations.Votify_Percentage);
            SetCommand("Votify_Count", CommandTranslations.Votify_Count);
            SetCommand("Votify_Set", CommandTranslations.Votify_Set);

            SetCommand("Welcome_Help", CommandTranslations.Welcome_Help);
            SetCommand("Welcome_Usage", CommandTranslations.Welcome_Usage);
            SetCommand("Welcome_Set", CommandTranslations.Welcome_Set);

            SetCommand("Whitelisted_Help", CommandTranslations.Whitelisted_Help);
            SetCommand("Whitelisted_Set", CommandTranslations.Whitelisted_Set);
            #endregion

            #region PointBlank Command Translations
            SetCommand("Base_NotEnoughArgs", CommandTranslations.Base_NotEnoughArgs);
            SetCommand("Base_InvalidGroup", CommandTranslations.Base_InvalidGroup);

            SetCommand("Group_Help", CommandTranslations.Group_Help);
            SetCommand("Group_Usage", CommandTranslations.Group_Usage);
            SetCommand("Group_NotFound", CommandTranslations.Group_NotFound);
            SetCommand("Group_Exists", CommandTranslations.Group_Exists);
            SetCommand("Group_Usage_Add", CommandTranslations.Group_Usage_Add);
            SetCommand("Group_Usage_IDs", CommandTranslations.Group_Usage_IDs);
            SetCommand("Group_Usage_List", CommandTranslations.Group_Usage_List);
            SetCommand("Group_Usage_Permissions", CommandTranslations.Group_Usage_Permissions);
            SetCommand("Group_Usage_Remove", CommandTranslations.Group_Usage_Remove);
            SetCommand("Group_Added", CommandTranslations.Group_Added);
            SetCommand("Group_Removed", CommandTranslations.Group_Removed);
            SetCommand("Group_Commands_Add", CommandTranslations.Group_Commands_Add);
            SetCommand("Group_Commands_Help", CommandTranslations.Group_Commands_Help);
            SetCommand("Group_Commands_IDs", CommandTranslations.Group_Commands_IDs);
            SetCommand("Group_Commands_List", CommandTranslations.Group_Commands_List);
            SetCommand("Group_Commands_Permissions", CommandTranslations.Group_Commands_Permissions);
            SetCommand("Group_Commands_Remove", CommandTranslations.Group_Commands_Remove);

            SetCommand("Player_Help", CommandTranslations.Player_Help);
            SetCommand("Player_Usage", CommandTranslations.Player_Usage);
            SetCommand("Player_Group", CommandTranslations.Player_Group);
            SetCommand("Player_Group_Modify", CommandTranslations.Player_Group_Modify);
            SetCommand("Player_Group_Added", CommandTranslations.Player_Group_Added);
            SetCommand("Player_Group_Removed", CommandTranslations.Player_Group_Removed);
            SetCommand("Player_Permissions", CommandTranslations.Player_Permissions);
            SetCommand("Player_Group_Invalid", CommandTranslations.Player_Group_Invalid);
            SetCommand("Player_Commands_Groups", CommandTranslations.Player_Commands_Groups);
            SetCommand("Player_Commands_Groups_Add", CommandTranslations.Player_Commands_Groups_Add);
            SetCommand("Player_Commands_Groups_Remove", CommandTranslations.Player_Commands_Groups_Remove);
            SetCommand("Player_Commands_Help", CommandTranslations.Player_Commands_Help);
            SetCommand("Player_Commands_Permissions", CommandTranslations.Player_Commands_Permissions);

            SetCommand("Usage_Help", CommandTranslations.Usage_Help);
            SetCommand("Usage_Usage", CommandTranslations.Usage_Usage);

            SetCommand("Permissions_Help", CommandTranslations.Permissions_Help);
            SetCommand("Permissions_Usage", CommandTranslations.Permissions_Usage);
            SetCommand("Permissions_Commands_Help", CommandTranslations.Permissions_Commands_Help);
            SetCommand("Permissions_Group", CommandTranslations.Permissions_Group);
            SetCommand("Permissions_Group_Modify", CommandTranslations.Permissions_Group_Modify);
            SetCommand("Permissions_Player", CommandTranslations.Permissions_Player);
            SetCommand("Permissions_Player_Modify", CommandTranslations.Permissions_Player_Modify);
            SetCommand("Permissions_Add_Success", CommandTranslations.Permissions_Add_Success);
            SetCommand("Permissions_Remove_Success", CommandTranslations.Permissions_Remove_Success);
            SetCommand("Permissions_Commands_Add", CommandTranslations.Permissions_Commands_Add);
            SetCommand("Permissions_Commands_Remove", CommandTranslations.Permissions_Commands_Remove);
            SetCommand("Permissions_Commands_Group", CommandTranslations.Permissions_Commands_Group);
            SetCommand("Permissions_Commands_Player", CommandTranslations.Permissions_Commands_Player);
            #endregion

            UniCommand.Save();
        }
        #endregion
    }
}
