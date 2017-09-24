using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace PointBlank.API.Permissions
{
    /// <inheritdoc />
    /// <summary>
    /// The permission instance of PointBlank
    /// </summary>
    public class PointBlankPermission : IEquatable<PointBlankPermission>
    {
        #region Properties
        /// <summary>
        /// The permission string
        /// </summary>
        public string Permission { get; set; }
        /// <summary>
        /// The permission cooldown in seconds
        /// </summary>
        public int? Cooldown { get; set; }
        #endregion

        /// <summary>
        /// Create a temporary permission with cooldown
        /// </summary>
        /// <param name="permission">The permission string</param>
        /// <param name="cooldown">The permission cooldown</param>
        public PointBlankPermission(string permission, int? cooldown)
        {
            // Set the variables
            Permission = permission;
            Cooldown = cooldown;
        }
        /// <summary>
        /// Create a temporary permission without cooldown
        /// </summary>
        /// <param name="permission">The permission string</param>
        public PointBlankPermission(string permission)
        {
            // Set the variables
            Permission = permission;
            Cooldown = null;
        }
        public PointBlankPermission() { }

        #region Functions
        public override string ToString() => Permission;

        public override bool Equals(object obj)
        {
            if (!(obj is PointBlankPermission))
                return false;

            return this == (PointBlankPermission)obj;
        }

        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Creates a duplicated instance of a permission
        /// </summary>
        /// <returns>The duplicated instance of a permission</returns>
        public PointBlankPermission Duplicate() => new PointBlankPermission(Permission, Cooldown);

        /// <summary>
        /// Checks if the permissions overlap mostly used for checking if a user has a permission
        /// </summary>
        /// <param name="permission">The permission to compare with</param>
        /// <returns>If the permissions overlap</returns>
        public bool IsOverlappingPermission(string permission)
        {
            if (string.IsNullOrEmpty(Permission))
                return true;
            if (string.IsNullOrEmpty(permission))
                return false;

            string[] sPermission = permission.Split('.');
            string[] sP = Permission.Split('.');

            for (int b = 0; b < sPermission.Length; b++)
            {
                if (b >= sP.Length)
                {
                    if (sPermission.Length > sP.Length)
                        break;

                    return true;
                }

                if (sP[b] == "*")
                    return true;
                if (sP[b] != sPermission[b])
                    break;
            }
            return false;
        }
        /// <summary>
        /// Checks if the permissions overlap mostly used for checking if a user has a permission
        /// </summary>
        /// <param name="permission">The permission to compare with</param>
        /// <returns>If the permissions overlap</returns>
        public bool IsOverlappingPermission(PointBlankPermission permission) => IsOverlappingPermission(permission.Permission);

        public bool Equals(PointBlankPermission other) => this == other;
        #endregion

        #region Operator Functions
        public static bool operator ==(PointBlankPermission permission1, PointBlankPermission permission2)
        {
            if (Object.ReferenceEquals(permission1, permission2))
                return true;
            if ((object)permission1 == null || (object)permission2 == null)
                return false;

            return (permission1.Permission == permission2.Permission);
        }

        public static bool operator !=(PointBlankPermission permission1, PointBlankPermission permission2) => !(permission1 == permission2);
        #endregion
    }
}
