using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using PointBlank.API;
using PointBlank.API.Plugins;
using PointBlank.API.Services;
using PointBlank.API.Unturned.Chat;

namespace PointBlank.Services.CommandManager
{
    [Service("CommandManager", true)]
    internal class CommandManager : Service
    {
        #region Override Functions
        public override void Load()
        {
            // Setup events
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

        private void OnUnturnedCommand(SteamPlayer player, string text, ref bool shouldExecuteCommand, ref bool shouldList)
        {
            shouldExecuteCommand = false;

            if(text.StartsWith("/") || text.StartsWith("@"))
            {

                shouldList = false;
            }
        }
        #endregion
    }
}
