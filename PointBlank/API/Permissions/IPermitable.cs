using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Permissions
{
    /// <summary>
    /// Applied to classes which contain their own permissions
    /// </summary>
    public interface IPermitable
    {
        /// <summary>
        /// The permissions of the class
        /// </summary>
        PointBlankPermission[] Permissions { get; }

        /// <summary>
        /// Converts a string to a permission object or returns null if not found
        /// </summary>
        /// <param name="permission">The permission string used for the conversion</param>
        /// <returns>The permission object or null if not found</returns>
        PointBlankPermission GetPermission(string permission);
    }
}
