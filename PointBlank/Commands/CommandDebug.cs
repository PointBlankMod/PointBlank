using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using PointBlank.API.Collections;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Debug", 0)]
    internal class CommandDebug : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "Debug"
        };

        public override string Help => Translations["Debug_Help"];

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.server.debug";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override EAllowedCaller AllowedCaller => EAllowedCaller.SERVER;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            CommandWindow.Log(Translations["Debug_Title"]);
            CommandWindow.Log(string.Format(Translations["Debug_SteamID"], SteamGameServer.GetSteamID()));
            CommandWindow.Log(string.Format(Translations["Debug_IP"], Parser.getIPFromUInt32(SteamGameServer.GetPublicIP())));
            CommandWindow.Log(string.Format(Translations["Debug_Port"], Provider.port));

            CommandWindow.Log(string.Format(Translations["Debug_BytesSent"], Provider.bytesSent));
            CommandWindow.Log(string.Format(Translations["Debug_BytesReceived"], Provider.bytesReceived));

            CommandWindow.Log(string.Format(Translations["Debug_ABytesSent"], (uint)(Provider.bytesSent / Time.realtimeSinceStartup)));
            CommandWindow.Log(string.Format(Translations["Debug_ABytesReceived"], (uint)(Provider.bytesReceived / Time.realtimeSinceStartup)));

            CommandWindow.Log(string.Format(Translations["Debug_PacketsSent"], Provider.packetsSent));
            CommandWindow.Log(string.Format(Translations["Debug_PacketsReceived"], Provider.packetsReceived));

            CommandWindow.Log(string.Format(Translations["Debug_APacketsSent"], (uint)(Provider.packetsSent / Time.realtimeSinceStartup)));
            CommandWindow.Log(string.Format(Translations["Debug_APacketsReceived"], (uint)(Provider.packetsReceived / Time.realtimeSinceStartup)));

            CommandWindow.Log(string.Format(Translations["Debug_UPS"], Mathf.CeilToInt(Provider.debugUPS / 50f * 100f)));
            CommandWindow.Log(string.Format(Translations["Debug_TPS"], Mathf.CeilToInt((Provider.debugTPS / 50f * 100f))));

            CommandWindow.Log(string.Format(Translations["Debug_Zombies"], ZombieManager.tickingZombies.Count));
            CommandWindow.Log(string.Format(Translations["Debug_Animals"], AnimalManager.tickingAnimals.Count));
        }
    }
}
