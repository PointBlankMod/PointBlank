using System;
using System.Collections.Generic;
using PointBlank.API.Collections;
using PointBlank.API.Interfaces;
using PointBlank.API.DataManagment;

namespace PointBlank.Framework.Configurations
{
    internal class PointBlankConfigurations
    {
        public string ConfigurationDirectory => null;

        public ConfigurationList Configurations => new ConfigurationList()
        {
            //{ "ConfigFormat", EDataType.Json }
        };

        public Dictionary<Type, Configurable> ConfigurationDictionary => PointBlankEnvironment.FrameworkConfig;
    }
}
