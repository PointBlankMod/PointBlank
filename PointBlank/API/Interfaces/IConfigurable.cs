using System;
using System.Collections.Generic;
using PointBlank.API.Collections;

namespace PointBlank.API.Interfaces
{
    /// <summary>
    /// Makes a class configurable by adding configurations to it
    /// </summary>
<<<<<<< HEAD
    public abstract class IConfigurable
    {
        #region Variables
        private ConfigurationList _Configurations = null;
=======
    public abstract class Configurable
    {
        #region Variables
        private ConfigurationList _configurations = null;
>>>>>>> master
        #endregion

        #region Properties
        /// <summary>
        /// The list of configurations
        /// </summary>
        public ConfigurationList Configurations
        {
            get
            {
<<<<<<< HEAD
                if (_Configurations == null)
                    _Configurations = DefaultConfigurations;
                return _Configurations;
=======
                if (_configurations == null)
                    _configurations = DefaultConfigurations;
                return _configurations;
>>>>>>> master
            }
        }
        #endregion

        #region Abstract Properties
        /// <summary>
        /// The directory inside the configurations folder where the file will be save(leave null or empty to use default path)
        /// </summary>
        public abstract string ConfigurationDirectory { get; }
        /// <summary>
        /// The list of configurations
        /// </summary>
        public abstract ConfigurationList DefaultConfigurations { get; }
        /// <summary>
        /// The dictionary to save the IConfigurable instance to(set to null if the Configurations and ConfigurationDirectory are static)
        /// </summary>
<<<<<<< HEAD
        public abstract Dictionary<Type, IConfigurable> ConfigurationDictionary { get; }
=======
        public abstract Dictionary<Type, Configurable> ConfigurationDictionary { get; }
>>>>>>> master
        #endregion
    }
}
