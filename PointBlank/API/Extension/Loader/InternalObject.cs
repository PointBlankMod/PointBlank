using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Extension.Loader
{
    public abstract class InternalObject : ILoadable
    {
        #region Abstract Functions
        public abstract void Load();

        public abstract void Reload();

        public abstract void Unload();
        #endregion
    }
}
