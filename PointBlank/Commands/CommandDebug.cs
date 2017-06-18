using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using CMD = PointBlank.API.Commands.Command;

namespace PointBlank.Commands
{
    [Command("Debug", 0)]
    internal class CommandDebug : CMD
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "debug",
            "Debug",
            "DEBUG"
        };

        public override string Help => "Shows server debug information";

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.server.debug";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;

        public override EAllowedCaller AllowedCaller => EAllowedCaller.SERVER;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            CommandWindow.Log("Debug Information:");
            CommandWindow.Log("SteamID: " + SteamGameServer.GetSteamID());
            CommandWindow.Log("IP: " + Parser.getIPFromUInt32(SteamGameServer.GetPublicIP()));
            CommandWindow.Log("Port: " + Provider.port);

            CommandWindow.Log("Bytes sent: " + Provider.bytesSent + "B");
            CommandWindow.Log("Bytes received: " + Provider.bytesReceived + "B");

            CommandWindow.Log("Average bytes sent: " + (uint)(Provider.bytesSent / Time.realtimeSinceStartup) + "B");
            CommandWindow.Log("Average bytes received: " + (uint)(Provider.bytesReceived / Time.realtimeSinceStartup) + "B");

            CommandWindow.Log("Packets sent: " + Provider.packetsSent);
            CommandWindow.Log("Packets received: " + Provider.packetsReceived);

            CommandWindow.Log("Average packets sent: " + (uint)(Provider.packetsSent / Time.realtimeSinceStartup));
            CommandWindow.Log("Average packets received: " + (uint)(Provider.packetsReceived / Time.realtimeSinceStartup));

            CommandWindow.Log("Updates per second: " + Mathf.CeilToInt((float)(Provider.debugUPS / 50f * 100f)));
            CommandWindow.Log("Ticks per second: " + Mathf.CeilToInt((float)(Provider.debugTPS / 50f * 100f)));

            CommandWindow.Log("Zombie count: " + ZombieManager.tickingZombies.Count);
            CommandWindow.Log("Animal count: " + AnimalManager.tickingAnimals.Count);
        }
    }
}
