using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            //{ "ConfigFormat", EDataType.JSON }
        };

        public Dictionary<Type, IConfigurable> ConfigurationDictionary => Enviroment.FrameworkConfig;
    }
}
