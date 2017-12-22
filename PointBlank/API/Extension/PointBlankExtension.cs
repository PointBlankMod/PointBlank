using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Extension.Loader;

namespace PointBlank.API.Extension
{
    public abstract class PointBlankExtension : ILoadable
    {
        #region Abstract Functions
        public abstract void Load();

        public abstract void Unload();
        #endregion
    }
}
