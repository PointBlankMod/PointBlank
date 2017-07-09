using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Security;

namespace PointBlank.Framework.Permissions.Ring
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Enum, AllowMultiple = false)]
    internal class RingPermissionAttribute : CodeAccessSecurityAttribute
    {
        #region Properties
        public RingPermissionRing ring { get; set; }
        #endregion

        public RingPermissionAttribute(SecurityAction action) : base(action)
        {
        }

        #region CodeAccessSecurityAttribute Functions
        public override IPermission CreatePermission()
        {
            return Unrestricted ? new RingPermission(PermissionState.Unrestricted) : new RingPermission(ring);
        }
        #endregion
    }
}
