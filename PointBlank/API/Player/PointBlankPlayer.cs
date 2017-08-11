using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Groups;
using PointBlank.API.Commands;
using UnityEngine;

namespace PointBlank.API.Player
{
    /// <summary>
    /// The player extension used to identify players
    /// </summary>
    public abstract class PointBlankPlayer
    {
        #region Variables
        private List<PointBlankGroup> _Groups = new List<PointBlankGroup>();
        private List<string> _Permissions = new List<string>();
        private readonly Dictionary<PointBlankCommand, DateTime> _Cooldowns = new Dictionary<PointBlankCommand, DateTime>();
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
        /// The command cooldown for the player
        /// </summary>
        public virtual int Cooldown { get; set; } = -1;
        /// <summary>
        /// The groups this player is part of
        /// </summary>
        public virtual PointBlankGroup[] Groups => _Groups.ToArray();
        /// <summary>
        /// The permissions this player has(groups not included)
        /// </summary>
        public virtual string[] Permissions => _Permissions.ToArray();
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

            _Groups.Add(group);
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

            _Groups.Remove(group);
            if (Loaded)
                PointBlankPlayerEvents.RunGroupRemove(this, group);
        }

        /// <summary>
        /// Adds a permission to the player
        /// </summary>
        /// <param name="permission">The permission to add</param>
        public virtual void AddPermission(string permission)
        {
            if (Permissions.Contains(permission))
                return;

            _Permissions.Add(permission);
            if (Loaded)
                PointBlankPlayerEvents.RunPermissionAdd(this, permission);
        }
        /// <summary>
        /// Removes a permission from the player
        /// </summary>
        /// <param name="permission">The permission to remove</param>
        public virtual void RemovePermission(string permission)
        {
            if (!Permissions.Contains(permission))
                return;

            _Permissions.Remove(permission);
            if (Loaded)
                PointBlankPlayerEvents.RunPermissionRemove(this, permission);
        }
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
        /// Checks if the player has the specified permission
        /// </summary>
        /// <param name="permission">The permission to check for</param>
        /// <returns>If the player has the specified permission</returns>
        public virtual bool HasPermission(string permission)
        {
            if (IsAdmin)
                return true;
            for (int i = 0; i < Groups.Length; i++)
                if (Groups[i].HasPermission(permission))
                    return true;

            string[] sPermission = permission.Split('.');

            for (int a = 0; a < sPermission.Length; a++)
            {
                bool found = false;

                for (int b = 0; b < Permissions.Length; b++)
                {
                    string[] sPerm = Permissions[b].Split('.');

                    if (sPerm[a] == "*")
                        return true;
                    if (sPerm[a] == sPermission[a])
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the command cooldown for the player
        /// </summary>
        /// <returns>The command cooldown for the player</returns>
        public virtual int GetCooldown()
        {
            if (HasPermission("pointblank.nocooldown"))
                return 0;
            if (Cooldown != -1)
                return Cooldown;
            for (int i = 0; i < Groups.Length; i++)
                if (Groups[i].Cooldown != -1)
                    return Groups[i].Cooldown;
            return -1;
        }
        /// <summary>
        /// Checks if the player has a cooldown on a specific command
        /// </summary>
        /// <param name="command">The command to check for</param>
        /// <returns>If there is a cooldown on the command</returns>
        public virtual bool HasCooldown(PointBlankCommand command)
        {
            if (!_Cooldowns.ContainsKey(command))
                return false;
            int cooldown = GetCooldown();

            if (cooldown != -1)
            {
                if (!((DateTime.Now - _Cooldowns[command]).TotalSeconds >= cooldown)) return true;
                _Cooldowns.Remove(command);
                return false;
            }
            if (command.Cooldown == -1) return false;
            if (!((DateTime.Now - _Cooldowns[command]).TotalSeconds >= command.Cooldown)) return true;
            _Cooldowns.Remove(command);
            return false;
        }
        /// <summary>
        /// Gives the player a cooldown on a specific command
        /// </summary>
        /// <param name="command">The command to set the cooldown on</param>
        /// <param name="time">The time when the cooldown was applied(set to null to remove cooldown)</param>
        public virtual void SetCooldown(PointBlankCommand command, DateTime time)
        {
            if (time == null)
            {
                _Cooldowns.Remove(command);
                return;
            }

            _Cooldowns.Add(command, time);
        }

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
        #endregion
    }
}
