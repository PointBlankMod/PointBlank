using System;
using System.Collections.Generic;
using System.Linq;
using PointBlank.API.Groups;
using PointBlank.API.Commands;
using PointBlank.API.Permissions;
using UnityEngine;

namespace PointBlank.API.Player
{
    /// <summary>
    /// The player extension used to identify players
    /// </summary>
    public abstract class PointBlankPlayer : IPermitable
    {
        #region Variables
        private List<PointBlankGroup> _groups = new List<PointBlankGroup>();
        private List<PointBlankPermission> _permissions = new List<PointBlankPermission>();
        #endregion

        #region Properties
        // Abstract
        /// <summary>
        /// Is the player an admin
        /// </summary>
        public abstract bool IsAdmin { get; set; }
        /// <summary>
        /// The gameobject of the player
        /// </summary>
        public abstract GameObject GameObject { get; }

        // Virtual
        /// <summary>
        /// Any custom data you want to attach to the player
        /// </summary>
        public virtual Dictionary<string, object> Metadata { get; private set; } = new Dictionary<string, object>();
        /// <summary>
        /// The groups this player is part of
        /// </summary>
        public virtual PointBlankGroup[] Groups => _groups.ToArray();
        /// <summary>
        /// The permissions this player has(groups not included)
        /// </summary>
        public virtual PointBlankPermission[] Permissions => _permissions.ToArray();
        /// <summary>
        /// Is the player loaded or not(used for event triggers)
        /// </summary>
        public virtual bool Loaded { get; set; } = false;
        #endregion

        #region Static Functions
        /// <summary>
        /// Checks if the UnturnedPlayer is the server player
        /// </summary>
        /// <param name="player">The unturned player instance to check</param>
        /// <returns>If the UnturnedPlayer instance is the server</returns>
        public static bool IsServer(PointBlankPlayer player) => (player == null);

        /// <summary>
        /// Sends a message to the player or the console(null = console, instance = player)
        /// </summary>
        /// <param name="player">The player(null for console) to send the message to</param>
        /// <param name="message">The message to send</param>
        public static void SendMessage(PointBlankPlayer player, object message, ConsoleColor color)
        {
            if (IsServer(player))
                PointBlankConsole.WriteLine(message, color);
            else
                player.SendMessage(message, PointBlankConsole.ConsoleColorToColor(color));
        }
        #endregion

        #region Virtual Functions
        // Virtual
        /// <summary>
        /// Add the player to a group
        /// </summary>
        /// <param name="group">The group to add the player to</param>
        public virtual void AddGroup(PointBlankGroup group)
        {
            if (Groups.Contains(group))
                return;

            _groups.Add(group);
            if (Loaded)
                PointBlankPlayerEvents.RunGroupAdd(this, group);
        }
        /// <summary>
        /// Remove the player from a group
        /// </summary>
        /// <param name="group">The group to remove the player from</param>
        public virtual void RemoveGroup(PointBlankGroup group)
        {
            if (!Groups.Contains(group))
                return;

            _groups.Remove(group);
            if (Loaded)
                PointBlankPlayerEvents.RunGroupRemove(this, group);
        }

        /// <summary>
        /// Adds a permission to the player
        /// </summary>
        /// <param name="permission">The permission to add</param>
        public virtual void AddPermission(string permission)
        {
            PointBlankPermission perm = PointBlankPermissionManager.GetPermission(this, permission);

            if (perm == null)
                return;
            AddPermission(perm);
        }
        /// <summary>
        /// Adds a permission to the player
        /// </summary>
        /// <param name="permission">The permission instance to add to the player</param>
        public virtual void AddPermission(PointBlankPermission permission)
        {
            if (Permissions.Contains(permission))
                return;

            _permissions.Add(permission);
            if (Loaded)
                PointBlankPlayerEvents.RunPermissionAdd(this, permission);
        }
        /// <summary>
        /// Removes a permission from the player
        /// </summary>
        /// <param name="permission">The permission to remove</param>
        public virtual void RemovePermission(string permission)
        {
            PointBlankPermission perm = PointBlankPermissionManager.GetPermission(this, permission);

            if (perm == null)
                return;
            RemovePermission(perm);
        }
        /// <summary>
        /// Removes a permission from the player
        /// </summary>
        /// <param name="permission">The permission to remove</param>
        public virtual void RemovePermission(PointBlankPermission permission)
        {
            if (!Permissions.Contains(permission))
                return;

            _permissions.Remove(permission);
            if (Loaded)
                PointBlankPlayerEvents.RunPermissionRemove(this, permission);
        }
        /// <summary>
        /// Gets all permissions attached to the user
        /// </summary>
        /// <returns>The list of permissions attached to the user</returns>
        public virtual PointBlankPermission[] GetPermissions()
        {
            List<PointBlankPermission> permissions = Permissions.ToList();

            for (int i = 0; i < Groups.Length; i++)
            {
                foreach (PointBlankPermission perm in Groups[i].GetPermissions())
                {
                    PointBlankPermission cPerm = permissions.FirstOrDefault(a => a == perm);

                    if (cPerm != null && cPerm.Cooldown != null && perm.Cooldown != null)
                        if (cPerm.Cooldown > perm.Cooldown)
                            permissions.Remove(cPerm);
                    permissions.Add(perm);
                }
            }

            return permissions.ToArray();

        }
        /// <summary>
        /// Converts a string to a permission object or returns null if not found
        /// </summary>
        /// <param name="permission">The permission string used for the conversion</param>
        /// <returns>The permission object or null if not found</returns>
        public virtual PointBlankPermission GetPermission(string permission) => GetPermissions().FirstOrDefault(a => a.Permission == permission);
        /// <summary>
        /// Checks if the player has the permissions specified
        /// </summary>
        /// <param name="permissions">The permissions to check for</param>
        /// <returns>If the player has the permissions specified</returns>
        public virtual bool HasPermissions(params string[] permissions)
        {
            for (int i = 0; i < permissions.Length; i++)
                if (!HasPermission(permissions[i]))
                    return false;
            return true;
        }
        /// <summary>
        /// Checks if the player has the permissions specified
        /// </summary>
        /// <param name="permissions">The permissions to check for</param>
        /// <returns>If the player has the permissions specified</returns>
        public virtual bool HasPermissions(params PointBlankPermission[] permissions)
        {
            for (int i = 0; i < permissions.Length; i++)
                if (!HasPermission(permissions[i]))
                    return false;
            return true;
        }
        /// <summary>
        /// Checks if the player has the specified permission
        /// </summary>
        /// <param name="permission">The permission to check for</param>
        /// <returns>If the player has the specified permission</returns>
        public virtual bool HasPermission(string permission)
        {
            PointBlankPermission perm = new PointBlankPermission(permission);

            if (perm == null)
                return false;
            return HasPermission(perm);
        }
        /// <summary>
        /// Checks if the player has the specified permission
        /// </summary>
        /// <param name="permission">The permission to check for</param>
        /// <returns>If the player has the specified permission</returns>
        public virtual bool HasPermission(PointBlankPermission permission)
        {
            if (IsAdmin)
                return true;
            foreach (PointBlankPermission perm in GetPermissions())
                if (perm.IsOverlappingPermission(permission))
                    return true;
            return false;
        }

        /// <summary>
        /// Adds a cooldown to the player on a specific permission
        /// </summary>
        /// <param name="permission">The permission to add the cooldown to</param>
        public virtual void AddCooldown(PointBlankPermission permission)
        {
            PointBlankPermission perm = GetPermission(permission.Permission);

            if (perm == null)
                return;
            PointBlankPermissionManager.AddCooldown(this, perm);
        }
        /// <summary>
        /// Adds a cooldown to the player on a specific permission
        /// </summary>
        /// <param name="permission">The permission to add the cooldown to</param>
        public virtual void AddCooldown(string permission) => AddCooldown(new PointBlankPermission(permission));
        /// <summary>
        /// Removes a cooldown from the player on a specific permission
        /// </summary>
        /// <param name="permission">The permission to remove the cooldown from</param>
        public virtual void RemoveCooldown(PointBlankPermission permission)
        {
            PointBlankPermission perm = GetPermission(permission.Permission);

            if (perm == null)
                return;
            PointBlankPermissionManager.RemoveCooldown(this, perm);
        }
        /// <summary>
        /// Removes a cooldown from the player on a specific permission
        /// </summary>
        /// <param name="permission">The permission to remove the cooldown from</param>
        public virtual void RemoveCooldown(string permission) => RemoveCooldown(new PointBlankPermission(permission));
        /// <summary>
        /// Checks if the player has a cooldown on a specific permission
        /// </summary>
        /// <param name="permission">The permission the cooldown is applied to</param>
        /// <returns>If the player has a cooldown or not</returns>
        public virtual bool HasCooldown(PointBlankPermission permission)
        {
            if (HasPermission("pointblank.nocooldown"))
                return false;

            if (PointBlankPermissionManager.HasCooldown(this, permission))
                return true;
            return false;
        }
        /// <summary>
        /// Checks if the player has a cooldown on a specific permission
        /// </summary>
        /// <param name="permission">The permission the cooldown is applied to</param>
        /// <returns>If the player has a cooldown or not</returns>
        public virtual bool HasCooldown(string permission) => HasCooldown(PointBlankPermissionManager.GetPermission(this, permission));

        /// <summary>
        /// Gets the player color
        /// </summary>
        /// <returns>The player color</returns>
        public virtual Color GetColor()
        {
            for (int i = 0; i < Groups.Length; i++)
            {
                if (Groups[i].Color != Color.clear)
                    return Groups[i].Color;
            }
            return Color.clear;
        }

        /// <summary>
        /// Returns the position the player is looking at
        /// </summary>
        /// <param name="lookPosition">The origin position</param>
        /// <param name="lookForward">The forward position Vector3</param>
        /// <param name="distance">The ray distance</param>
        /// <param name="masks">The ray masks</param>
        /// <returns>The position the player is looking at</returns>
        public virtual Vector3? GetEyePosition(Vector3 lookPosition, Vector3 lookForward, float distance, int masks)
        {
            RaycastHit hit;

            if (!Physics.Raycast(lookPosition, lookForward, out hit, distance, masks))
                return null;
            if (hit.transform == null)
                return null;

            return hit.point;
        }

        // Abstract
        /// <summary>
        /// Sends a message to the player's chat
        /// </summary>
        /// <param name="message">The message of the player</param>
        public abstract void SendMessage(object message, Color color);
        #endregion

        #region Functions
        /// <summary>
        /// Gets the PointBlankPlayerComponent instance that was attached to the player
        /// </summary>
        /// <typeparam name="T">The PointBlankPlayerComponent type to search for</typeparam>
        /// <returns>The instance of the PointBlankPlayerComponent</returns>
        public T GetComponent<T>() where T : PointBlankPlayerComponent => GameObject.GetComponent<T>();
        /// <summary>
        /// Gets the PointBlankPlayerComponent instance that was attached to the player
        /// </summary>
        /// <param name="type">The PointBlankPlayerComponent type to search for</param>
        /// <returns>The instance of the PointBlankPlayerComponent</returns>
        public PointBlankPlayerComponent GetComponent(Type type)
        {
            if (!typeof(PointBlankPlayerComponent).IsAssignableFrom(type))
                return null;

            return (PointBlankPlayerComponent)GameObject.GetComponent(type);
        }

        /// <summary>
        /// Adds the PointBlankPlayerComponent to the player
        /// </summary>
        /// <typeparam name="T">The PointBlankPlayerComponent type to add</typeparam>
        /// <returns>The instance of PointBlankPlayerComponent that was added</returns>
        public T AddComponent<T>() where T : PointBlankPlayerComponent => GameObject.AddComponent<T>();
        /// <summary>
        /// Adds the PointBlankPlayerComponent to the player
        /// </summary>
        /// <param name="type">The PointBlankPlayerComponent type to add</param>
        /// <returns>The instance of PointBlankPlayerComponent that was added</returns>
        public PointBlankPlayerComponent AddComponent(Type type)
        {
            if (!typeof(PointBlankPlayerComponent).IsAssignableFrom(type))
                return null;

            return (PointBlankPlayerComponent)GameObject.AddComponent(type);
        }

        /// <summary>
        /// Converts the PointBlankPlayer to any type for easier access
        /// </summary>
        /// <typeparam name="T">The type to convert it to</typeparam>
        /// <returns>The converted value</returns>
        public T Get<T>() where T : PointBlankPlayer => (T)this;
        #endregion
    }
}
