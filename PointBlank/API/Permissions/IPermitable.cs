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
    }
}
