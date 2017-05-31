using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.Services.RCON
{
    internal static class RCONConfiguration
    {
        public static int Port;
        public static string Password;
        public static bool CanReadLogs;
        public static bool CanSendCommands;
        public static bool ShowExecutedCommands;
        public static int MaxClients;
    }
}
