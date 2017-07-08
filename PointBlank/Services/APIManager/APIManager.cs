using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using PointBlank.API.Plugins;
using PointBlank.API.Groups;
using PointBlank.API.Services;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Structure;
using PointBlank.API.Unturned.Barricade;
using GM = PointBlank.API.Groups.GroupManager;
using Steamworks;
using PM = PointBlank.Services.PluginManager.PluginManager;

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
            Provider.onServerHosted += new Provider.ServerHosted(ServerEvents.RunServerInitialized);
            LightingManager.onDayNightUpdated += new DayNightUpdated(ServerEvents.RunDayNight);
            LightingManager.onMoonUpdated += new MoonUpdated(ServerEvents.RunFullMoon);
            LightingManager.onRainUpdated += new RainUpdated(ServerEvents.RunRainUpdated);
            StructureEvents.OnDestroyStructure += new StructureEvents.StructureDestroyHandler(ServerEvents.RunStructureRemoved);
            StructureEvents.OnSalvageStructure += new StructureEvents.StructureDestroyHandler(ServerEvents.RunStructureRemoved);
            BarricadeEvents.OnBarricadeDestroy += new BarricadeEvents.BarricadeDestroyHandler(ServerEvents.RunBarricadeRemoved);
            BarricadeEvents.OnBarricadeSalvage += new BarricadeEvents.BarricadeDestroyHandler(ServerEvents.RunBarricadeRemoved);

            // Setup pointblank events
            ServerEvents.OnPlayerConnected += new ServerEvents.PlayerConnectionHandler(OnPlayerJoin);
            ServerEvents.OnPlayerDisconnected += new ServerEvents.PlayerConnectionHandler(OnPlayerLeave);
            ChatManager.onChatted += new Chatted(OnPlayerChat);
            PlayerEvents.OnInvisiblePlayerAdded += new PlayerEvents.InvisiblePlayersChangedHandler(OnSetInvisible);
            PlayerEvents.OnInvisiblePlayerRemoved += new PlayerEvents.InvisiblePlayersChangedHandler(OnSetVisible);
            PluginEvents.OnPluginsLoaded += new OnVoidDelegate(OnPluginsLoaded);
            ServerEvents.OnServerInitialized += new OnVoidDelegate(OnServerInitialized);
            ServerEvents.OnPacketSent += new ServerEvents.PacketSentHandler(OnPacketSend);
            PlayerEvents.OnPrefixAdded += new PlayerEvents.PrefixesChangedHandler(OnPrefixChange);
            PlayerEvents.OnPrefixRemoved += new PlayerEvents.PrefixesChangedHandler(OnPrefixChange);
            PlayerEvents.OnSuffixAdded += new PlayerEvents.SuffixesChangedHandler(OnSuffixChange);
            PlayerEvents.OnSuffixRemoved += new PlayerEvents.SuffixesChangedHandler(OnSuffixChange);
            PlayerEvents.OnGroupAdded += new PlayerEvents.GroupsChangedHandler(OnGroupChange);
            PlayerEvents.OnGroupRemoved += new PlayerEvents.GroupsChangedHandler(OnGroupChange);
            PlayerEvents.OnPlayerDied += new PlayerEvents.PlayerDeathHandler(OnPlayerDie);
            PlayerEvents.OnPlayerKill += new PlayerEvents.PlayerKillHandler(OnPlayerKill);

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
        void Update() => ServerEvents.RunGameTick();
        #endregion

        #region Event Functions
        private void OnPlayerJoin(UnturnedPlayer player)
        {
            Group[] groups = GM.Groups.Where(a => a.Default).ToArray();

            foreach (Group g in groups)
                if (!player.Groups.Contains(g))
                    player.AddGroup(g);
        }
        private void OnPlayerLeave(UnturnedPlayer player) => UnturnedServer.RemovePlayer(player);
        private void OnPlayerChat(SteamPlayer player, EChatMode mode, ref UnityEngine.Color color, string text, ref bool visible)
        {
            UnturnedPlayer ply = UnturnedPlayer.Get(player);
            UnityEngine.Color c = ply.GetColor();

            if (c != UnityEngine.Color.clear)
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
            byte[] bytes = SteamPacker.getBytes(0, out int size, new object[]
            {
                11,
                target.SteamPlayerID.steamID,
                target.SteamPlayerID.characterID,
                target.PlayerName,
                target.CharacterName,
                target.SteamPlayer.model.transform.position,
                (byte)(target.SteamPlayer.model.transform.rotation.eulerAngles.y / 2f),
                target.SteamPlayer.isPro,
                target.SteamPlayer.isAdmin && !Provider.hideAdmins,
                target.SteamPlayer.channel,
                target.SteamPlayer.playerID.group,
                target.NickName,
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
        private void OnPrefixChange(UnturnedPlayer player, string prefix)
        {
            player.CharacterName = player.GetPrefix() + player.UnturnedCharacterName + player.GetSuffix();
            player.NickName = player.GetPrefix() + player.UnturnedNickName + player.GetSuffix();

            for (int i = 0; i < UnturnedServer.Players.Length; i++)
            {
                if (UnturnedServer.Players[i].SteamID == player.SteamID)
                    continue;

                OnSetInvisible(UnturnedServer.Players[i], player);
                OnSetVisible(UnturnedServer.Players[i], player);
            }
        }
        private void OnSuffixChange(UnturnedPlayer player, string suffix)
        {
            player.CharacterName = player.GetPrefix() + player.UnturnedCharacterName + player.GetSuffix();
            player.NickName = player.GetPrefix() + player.UnturnedNickName + player.GetSuffix();

            for (int i = 0; i < UnturnedServer.Players.Length; i++)
            {
                if (UnturnedServer.Players[i].SteamID == player.SteamID)
                    continue;

                OnSetInvisible(UnturnedServer.Players[i], player);
                OnSetVisible(UnturnedServer.Players[i], player);
            }
        }
        private void OnGroupChange(UnturnedPlayer player, Group group)
        {
            player.CharacterName = player.GetPrefix() + player.UnturnedCharacterName + player.GetSuffix();
            player.NickName = player.GetPrefix() + player.UnturnedNickName + player.GetSuffix();

            for (int i = 0; i < UnturnedServer.Players.Length; i++)
            {
                if (UnturnedServer.Players[i].SteamID == player.SteamID)
                    continue;

                OnSetInvisible(UnturnedServer.Players[i], player);
                OnSetVisible(UnturnedServer.Players[i], player);
            }
        }
        private void OnPlayerDie(UnturnedPlayer player, ref EDeathCause cause, ref UnturnedPlayer killer)
        {
            player.Deaths++;
            player.TotalDeaths++;
        }
        private void OnPlayerKill(UnturnedPlayer player, ref EDeathCause cause, ref UnturnedPlayer victim)
        {
            player.Kills++;
            player.TotalKills++;
        }

        private void OnPluginsLoaded()
        {
            string plugins = string.Join(",", PM.Plugins.Select(a => a.Name).ToArray());
            SteamGameServer.SetKeyValue("pointblankplugins", plugins);
        }
        private void OnServerInitialized()
        {
            SteamGameServer.SetKeyValue("untured", Provider.APP_VERSION);
            SteamGameServer.SetKeyValue("pointblank", PointBlankInfo.Version);
        }
        private void OnPacketSend(ref CSteamID steamID, ref ESteamPacket type, ref byte[] packet, ref int size, ref int channel, ref bool cancel)
        {
            if (type != ESteamPacket.CONNECTED)
                return;

            object[] info = SteamPacker.getObjects(steamID, 0, 0, packet, new Type[]
            {
                Types.BYTE_TYPE,
                Types.STEAM_ID_TYPE,
                Types.BYTE_TYPE,
                Types.STRING_TYPE,
                Types.STRING_TYPE,
                Types.VECTOR3_TYPE,
                Types.BYTE_TYPE,
                Types.BOOLEAN_TYPE,
                Types.BOOLEAN_TYPE,
                Types.INT32_TYPE,
                Types.STEAM_ID_TYPE,
                Types.STRING_TYPE,
                Types.BYTE_TYPE,
                Types.BYTE_TYPE,
                Types.BYTE_TYPE,
                Types.COLOR_TYPE,
                Types.COLOR_TYPE,
                Types.BOOLEAN_TYPE,
                Types.INT32_TYPE,
                Types.INT32_TYPE,
                Types.INT32_TYPE,
                Types.INT32_TYPE,
                Types.INT32_TYPE,
                Types.INT32_TYPE,
                Types.INT32_TYPE,
                Types.INT32_ARRAY_TYPE,
                Types.BYTE_TYPE,
                Types.STRING_TYPE
            });
            UnturnedPlayer player = UnturnedPlayer.Get((CSteamID)info[1]);

            if(player.SteamID != steamID)
            {
                info[3] = player.PlayerName;
                info[4] = player.CharacterName;
                info[11] = player.NickName;
            }
            else
            {
                info[3] = player.PlayerName;
                info[4] = player.UnturnedCharacterName;
                info[11] = player.UnturnedNickName;
            }

            packet = SteamPacker.getBytes(0, out size, info);
        }
        #endregion
    }
}
