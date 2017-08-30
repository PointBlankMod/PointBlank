using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Permissions
{
    /// <summary>
    /// The manager for PointBlank permissions
    /// </summary>
    public static class PointBlankPermissionManager
    {
        #region Variables
        private static List<PointBlankCooldown> _Cooldowns = new List<PointBlankCooldown>();
        #endregion

        #region Properties
        /// <summary>
        /// The list of all cooldowns
        /// </summary>
        public static PointBlankCooldown[] Cooldowns => _Cooldowns.ToArray();
        #endregion

        #region Functions
        /// <summary>
        /// Gets and returns the permission using the permission string
        /// </summary>
        /// <param name="permission">The permission string used to find the permission instance</param>
        /// <param name="target">The targeted class containing the permission list</param>
        /// <returns>The permission instance</returns>
        public static PointBlankPermission GetPermission(IPermitable target, string permission) => target.Permissions.FirstOrDefault(a => a.Permission == permission);

        /// <summary>
        /// Gets the current cooldown of a targeted object and permission
        /// </summary>
        /// <param name="target">The targeted object that has the cooldown</param>
        /// <param name="permission">The permission that the cooldown is applied to</param>
        /// <returns>The cooldown that is applied to the target object and permission or null if no cooldown is applied</returns>
        public static PointBlankCooldown GetCooldown(IPermitable target, PointBlankPermission permission) =>
            Cooldowns.FirstOrDefault(a => a.Target == target && a.Permission == permission);

        /// <summary>
        /// Checks to see if a target has a cooldown on a permission
        /// </summary>
        /// <param name="target">The target to check for the cooldown</param>
        /// <param name="permission">The permission the cooldown is applied to</param>
        /// <returns>If the target has a cooldown on that permission</returns>
        public static bool HasCooldown(IPermitable target, PointBlankPermission permission)
        {
            PointBlankCooldown cooldown = GetCooldown(target, permission);

            if (cooldown == null)
                return false;
            if (cooldown.IsExpired)
            {
                RemoveCooldown(target, permission);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Checks to see if a target has a cooldown on a permission
        /// </summary>
        /// <param name="target">The target to check for the cooldown</param>
        /// <param name="permission">The permission the cooldown is applied to</param>
        /// <returns>If the target has a cooldown on that permission</returns>
        public static bool HasCooldown(IPermitable target, string permission)
        {
            PointBlankPermission perm = GetPermission(target, permission);

            if (perm == null)
                return false;
            return HasCooldown(target, perm);
        }
        
        /// <summary>
        /// Removes a cooldown from a target and permission
        /// </summary>
        /// <param name="target">The target that the cooldown is applied to</param>
        /// <param name="permission">The permission the cooldown is applied to</param>
        public static void RemoveCooldown(IPermitable target, PointBlankPermission permission)
        {
            PointBlankCooldown cooldown = GetCooldown(target, permission);

            if (cooldown == null)
                return;
            _Cooldowns.Remove(cooldown);
        }
        /// <summary>
        /// Removes a cooldown from a target and permission
        /// </summary>
        /// <param name="target">The target that the cooldown is applied to</param>
        /// <param name="permission">The permission the cooldown is applied to</param>
        public static void RemoveCooldown(IPermitable target, string permission)
        {
            PointBlankPermission perm = GetPermission(target, permission);

            if (perm == null)
                return;
            RemoveCooldown(target, perm);
        }

        /// <summary>
        /// Adds a cooldown to a target on a specific permission
        /// </summary>
        /// <param name="target">The target to add the cooldown to</param>
        /// <param name="permission">The permission to apply it to</param>
        /// <returns>The cooldown instance of an existing cooldown or the new cooldown</returns>
        public static PointBlankCooldown AddCooldown(IPermitable target, PointBlankPermission permission)
        {
            PointBlankCooldown cooldown = GetCooldown(target, permission);

            if (cooldown == null)
                cooldown = new PointBlankCooldown(permission, DateTime.Now, target);

            return cooldown;
        }
        /// <summary>
        /// Adds a cooldown to a target on a specific permission
        /// </summary>
        /// <param name="target">The target to add the cooldown to</param>
        /// <param name="permission">The permission to apply it to</param>
        /// <returns>The cooldown instance of an existing cooldown or the new cooldown</returns>
        public static PointBlankCooldown AddCooldown(IPermitable target, string permission)
        {
            PointBlankPermission perm = GetPermission(target, permission);

            if (perm == null)
                return null;
            return AddCooldown(target, perm);
        }
        #endregion
    }
}
