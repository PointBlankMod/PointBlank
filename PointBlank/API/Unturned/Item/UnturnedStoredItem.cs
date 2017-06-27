using System;
using System.Collections.Generic;
using System.Linq;
using SDG.Unturned;
using UnityEngine;
using PointBlank.API.Unturned.Server;
using UItem = SDG.Unturned.Item;

namespace PointBlank.API.Unturned.Item
{
    public class UnturnedStoredItem
    {
        #region Properties

        public ItemJar Jar { get; private set; }

        public Byte Rot => Jar.rot;

        public Byte Size_X => Jar.size_x;

        public Byte Size_Y => Jar.size_y;

        public Byte X => Jar.x;

        public Byte Y => Jar.y;

        public UItem Item => Jar.item;

        public InteractableItem Interactable => Jar.interactableItem;

        public ushort ID => Item.id;

        public Byte Amount => Item.amount;

        public ItemAsset Asset => Interactable.asset;

        public String Name => Asset.itemName;

        public String Description => Asset.itemDescription;

        public GameObject Object => Asset.item;

        public AudioClip EquipAudio => Asset.equip;

        public AnimationClip[] Animations => Asset.animations;

        public List<Blueprint> Blueprints => Asset.blueprints;

        public List<SDG.Unturned.Action> Actions => Asset.actions;

        public Texture AlbedoBase => Asset.albedoBase;

        public Texture MetallicBase => Asset.metallicBase;

        public Texture EmissionBase => Asset.emissionBase;

        public EAssetType AssetCategory = EAssetType.ITEM;

        public bool IsDangerous = false;

        public Transform Transform => Object.transform;

        public Quaternion Rotation => Transform.rotation;

        public Vector3 Position => Transform.position;

        public Byte[] Metadata
        {
            get => Item.state;
            set => Item.state = value;
        }

        public Byte Durability
        {
            get => Item.quality;
            set => Item.quality = value;
        }

        #endregion Properties

        internal UnturnedStoredItem(ItemJar jar)
        {
            // Set the variables
            Jar = jar;
        }
    }
}