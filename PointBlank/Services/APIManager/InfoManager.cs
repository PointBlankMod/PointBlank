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

                foreach(JToken token in (JArray)obj["Inherits"])
                {
                    Group i = GroupManager.Groups.FirstOrDefault(a => a.ID == (string)token);

                    if (i == null)
                        continue;
                    g.AddInherit(i);
                }
                foreach(JToken token in (JArray)obj["Permissions"])
                {
                    if (g.Permissions.Contains((string)token))
                        continue;

                    g.AddPermission((string)token);
                }
                foreach (JToken token in (JArray)obj["Prefixes"])
                {
                    if (g.Prefixes.Contains((string)token))
                        continue;

                    g.AddPrefix((string)token);
                }
                foreach (JToken token in (JArray)obj["Suffixes"])
                {
                    if (g.Suffixes.Contains((string)token))
                        continue;

                    g.AddSuffix((string)token);
                }
            }
        }

        private void FirstGroups()
        {
            Group g = new Group("Guest", "Guest Group", true, 100);
        }

        private void SaveGroups()
        {

        }

        private void LoadSteamGroups()
        {

        }

        private void FirstSteamGroups()
        {

        }

        private void SaveSteamGroups()
        {

        }
        #endregion

        #region Event Functions
        #endregion
    }
}
