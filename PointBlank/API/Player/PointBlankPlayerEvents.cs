using PointBlank.API.Groups;
using PointBlank.API.Permissions;

namespace PointBlank.API.Player
{
    public static class PointBlankPlayerEvents
    {
        #region Handlers
        /// <summary>
        /// Handles permission changes of the player
        /// </summary>
        /// <param name="player">The affected player</param>
        /// <param name="permission">The changed permission</param>
        public delegate void PermissionsChangedHandler(PointBlankPlayer player, PointBlankPermission permission);
        /// <summary>
        /// Handles group changes of the player
        /// </summary>
        /// <param name="player">The affected player</param>
        /// <param name="group">The changed group</param>
        public delegate void GroupsChangedHandler(PointBlankPlayer player, PointBlankGroup group);
        #endregion

        #region Events
        /// <summary>
        /// Called when a permission is added
        /// </summary>
        public static event PermissionsChangedHandler OnPermissionAdded;
        /// <summary>
        /// Called when a permission is removed
        /// </summary>
        public static event PermissionsChangedHandler OnPermissionRemoved;

        /// <summary>
        /// Called when a group is added
        /// </summary>
        public static event GroupsChangedHandler OnGroupAdded;
        /// <summary>
        /// Called when a group is removed
        /// </summary>
        public static event GroupsChangedHandler OnGroupRemoved;
        #endregion

        #region Functions
        internal static void RunPermissionAdd(PointBlankPlayer player, PointBlankPermission permission) => OnPermissionAdded?.Invoke(player, permission);
        internal static void RunPermissionRemove(PointBlankPlayer player, PointBlankPermission permission) => OnPermissionRemoved?.Invoke(player, permission);

        internal static void RunGroupAdd(PointBlankPlayer player, PointBlankGroup group) => OnGroupAdded?.Invoke(player, group);
        internal static void RunGroupRemove(PointBlankPlayer player, PointBlankGroup group) => OnGroupRemoved?.Invoke(player, group);
        #endregion
    }
}
