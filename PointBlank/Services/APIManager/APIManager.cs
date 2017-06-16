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
using Steamworks;
using UnityEngine;

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
                while (RunThread)
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
            ChatManager.onChatted += new Chatted(OnPlayerChat);
            PlayerEvents.OnInvisiblePlayerAdded += new PlayerEvents.InvisiblePlayersChangedHandler(OnSetInvisible);
            PlayerEvents.OnInvisiblePlayerRemoved += new PlayerEvents.InvisiblePlayersChangedHandler(OnSetVisible);

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

            foreach (Group g in groups)
                if (!player.Groups.Contains(g))
                    player.AddGroup(g);
        }

        private void OnPlayerLeave(UnturnedPlayer player)
        {
            UnturnedServer.RemovePlayer(player);
        }

        private void OnPlayerChat(SteamPlayer player, EChatMode mode, ref Color color, string text, ref bool visible)
        {
            UnturnedPlayer ply = UnturnedPlayer.Get(player);
            Color c = ply.GetColor();

            if (c != Color.clear)
                color = c;
        }

        private void OnSetInvisible(UnturnedPlayer player, UnturnedPlayer target)
        {
            List<SteamPlayer> plys = Provider.clients.ToList();

            for (int i = 0; i < player.InvisiblePlayers.Length; i++)
                if (plys.Contains(player.InvisiblePlayers[i].SteamPlayer))
                    plys.Remove(player.InvisiblePlayers[i].SteamPlayer);
            int index = plys.FindIndex(x => x == target.SteamPlayer);
            Provider.send(player.SteamID, ESteamPacket.DISCONNECTED, new byte[]
            {
                12,
                (byte)index
            }, 2, 0);
        }

        private void OnSetVisible(UnturnedPlayer player, UnturnedPlayer target)
        {
            int size;
            byte[] bytes = SteamPacker.getBytes(0, out size, new object[]
            {
                11,
                target.SteamPlayerID.steamID,
                target.SteamPlayerID.characterID,
                target.SteamPlayerID.playerName,
                target.SteamPlayerID.characterName,
                target.SteamPlayer.model.transform.position,
                (byte)(target.SteamPlayer.model.transform.rotation.eulerAngles.y / 2f),
                target.SteamPlayer.isPro,
                target.SteamPlayer.isAdmin && !Provider.hideAdmins,
                target.SteamPlayer.channel,
                target.SteamPlayer.playerID.group,
                target.SteamPlayer.playerID.nickName,
                target.SteamPlayer.face,
                target.SteamPlayer.hair,
                target.SteamPlayer.beard,
                target.SteamPlayer.skin,
                target.SteamPlayer.color,
                target.SteamPlayer.hand,
                target.SteamPlayer.shirtItem,
                target.SteamPlayer.pantsItem,
                target.SteamPlayer.hatItem,
                target.SteamPlayer.backpackItem,
                target.SteamPlayer.vestItem,
                target.SteamPlayer.maskItem,
                target.SteamPlayer.glassesItem,
                target.SteamPlayer.skinItems,
                (byte)target.SteamPlayer.skillset,
                target.SteamPlayer.language
            });

            Provider.send(player.SteamID, ESteamPacket.CONNECTED, bytes, size, 0);
        }
        #endregion
    }
}
