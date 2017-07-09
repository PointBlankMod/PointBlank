using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using PointBlank.Framework.Permissions.Ring;
using PointBlank.API.Unturned.Player;

namespace PointBlank.API.Unturned.Chat
{
    /// <summary>
    /// Contains events for the chat
    /// </summary>
    [RingPermission(SecurityAction.Demand, ring = RingPermissionRing.None)]
    public static class ChatEvents
    {
        #region Handlers
        public delegate void ChattedHandler(ref UnturnedPlayer player, ref byte mode, ref string text, ref bool cancel);
        #endregion

        #region Events
        public static event ChattedHandler OnChatted;
        #endregion

        #region Functions
        internal static void RunChatted(ref UnturnedPlayer player, ref byte mode, ref string text, ref bool cancel) => OnChatted?.Invoke(ref player, ref mode, ref text, ref cancel);

        #endregion
    }
}
