using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using PointBlank.API.Groups;
using PointBlank.API.Services;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Structure;
using GM = PointBlank.API.Groups.GroupManager;

namespace PointBlank.Services.APIManager
{
    [Service("APIManager", true)]
    internal class APIManager : Service
    {
        #region Variables
        // Thread variables
        private static Thread tGame;
        private static bool RunThread = true;
        #endregion

        #region Service Functions
        public override void Load()
        {
            // Setup thread
            tGame = new Thread(new ThreadStart(delegate ()
            {
                while(RunThread)
                    ServerEvents.RunThreadTick();
            }));

            // Setup events
            Provider.onEnemyConnected += new Provider.EnemyConnected(ServerEvents.RunPlayerConnected);
            Provider.onEnemyDisconnected += new Provider.EnemyDisconnected(ServerEvents.RunPlayerDisconnected);
            Provider.onServerShutdown += new Provider.ServerShutdown(ServerEvents.RunServerShutdown);
            LightingManager.onDayNightUpdated += new DayNightUpdated(ServerEvents.RunDayNight);
            LightingManager.onMoonUpdated += new MoonUpdated(ServerEvents.RunFullMoon);
            LightingManager.onRainUpdated += new RainUpdated(ServerEvents.RunRainUpdated);
            StructureEvents.OnDestroyStructure += new StructureEvents.StructureDestroyHandler(ServerEvents.RunStructureRemoved);
            StructureEvents.OnSalvageStructure += new StructureEvents.StructureDestroyHandler(ServerEvents.RunStructureRemoved);

            // Setup pointblank events
            ServerEvents.OnPlayerConnected += new ServerEvents.PlayerConnectionHandler(OnPlayerJoin);
            ServerEvents.OnPlayerDisconnected += new ServerEvents.PlayerConnectionHandler(OnPlayerLeave);

            // Run code
            tGame.Start();
        }

        public override void Unload()
        {
            // Stop the thread
            RunThread = false;
            tGame.Abort();
        }
        #endregion

        #region Mono Functions
        void Update()
        {
            ServerEvents.RunGameTick();
        }
        #endregion

        #region Event Functions
        private void OnPlayerJoin(UnturnedPlayer player)
        {
            Group[] groups = GM.Groups.Where(a => a.Default).ToArray();

            foreach(Group g in groups)
                if (!player.Groups.Contains(g))
                    player.AddGroup(g);
        }

        private void OnPlayerLeave(UnturnedPlayer player)
        {
            UnturnedServer.RemovePlayer(player);
        }
        #endregion
    }
}
