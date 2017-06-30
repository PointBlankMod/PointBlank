using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Server;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace PointBlank.Framework.Overrides
{
    internal class _Provider
    {
        private static FieldInfo fiBytesSent = typeof(Provider).GetField("_bytesSent", BindingFlags.NonPublic | BindingFlags.Static);
        private static FieldInfo fiPacketsSent = typeof(Provider).GetField("_packetsSent", BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo miReceiveServer = typeof(Provider).GetMethod("receiveServer", BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo miIsUnreliable = typeof(Provider).GetMethod("isUnreliable", BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo miIsInstant = typeof(Provider).GetMethod("isInstant", BindingFlags.NonPublic | BindingFlags.Static);

        [Detour(typeof(Provider), "send", BindingFlags.Public | BindingFlags.Static)]
        public static void send(CSteamID steamID, ESteamPacket type, byte[] packet, int size, int channel)
        {
            if (!Provider.isConnected)
                return;
            bool cancel = false;

            ServerEvents.RunPacketSent(ref steamID, ref type, ref packet, ref size, ref channel, ref cancel);
            if (cancel)
                return;
            if(steamID == Provider.server)
            {
                miReceiveServer.Invoke(null, new object[] { Provider.server, packet, 0, size, channel });
                return;
            }
            if(steamID.m_SteamID == 0uL)
            {
                Debug.LogError("Failed to send to invalid steam ID.");
                return;
            }
            if((bool)miIsUnreliable.Invoke(null, new object[] { type }))
            {
                for(int i = 0; i < 3; i++) // Fix for the queue #1 stuck position
                    if (SteamGameServerNetworking.SendP2PPacket(steamID, packet, (uint)size, (!(bool)miIsInstant.Invoke(null, new object[] { type })) ? EP2PSend.k_EP2PSendUnreliable : EP2PSend.k_EP2PSendUnreliableNoDelay, channel))
                        break;
                return;
            }
            for(int i = 0; i < 3; i++) // Fix for the queue #1 stuck position
                if (SteamGameServerNetworking.SendP2PPacket(steamID, packet, (uint)size, (!(bool)miIsInstant.Invoke(null, new object[] { type })) ? EP2PSend.k_EP2PSendReliableWithBuffering : EP2PSend.k_EP2PSendReliable, channel))
                    break;
        }
    }
}
