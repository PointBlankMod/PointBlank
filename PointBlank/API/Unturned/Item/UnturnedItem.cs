using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Unturned.Server;
using SDG.Unturned;
using UnityEngine;

namespace PointBlank.API.Unturned.Item
{
    public class UnturnedItem
    {
        #region Properties

        public ItemDrop Item { get; private set;  }

        public Transform Model => Item.model;

        public InteractableItem Interactable => Item.interactableItem;

        public uint Instance => Item.instanceID;

        #endregion Properties

        private UnturnedItem(ItemDrop item)
        {
            Item = item;
        }
    }
}
