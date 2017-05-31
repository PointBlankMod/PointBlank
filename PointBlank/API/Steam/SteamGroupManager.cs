using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Steam
{
    public static class SteamGroupManager
    {
        #region Variables
        private static Dictionary<ulong, SteamGroup> _Groups = new Dictionary<ulong, SteamGroup>();
        #endregion

        #region Properties
        /// <summary>
        /// The steam group list on the server
        /// </summary>
        public static SteamGroup[] Groups { get { return _Groups.Values.ToArray(); } }
        #endregion

        #region Handlers
        /// <summary>
        /// The handler for all steam group based events
        /// </summary>
        /// <param name="group">The affected steam group</param>
        public delegate void SteamGroupEventHandler(SteamGroup group);
        #endregion

        #region Events
        /// <summary>
        /// Called when a steam group is added
        /// </summary>
        public static event SteamGroupEventHandler OnSteamGroupAdded;

        /// <summary>
        /// Called when a steam group is removed
        /// </summary>
        public static event SteamGroupEventHandler OnSteamGroupRemoved;
        #endregion

        #region Public Functions
        /// <summary>
        /// Adds a steam group to the server
        /// </summary>
        /// <param name="group">The steam group to add</param>
        public static void AddSteamGroup(SteamGroup group)
        {
            if (_Groups.ContainsKey(group.ID))
                return;
            _Groups.Add(group.ID, group);

            if (OnSteamGroupAdded == null)
                return;
            OnSteamGroupAdded(group);
        }

        /// <summary>
        /// Creates and adds a steam group to the server
        /// </summary>
        /// <param name="ID">The ID of the steam group</param>
        /// <param name="cooldown">The command cooldown of the steam group</param>
        public static void AddSteamGroup(ulong ID, int cooldown)
        {
            if (_Groups.ContainsKey(ID))
                return;
            SteamGroup group = new SteamGroup(ID, cooldown);

            _Groups.Add(ID, group);

            if (OnSteamGroupAdded == null)
                return;
            OnSteamGroupAdded(group);
        }

        /// <summary>
        /// Removes a steam group from the server
        /// </summary>
        /// <param name="group">The group to remove</param>
        public static void RemoveSteamGroup(SteamGroup group)
        {
            if (!_Groups.ContainsValue(group))
                return;
            _Groups.Remove(group.ID);

            if (OnSteamGroupRemoved == null)
                return;
            OnSteamGroupRemoved(group);
        }

        /// <summary>
        /// Removes a steam group from the server
        /// </summary>
        /// <param name="ID">The steam group ID to remove</param>
        public static void RemoveSteamGroup(ulong ID)
        {
            if (_Groups.ContainsKey(ID))
                return;
            SteamGroup group = _Groups[ID];

            _Groups.Remove(ID);

            if (OnSteamGroupRemoved == null)
                return;
            OnSteamGroupRemoved(group);
        }
        #endregion
    }
}
