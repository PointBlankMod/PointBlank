using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.Services.PermissionManager;

namespace PointBlank.API.Permissions
{
    /// <summary>
    /// The manager for PointBlank permissions
    /// </summary>
    public static class PointBlankPermissionManager
    {
        #region Properties
        /// <summary>
        /// The list of all custom PointBlank permissions
        /// </summary>
        public static PointBlankPermission[] Permissions => PermissionManager.Permissions.ToArray();
        #endregion

        #region Functions
        /// <summary>
        /// Adds a permission or edits an existing one then returns it's instance
        /// </summary>
        /// <param name="permission">The permission string</param>
        /// <param name="cooldown">The permission cooldown</param>
        /// <returns>The permission instance</returns>
        public static PointBlankPermission AddPermission(string permission, int cooldown)
        {
            PointBlankPermission perm = Permissions.FirstOrDefault(a => a.Permission == permission);

            if(perm == null)
            {
                perm = new PointBlankPermission(permission, cooldown);
                return AddPermission(perm);
            }
            else
            {
                perm.Cooldown = cooldown;
                return perm;
            }
        }
        /// <summary>
        /// Adds a permission or returns an existing one
        /// </summary>
        /// <param name="permission">The permission string</param>
        /// <returns>The existing/new permission instance</returns>
        public static PointBlankPermission AddPermission(string permission)
        {
            PointBlankPermission perm = Permissions.FirstOrDefault(a => a.Permission == permission);

            if (perm != null)
                return perm;
            perm = AddPermission(permission, -1);

            return perm;
        }
        /// <summary>
        /// Adds a permission to PointBlank
        /// </summary>
        /// <param name="permission">The permission to add</param>
        /// <returns>The permission instance</returns>
        public static PointBlankPermission AddPermission(PointBlankPermission permission)
        {
            PointBlankPermission perm = Permissions.FirstOrDefault(a => a == permission);

            if(perm != null)
            {
                if (permission.Cooldown != null)
                    perm.Cooldown = permission.Cooldown;
                return perm;
            }

            ((PermissionManager)Enviroment.services["PermissionManager.PermissionManager"].ServiceClass).AddPermission(permission);
            return permission;
        }

        /// <summary>
        /// Removes a permission from PointBlank using the permission string
        /// </summary>
        /// <param name="permission">The permission string of the targeted permission</param>
        public static void RemovePermission(string permission)
        {
            PointBlankPermission perm = Permissions.FirstOrDefault(a => a.Permission == permission);

            if (perm == null)
                return;
            RemovePermission(perm);
        }
        /// <summary>
        /// Removes a permission from PointBlank using the permission instance
        /// </summary>
        /// <param name="permission">The permission instance</param>
        public static void RemovePermission(PointBlankPermission permission)
        {
            if (!Permissions.Contains(permission))
                return;

            ((PermissionManager)Enviroment.services["PermissionManager.PermissionManager"].ServiceClass).RemovePermission(permission);
        }

        /// <summary>
        /// Gets and returns the permission using the permission string
        /// </summary>
        /// <param name="permission">The permission string used to find the permission instance</param>
        /// <returns>The permission instance</returns>
        public static PointBlankPermission GetPermission(string permission) => Permissions.FirstOrDefault(a => a.Permission == permission);
        #endregion
    }
}
