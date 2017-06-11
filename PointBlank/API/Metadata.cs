using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API
{
    /// <summary>
    /// The metadata class for storing any useful information
    /// </summary>
    public class Metadata
    {
        #region Variables
        /// <summary>
        /// Should the data be saved to a file
        /// </summary>
        public bool Save;
        /// <summary>
        /// The data to store
        /// </summary>
        public object Data;
        #endregion

        /// <summary>
        /// The metadata class for storing any useful information
        /// </summary>
        /// <param name="data">The data to store</param>
        /// <param name="save">Should the data be saved to a file</param>
        public Metadata(object data, bool save = false)
        {
            this.Save = save;
            this.Data = data;
        }
    }
}
