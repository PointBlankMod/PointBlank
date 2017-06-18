using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using SDG.Unturned;
using CM = PointBlank.API.Unturned.Chat.ChatManager;
using CMD = PointBlank.API.Commands.Command;

namespace PointBlank.Commands
{
    [Command("Item", 1)]
    internal class CommandItem : CMD
    {
        #region Properties
        public override string[] DefaultCommands => new string[]
        {
            "i",
            "I",
            "item",
            "Item",
            "ITEM"
        };

        public override string Help => "Gives the player a specific item";

        public override string Usage => Commands[0] + " <item> [amount] [player]";

        public override string DefaultPermission => "pointblank.commands.admin.item";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            UnturnedPlayer ply;
            ItemAsset item;
            ushort id;
            byte amount;

            if(!ushort.TryParse(args[0], out id))
            {
                ItemAsset[] items = Assets.find(EAssetType.ITEM) as ItemAsset[];
                item = items.Where(a => a != null).OrderBy(a => a.itemName.Length).FirstOrDefault(a => a.itemName.ToLower().Contains(args[0]));
            }
            else
            {
                item = Assets.find(EAssetType.ITEM, id) as ItemAsset;
            }
            if (item == null)
            {
                CM.SendMessage(executor, "Could not find item!", ConsoleColor.Red);
                return;
            }

            if(args.Length < 2 || !byte.TryParse(args[1], out amount))
                amount = 1;

            if(args.Length < 3 || UnturnedPlayer.TryGetPlayer(args[2], out ply))
            {
                if(executor == null)
                {
                    CM.SendMessage(executor, "Player not found!", ConsoleColor.Red);
                    return;
                }

                ply = executor;
            }

            if(!ItemTool.tryForceGiveItem(ply.Player, id, amount))
            {
                CM.SendMessage(executor, "Could not give item to player!", ConsoleColor.Red);
                return;
            }
            CM.SendMessage(executor, item.itemName + " given to " + ply.PlayerName, ConsoleColor.Green);
        }
    }
}
