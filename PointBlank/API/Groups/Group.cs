using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PointBlank.API.Groups
{
    /// <summary>
    /// A server group
    /// </summary>
    public class Group
    {
        #region Variables
        private List<string> _Permissions = new List<string>();
        private List<Group> _Inherits = new List<Group>();

        private List<string> _Prefixes = new List<string>();
        private List<string> _Suffixes = new List<string>();
        #endregion

        #region Properties
        /// <summary>
        /// The ID of the group
        /// </summary>
        public string ID { get; private set; }
        /// <summary>
        /// The name of the group
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Is this the default group
        /// </summary>
        public bool Default { get; private set; }

        /// <summary>
        /// The list of permissions this group has
        /// </summary>
        public string[] Permissions => Permissions.ToArray();
        /// <summary>
        /// List of permission inherits this group has
        /// </summary>
        public Group[] Inherits => _Inherits.ToArray();

        /// <summary>
        /// List of prefixes the group has
        /// </summary>
        public string[] Prefixes => _Prefixes.ToArray();
        /// <summary>
        /// List of suffixes the group has
        /// </summary>
        public string[] Suffixes => _Suffixes.ToArray();

        /// <summary>
        /// The cooldown for the commands
        /// </summary>
        public int Cooldown { get; set; }
        /// <summary>
        /// The color of the group(visible in chat)
        /// </summary>
        public Color Color { get; set; }
        #endregion

        /// <summary>
        /// Creates a group instance
        /// </summary>
        /// <param name="id">The ID of the group</param>
        /// <param name="name">The name of the group</param>
        /// <param name="cooldown">The cooldown of the group</param>
        public Group(string id, string name, bool isDefault, int cooldown, Color color)
        {
            // Set the variables
            this.ID = id;
            this.Name = name;
            this.Cooldown = cooldown;
            this.Default = isDefault;
            this.Color = color;
        }

        #region Static Functions
        /// <summary>
        /// Finds and returns the instance of a group by ID
        /// </summary>
        /// <param name="ID">The ID to query</param>
        /// <returns>The instance of the group the ID belongs to or null if not found</returns>
        public static Group Find(string ID) => GroupManager.Find(ID);

        /// <summary>
        /// Tries to find the group by ID and returns it
        /// </summary>
        /// <param name="ID">The group ID to look for</param>
        /// <param name="group">The group instance</param>
        /// <returns>If the group was found</returns>
        public static bool TryFindGroup(string ID, out Group group) => Group.TryFindGroup(ID, out group);

        /// <summary>
        /// Checks if the specified group ID is already in use
        /// </summary>
        /// <param name="ID">The group ID to check for</param>
        /// <returns>If the group ID is already in use</returns>
        public static bool Exists(string ID) => (Find(ID) != null);
        #endregion

        #region Public Functions
        /// <summary>
        /// Add a permission to the group
        /// </summary>
        /// <param name="permission">The permission to add</param>
        public void AddPermission(string permission)
        {
            if (_Permissions.Contains(permission))
                return;

            _Permissions.Add(permission);
            GroupEvents.RunPermissionAdded(this, permission);
        }

        /// <summary>
        /// Removes a permission from the group
        /// </summary>
        /// <param name="permission">The permission to remove</param>
        public void RemovePermission(string permission)
        {
            if (!_Permissions.Contains(permission))
                return;

            _Permissions.Remove(permission);
            GroupEvents.RunPermissionRemoved(this, permission);
        }

        /// <summary>
        /// Adds a prefix to the group
        /// </summary>
        /// <param name="prefix">The prefix to add</param>
        public void AddPrefix(string prefix)
        {
            if (_Prefixes.Contains(prefix))
                return;

            _Prefixes.Add(prefix);
            GroupEvents.RunPrefixAdded(this, prefix);
        }

        /// <summary>
        /// Removes a prefix from the group
        /// </summary>
        /// <param name="prefix">The prefix to remove</param>
        public void RemovePrefix(string prefix)
        {
            if (!_Prefixes.Contains(prefix))
                return;

            _Prefixes.Remove(prefix);
            GroupEvents.RunPrefixRemoved(this, prefix);
        }

        /// <summary>
        /// Adds a suffix to the group
        /// </summary>
        /// <param name="suffix">The suffix to add</param>
        public void AddSuffix(string suffix)
        {
            if (_Suffixes.Contains(suffix))
                return;

            _Suffixes.Add(suffix);
            GroupEvents.RunSuffixAdded(this, suffix);
        }

        /// <summary>
        /// Removes a suffix from the group
        /// </summary>
        /// <param name="suffix">The suffix to remove</param>
        public void RemoveSuffix(string suffix)
        {
            if (!_Suffixes.Contains(suffix))
                return;

            _Suffixes.Remove(suffix);
            GroupEvents.RunSuffixRemoved(this, suffix);
        }

        /// <summary>
        /// Adds a group inherit to the group
        /// </summary>
        /// <param name="group">The group to inherit from</param>
        public void AddInherit(Group group)
        {
            if (_Inherits.Contains(group))
                return;
            if (_Inherits.Count(a => a.ID == group.ID) > 0)
                return;

            _Inherits.Add(group);
            GroupEvents.RunInheritAdded(this, group);
        }

        /// <summary>
        /// Removes a group inherit from the group
        /// </summary>
        /// <param name="group">The group inherit to remove</param>
        public void RemoveInherit(Group group)
        {
            if (!_Inherits.Contains(group))
                return;

            _Inherits.Remove(group);
            GroupEvents.RunInheritRemoved(this, group);
        }

        /// <summary>
        /// Gets the list of all permissions including inheritences
        /// </summary>
        /// <returns>The list of all permissions including inheritences</returns>
        public string[] GetPermissions()
        {
            List<string> permissions = new List<string>();

            permissions.AddRange(Permissions);
            PBTools.ForeachLoop<Group>(Inherits, delegate (int index, Group value)
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
        /// Checks if the group has the permission specified
        /// </summary>
        /// <param name="permission">The permission to check for</param>
        /// <returns>If the group has the permission specified</returns>
        public bool HasPermission(string permission)
        {
            string[] permissions = GetPermissions();
            string[] sPermission = permission.Split('.');

            for(int a = 0; a < sPermission.Length; a++)
            {
                bool found = false;

                for(int b = 0; b < permissions.Length; b++)
                {
                    string[] sPerm = permissions[b].Split('.');

                    if (sPerm[a] == "*")
                        return true;
                    if(sPerm[a] == sPermission[a])
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    return false;
            }
            return true;
        }
        #endregion
    }
}
