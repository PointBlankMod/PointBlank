namespace PointBlank.API.Groups
{
    /// <summary>
    /// List of events for groups
    /// </summary>
    public static class PointBlankGroupEvents
    {
        #region Handlers
        /// <summary>
        /// Handler for all group events such as OnGroupAdded and OnGroupRemoved
        /// </summary>
        /// <param name="group">The group that the event affects</param>
        public delegate void GroupEventHandler(PointBlankGroup group);

        /// <summary>
        /// Handler for group permission events
        /// </summary>
        /// <param name="permission">The affected permission</param>
        /// <param name="instance">The affected group's instance</param>
        public delegate void PermissionEventHandler(PointBlankGroup instance, string permission);
        /// <summary>
        /// Handler for inherited groups events
        /// </summary>
        /// <param name="group">The affected group</param>
        /// <param name="instance">The affected group's instance</param>
        public delegate void InheritEventHandler(PointBlankGroup instance, PointBlankGroup group);

        /// <summary>
        /// Handler for group prefix events
        /// </summary>
        /// <param name="prefix">The affected prefix</param>
        /// <param name="instance">The affected group's instance</param>
        public delegate void PrefixEventHandler(PointBlankGroup instance, string prefix);
        /// <summary>
        /// Handler for the group suffix events
        /// </summary>
        /// <param name="suffix">The affected suffix</param>
        /// <param name="instance">The affected group's instance</param>
        public delegate void SuffixEventHandler(PointBlankGroup instance, string suffix);
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
        internal static void RunGroupAdded(PointBlankGroup g) => OnGroupAdded?.Invoke(g);
        internal static void RunGroupRemoved(PointBlankGroup g) => OnGroupRemoved?.Invoke(g);

        internal static void RunPermissionAdded(PointBlankGroup instance, string permission) => OnPermissionAdded?.Invoke(instance, permission);
        internal static void RunPermissionRemoved(PointBlankGroup instance, string permission) => OnPermissionRemoved?.Invoke(instance, permission);

        internal static void RunPrefixAdded(PointBlankGroup instance, string prefix) => OnPrefixAdded?.Invoke(instance, prefix);
        internal static void RunPrefixRemoved(PointBlankGroup instance, string prefix) => OnPrefixRemoved?.Invoke(instance, prefix);

        internal static void RunSuffixAdded(PointBlankGroup instance, string suffix) => OnSuffixAdded?.Invoke(instance, suffix);
        internal static void RunSuffixRemoved(PointBlankGroup instance, string suffix) => OnSuffixRemoved?.Invoke(instance, suffix);

        internal static void RunInheritAdded(PointBlankGroup instance, PointBlankGroup group) => OnInheritAdded?.Invoke(instance, @group);
        internal static void RunInheritRemoved(PointBlankGroup instance, PointBlankGroup group) => OnInheritRemoved?.Invoke(instance, @group);

        #endregion
    }
}
