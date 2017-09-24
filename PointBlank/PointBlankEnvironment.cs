using System;
using System.Collections.Generic;
using PointBlank.Framework.Objects;
using PointBlank.Framework.Wrappers;
using PointBlank.API.DataManagment;
using PointBlank.API.Interfaces;

namespace PointBlank
{
    internal static class PointBlankEnvironment // Yes I know it is spelled wrong but Environment already exists so we need to use the name
    {
        public static Dictionary<string, ServiceWrapper> Services = new Dictionary<string, ServiceWrapper>(); // The list of services and their properties
        public static Dictionary<string, RuntimeObject> RuntimeObjects = new Dictionary<string, RuntimeObject>(); // The list of runtime objects

        public static List<SqlData> SqlConnections = new List<SqlData>(); // The list of all the sql connections

        public static Dictionary<Type, Configurable> FrameworkConfig = new Dictionary<Type, Configurable>(); // Configuration for the framework
        public static Dictionary<Type, Configurable> ServiceConfig = new Dictionary<Type, Configurable>(); // Configuration for the services
        public static Dictionary<Type, Translatable> ServiceTranslations = new Dictionary<Type, Translatable>(); // Translations for the services

        public static bool Running = true; // Is pointblank running
    }
}
