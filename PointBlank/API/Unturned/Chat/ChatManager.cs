using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Steamworks;
using SDG.Unturned;
using CM = SDG.Unturned.ChatManager;

namespace PointBlank.API.Unturned.Chat
{
    /// <summary>
    /// The unturned chat manager
    /// </summary>
    public static class ChatManager
    {
        #region Public Functions
        /// <summary>
        /// Broadcasts a message to all the players
        /// </summary>
        /// <param name="text">The text to broadcast</param>
        /// <param name="color">The color of the broadcast</param>
        public static void Broadcast(string text, Color color)
        {
            CM.say(text, color);
        }

        /// <summary>
        /// Sends a private message to a player
        /// </summary>
        /// <param name="user">The steamID of the player to send to</param>
        /// <param name="text">The message text to send</param>
        /// <param name="color">The message color to send</param>
        /// <param name="mode">The message mode</param>
        public static void Tell(CSteamID user, string text, Color color, EChatMode mode = EChatMode.SAY)
        {
            CM.say(user, text, color, mode);
        }

        /// <summary>
        /// Sends a fake global message as a user
        /// </summary>
        /// <param name="sender">The person that "sent" the message</param>
        /// <param name="text">The text he sent</param>
        /// <param name="color">The color of the message</param>
        public static void FakeMessage(CSteamID sender, string text, Color color)
        {
            if (text.Length > CM.LENGTH)
                text = text.Substring(0, CM.LENGTH);

            CM.instance.channel.send("tellChat", ESteamCall.CLIENTS, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
            {
                sender,
                0,
                color,
                text
            });
        }
        #endregion
    }
}
