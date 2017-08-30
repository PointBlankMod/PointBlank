using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace PointBlank.API.Permissions
{
    /// <summary>
    /// The permission instance of PointBlank
    /// </summary>
    public class PointBlankPermission
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
        #endregion

        #region Operator Functions
        public static bool operator ==(PointBlankPermission permission1, PointBlankPermission permission2)
        {
            if (Object.ReferenceEquals(permission1, permission2))
                return true;

            return (permission1.Permission == permission2.Permission);
        }

        public static bool operator !=(PointBlankPermission permission1, PointBlankPermission permission2) => !(permission1 == permission2);
        #endregion
    }
}
