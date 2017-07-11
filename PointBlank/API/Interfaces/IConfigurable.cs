using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Collections;

namespace PointBlank.API.Interfaces
{
    /// <summary>
    /// Makes a class configurable by adding configurations to it
    /// </summary>
    public interface IConfigurable
    {
        /// <summary>
        /// The directory inside the configurations folder where the file will be save(leave null or empty to use default path)
        /// </summary>
        string ConfigurationDirectory { get; }
        /// <summary>
        /// The list of configurations
        /// </summary>
        ConfigurationList Configurations { get; }
        /// <summary>
        /// The dictionary to save the IConfigurable instance to(set to null if the Configurations and ConfigurationDirectory are static)
        /// </summary>
        Dictionary<Type, IConfigurable> ConfigurationDictionary { get; }
    }
}
