using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.PointBlankImplements
{
    /// <summary>
    /// Assembly/Reflection extensions for the mod loader
    /// </summary>
    public static class Assemblies
    {
        /// <summary>
        /// Checks if the assembly is an extension or not
        /// </summary>
        /// <param name="asm">The assembly to check</param>
        /// <returns>If the assembly is an extension or not</returns>
        public static bool IsExtension(this Assembly asm) =>
            PointBlankEnvironment.ModLoaderExtensions.ContainsKey(asm);
    }
}
