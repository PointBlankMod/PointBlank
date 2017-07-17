using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Security
{
    /// <summary>
    /// All supported hashing types
    /// </summary>
    public enum EHashType
    {
        NONE,
        MD5,
        SHA1,
        SHA256,
        SHA512
    }
}
