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
        public string[] Permissions { get { return _Permissions.ToArray(); } }
        /// <summary>
        /// The group inherits
        /// </summary>
        public SteamGroup[] Inherits { get { return _Inherits.ToArray(); } }

        /// <summary>
        /// The group prefixes
        /// </summary>
        public string[] Prefixes { get { return _Prefixes.ToArray(); } }
        /// <summary>
        /// The group suffixes
        /// </summary>
        public string[] Suffixes { get { return _Suffixes.ToArray(); } }

        /// <summary>
        /// The group cooldown for commands
        /// </summary>
        public int Cooldown { get; set; }
        #endregion

        /// <summary>
        /// The steam group instance
        /// </summary>
        /// <param name="id">The ID of the steam group</param>
        public SteamGroup(ulong id)
        {
            // Set the variables
            this.ID = id;
            this.Cooldown = -1;

            // Setup the XML
            XmlDocument document = new XmlDocument();
            document.LoadXml(string.Format("http://steamcommunity.com/gid/{0}/memberslistxml/?xml=1", ID.ToString()));
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
        /// The steam group instance using async
        /// </summary>
        /// <param name="id">The ID of the steam group</param>
        /// <param name="cooldown">The cooldown for the group</param>
        public SteamGroup(ulong id, int cooldown)
        {
            // Set the variables
            this.ID = id;
            this.Cooldown = cooldown;

            // Setup the XML
            WebsiteData.GetDataAsync(string.Format("http://steamcommunity.com/gid/{0}/memberslistxml/?xml=1", ID.ToString()), new DownloadStringCompletedEventHandler(delegate (object sender, DownloadStringCompletedEventArgs args)
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(args.Result);
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
            }));
        }

        #region Public Functions
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
        #endregion
    }
}
