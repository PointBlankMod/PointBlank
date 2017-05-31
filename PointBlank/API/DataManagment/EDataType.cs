using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using PointBlank.Framework.Permissions.Ring;

namespace PointBlank.API.DataManagment
{
    [RingPermission(SecurityAction.Demand, ring = RingPermissionRing.None)]
    public enum EDataType
    {
        JSON,
        XML,
        //CONF, // Not done yet
        UNKNOWN
    }
}
