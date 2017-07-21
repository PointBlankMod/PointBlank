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
        SHA2_256,
        SHA2_512,
        SHA3_256,
        SHA3_512
    }
}
