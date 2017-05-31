using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Unturned.Server
{
    /// <summary>
    /// All the server based events
    /// </summary>
    public static class ServerEvents
    {
        #region Handlers
        #endregion

        #region Events
        /// <summary>
        /// Called every game tick
        /// </summary>
        public static event OnVoidDelegate OnGameTick;
        #endregion

        #region Functions
        internal static void RunGameTick()
        {
            if (OnGameTick == null)
                return;

            OnGameTick();
        }
        #endregion
    }
}
