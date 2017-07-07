using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using Steamworks;

namespace PointBlank.API.Unturned
{
    /// <summary>
    /// Implementation of the unturned ownership tool
    /// </summary>
    public static class OwnershipTool
    {
        public static bool checkToggle(ulong player, ulong group)
        {
            return !Dedicator.isDedicated && OwnershipTool.checkToggle(Provider.client, player, SDG.Unturned.Player.player.quests.groupID, group);
        }

        public static bool checkToggle(CSteamID player_0, ulong player_1, CSteamID group_0, ulong group_1)
        {
            return (Provider.isServer && !Dedicator.isDedicated) || player_0.m_SteamID == player_1 || (group_0 != CSteamID.Nil && group_0.m_SteamID == group_1);
        }
    }
}
