using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// The list of permissions this group has
        /// </summary>
        public string[] Permissions { get { return _Permissions.ToArray(); } }
        /// <summary>
        /// List of permission inherits this group has
        /// </summary>
        public Group[] Inherits { get { return _Inherits.ToArray(); } }

        /// <summary>
        /// List of prefixes the group has
        /// </summary>
        public string[] Prefixes { get { return _Prefixes.ToArray(); } }
        /// <summary>
        /// List of suffixes the group has
        /// </summary>
        public string[] Suffixes { get { return _Suffixes.ToArray(); } }

        /// <summary>
        /// The cooldown for the commands
        /// </summary>
        public int Cooldown { get; set; }
        #endregion

        /// <summary>
        /// Creates a group instance
        /// </summary>
        /// <param name="id">The ID of the group</param>
        /// <param name="name">The name of the group</param>
        /// <param name="cooldown">The cooldown of the group</param>
        public Group(string id, string name, int cooldown)
        {
            // Set the variables
            this.ID = id;
            this.Name = name;
            this.Cooldown = cooldown;
        }

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
        #endregion
    }
}
