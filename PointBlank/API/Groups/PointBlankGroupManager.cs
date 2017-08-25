using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PointBlank.API.Services;
using PointBlank.Services.GroupManager;

namespace PointBlank.API.Groups
{
    /// <summary>
    /// Used for managing server groups
    /// </summary>
    public static class PointBlankGroupManager
    {
        #region Variables
        private static Dictionary<string, PointBlankGroup> _Groups = new Dictionary<string, PointBlankGroup>();
        #endregion

        #region Properties
        /// <summary>
        /// Array of groups in the server
        /// </summary>
        public static PointBlankGroup[] Groups => _Groups.Values.ToArray();

        /// <summary>
        /// Is the group manager loaded(this is for events)
        /// </summary>
        public static bool Loaded { get; set; } = false;
        #endregion

        #region Public Functions
        /// <summary>
        /// Adds a group to the server groups
        /// </summary>
        /// <param name="group">The group instance to add</param>
        public static void AddGroup(PointBlankGroup group)
        {
            if (_Groups.ContainsKey(group.ID))
                return;
            _Groups.Add(group.ID, group);
            
            if (Loaded)
                PointBlankGroupEvents.RunGroupAdded(group);
        }
        /// <summary>
        /// Creates and adds a group to the server group
        /// </summary>
        /// <param name="ID">The group ID</param>
        /// <param name="Name">The group name</param>
        /// <param name="Cooldown">The command cooldown for the group</param>
        public static void AddGroup(string ID, string Name, bool isDefault, int Cooldown, Color color)
        {
            if (_Groups.ContainsKey(ID))
                return;
            PointBlankGroup group = new PointBlankGroup(ID, Name, isDefault, Cooldown, color);

            _Groups.Add(ID, group);

            if (Loaded)
                PointBlankGroupEvents.RunGroupAdded(group);
        }

        /// <summary>
        /// Removes a group from the server
        /// </summary>
        /// <param name="group">The group to remove</param>
        public static void RemoveGroup(PointBlankGroup group)
        {
            if (!_Groups.ContainsValue(group))
                return;
            _Groups.Remove(group.ID);

            if (Loaded)
                PointBlankGroupEvents.RunGroupRemoved(group);
        }
        /// <summary>
        /// Removes a group from the server
        /// </summary>
        /// <param name="ID">The ID of the group</param>
        public static void RemoveGroup(string ID)
        {
            if (!_Groups.ContainsKey(ID))
                return;
            PointBlankGroup group = _Groups[ID];

            _Groups.Remove(ID);

            if (Loaded)
                PointBlankGroupEvents.RunGroupRemoved(group);
        }

        /// <summary>
        /// Find a server group and returns it
        /// </summary>
        /// <param name="ID">The ID of the group</param>
        /// <returns>The group instance</returns>
        public static PointBlankGroup Find(string ID) => Groups.FirstOrDefault(a => a.ID == ID);

        /// <summary>
        /// Tries to find the group by ID and returns it
        /// </summary>
        /// <param name="ID">The group ID to look for</param>
        /// <param name="group">The group instance</param>
        /// <returns>If the group was found</returns>
        public static bool TryFindGroup(string ID, out PointBlankGroup group)
        {
            PointBlankGroup g = Find(ID);

            group = g;
            return g != null;
        }

        /// <summary>
        /// Reloads the groups
        /// </summary>
        public static void Reload()
        {
            GroupManager gm = (GroupManager)PointBlankServiceManager.GetService("GroupManager.GroupManager");

            gm.GroupConfig.Reload();
            gm.LoadGroups();
        }
        #endregion
    }
}
