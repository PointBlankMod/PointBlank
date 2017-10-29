using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.Framework.Permissions.Ring
{
    [Flags]
    [Serializable]
    internal enum RingPermissionRing
    {
        None = 1,
        Plugins = 2,
        Mods = 4,
        Extensions = 8,
        Framework = 16
    }
}
