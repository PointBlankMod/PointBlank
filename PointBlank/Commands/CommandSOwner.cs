using System.Collections.Generic;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using Translation = PointBlank.Framework.Translations.CommandTranslations;
using UnityEngine;

namespace PointBlank.Commands
{
    [PointBlankCommand("SOwner", 0)]
    internal class CommandSOwner : PointBlankCommand
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "SOwner"
        };

        public override string Help => Translation.SOwner_Help;

        public override string Usage => Commands[0];

        public override string DefaultPermission => "unturned.commands.sowner";
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            List<RegionCoordinate> r = new List<RegionCoordinate>();
            Regions.getRegionsInRadius(executor.Position, 5 , r);
            List<Transform> structures = new List<Transform>();
            StructureManager.getStructuresInRadius(executor.Position, 5, r, structures);
            foreach (Transform transform in structures)
            {
                byte x;
                byte y;
                ushort index;
                StructureRegion region;
                if (StructureManager.tryGetInfo(transform, out x, out y, out index, out region))
                {
                    UnturnedChat.Tell(executor.SteamID, "[Owner: " + region.structures[(int)index].owner + "] : " + region.structures[(int)index].structure.asset.itemName, Color.blue, EChatMode.GLOBAL);
                }
            }
        }
    }
}
