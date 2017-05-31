using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Plugins;
using PointBlank.API.Services;

namespace PointBlank.Services.CommandManager
{
    [Service("CommandManager", true)]
    internal class CommandManager : Service
    {
        #region Override Functions
        public override void Load()
        {
            Logging.Log("Loading commands...");
            PluginEvents.OnPluginLoaded += new PluginEvents.PluginEventHandler(OnPluginLoaded); // Run code every time a plugin is loaded
        }

        public override void Unload()
        {
        }
        #endregion

        #region Event Functions
        public void OnPluginLoaded(Plugin plugin)
        {
            
        }
        #endregion
    }
}
