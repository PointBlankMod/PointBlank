using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using PointBlank.API;
using PointBlank.API.Steam;
using PointBlank.API.Groups;
using PointBlank.API.Services;
using PointBlank.API.DataManagment;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Server;
using UnityEngine;

namespace PointBlank.Services.APIManager
{
    [Service("InfoManager", true)]
    internal class InfoManager : Service
    {
        #region Info
        public static readonly string SteamGroupPath = ServerInfo.ConfigurationsPath + "/SteamGroups";
        public static readonly string PlayerPath = ServerInfo.ConfigurationsPath + "/Players";
        public static readonly string GroupPath = ServerInfo.ConfigurationsPath + "/Groups";
        #endregion

        #region Properties
        public UniversalData UniSteamGoupConfig { get; private set; }
        public UniversalData UniPlayerConfig { get; private set; }
        public UniversalData UniGroupConfig { get; private set; }

        public JsonData SteamGroupConfig { get; private set; }
        public JsonData PlayerConfig { get; private set; }
        public JsonData GroupConfig { get; private set; }
        #endregion

        #region Override Functions
        public override void Load()
        {
            // Setup universal configs
            UniSteamGoupConfig = new UniversalData(SteamGroupPath);
            UniPlayerConfig = new UniversalData(PlayerPath);
            UniGroupConfig = new UniversalData(GroupPath);

            // Setup configs
            SteamGroupConfig = UniSteamGoupConfig.GetData(EDataType.JSON) as JsonData;
            PlayerConfig = UniPlayerConfig.GetData(EDataType.JSON) as JsonData;
            GroupConfig = UniGroupConfig.GetData(EDataType.JSON) as JsonData;

            // Setup events
            ServerEvents.OnPlayerConnected += new ServerEvents.PlayerConnectionHandler(OnPlayerJoin);
            ServerEvents.OnPlayerDisconnected += new ServerEvents.PlayerConnectionHandler(OnPlayerLeave);

            // Load the configs
            if (!UniGroupConfig.CreatedNew)
                LoadGroups();
            else
                FirstGroups();
            if (!UniSteamGoupConfig.CreatedNew)
                LoadSteamGroups();
            else
                FirstSteamGroups();
            if (UniPlayerConfig.CreatedNew)
                FirstPlayers();
        }

        public override void Unload()
        {
            // Save the configs
            SaveGroups();
            SaveSteamGroups();
            SavePlayers();
        }
        #endregion

        #region Private Functions
        internal void LoadGroups()
        {
            foreach(JProperty obj in GroupConfig.Document.Properties())
            {
                if (GroupManager.Groups.Count(a => a.ID == obj.Name) > 0)
                    continue;

                if(!ColorUtility.TryParseHtmlString((string)obj.Value["Color"], out Color color))
                    color = Color.clear;

                Group g = new Group(obj.Name, (string)obj.Value["Name"], (bool)obj.Value["Default"], (int)obj.Value["Cooldown"], color);

                GroupManager.AddGroup(g);
            }

            foreach(Group g in GroupManager.Groups)
            {
                JObject obj = GroupConfig.Document[g.ID] as JObject;

                if(obj["Inherits"] is JArray)
                {
                    foreach (JToken token in (JArray)obj["Inherits"])
                    {
                        Group i = GroupManager.Groups.FirstOrDefault(a => a.ID == (string)token);

                        if (i == null || g.Inherits.Contains(i) || g == i)
                            continue;
                        g.AddInherit(i);
                    }
                }
                else
                {
                    Group i = GroupManager.Groups.FirstOrDefault(a => a.ID == (string)obj["Inherits"]);

                    if (i == null || g.Inherits.Contains(i) || g == i)
                        continue;
                    g.AddInherit(i);
                }
                if(obj["Permissions"] is JArray)
                {
                    foreach (JToken token in (JArray)obj["Permissions"])
                    {
                        if (g.Permissions.Contains((string)token))
                            continue;

                        g.AddPermission((string)token);
                    }
                }
                else
                {
                    if (g.Permissions.Contains((string)obj["Permissions"]))
                        continue;

                    g.AddPermission((string)obj["Permissions"]);
                }
                if(obj["Prefixes"] is JArray)
                {
                    foreach (JToken token in (JArray)obj["Prefixes"])
                    {
                        if (g.Prefixes.Contains((string)token))
                            continue;

                        g.AddPrefix((string)token);
                    }
                }
                else
                {
                    if (g.Prefixes.Contains((string)obj["Prefixes"]))
                        continue;

                    g.AddPrefix((string)obj["Prefixes"]);
                }
                if(obj["Suffixes"] is JArray)
                {
                    foreach (JToken token in (JArray)obj["Suffixes"])
                    {
                        if (g.Suffixes.Contains((string)token))
                            continue;

                        g.AddSuffix((string)token);
                    }
                }
                else
                {
                    if (g.Suffixes.Contains((string)obj["Suffixes"]))
                        continue;

                    g.AddSuffix((string)obj["Suffixes"]);
                }
            }
        }

        internal void FirstGroups()
        {
            // Create the groups
            Group guest = new Group("Guest", "Guest Group", true, -1, Color.clear);
            Group admin = new Group("Admin", "Admin Group", false, 0, Color.blue);

            // Configure guest group
            guest.AddPermission("unturned.commands.nonadmin.*");
            guest.AddPrefix("Guest");
            guest.AddSuffix("Guest");
            GroupManager.AddGroup(guest);

            // Configure admin group
            admin.AddPermission("unturned.commands.admin.*");
            admin.AddPrefix("Admin");
            admin.AddSuffix("Admin");
            admin.AddInherit(guest);
            GroupManager.AddGroup(admin);

            // Save the groups
            SaveGroups();
        }

        internal void SaveGroups()
        {
            foreach(Group g in GroupManager.Groups)
            {
                if(GroupConfig.Document[g.ID] != null)
                {
                    JObject obj = GroupConfig.Document[g.ID] as JObject;

                    obj["Permissions"] = JToken.FromObject(g.Permissions);
                    obj["Prefixes"] = JToken.FromObject(g.Prefixes);
                    obj["Suffixes"] = JToken.FromObject(g.Suffixes);
                    obj["Inherits"] = JToken.FromObject(g.Inherits.Select(a => a.ID));
                    obj["Cooldown"] = g.Cooldown;
                    obj["Color"] = (g.Color == Color.clear ? "none" : "#" + ColorUtility.ToHtmlStringRGB(g.Color));
                }
                else
                {
                    JObject obj = new JObject
                    {
                        {"Name", g.Name},
                        {"Default", g.Default},
                        {"Permissions", JToken.FromObject(g.Permissions)},
                        {"Prefixes", JToken.FromObject(g.Prefixes)},
                        {"Suffixes", JToken.FromObject(g.Suffixes)},
                        {"Inherits", JToken.FromObject(g.Inherits.Select(a => a.ID))},
                        {"Cooldown", g.Cooldown},
                        {"Color", (g.Color == Color.clear ? "none" : "#" + ColorUtility.ToHtmlStringRGB(g.Color))}
                    };


                    GroupConfig.Document.Add(g.ID, obj);
                }
            }
            UniGroupConfig.Save();
        }

        internal void LoadSteamGroups()
        {
            foreach(JObject obj in (JArray)SteamGroupConfig.Document["SteamGroups"])
            {
                if (SteamGroupManager.Groups.Count(a => a.ID == (ulong)obj["Steam64"]) > 0)
                    continue;

                SteamGroup g = new SteamGroup((ulong)obj["Steam64"], (int)obj["Cooldown"], false, false);

                SteamGroupManager.AddSteamGroup(g);
            }

            foreach(SteamGroup g in SteamGroupManager.Groups)
            {
                JObject obj = SteamGroupConfig.Document["SteamGroups"].FirstOrDefault(a => (ulong)a["Steam64"] == g.ID) as JObject;

                if(obj["Inherits"] is JArray)
                {
                    foreach(JToken token in (JArray)obj["Inherits"])
                    {
                        if (string.IsNullOrEmpty((string)token))
                            continue;
                        SteamGroup i = SteamGroupManager.Groups.FirstOrDefault(a => a.ID == ulong.Parse((string)token));

                        if (i == null || g.Inherits.Contains(i) || g == i)
                            continue;
                        g.AddInherit(i);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty((string)obj["Inherits"]))
                        continue;
                    SteamGroup i = SteamGroupManager.Groups.FirstOrDefault(a => a.ID == ulong.Parse((string)obj["Inherits"]));

                    if (i == null || g.Inherits.Contains(i) || g == i)
                        continue;
                    g.AddInherit(i);
                }
                if(obj["Permissions"] is JArray)
                {
                    foreach(JToken token in (JArray)obj["Permissions"])
                    {
                        if (g.Permissions.Contains((string)token))
                            continue;

                        g.AddPermission((string)token);
                    }
                }
                else
                {
                    if (g.Permissions.Contains((string)obj["Permissions"]))
                        continue;

                    g.AddPermission((string)obj["Permissions"]);
                }
                if(obj["Prefixes"] is JArray)
                {
                    foreach(JToken token in (JArray)obj["Prefixes"])
                    {
                        if (g.Prefixes.Contains((string)token))
                            continue;

                        g.AddPrefix((string)token);
                    }
                }
                else
                {
                    if (g.Prefixes.Contains((string)obj["Prefixes"]))
                        continue;

                    g.AddPrefix((string)obj["Prefixes"]);
                }
                if(obj["Suffixes"] is JArray)
                {
                    foreach(JToken token in (JArray)obj["Suffixes"])
                    {
                        if (g.Suffixes.Contains((string)token))
                            continue;

                        g.AddSuffix((string)token);
                    }
                }
                else
                {
                    if (g.Suffixes.Contains((string)obj["Suffixes"]))
                        continue;

                    g.AddSuffix((string)obj["Suffixes"]);
                }
            }
        }

        internal void FirstSteamGroups()
        {
            // Create the array
            SteamGroupConfig.Document.Add("SteamGroups", new JArray());

            // Ceate the groups
            SteamGroup group = new SteamGroup(103582791437463178, -1, false, false);

            // Configure steam group
            group.AddPermission("unturned.commands.noadmin.*");
            group.AddPrefix("Workshopper");
            group.AddSuffix("Workshopper");
            SteamGroupManager.AddSteamGroup(group);

            // Save the groups
            SaveSteamGroups();
        }

        internal void SaveSteamGroups()
        {
            foreach(SteamGroup g in SteamGroupManager.Groups)
            {
                if (g.Ignore)
                    continue;

                var obj = SteamGroupConfig.Document["SteamGroups"].FirstOrDefault(a => (ulong)a["Steam64"] == g.ID);
                if (obj != null)
                {
                    obj["Permissions"] = JToken.FromObject(g.Permissions);
                    obj["Prefixes"] = JToken.FromObject(g.Prefixes);
                    obj["Suffixes"] = JToken.FromObject(g.Suffixes);
                    obj["Inherits"] = JToken.FromObject(g.Inherits.Select(a => a.ID.ToString()));
                    obj["Cooldown"] = g.Cooldown;
                }
                else
                {
                    obj = new JObject();

                    ((JObject)obj).Add("Steam64", g.ID);
                    ((JObject)obj).Add("Permissions", JToken.FromObject(g.Permissions));
                    ((JObject)obj).Add("Prefixes", JToken.FromObject(g.Prefixes));
                    ((JObject)obj).Add("Suffixes", JToken.FromObject(g.Suffixes));
                    ((JObject)obj).Add("Inherits", JToken.FromObject(g.Inherits.Select(a => a.ID)));
                    ((JObject)obj).Add("Cooldown", g.Cooldown);

                    ((JArray)SteamGroupConfig.Document["SteamGroups"]).Add(obj);
                }
            }
            UniSteamGoupConfig.Save();
        }

        internal void FirstPlayers() => PlayerConfig.Document.Add("Players", new JArray());

        internal void SavePlayers() // Force save players
        {
            JArray arr = PlayerConfig.Document["Players"] as JArray;

            foreach(UnturnedPlayer player in UnturnedServer.Players)
            {
                JToken token = arr.FirstOrDefault(a => (string)a["Steam64"] == player.SteamID.ToString());

                if(token != null)
                {
                    token["Cooldown"] = player.Cooldown;
                    token["Permissions"] = JToken.FromObject(player.Permissions);
                    token["Prefixes"] = JToken.FromObject(player.Prefixes);
                    token["Suffixes"] = JToken.FromObject(player.Suffixes);
                    token["Groups"] = JToken.FromObject(player.Groups.Select(a => a.ID));
                    token["Kills"] = player.TotalKills;
                    token["Deaths"] = player.TotalDeaths;
                }
                else
                {
                    JObject obj = new JObject
                    {
                        {"Steam64", player.SteamID.ToString()},
                        {"Permissions", JToken.FromObject(player.Permissions)},
                        {"Groups", JToken.FromObject(player.Groups.Select(a => a.ID))},
                        {"Prefixes", JToken.FromObject(player.Prefixes)},
                        {"Suffixes", JToken.FromObject(player.Suffixes)},
                        {"Cooldown", player.Cooldown},
                        {"Kills", player.TotalKills},
                        {"Deaths", player.TotalDeaths}
                    };


                    arr.Add(obj);
                }
            }
            UniPlayerConfig.Save();
        }
        #endregion

        #region Event Functions
        internal void OnPlayerJoin(UnturnedPlayer player)
        {
            JArray arr = PlayerConfig.Document["Players"] as JArray;
            JToken token = arr.FirstOrDefault(a => (string)a["Steam64"] == player.SteamID.ToString());

            if(token != null)
            {
                player.Cooldown = (int)token["Cooldown"];
                player.TotalKills = (int)token["Kills"];
                player.TotalDeaths = (int)token["Deaths"];

                if (token["Permissions"] is JArray)
                {
                    foreach(JToken t in (JArray)token["Permissions"])
                    {
                        if (player.Permissions.Contains((string)t))
                            continue;

                        player.AddPermission((string)t);
                    }
                }
                else
                {
                    if (!player.Permissions.Contains((string)token["Permissions"]))
                        player.AddPermission((string)token["Permissions"]);
                }
                if(token["Groups"] is JArray)
                {
                    foreach(JToken t in (JArray)token["Groups"])
                    {
                        Group g = GroupManager.Groups.FirstOrDefault(a => a.ID == (string)t);

                        if (g == null || player.Groups.Contains(g))
                            continue;
                        player.AddGroup(g);
                    }
                }
                else
                {
                    Group g = GroupManager.Groups.FirstOrDefault(a => a.ID == (string)token["Groups"]);

                    if (g != null && !player.Groups.Contains(g))
                        player.AddGroup(g);
                }
                if(token["Prefixes"] is JArray)
                {
                    foreach(JToken t in (JArray)token["Prefixes"])
                    {
                        if (player.Prefixes.Contains((string)t))
                            continue;

                        player.AddPrefix((string)t);
                    }
                }
                else
                {
                    if (!player.Prefixes.Contains((string)token["Prefixes"]))
                        player.AddPrefix((string)token["Prefixes"]);
                }
                if(token["Suffixes"] is JArray)
                {
                    foreach(JToken t in (JArray)token["Suffixes"])
                    {
                        if (player.Suffixes.Contains((string)t))
                            continue;

                        player.AddSuffix((string)t);
                    }
                }
                else
                {
                    if (!player.Suffixes.Contains((string)token["Suffixes"]))
                        player.AddSuffix((string)token["Suffixes"]);
                }
            }

            player.UnturnedCharacterName = player.CharacterName;
            player.UnturnedNickName = player.NickName;
            player.CharacterName = player.GetPrefix() + player.UnturnedCharacterName + player.GetSuffix();
            player.NickName = player.GetPrefix() + player.UnturnedNickName + player.GetSuffix();
            player.Loaded = true;
        }

        internal void OnPlayerLeave(UnturnedPlayer player)
        {
            JArray arr = PlayerConfig.Document["Players"] as JArray;
            JToken token = arr.FirstOrDefault(a => (string)a["Steam64"] == player.SteamID.ToString());

            if (token != null)
            {
                token["Cooldown"] = player.Cooldown;
                token["Permissions"] = JToken.FromObject(player.Permissions);
                token["Prefixes"] = JToken.FromObject(player.Prefixes);
                token["Suffixes"] = JToken.FromObject(player.Suffixes);
                token["Groups"] = JToken.FromObject(player.Groups.Select(a => a.ID));
                token["Kills"] = player.TotalKills;
                token["Deaths"] = player.TotalDeaths;
            }
            else
            {
                JObject obj = new JObject
                {
                    {"Steam64", player.SteamID.ToString()},
                    {"Permissions", JToken.FromObject(player.Permissions)},
                    {"Groups", JToken.FromObject(player.Groups.Select(a => a.ID))},
                    {"Prefixes", JToken.FromObject(player.Prefixes)},
                    {"Suffixes", JToken.FromObject(player.Suffixes)},
                    {"Cooldown", player.Cooldown},
                    {"Kills", player.TotalKills},
                    {"Deaths", player.TotalDeaths}
                };


                arr.Add(obj);
            }
            UniPlayerConfig.Save();
        }
        #endregion
    }
}
