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
        private static Dictionary<string, PointBlankGroup> _groups = new Dictionary<string, PointBlankGroup>();

	    private static GroupManager _groupManager =
		    (GroupManager) PointBlankServiceManager.GetService("GroupManager.GroupManager");
		#endregion

		#region Properties
		/// <summary>
		/// Array of groups in the server
		/// </summary>
		public static PointBlankGroup[] Groups => _groups.Values.ToArray();

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
            if (_groups.ContainsKey(group.Id))
                return;
            _groups.Add(group.Id, group);
            
            if (Loaded)
                PointBlankGroupEvents.RunGroupAdded(group);

			_groupManager.SaveGroups();
        }
        /// <summary>
        /// Creates and adds a group to the server group
        /// </summary>
        /// <param name="id">The group ID</param>
        /// <param name="name">The group name</param>
        /// <param name="Cooldown">The command cooldown for the group</param>
<<<<<<< HEAD
        public static void AddGroup(string ID, string Name, bool isDefault, Color color)
=======
        public static void AddGroup(string id, string name, bool isDefault, Color color)
>>>>>>> master
        {
            if (_groups.ContainsKey(id))
                return;
<<<<<<< HEAD
            PointBlankGroup group = new PointBlankGroup(ID, Name, isDefault, color);

            _Groups.Add(ID, group);
=======
            PointBlankGroup group = new PointBlankGroup(id, name, isDefault, color);
>>>>>>> master

           AddGroup(group);
        }

        /// <summary>
        /// Removes a group from the server
        /// </summary>
        /// <param name="group">The group to remove</param>
        public static void RemoveGroup(PointBlankGroup group)
        {
            if (!_groups.ContainsValue(group))
                return;
            _groups.Remove(group.Id);

            if (Loaded)
                PointBlankGroupEvents.RunGroupRemoved(group);

			_groupManager.SaveGroups();
		}
        /// <summary>
        /// Removes a group from the server
        /// </summary>
        /// <param name="id">The ID of the group</param>
        public static void RemoveGroup(string id)
        {
            if (!_groups.ContainsKey(id))
                return;
            PointBlankGroup group = _groups[id];

            RemoveGroup(group);
        }

        /// <summary>
        /// Find a server group and returns it
        /// </summary>
        /// <param name="id">The ID of the group</param>
        /// <returns>The group instance</returns>
        public static PointBlankGroup Find(string id) => Groups.FirstOrDefault(a => a.Id == id);

        /// <summary>
        /// Tries to find the group by ID and returns it
        /// </summary>
        /// <param name="id">The group ID to look for</param>
        /// <param name="group">The group instance</param>
        /// <returns>If the group was found</returns>
        public static bool TryFindGroup(string id, out PointBlankGroup group)
        {
            PointBlankGroup g = Find(id);

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
