using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Interfaces
{
    /// <summary>
    /// Used for setting loadable objects
    /// </summary>
    public interface ILoadable
    {
        /// <summary>
        /// Called when the object is loaded
        /// </summary>
        void Load();
        /// <summary>
        /// Called when object is unloaded
        /// </summary>
        void Unload();
    }
}
