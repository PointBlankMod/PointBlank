using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Groups;
using PointBlank.API.Server;
using PointBlank.API.Services;
using PointBlank.API.DataManagment;
using Newtonsoft.Json.Linq;
using UnityEngine;
using GM = PointBlank.API.Groups.PointBlankGroupManager;

namespace PointBlank.Services.GroupManager
{
    internal class GroupManager : PointBlankService
    {
        #region Info
        public static readonly string GroupPath = PointBlankServer.ConfigurationsPath + "/Groups";
        #endregion

        #region Properties
        public UniversalData UniGroupConfig { get; private set; }

        public JsonData GroupConfig { get; private set; }

        public override int LaunchIndex => 0;
        #endregion

        public override void Load()
        {
            // Setup config
            UniGroupConfig = new UniversalData(GroupPath);
            GroupConfig = UniGroupConfig.GetData(EDataType.JSON) as JsonData;

            if (!UniGroupConfig.CreatedNew)
                LoadGroups();
            else
                FirstGroups();
        }

        public override void Unload() => SaveGroups();

        #region Private Functions
        internal void LoadGroups()
        {
            GM.Loaded = false;
            foreach (JProperty obj in GroupConfig.Document.Properties())
            {
                if (GM.Groups.Count(a => a.ID == obj.Name) > 0)
                    continue;

                PointBlankGroup g = new PointBlankGroup(obj.Name);

                GM.AddGroup(g);
            }

            foreach (PointBlankGroup g in GM.Groups)
            {
                JObject obj = GroupConfig.Document[g.ID] as JObject;

                while (g.Inherits.Length > 0)
                    g.RemoveInherit(g.Inherits[0]);
                while (g.Permissions.Length > 0)
                    g.RemovePermission(g.Permissions[0]);
                while (g.Prefixes.Length > 0)
                    g.RemovePrefix(g.Prefixes[0]);
                while (g.Suffixes.Length > 0)
                    g.RemoveSuffix(g.Suffixes[0]);
                g.Name = (string)obj["Name"];
                g.Default = (bool)obj["Default"];
                g.Cooldown = (int)obj["Cooldown"];
                if (!ColorUtility.TryParseHtmlString((string)obj["Color"], out Color color))
                    color = Color.clear;
                g.Color = color;
                if (obj["Inherits"] is JArray)
                {
                    foreach (JToken token in (JArray)obj["Inherits"])
                    {
                        PointBlankGroup i = GM.Groups.FirstOrDefault(a => a.ID == (string)token);

                        if (i == null || g.Inherits.Contains(i) || g == i)
                            continue;
                        g.AddInherit(i);
                    }
                }
                else
                {
                    PointBlankGroup i = GM.Groups.FirstOrDefault(a => a.ID == (string)obj["Inherits"]);

                    if (i == null || g.Inherits.Contains(i) || g == i)
                        continue;
                    g.AddInherit(i);
                }
                if (obj["Permissions"] is JArray)
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
                if (obj["Prefixes"] is JArray)
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
                if (obj["Suffixes"] is JArray)
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
            GM.Loaded = true;
        }

        internal void FirstGroups()
        {
            // Create the groups
            PointBlankGroup guest = new PointBlankGroup("Guest", "Guest Group", true, -1, Color.clear);
            PointBlankGroup admin = new PointBlankGroup("Admin", "Admin Group", false, 0, Color.blue);

            // Configure guest group
            guest.AddPermission("unturned.commands.nonadmin.*");
            guest.AddPrefix("Guest");
            guest.AddSuffix("Guest");
            GM.AddGroup(guest);

            // Configure admin group
            admin.AddPermission("unturned.commands.admin.*");
            admin.AddPrefix("Admin");
            admin.AddSuffix("Admin");
            admin.AddInherit(guest);
            GM.AddGroup(admin);

            // Save the groups
            SaveGroups();
        }

        internal void SaveGroups()
        {
            foreach (PointBlankGroup g in GM.Groups)
            {
                if (GroupConfig.Document[g.ID] != null)
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
        #endregion
    }
}
