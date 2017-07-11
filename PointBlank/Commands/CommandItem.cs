using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Unturned.Player;
using PointBlank.API.Unturned.Chat;
using SDG.Unturned;
using PointBlank.API.Collections;
using Translation = PointBlank.Framework.Translations.CommandTranslations;

namespace PointBlank.Commands
{
    [PointBlankCommand("Item", 1)]
    internal class CommandItem : PointBlankCommand
    {
        #region Properties
        public TranslationList Translations = Enviroment.ServiceTranslations[typeof(Translation)].Translations;

        public override string[] DefaultCommands => new string[]
        {
            "i",
            "Item"
        };

        public override string Help => Translations["Item_Help"];

        public override string Usage => Commands[0] + Translations["Item_Usage"];

        public override string DefaultPermission => "pointblank.commands.admin.item";

        public override EAllowedServerState AllowedServerState => EAllowedServerState.RUNNING;
        #endregion

        public override void Execute(UnturnedPlayer executor, string[] args)
        {
            ItemAsset item;

            if(!ushort.TryParse(args[0], out ushort id))
            {
                ItemAsset[] items = Assets.find(EAssetType.ITEM) as ItemAsset[];

                item = items.Where(a => a != null).OrderBy(a => a.itemName.Length).FirstOrDefault(a => a.itemName.ToLower().Contains(args[0].ToLower()));
            }
            else
            {
                item = Assets.find(EAssetType.ITEM, id) as ItemAsset;
            }
            if (item == null)
            {
                UnturnedChat.SendMessage(executor, Translations["Item_Invalid"], ConsoleColor.Red);
                return;
            }

            if(args.Length < 2 || !byte.TryParse(args[1], out byte amount))
                amount = 1;

            if(args.Length < 3 || UnturnedPlayer.TryGetPlayer(args[2], out UnturnedPlayer ply))
            {
                if(executor == null)
                {
                    UnturnedChat.SendMessage(executor, Translations["Base_InvalidPlayer"], ConsoleColor.Red);
                    return;
                }

                ply = executor;
            }

            if(!ItemTool.tryForceGiveItem(ply.Player, item.id, amount))
            {
                UnturnedChat.SendMessage(executor, Translations["Item_Fail"], ConsoleColor.Red);
                return;
            }
            UnturnedChat.SendMessage(executor, string.Format(Translations["Item_Give"], item.itemName, ply.PlayerName), ConsoleColor.Green);
        }
    }
}
