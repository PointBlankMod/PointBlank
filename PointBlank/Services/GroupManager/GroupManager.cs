using System.Linq;
using PointBlank.API.Groups;
using PointBlank.API.Server;
using PointBlank.API.Services;
using PointBlank.API.Permissions;
using PointBlank.API.DataManagment;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace PointBlank.Services.GroupManager
{
    internal class GroupManager : PointBlankService
    {
        #region Properties
        public static string GroupPath => PointBlankServer.ConfigurationsPath + "/Groups";

        public UniversalData UniGroupConfig { get; private set; }

        public JsonData GroupConfig { get; private set; }

        public override int LaunchIndex => 0;
        #endregion

        public override void Load()
        {
            // Setup config
            UniGroupConfig = new UniversalData(GroupPath);
            GroupConfig = UniGroupConfig.GetData(EDataType.Json) as JsonData;

            if (!UniGroupConfig.CreatedNew)
                LoadGroups();
            else
                FirstGroups();
        }

        public override void Unload() => SaveGroups();

        #region Private Functions
        internal void LoadGroups()
        {
            PointBlankGroupManager.Loaded = false;
            foreach (JProperty obj in GroupConfig.Document.Properties())
            {
                if (PointBlankGroupManager.Groups.Count(a => a.Id == obj.Name) > 0)
                    continue;

                PointBlankGroup g = new PointBlankGroup(obj.Name);

                PointBlankGroupManager.AddGroup(g);
            }

            foreach (PointBlankGroup g in PointBlankGroupManager.Groups)
            {
                JObject obj = GroupConfig.Document[g.Id] as JObject;

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
                if (!ColorUtility.TryParseHtmlString((string)obj["Color"], out Color color))
                    color = Color.clear;
                g.Color = color;
                if (obj["Inherits"] is JArray)
                {
                    foreach (JToken token in (JArray)obj["Inherits"])
                    {
                        PointBlankGroup i = PointBlankGroupManager.Groups.FirstOrDefault(a => a.Id == (string)token);

                        if (i == null || g.Inherits.Contains(i) || g == i)
                            continue;
                        g.AddInherit(i);
                    }
                }
                else
                {
                    PointBlankGroup i = PointBlankGroupManager.Groups.FirstOrDefault(a => a.Id == (string)obj["Inherits"]);

                    if (i == null || g.Inherits.Contains(i) || g == i)
                        continue;
                    g.AddInherit(i);
                }
                if (obj["Permissions"] is JArray)
                {
                    foreach (JToken token in (JArray)obj["Permissions"])
                    {
                        if (g.Permissions.Contains(token.ToObject<PointBlankPermission>()))
                            continue;

                        g.AddPermission(token.ToObject<PointBlankPermission>());
                    }
                }
                else
                {
                    if (g.Permissions.Contains(obj["Permissions"].ToObject<PointBlankPermission>()))
                        continue;

                    g.AddPermission(obj["Permissions"].ToObject<PointBlankPermission>());
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
            PointBlankGroupManager.Loaded = true;
        }

        internal void FirstGroups()
        {
            // Create the groups
            PointBlankGroup guest = new PointBlankGroup("Guest", "Guest Group", true, Color.clear);
            PointBlankGroup admin = new PointBlankGroup("Admin", "Admin Group", false, Color.blue);

            // Configure guest group
            guest.AddPermission(new PointBlankPermission("unturned.commands.nonadmin.*", 0));
            guest.AddPrefix("Guest");
            guest.AddSuffix("Guest");
            PointBlankGroupManager.AddGroup(guest);

            // Configure admin group
            admin.AddPermission(new PointBlankPermission("unturned.commands.admin.*", 0));
            admin.AddPrefix("Admin");
            admin.AddSuffix("Admin");
            admin.AddInherit(guest);
            PointBlankGroupManager.AddGroup(admin);

            // Save the groups
            SaveGroups();
        }

        internal void SaveGroups()
        {
            foreach (PointBlankGroup g in PointBlankGroupManager.Groups)
            {
                if (GroupConfig.Document[g.Id] != null)
                {
                    JObject obj = GroupConfig.Document[g.Id] as JObject;

                    obj["Permissions"] = JToken.FromObject(g.Permissions);
                    obj["Prefixes"] = JToken.FromObject(g.Prefixes);
                    obj["Suffixes"] = JToken.FromObject(g.Suffixes);
                    obj["Inherits"] = JToken.FromObject(g.Inherits.Select(a => a.Id));
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
                        {"Inherits", JToken.FromObject(g.Inherits.Select(a => a.Id))},
                        {"Color", (g.Color == Color.clear ? "none" : "#" + ColorUtility.ToHtmlStringRGB(g.Color))}
                    };


                    GroupConfig.Document.Add(g.Id, obj);
                }
            }
            UniGroupConfig.Save();
        }
        #endregion
    }
}
