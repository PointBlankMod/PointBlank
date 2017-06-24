using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PointBlank.API.Groups
{
    /// <summary>
    /// Used for managing server groups
    /// </summary>
    public static class GroupManager
    {
        #region Variables
        private static Dictionary<string, Group> _Groups = new Dictionary<string, Group>();
        #endregion

        #region Properties
        /// <summary>
        /// Array of groups in the server
        /// </summary>
        public static Group[] Groups => _Groups.Values.ToArray();
        #endregion

        #region Handlers
        /// <summary>
        /// Handler for all group events such as OnGroupAdded and OnGroupRemoved
        /// </summary>
        /// <param name="group">The group that the event affects</param>
        public delegate void GroupEventHandler(Group group);
        #endregion

        #region Events
        /// <summary>
        /// Called when a group is added to the server
        /// </summary>
        public static event GroupEventHandler OnGroupAdded;
        /// <summary>
        /// Called when a group is removed from the server
        /// </summary>
        public static event GroupEventHandler OnGroupRemoved;
        #endregion

        #region Public Functions
        /// <summary>
        /// Adds a group to the server groups
        /// </summary>
        /// <param name="group">The group instance to add</param>
        public static void AddGroup(Group group)
        {
            if (_Groups.ContainsKey(group.ID))
                return;
            _Groups.Add(group.ID, group);

            if (OnGroupAdded == null)
                return;
            OnGroupAdded(group);
        }

        /// <summary>
        /// Creates and adds a group to the server group
        /// </summary>
        /// <param name="ID">The group ID</param>
        /// <param name="Name">The group name</param>
        /// <param name="Cooldown">The command cooldown for the group</param>
        public static void AddGroup(string ID, string Name, bool isDefault, int cooldown, Color color)
        {
            if (_Groups.ContainsKey(ID))
                return;
            Group group = new Group(ID, Name, isDefault, cooldown, color);

            _Groups.Add(ID, group);

            if (OnGroupAdded == null)
                return;
            OnGroupAdded(group);
        }

        /// <summary>
        /// Removes a group from the server
        /// </summary>
        /// <param name="group">The group to remove</param>
        public static void RemoveGroup(Group group)
        {
            if (!_Groups.ContainsValue(group))
                return;
            _Groups.Remove(group.ID);

            if (OnGroupRemoved == null)
                return;
            OnGroupRemoved(group);
        }

        /// <summary>
        /// Removes a group from the server
        /// </summary>
        /// <param name="ID">The ID of the group</param>
        public static void RemoveGroup(string ID)
        {
            if (!_Groups.ContainsKey(ID))
                return;
            Group group = _Groups[ID];

            _Groups.Remove(ID);

            if (OnGroupRemoved == null)
                return;
            OnGroupRemoved(group);
        }
        #endregion
    }
}
