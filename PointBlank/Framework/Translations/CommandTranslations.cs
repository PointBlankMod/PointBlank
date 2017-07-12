using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Collections;
using PointBlank.API.Interfaces;

namespace PointBlank.Framework.Translations
{
    internal class CommandTranslations : ITranslatable
    {
        public string TranslationDirectory => null;

        public TranslationList Translations => new TranslationList()
        {
            //------------------------------------- UNTURNED COMMANDS -----------------------------------------//
            // Base for commands
            { "Base_InvalidPlayer", "Invalid player or player ID!" },
            { "Base_NoArenaTime", "Cannot change time in arena!" },
            { "Base_NoHordeTime", "Cannot change time in horde!" },
            { "Base_CommandInvalid", "The specified command is invalid!" },

            // Admin
            { "Admin_Help", "This adds the specified player to the list of users allowed to use admin commands in the chat" },
            { "Admin_Set", "{0} has been set as admin!" },
            { "Admin_Usage", " <player/id>" },

            // Admins
            { "Admins_Help", "This shows a list of the current admins." },
            { "Admins_Hidden", "Admins are hidden for this server." },
            { "Admins_List", "Admins: " },

            // Airdrop
            { "Airdrop_Help", "This forces a dropship to fly over and perform an airdrop." },
            { "Airdrop_Success", "Airdrop spawned successfully!" },

            // Ban
            { "Ban_Help", "This adds the specified player to the list of users not allowed to join the server." },
            { "Ban_Usage", " <player> [duration] [reason]" },
            { "Ban_Reason", "Undefined" },
            { "Ban_Success", "{0} has been banned successfully!" },

            // Bans
            { "Bans_Help", "This shows a list of the current bans." },
            { "Bans_List", "Bans: " },

            // Bind
            { "Bind_Help", "This binds a specific internal IP to the socket." },
            { "Bind_InvalidIP", "The IP specified is invalid!" },
            { "Bind_Usage", " <IP>" },

            // Camera
            { "Camera_Help", "This assigns the perspective of the server." },
            { "Camera_Usage", " <first/third/both/vehicle>" },
            { "Camera_SetTo", "Allowed camera mode set to {0}." },

            // Chatrate
            { "Chatrate_Help", "This assigns a minimum time between chat messages." },
            { "Chatrate_Usage", " <rate>" },
            { "Chatrate_Invalid", "The rate specified is invalid!" },
            { "Chatrate_SetTo", "Server chat rate set to {0}." },
            { "Chatrate_TooHigh", "The chat rate cannot be set higher than {0}." },
            { "Chatrate_TooLow", "The chat rate cannot be set lower than {0}." },

            // Cheats
            { "Cheats_Help", "This allows the use of cheats like spawning items, vehicles and experience." },
            { "Cheats_Enabled", "Cheats are now enabled for this server." },

            // Cycle
            { "Cycle_Help", "This assigns the length of the day/night cycle in seconds." },
            { "Cycle_Usage", " <cycle>" },
            { "Cycle_Invalid", "Invalid number of cycles!" },
            { "Cycle_SetTo", "Server cycle set to {0}." },

            // Day
            { "Day_Help", "This assigns the current time to day." },
            { "Day_Set", "Successfully set time to day!" },

            // Debug
            { "Debug_Help", "This provides information on the state of the server." },
            { "Debug_Title", "Debug Information" },
            { "Debug_SteamID", "SteamID: {0}" },
            { "Debug_IP", "IP: {0}" },
            { "Debug_Port", "Port: {0}" },
            { "Debug_BytesSent", "Bytes sent: {0}B" },
            { "Debug_BytesReceived", "Bytes received: {0}B" },
            { "Debug_ABytesSent", "Average bytes sent: {0}B" },
            { "Debug_ABytesReceived", "Average bytes received: {0}B" },
            { "Debug_PacketsSent", "Packets sent: {0}" },
            { "Debug_PacketsReceived", "Packets received: {0}" },
            { "Debug_APacketsSent", "Average packets sent: {0}" },
            { "Debug_APacketsReceived", "Average packets received: {0}" },
            { "Debug_UPS", "Updates per second: {0}" },
            { "Debug_TPS", "Ticks per second: {0}" },
            { "Debug_Zombies", "Zombie count: {0}" },
            { "Debug_Animals", "Animal count: {0}" },

            // Experience
            { "Experience_Help", "This gives a player some experience." },
            { "Experience_Usage", " <amount> [player]" },
            { "Experience_Invalid", "Invalid number of experience!" },
            { "Experience_Give", "Gave {0} {1} experience!" },

            // Filter
            { "Filter_Enable", "Succesfully enabled name filter!" },
            { "Filter_Help", "This filters out players with non-English-alphanumeric names." },

            // Flag
            { "Flag_Help", "This sets a player's flag." },
            { "Flag_Usage", " <flag> <value> [player]" },
            { "Flag_InvalidFlag", "Selected flag ID is invalid!" },
            { "Flag_InvalidValue", "Selected flag value is invalid!" },
            { "Flag_Set", "The flag for {0} has been set!" },

            // GameMode
            { "GameMode_Help", "This assigns the game mode of the server." },
            { "GameMode_Usage", " <game mode>" },
            { "GameMode_Set", "Server game mode set to {0}!" },

            // Gold
            { "Gold_Help", "This marks the server as joinable only by Gold members." },
            { "Gold_Set", "Server has been set to gold only!" },

            // Help
            { "Help_Help", "This displays all enabled commands or information on a specific command." },
            { "Help_Usage", " [command]" },

            // HideAdmins
            { "HideAdmins_Help", "This allows the recording of cinematic footage without the admin labels visible to players." },
            { "HideAdmins_Set", "Admin labels are now hidden!" },

            // Item
            { "Item_Help", "This gives the player an item." },
            { "Item_Usage", " <item> [amount] [player]" },
            { "Item_Invalid", "Failed to find the specified item!" },
            { "Item_Fail", "Failed to give the specified item to player!" },
            { "Item_Give", "{0} has been given to {1}" },

            // Kick
            { "Kick_Help", "This disconnects the specified player from the server." },
            { "Kick_Usage", " <player> [reason]" },
            { "Kick_Reason", "Undefined" },
            { "Kick_Kicked", "{0} has been kicked from the server!" },

            // Kill
            { "Kill_Help", "This kills the specified player in-game." },
            { "Kill_Usage", " [player]" },
            { "Kill_Killed", "{0} has been killed!" },

            // Log
            { "Log_Help", "This assigns the console log options." },
            { "Log_Usage", " <chat(y/n)> <join/leave(y/n)> <deaths(y/n)> <anticheat(y/n)>" },
            { "Log_Set", "Console logging settings have been set!" },

            // Map
            { "Map_Help", "This sets the map that the server loads on startup." },
            { "Map_Usage", " <map>" },
            { "Map_Invalid", "The specified map is invalid!" },
            { "Map_Set", "Server map has been set to {0}" },

            // MaxPlayers
            { "MaxPlayers_Help", "This sets the maximum number of connections the server is willing to accept." },
            { "MaxPlayers_Usage", " <max players>" },
            { "MaxPlayers_Invalid", "Invalid number of players!" },
            { "MaxPlayers_TooHigh", "Max player count cannot be higher than {0}!" },
            { "MaxPlayers_TooLow", "Max player count cannot be lower than {0}!" },
            { "MaxPlayers_Set", "Max players set to {0}!" },

            // Mode
            { "Mode_Help", "This assigns the difficulty of the server." },
            { "Mode_Usage", " <difficulty(easy/normal/hard)>" },
            { "Mode_Invalid", "The selected difficulty is invalid!" },
            { "Mode_Set", "The server difficulty has been set to {0}!" },

            // Modules
            { "Modules_Help", "This shows a list of the loaded modules." },
            { "Modules_Title", "Module list" },
            { "Modules_Name", "Name: {0}" },
            { "Modules_Version", "Version: {0}" },

            // Name
            { "Name_Help", "This assigns the name of the server on the server list." },
            { "Name_Usage", " <name>" },
            { "Name_TooLong", "The server name cannot be longer than {0}!" },
            { "Name_TooShort", "The server name cannot be shorter than {0}!" },
            { "Name_Set", "The server name has been set to {0}!" },

            // Night
            { "Night_Help", "This assigns the current time to night." },
            { "Night_Set", "The server time has been changed to night!" },

            // Owner
            { "Owner_Help", "This sets the owner of the server." },
            { "Owner_Usage", " <owner>" },
            { "Owner_Set", "The server owner has been set to {0}!" },

            // Password
            { "Password_Help", "This assigns the codeword required for entry to the server." },
            { "Password_Usage", " <password>" },
            { "Password_Removed", "The password has been removed from the server!" },
            { "Password_Set", "The server password has been set to {0}!" },

            // Permit
            { "Permit_Help", "This adds the specified player to the list of users allowed to join the server." },
            { "Permit_Usage", " <player> <tag>" },
            { "Permit_Added", "{0} has been added to the server whitelist!" },

            // Permits
            { "Permits_Help", "This shows a list of the current players allowed to join the server." },
            { "Permits_List", "Permits: {0}" },

            // Port
            { "Port_Help", "This assigns the port of the server. Port + 1 and port + 2 are also used, so remember to open them on the router as well." },
            { "Port_Usage", " <port>" },
            { "Port_Invalid", "The specified port is invalid!" },
            { "Port_Set", "The server port has been set to {0}!" },

            // PvE
            { "PvE_Help", "This allows player versus environment combat." },
            { "PvE_Set", "The server has been now set to PvE!" },

            // Quest
            { "Quest_Help", "This gives a player a flag." },
            { "Quest_Usage", " <quest> [player]" },
            { "Quest_Invalid", "The specified quest ID is invalid!" },
            { "Quest_Added", "Quest {0} has been added to {1}!" },

            // Queue
            { "Queue_Help", "This sets the maximum number of queued connections the server is willing to hold on to." },
            { "Queue_Usage", " <queue>" },
            { "Queue_Invalid", "The specified queue is invalid!" },
            { "Queue_TooHigh", "The queue number cannot be higher than {0}!" },
            { "Queue_Set", "The server queue has been set to {0}!" },

            // Reputation
            { "Reputation_Help", "This gives a player some reputation." },
            { "Reputation_Usage", " <reputation> [player]" },
            { "Reputation_Invalid", "The specified amount of reputation is invalid!" },
            { "Reputation_Give", "{0} has been given {1} reputation!" },

            // ResetConfig
            { "ResetConfig_Help", "This resets the config file to the default values." },
            { "ResetConfig_Reset", "The server configuration has been reset!" },

            // Save
            { "Save_Help", "This forces a proper save of the server state." },
            { "Save_Save", "The server state has been successfully saved!" },

            // Say
            { "Say_Help", "This broadcasts a message to all of the connected clients." },
            { "Say_Usage", " <message> [red] [green] [blue]" },

            // Shutdown
            { "Shutdown_Help", "This properly saves the server state, disconnects the clients and closes the program." },
            { "Shutdown_Usage", " [seconds until shutdown]" },

            // Slay
            { "Slay_Help", "This kills the specified player in-game and permanently bans them." },
            { "Slay_Usage", " <player> [reason]" },
            { "Slay_Slay", "{0} has been slayed!" },

            // Spy
            { "Spy_Help", "This requests a screenshot from the target player and saves it on the caller's computer as Spy.jpg." },
            { "Spy_Usage", " <player>" },
            { "Spy_Spy", "Successfully spyed {0}!" },

            // Storm
            { "Storm_Help", "This starts or stops the current rain cycle." },
            { "Storm_Change", "Weather has been chaged successfully!" },

            // Sync
            { "Sync_Help", "This allows players to share savedata between your servers." },
            { "Sync_Sync", "The server sync has been enabled!" },

            // Teleport
            { "Teleport_Help", "This teleports the first player to the second or a location." },
            { "Teleport_Usage", " <target/node> [player]" },
            { "Teleport_Invalid", "The specified teleport location is invalid!" },
            { "Teleport_Teleport", "{0} has been teleported to {1}!" },

            // Time
            { "Time_Help", "This assigns the current time in seconds of the day/night cycle." },
            { "Time_Usage", " <time>" },
            { "Time_Invalid", "The specified time is invalid!" },
            { "Time_Set", "The server time has been set to {0}!" },

            // Timeout
            { "Timeout_Help", "This assigns a maximum ping threshhold to the server before a client is kicked." },
            { "Timeout_Usage", " <timeout>" },
            { "Timeout_Invalid", "The specified timeout is invalid!" },
            { "Timeout_TooHigh", "The timeout cannot be higher than {0}!" },
            { "Timeout_TooLow", "The timeout cannot be lower than {0}!" },
            { "Timeout_Set", "The server timeout has been set to {0}!" },

            // UnAdmin
            { "Unadmin_Help", "This removes the specified player from the list of users allowed to use admin commands in the chat." },
            { "Unadmin_Usage", " <player>" },
            { "Unadmin_Unadmin", "{0} is no longer admin!" },

            // UnBan
            { "Unban_Help", "This removes the specified player from the list of users not allowed to join the server." },
            { "Unban_Usage", " <player>" },
            { "Unban_NotBanned", "{0} is not on the ban list!" },
            { "Unban_Unban", "{0} is no longer banned!" },

            // UnPermit
            { "Unpermit_Help", "This removes the specified player from the list of users allowed to join the server." },
            { "Unpermit_Usage", " <player>" },
            { "Unpermit_NotWhitelisted", "{0} is not on the whitelist!" },
            { "Unpermit_Unpermit", "{0} is no longer whitelisted!" },

            // Vehicle
            { "Vehicle_Help", "This gives a player a vehicle." },
            { "Vehicle_Usage", " <vehicle> [player]" },
            { "Vehicle_Invalid", "The specified vehicle is invalid!" },
            { "Vehicle_Fail", "Failed to spawn a {0}!" },
            { "Vehicle_Spawn", "{0} has been spawned!" },

            // Votify
            { "Votify_Help", "This configures voting for the server." },
            { "Votify_Usage", " <allowed> <pass cooldown> <fail cooldown> <duration> <percentage> <player count>" },
            { "Votify_Pass", "Invalid cooldown for a passed vote!" },
            { "Votify_Fail", "Invalid cooldown for a failed vote!" },
            { "Votify_Duration", "Invalid vote duration!" },
            { "Votify_Percentage", "Invalid vote percentage!" },
            { "Votify_Count", "Invalid player count for vote!" },
            { "Votify_Set", "The server vote settings have been set!" },

            // Welcome
            { "Welcome_Help", "This sets a welcome message shown to clients as they connect." },
            { "Welcome_Usage", " <message>" },
            { "Welcome_Set", "The server welcome message has been set!" },

            // Whitelisted
            { "Whitelisted_Help", "This makes only permited players allowed to join the server." },
            { "Whitelisted_Set", "The server is now in whitelist only mode!" },


            //------------------------ POINTBLANK COMMANDS ----------------------------//
            // Base for commands
            { "Base_NotEnoughArgs", "Not enough arguments specified!" },
            { "Base_InvalidGroup", "The specified group is invalid!" },

            // Group
            { "Group_Help", "This allows you to get information on groups or modify them." },
            { "Group_Usage", " {0}" },
            { "Group_NotFound", "The specified group has not been found!" },
            { "Group_Exists", "The specified group ID is already in use!" },
            { "Group_Usage_Add", " {0} <ID> <Name>" },
            { "Group_Usage_IDs", " {0}" },
            { "Group_Usage_List", " {0}" },
            { "Group_Usage_Permissions", " {0} <ID>" },
            { "Group_Usage_Remove", " {0} <ID>" },
            { "Group_Added", "The group has been added!" },
            { "Group_Removed", "The group has been removed!" },
            { "Group_Reloaded", "The group config has been reloaded!" },
            { "Group_Commands_Add", "Add" },
            { "Group_Commands_Help", "Help" },
            { "Group_Commands_IDs", "IDs" },
            { "Group_Commands_List", "List" },
            { "Group_Commands_Permissions", "Permissions" },
            { "Group_Commands_Remove", "Remove" },
            { "Group_Commands_Reload", "Reload" },

            // Player
            { "Player_Help", "This allows you to get information on the player or modify it." },
            { "Player_Usage", " {0}" },
            { "Player_Group", " {0} <player>" },
            { "Player_Group_Modify", " {0} <player> {1} <group ID>" },
            { "Player_Group_Added", "The group has been added to player!" },
            { "Player_Group_Removed", "The group has been removed from the player!" },
            { "Player_Reloaded", "The player config has been reloaded!" },
            { "Player_Permissions", " {0} <player>" },
            { "Player_Group_Invalid", "The specified group ID is invalid!" },
            { "Player_Commands_Groups", "Groups" },
            { "Player_Commands_Groups_Add", "Add" },
            { "Player_Commands_Groups_Remove", "Remove" },
            { "Player_Commands_Help", "Help" },
            { "Player_Commands_Permissions", "Permissions" },
            { "Player_Commands_Reload", "Reload" },

            // Usage
            { "Usage_Help", "This provides information on how to use the specified command." },
            { "Usage_Usage", " <command>" },

            // Permissions
            { "Permissions_Help", "This allows you to view or modify permissions of groups or players." },
            { "Permissions_Usage", " {0}" },
            { "Permissions_Commands_Help", "Help" },
            { "Permissions_Group", " {0} <group ID>" },
            { "Permissions_Group_Modify", " {0} <group ID> {1} <permission>" },
            { "Permissions_Player", " {0} <player>" },
            { "Permissions_Player_Modify", " {0} <player> {1} <permission>" },
            { "Permissions_Commands_Group", "Group" },
            { "Permissions_Commands_Player", "Player" },
            { "Permissions_Add_Success", "{0} has been successfully added to {1}!" },
            { "Permissions_Remove_Success", "{0} has been successfully removed from {1}!" },
            { "Permissions_Commands_Add", "Add" },
            { "Permissions_Commands_Remove", "Remove" },

            // PointBlank
            { "PointBlank_Help", "This allows you to view information about PointBlank and do actions to the framework" },
            { "PointBlank_Usage", " <reloadall/reloadsteam/version/restartplugins>" },
            { "PointBlank_ReloadSteam", "Steam group config has been reloaded!" },
            { "PointBlank_ReloadAll", "PointBlank has been reloaded!" },
            { "PointBlank_Version", "Version: {0}" },
            { "PointBlank_RestartPlugins", "PointBlank plugins have been restarted!" },
            { "PointBlank_Invalid", "Invalid option!" }
        };

        public Dictionary<Type, ITranslatable> TranslationDictionary => Enviroment.ServiceTranslations;
    }
}
