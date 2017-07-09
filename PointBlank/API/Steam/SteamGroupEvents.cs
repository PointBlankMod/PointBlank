using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Steam
{
    /// <summary>
    /// List of events for steam groups
    /// </summary>
    public static class SteamGroupEvents
    {
        #region Handlers
        /// <summary>
        /// The handler for all steam group based events
        /// </summary>
        /// <param name="group">The affected steam group</param>
        public delegate void SteamGroupEventHandler(SteamGroup group);

        /// <summary>
        /// Handler for any permission based event
        /// </summary>
        /// <param name="permission">The permission affected</param>
        /// <param name="instance">The effect steam group's instance</param>
        public delegate void PermissionEventHandler(SteamGroup instance, string permission);
        /// <summary>
        /// Handler for any inherit based event
        /// </summary>
        /// <param name="group">The group affected</param>
        /// <param name="instance">The effect steam group's instance</param>
        public delegate void InheritEventHandler(SteamGroup instance, SteamGroup group);

        /// <summary>
        /// Handler for any prefix based event
        /// </summary>
        /// <param name="prefix">The prefix affected</param>
        /// <param name="instance">The effect steam group's instance</param>
        public delegate void PrefixEventHandler(SteamGroup instance, string prefix);
        /// <summary>
        /// Handler for any suffix based event
        /// </summary>
        /// <param name="suffix">The suffix affected</param>
        /// <param name="instance">The effect steam group's instance</param>
        public delegate void SuffixEventHandler(SteamGroup instance, string suffix);
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

        /// <summary>
        /// Called when a permission is added
        /// </summary>
        public static event PermissionEventHandler OnPermissionAdded;
        /// <summary>
        /// Called when a permission is removed
        /// </summary>
        public static event PermissionEventHandler OnPermissionRemoved;

        /// <summary>
        /// Called when an inherit is added
        /// </summary>
        public static event InheritEventHandler OnInheritAdded;
        /// <summary>
        /// Called when an inherit is removed
        /// </summary>
        public static event InheritEventHandler OnInheritRemoved;

        /// <summary>
        /// Called when a prefix is added
        /// </summary>
        public static event PrefixEventHandler OnPrefixAdded;
        /// <summary>
        /// Called when a prefix is removed
        /// </summary>
        public static event PrefixEventHandler OnPrefixRemoved;

        /// <summary>
        /// Called when a suffix is added
        /// </summary>
        public static event SuffixEventHandler OnSuffixAdded;
        /// <summary>
        /// Called when a suffix is removed
        /// </summary>
        public static event SuffixEventHandler OnSuffixRemoved;
        #endregion

        #region Functions
        internal static void RunSteamGroupAdded(SteamGroup g) => OnSteamGroupAdded?.Invoke(g);

        internal static void RunSteamGroupRemoved(SteamGroup g) => OnSteamGroupRemoved?.Invoke(g);

        internal static void RunPermissionAdded(SteamGroup instance, string permission) => OnPermissionAdded?.Invoke(instance, permission);

        internal static void RunPermissionRemoved(SteamGroup instance, string permission) => OnPermissionRemoved?.Invoke(instance, permission);

        internal static void RunPrefixAdded(SteamGroup instance, string prefix) => OnPrefixAdded?.Invoke(instance, prefix);

        internal static void RunPrefixRemoved(SteamGroup instance, string prefix) => OnPrefixRemoved?.Invoke(instance, prefix);

        internal static void RunSuffixAdded(SteamGroup instance, string suffix) => OnSuffixAdded?.Invoke(instance, suffix);

        internal static void RunSuffixRemoved(SteamGroup instance, string suffix) => OnSuffixRemoved?.Invoke(instance, suffix);

        internal static void RunInheritAdded(SteamGroup instance, SteamGroup group) => OnInheritAdded?.Invoke(instance, @group);

        internal static void RunInheritRemoved(SteamGroup instance, SteamGroup group) => OnInheritRemoved?.Invoke(instance, @group);

        #endregion
    }
}
