using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Steamworks;
using SDG.Unturned;
using PointBlank.API.Unturned.Player;
using CM = SDG.Unturned.ChatManager;

namespace PointBlank.API.Unturned.Chat
{
    /// <summary>
    /// The unturned chat manager
    /// </summary>
    public static class UnturnedChat
    {
        #region Public Functions
        /// <summary>
        /// Broadcasts a message to all the players
        /// </summary>
        /// <param name="text">The text to broadcast</param>
        /// <param name="color">The color of the broadcast</param>
        public static void Broadcast(string text, Color color) => CM.say(text, color);

        /// <summary>
        /// Sends a private message to a player
        /// </summary>
        /// <param name="user">The steamID of the player to send to</param>
        /// <param name="text">The message text to send</param>
        /// <param name="color">The message color to send</param>
        /// <param name="mode">The message mode</param>
        public static void Tell(CSteamID user, string text, Color color, EChatMode mode = EChatMode.SAY) => CM.say(user, text, color, mode);

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

        /// <summary>
        /// Sends a message to the player or the console
        /// </summary>
        /// <param name="player">The player(null if the console)</param>
        /// <param name="text">The message</param>
        /// <param name="color">The message color</param>
        public static void SendMessage(UnturnedPlayer player, object text, ConsoleColor color = ConsoleColor.White)
        {
            if(player == null)
            {
                CommandWindow.Log(text, color);
                return;
            }

            Color c;

            switch (color)
            {
                case ConsoleColor.Black:
                    c = Color.black;
                    break;
                case ConsoleColor.Blue:
                    c = Color.blue;
                    break;
                case ConsoleColor.Cyan:
                    c = Color.cyan;
                    break;
                case ConsoleColor.DarkBlue:
                    c = new Color(0, 0, 139);
                    break;
                case ConsoleColor.DarkCyan:
                    c = new Color(0, 139, 139);
                    break;
                case ConsoleColor.DarkGray:
                    c = new Color(169, 169, 169);
                    break;
                case ConsoleColor.DarkGreen:
                    c = new Color(0, 100, 0);
                    break;
                case ConsoleColor.DarkMagenta:
                    c = new Color(139, 0, 139);
                    break;
                case ConsoleColor.DarkRed:
                    c = new Color(139, 0, 0);
                    break;
                case ConsoleColor.DarkYellow:
                    c = new Color(153, 153, 0);
                    break;
                case ConsoleColor.Gray:
                    c = Color.gray;
                    break;
                case ConsoleColor.Green:
                    c = Color.green;
                    break;
                case ConsoleColor.Magenta:
                    c = Color.magenta;
                    break;
                case ConsoleColor.Red:
                    c = Color.red;
                    break;
                case ConsoleColor.White:
                    c = Color.white;
                    break;
                case ConsoleColor.Yellow:
                    c = Color.yellow;
                    break;
                default:
                    c = Color.white;
                    break;
            }
            player.SendMessage(text.ToString(), c);
        }
        #endregion
    }
}
