using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Groups
{
    /// <summary>
    /// List of events for groups
    /// </summary>
    public static class GroupEvents
    {
        #region Handlers
        /// <summary>
        /// Handler for group permission events
        /// </summary>
        /// <param name="permission">The affected permission</param>
        /// <param name="instance">The affected group's instance</param>
        public delegate void PermissionEventHandler(Group instance, string permission);
        /// <summary>
        /// Handler for inherited groups events
        /// </summary>
        /// <param name="group">The affected group</param>
        /// <param name="instance">The affected group's instance</param>
        public delegate void InheritEventHandler(Group instance, Group group);

        /// <summary>
        /// Handler for group prefix events
        /// </summary>
        /// <param name="prefix">The affected prefix</param>
        /// <param name="instance">The affected group's instance</param>
        public delegate void PrefixEventHandler(Group instance, string prefix);
        /// <summary>
        /// Handler for the group suffix events
        /// </summary>
        /// <param name="suffix">The affected suffix</param>
        /// <param name="instance">The affected group's instance</param>
        public delegate void SuffixEventHandler(Group instance, string suffix);
        #endregion

        #region Events
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
        internal static void RunPermissionAdded(Group instance, string permission)
        {
            if (OnPermissionAdded == null)
                return;

            OnPermissionAdded(instance, permission);
        }
        internal static void RunPermissionRemoved(Group instance, string permission)
        {
            if (OnPermissionRemoved == null)
                return;

            OnPermissionRemoved(instance, permission);
        }

        internal static void RunPrefixAdded(Group instance, string prefix)
        {
            if (OnPrefixAdded == null)
                return;

            OnPrefixAdded(instance, prefix);
        }
        internal static void RunPrefixRemoved(Group instance, string prefix)
        {
            if (OnPrefixRemoved == null)
                return;

            OnPrefixRemoved(instance, prefix);
        }

        internal static void RunSuffixAdded(Group instance, string suffix)
        {
            if (OnSuffixAdded == null)
                return;

            OnSuffixAdded(instance, suffix);
        }
        internal static void RunSuffixRemoved(Group instance, string suffix)
        {
            if (OnSuffixRemoved == null)
                return;

            OnSuffixRemoved(instance, suffix);
        }

        internal static void RunInheritAdded(Group instance, Group group)
        {
            if (OnInheritAdded == null)
                return;

            OnInheritAdded(instance, group);
        }
        internal static void RunInheritRemoved(Group instance, Group group)
        {
            if (OnInheritRemoved == null)
                return;

            OnInheritRemoved(instance, group);
        }
        #endregion
    }
}
