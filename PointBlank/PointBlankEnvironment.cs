using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using PointBlank.API.Extension.Loader;

namespace PointBlank
{
    internal static class PointBlankEnvironment
    {
        #region System Variables
        public static bool Enabled = false;
        #endregion

        #region ModLoader Variables
        public static Dictionary<Type, InternalObject> ModLoaderInternals = new Dictionary<Type, InternalObject>();
        public static List<Assembly> ModLoaderExtensions = new List<Assembly>();
        #endregion

        #region Classes
        #endregion
    }
}
