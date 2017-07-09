using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Groups;
using SDG.Unturned;

namespace PointBlank.API.Unturned.Player
{
    /// <summary>
    /// Contains all unturned player based events
    /// </summary>
    public static class PlayerEvents
    {
        #region Handlers
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
        /// Handles invisible player changes of the player
        /// </summary>
        /// <param name="player">The affected player</param>
        /// <param name="target">The changed player</param>
        public delegate void InvisiblePlayersChangedHandler(UnturnedPlayer player, UnturnedPlayer target);

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

        /// <summary>
        /// Handles player deaths
        /// </summary>
        /// <param name="player">The player that got killed</param>
        /// <param name="cause">The cause of death</param>
        /// <param name="killer">The person who killed the player</param>
        public delegate void PlayerDeathHandler(UnturnedPlayer player, ref EDeathCause cause, ref UnturnedPlayer killer);
        /// <summary>
        /// Handles player hurt events
        /// </summary>
        /// <param name="player">The player that got hurt</param>
        /// <param name="damage">The amount of damage done to the player</param>
        /// <param name="cause">The cause of the damage</param>
        /// <param name="limb">The limb that got hurt</param>
        /// <param name="damager">The person that caused damage</param>
        /// <param name="cancel">Should the damage be canceled</param>
        public delegate void PlayerHurtHandler(UnturnedPlayer player, ref byte damage, ref EDeathCause cause, ref ELimb limb, ref UnturnedPlayer damager, ref bool cancel);
        /// <summary>
        /// Handles player kills
        /// </summary>
        /// <param name="player">The player that killed another player</param>
        /// <param name="cause">The cause of the kill</param>
        /// <param name="victim">The player that got killed</param>
        public delegate void PlayerKillHandler(UnturnedPlayer player, ref EDeathCause cause, ref UnturnedPlayer victim);
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

        /// <summary>
        /// Called when an invisible player is added
        /// </summary>
        public static event InvisiblePlayersChangedHandler OnInvisiblePlayerAdded;
        /// <summary>
        /// Called when an invisible player is removed
        /// </summary>
        public static event InvisiblePlayersChangedHandler OnInvisiblePlayerRemoved;

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

        /// <summary>
        /// Called when a player dies
        /// </summary>
        public static event PlayerDeathHandler OnPlayerDied;
        /// <summary>
        /// Called when a player is hurt
        /// </summary>
        public static event PlayerHurtHandler OnPlayerHurt;
        /// <summary>
        /// Called when a player kills another player
        /// </summary>
        public static event PlayerKillHandler OnPlayerKill;
        #endregion

        #region Functions
        internal static void RunPermissionAdd(UnturnedPlayer player, string permission) => OnPermissionAdded?.Invoke(player, permission);

        internal static void RunPermissionRemove(UnturnedPlayer player, string permission) => OnPermissionRemoved?.Invoke(player, permission);

        internal static void RunGroupAdd(UnturnedPlayer player, Group group) => OnGroupAdded?.Invoke(player, @group);

        internal static void RunGroupRemove(UnturnedPlayer player, Group group) => OnGroupRemoved?.Invoke(player, @group);

        internal static void RunInvisiblePlayerAdd(UnturnedPlayer player, UnturnedPlayer target) => OnInvisiblePlayerAdded?.Invoke(player, target);

        internal static void RunInvisiblePlayerRemove(UnturnedPlayer player, UnturnedPlayer target) => OnInvisiblePlayerRemoved?.Invoke(player, target);

        internal static void RunPrefixAdd(UnturnedPlayer player, string prefix) => OnPrefixAdded?.Invoke(player, prefix);

        internal static void RunPrefixRemove(UnturnedPlayer player, string prefix) => OnPrefixRemoved?.Invoke(player, prefix);

        internal static void RunSuffixAdd(UnturnedPlayer player, string suffix) => OnSuffixAdded?.Invoke(player, suffix);

        internal static void RunSuffixRemove(UnturnedPlayer player, string suffix) => OnSuffixRemoved?.Invoke(player, suffix);

        internal static void RunPlayerHurt(SteamPlayer player, ref byte damage, ref EDeathCause cause, ref ELimb limb, ref UnturnedPlayer damager, ref bool cancel)
        {
            if (OnPlayerHurt == null)
                return;
            UnturnedPlayer ply = UnturnedPlayer.Get(player);

            OnPlayerHurt(ply, ref damage, ref cause, ref limb, ref damager, ref cancel);
            if (cancel)
                return;
            if ((ply.Health - damage) >= 0) return;
            OnPlayerDied(ply, ref cause, ref damager);
            OnPlayerKill(damager, ref cause, ref ply);
        }
        #endregion
    }
}
