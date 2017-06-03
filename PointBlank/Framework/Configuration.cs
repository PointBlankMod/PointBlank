using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.DataManagment;

namespace PointBlank.Framework
{
    internal static class Configuration
    {
        public static EDataType SaveDataType;
        public static bool ServerAutoUpdate;
        public static string UpdateScriptLocation;
        public static int CheckUpdateTimeSeconds;
    }
}
