using System;
using System.Net;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.DataManagment;

namespace PointBlank.API.Steam
{
    /// <summary>
    /// The steam group instance
    /// </summary>
    public class SteamGroup
    {
        #region Variables
        private List<string> _Permissions = new List<string>();
        private List<SteamGroup> _Inherits = new List<SteamGroup>();

        private List<string> _Prefixes = new List<string>();
        private List<string> _Suffixes = new List<string>();
        #endregion

        #region Properties
        /// <summary>
        /// The steam group name
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The steam group ID
        /// </summary>
        public ulong ID { get; private set; }
        /// <summary>
        /// The amount of members in the group
        /// </summary>
        public int MemberCount { get; private set; }
        /// <summary>
        /// The amount of members currently online
        /// </summary>
        public int MembersOnline { get; private set; }
        /// <summary>
        /// The amount of members in game
        /// </summary>
        public int MembersInGame { get; private set; }
        /// <summary>
        /// The amount of members in chat
        /// </summary>
        public int MembersInChat { get; private set; }

        /// <summary>
        /// The group permissions
        /// </summary>
        public string[] Permissions => _Permissions.ToArray();
        /// <summary>
        /// The group inherits
        /// </summary>
        public SteamGroup[] Inherits => _Inherits.ToArray();

        /// <summary>
        /// The group prefixes
        /// </summary>
        public string[] Prefixes => _Prefixes.ToArray();
        /// <summary>
        /// The group suffixes
        /// </summary>
        public string[] Suffixes => _Suffixes.ToArray();

        /// <summary>
        /// The group cooldown for commands
        /// </summary>
        public int Cooldown { get; set; }

        /// <summary>
        /// Should the group be ignored while saving
        /// </summary>
        public bool Ignore { get; private set; }
        #endregion

        /// <summary>
        /// The steam group instance using async
        /// </summary>
        /// <param name="id">The ID of the steam group</param>
        /// <param name="cooldown">The default cooldown for the steam group</param>
        /// <param name="downloadData">Should the information for the steam group be downloaded</param>
        /// <param name="ignore">Should the group be ignored while saving</param>
        public SteamGroup(ulong id, int cooldown = -1, bool downloadData = false, bool ignore = true)
        {
            // Set the variables
            this.ID = id;
            this.Cooldown = cooldown;
            this.Ignore = ignore;

            // Run the code
            if (downloadData)
                DownloadData();
        }

        #region Public Functions
        /// <summary>
        /// Downloads the steam group data from steam
        /// </summary>
        public void DownloadData()
        {
            XmlDocument document = new XmlDocument();
            document.Load($"http://steamcommunity.com/gid/{ID.ToString()}/memberslistxml/?xml=1");
            XmlNode root = document.DocumentElement;

            // Set the data
            if (root != null)
            {
                Name = root.SelectSingleNode("groupDetails/groupName").InnerText.Replace("<![CDATA[ ", "").Replace(" ]]>", "");
                MemberCount = int.Parse(root.SelectSingleNode("groupDetails/memberCount").InnerText);
                MembersOnline = int.Parse(root.SelectSingleNode("groupDetails/membersOnline").InnerText);
                MembersInGame = int.Parse(root.SelectSingleNode("groupDetails/membersInGame").InnerText);
                MembersInChat = int.Parse(root.SelectSingleNode("groupDetails/membersInChat").InnerText);
            }
            else
            {
                Name = "";
                MemberCount = 0;
                MembersOnline = 0;
                MembersInGame = 0;
                MembersInChat = 0;
            }
        }

        /// <summary>
        /// Add a permission to the steam group
        /// </summary>
        /// <param name="permission">The permission to add</param>
        public void AddPermission(string permission)
        {
            if (_Permissions.Contains(permission))
                return;

            _Permissions.Add(permission);
            SteamGroupEvents.RunPermissionAdded(this, permission);
        }

        /// <summary>
        /// Remove a permission from the steam group
        /// </summary>
        /// <param name="permission">The permission to remove</param>
        public void RemovePermission(string permission)
        {
            if (!_Permissions.Contains(permission))
                return;

            _Permissions.Remove(permission);
            SteamGroupEvents.RunPermissionRemoved(this, permission);
        }

        /// <summary>
        /// Add a prefix to the steam group
        /// </summary>
        /// <param name="prefix">The prefix to add</param>
        public void AddPrefix(string prefix)
        {
            if (_Prefixes.Contains(prefix))
                return;

            _Prefixes.Add(prefix);
            SteamGroupEvents.RunPrefixAdded(this, prefix);
        }

        /// <summary>
        /// Remove a prefix from the steam group
        /// </summary>
        /// <param name="prefix">The prefix to remove</param>
        public void RemovePrefix(string prefix)
        {
            if (!_Prefixes.Contains(prefix))
                return;

            _Prefixes.Remove(prefix);
            SteamGroupEvents.RunPrefixRemoved(this, prefix);
        }

        /// <summary>
        /// Adds a suffix to the steam group
        /// </summary>
        /// <param name="suffix">The suffix to add</param>
        public void AddSuffix(string suffix)
        {
            if (_Suffixes.Contains(suffix))
                return;

            _Suffixes.Add(suffix);
            SteamGroupEvents.RunSuffixAdded(this, suffix);
        }

        /// <summary>
        /// Removes a suffix from the steam group
        /// </summary>
        /// <param name="suffix">The suffix to remove</param>
        public void RemoveSuffix(string suffix)
        {
            if (!_Suffixes.Contains(suffix))
                return;

            _Suffixes.Remove(suffix);
            SteamGroupEvents.RunSuffixRemoved(this, suffix);
        }

        /// <summary>
        /// Adds an inherit to the steam group
        /// </summary>
        /// <param name="group">The group to add to inherits</param>
        public void AddInherit(SteamGroup group)
        {
            if (_Inherits.Contains(group))
                return;
            if (_Inherits.Count(a => a.ID == group.ID) > 0)
                return;

            _Inherits.Add(group);
            SteamGroupEvents.RunInheritAdded(this, group);
        }

        /// <summary>
        /// Removes an inherit from the steam group
        /// </summary>
        /// <param name="group">The inherit to remove</param>
        public void RemoveInherit(SteamGroup group)
        {
            if (!_Inherits.Contains(group))
                return;

            _Inherits.Remove(group);
            SteamGroupEvents.RunInheritRemoved(this, group);
        }

        /// <summary>
        /// Gets the list of all permissions including inheritences
        /// </summary>
        /// <returns>The list of all permissions including inheritences</returns>
        public string[] GetPermissions()
        {
            List<string> permissions = new List<string>();

            permissions.AddRange(Permissions);
            PBTools.ForeachLoop<SteamGroup>(Inherits, delegate (int index, SteamGroup value)
            {
                PBTools.ForeachLoop<string>(value.GetPermissions(), delegate (int i, string v)
                {
                    if (permissions.Contains(v))
                        return;

                    permissions.Add(v);
                });
            });

            return permissions.ToArray();
        }

        /// <summary>
        /// Checks if the steam group has the permission specified
        /// </summary>
        /// <param name="permission">The permission to check for</param>
        /// <returns>If the steam group has the permission specified</returns>
        public bool HasPermission(string permission)
        {
            string[] permissions = GetPermissions();
            string[] sPermission = permission.Split('.');

            for (int a = 0; a < sPermission.Length; a++)
            {
                bool found = false;

                for (int b = 0; b < permissions.Length; b++)
                {
                    string[] sPerm = permissions[b].Split('.');

                    if (sPerm[a] == "*")
                        return true;
                    if (sPerm[a] != sPermission[a]) continue;
                    found = true;
                    break;
                }

                if (!found)
                    return false;
            }
            return true;
        }
        #endregion
    }
}
