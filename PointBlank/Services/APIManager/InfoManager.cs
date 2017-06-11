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

            // Load the configs
            if (!UniGroupConfig.CreatedNew)
                LoadGroups();
            else
                FirstGroups();
            if (!UniSteamGoupConfig.CreatedNew)
                LoadSteamGroups();
            else
                FirstSteamGroups();
        }

        public override void Unload()
        {
            // Save the configs
            SaveGroups();
            SaveSteamGroups();
        }
        #endregion

        #region Private Functions
        private void LoadGroups()
        {
            foreach(JProperty obj in GroupConfig.Document.Properties())
            {
                if (GroupManager.Groups.Count(a => a.ID == obj.Name) > 0)
                    continue;

                Group g = new Group(obj.Name, (string)obj.Value["Name"], (bool)obj.Value["Default"], (int)obj.Value["Cooldown"]);

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

                        if (i == null)
                            continue;
                        g.AddInherit(i);
                    }
                }
                else
                {
                    Group i = GroupManager.Groups.FirstOrDefault(a => a.ID == (string)obj["Inherits"]);

                    if (i == null)
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

        private void FirstGroups()
        {
            // Create the groups
            Group guest = new Group("Guest", "Guest Group", true, 100);
            Group admin = new Group("Admin", "Admin Group", false, 0);

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

        private void SaveGroups()
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
                }
                else
                {
                    JObject obj = new JObject();

                    obj.Add("Name", g.Name);
                    obj.Add("Default", g.Default);
                    obj.Add("Permissions", JToken.FromObject(g.Permissions));
                    obj.Add("Prefixes", JToken.FromObject(g.Prefixes));
                    obj.Add("Suffixes", JToken.FromObject(g.Suffixes));
                    obj.Add("Inherits", JToken.FromObject(g.Inherits.Select(a => a.ID)));
                    obj.Add("Cooldown", g.Cooldown);

                    GroupConfig.Document.Add(g.ID, obj);
                }
            }
            UniGroupConfig.Save();
        }

        private void LoadSteamGroups()
        {
            foreach(JProperty obj in SteamGroupConfig.Document.Properties())
            {
                if (SteamGroupManager.Groups.Count(a => a.ID == ulong.Parse(obj.Name)) > 0)
                    continue;

                SteamGroup g = new SteamGroup(ulong.Parse(obj.Name), (int)obj.Value["Cooldown"]);

                SteamGroupManager.AddSteamGroup(g);
            }

            foreach(SteamGroup g in SteamGroupManager.Groups)
            {
                JObject obj = SteamGroupConfig.Document[g.ID.ToString()] as JObject;

                if(obj["Inherits"] is JArray)
                {
                    foreach(JToken token in (JArray)obj["Inherits"])
                    {
                        SteamGroup i = SteamGroupManager.Groups.FirstOrDefault(a => a.ID == ulong.Parse((string)token));

                        if (i == null)
                            continue;
                        g.AddInherit(i);
                    }
                }
                else
                {
                    SteamGroup i = SteamGroupManager.Groups.FirstOrDefault(a => a.ID == ulong.Parse((string)obj["Inherits"]));

                    if (i == null)
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

        private void FirstSteamGroups()
        {
            // Ceate the groups
            SteamGroup group = new SteamGroup(103582791437463178, 100);

            // Configure steam group
            group.AddPermission("unturned.commands.noadmin.*");
            group.AddPrefix("Workshopper");
            group.AddSuffix("Workshopper");
            SteamGroupManager.AddSteamGroup(group);

            // Save the groups
            SaveSteamGroups();
        }

        private void SaveSteamGroups()
        {
            foreach(SteamGroup g in SteamGroupManager.Groups)
            {
                if(SteamGroupConfig.Document[g.ID.ToString()] != null)
                {
                    JObject obj = SteamGroupConfig.Document[g.ID.ToString()] as JObject;

                    obj["Permissions"] = JToken.FromObject(g.Permissions);
                    obj["Prefixes"] = JToken.FromObject(g.Prefixes);
                    obj["Suffixes"] = JToken.FromObject(g.Suffixes);
                    obj["Inherits"] = JToken.FromObject(g.Inherits.Select(a => a.ID.ToString()));
                    obj["Cooldown"] = g.Cooldown;
                }
                else
                {
                    JObject obj = new JObject();

                    obj.Add("Permissions", JToken.FromObject(g.Permissions));
                    obj.Add("Prefixes", JToken.FromObject(g.Prefixes));
                    obj.Add("Suffixes", JToken.FromObject(g.Suffixes));
                    obj.Add("Inherits", JToken.FromObject(g.Inherits.Select(a => a.ID)));
                    obj.Add("Cooldown", g.Cooldown);

                    SteamGroupConfig.Document.Add(g.ID.ToString(), obj);
                }
            }
            UniSteamGoupConfig.Save();
        }
        #endregion

        #region Event Functions
        #endregion
    }
}
