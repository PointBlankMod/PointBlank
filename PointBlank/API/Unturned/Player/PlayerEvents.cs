using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Groups;

namespace PointBlank.API.Unturned.Player
{
    /// <summary>
    /// Contains all unturned player based events
    /// </summary>
    public static class PlayerEvents
    {
        #region PointBlank Handlers
        /// <summary>
        /// Handles permission changes of the player
        /// </summary>
        /// <param name="player">The affected player</param>
        /// <param name="permission">The changed permission</param>
        public delegate void PermissionsChangedHandler(UnturnedPlayer player, string permission);
        /// <summary>
        /// Handles group changes of the player
        /// </summary>
        /// <param name="player">The affected player</param>
        /// <param name="group">The changed group</param>
        public delegate void GroupsChangedHandler(UnturnedPlayer player, Group group);
        /// <summary>
        /// Handles visible player changes of the player
        /// </summary>
        /// <param name="player">The affected player</param>
        /// <param name="target">The changed player</param>
        public delegate void VisiblePlayersChangedHandler(UnturnedPlayer player, UnturnedPlayer target);

        /// <summary>
        /// Handles prefix changes of the player
        /// </summary>
        /// <param name="player">The affected player</param>
        /// <param name="prefix">The changed prefix</param>
        public delegate void PrefixesChangedHandler(UnturnedPlayer player, string prefix);
        /// <summary>
        /// Handles suffix changes of the player
        /// </summary>
        /// <param name="player">The affected player</param>
        /// <param name="suffix">The changed suffix</param>
        public delegate void SuffixesChangedHandler(UnturnedPlayer player, string suffix);
        #endregion

        #region PointBlank Events
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

        /// <summary>
        /// Called when a visible player is added
        /// </summary>
        public static event VisiblePlayersChangedHandler OnVisiblePlayerAdded;
        /// <summary>
        /// Called when a visible player is removed
        /// </summary>
        public static event VisiblePlayersChangedHandler OnVisiblePlayerRemoved;

        /// <summary>
        /// Called when a prefix is added
        /// </summary>
        public static event PrefixesChangedHandler OnPrefixAdded;
        /// <summary>
        /// Called when a prefix is removed
        /// </summary>
        public static event PrefixesChangedHandler OnPrefixRemoved;

        /// <summary>
        /// Called when a suffix is added
        /// </summary>
        public static event SuffixesChangedHandler OnSuffixAdded;
        /// <summary>
        /// Called when a suffix is removed
        /// </summary>
        public static event SuffixesChangedHandler OnSuffixRemoved;
        #endregion

        #region Functions
        internal static void RunPermissionAdd(UnturnedPlayer player, string permission)
        {
            if (OnPermissionAdded == null)
                return;

            OnPermissionAdded(player, permission);
        }
        internal static void RunPermissionRemove(UnturnedPlayer player, string permission)
        {
            if (OnPermissionRemoved == null)
                return;

            OnPermissionRemoved(player, permission);
        }

        internal static void RunGroupAdd(UnturnedPlayer player, Group group)
        {
            if (OnGroupAdded == null)
                return;

            OnGroupAdded(player, group);
        }
        internal static void RunGroupRemove(UnturnedPlayer player, Group group)
        {
            if (OnGroupRemoved == null)
                return;

            OnGroupRemoved(player, group);
        }

        internal static void RunVisiblePlayerAdd(UnturnedPlayer player, UnturnedPlayer target)
        {
            if (OnVisiblePlayerAdded == null)
                return;

            OnVisiblePlayerAdded(player, target);
        }
        internal static void RunVisiblePlayerRemove(UnturnedPlayer player, UnturnedPlayer target)
        {
            if (OnVisiblePlayerRemoved == null)
                return;

            OnVisiblePlayerRemoved(player, target);
        }

        internal static void RunPrefixAdd(UnturnedPlayer player, string prefix)
        {
            if (OnPrefixAdded == null)
                return;

            OnPrefixAdded(player, prefix);
        }
        internal static void RunPrefixRemove(UnturnedPlayer player, string prefix)
        {
            if (OnPrefixRemoved == null)
                return;

            OnPrefixRemoved(player, prefix);
        }

        internal static void RunSuffixAdd(UnturnedPlayer player, string suffix)
        {
            if (OnSuffixAdded == null)
                return;

            OnSuffixAdded(player, suffix);
        }
        internal static void RunSuffixRemove(UnturnedPlayer player, string suffix)
        {
            if (OnSuffixRemoved == null)
                return;

            OnSuffixRemoved(player, suffix);
        }
        #endregion
    }
}
