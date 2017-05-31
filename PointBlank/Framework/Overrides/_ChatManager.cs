using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using Steamworks;
using PointBlank.API;
using PointBlank.API.Detour;
using PointBlank.API.Unturned;

namespace PointBlank.Framework.Overrides
{
    internal class _ChatManager
    {
        [Detour(typeof(ChatManager), "askChat", BindingFlags.Instance | BindingFlags.Public)]
        [SteamCall]
        public void askChat(CSteamID steamID, byte mode, string text)
        {
            // Set the variables
            bool cancel = false;

            // Run methods
            ChatEvents.RunChatted(ref steamID, ref mode, ref text, ref cancel);

            // Do checks
            if (steamID == null || steamID == CSteamID.Nil)
                return;
            if (string.IsNullOrEmpty(text))
                return;
            if (cancel)
                return;

            // Restore original
            DetourManager.CallOriginal(typeof(ChatManager).GetMethod("askChat", BindingFlags.Public | BindingFlags.Instance), ChatManager.instance, steamID, mode, text);
        }
    }
}
