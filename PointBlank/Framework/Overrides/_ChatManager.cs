using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using Steamworks;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Unturned.Chat;
using PointBlank.API.Unturned.Player;
using CM = SDG.Unturned.ChatManager;

namespace PointBlank.Framework.Overrides
{
    internal class _ChatManager
    {
        [Detour(typeof(CM), "askChat", BindingFlags.Instance | BindingFlags.Public)]
        [SteamCall]
        public void askChat(CSteamID steamID, byte mode, string text)
        {
            // Set the variables
            bool cancel = false;
            UnturnedPlayer player = UnturnedPlayer.Get(steamID);

            // Run methods
            ChatEvents.RunChatted(ref player, ref mode, ref text, ref cancel);

            // Do checks
            if (player?.SteamID == null || player.SteamID == CSteamID.Nil)
                return;
            if (string.IsNullOrEmpty(text))
                return;
            if (cancel)
                return;

            // Restore original
            DetourManager.CallOriginal(typeof(CM).GetMethod("askChat", BindingFlags.Public | BindingFlags.Instance), CM.instance, player.SteamID, mode, text);
        }
    }
}
