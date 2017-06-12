using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UPlayer = SDG.Unturned.Player;
using SPlayer = SDG.Unturned.SteamPlayer;
using Steamworks;
using UnityEngine;
using PointBlank.API.Groups;
using PointBlank.API.Unturned.Server;
using PointBlank.API.DataManagment;
using SP = PointBlank.API.Steam.SteamPlayer;
using Newtonsoft.Json.Linq;
using System.Globalization;
using RG = PointBlank.API.Steam.SteamGroup;
using PointBlank.API.Unturned.Vehicle;

namespace PointBlank.API.Unturned.Player
{
    /// <summary>
    /// The unturned player instance
    /// </summary>
    public class UnturnedPlayer
    {
        #region Variables
        private List<UnturnedPlayer> _VisiblePlayers = new List<UnturnedPlayer>();
        private List<Group> _Groups = new List<Group>();
        private List<string> _Prefixes = new List<string>();
        private List<string> _Suffixes = new List<string>();
        private List<string> _Permissions = new List<string>();
        #endregion

        #region Properties
        // Important information
        /// <summary>
        /// The unturned player instance
        /// </summary>
        public UPlayer Player => SteamPlayer.player;
        /// <summary>
        /// The steam player instance
        /// </summary>
        public SPlayer SteamPlayer { get; private set; }
        /// <summary>
        /// The steam player ID instance
        /// </summary>
        public SteamPlayerID SteamPlayerID => SteamPlayer.playerID;
        /// <summary>
        /// The steam information about the player
        /// </summary>
        public SP Steam { get; private set; }

        // Steam player ID information
        /// <summary>
        /// The player's name
        /// </summary>
        public string PlayerName => SteamPlayerID.playerName;
        /// <summary>
        /// The character's name
        /// </summary>
        public string CharacterName => SteamPlayerID.characterName;
        /// <summary>
        /// The player's steam ID
        /// </summary>
        public CSteamID SteamID => SteamPlayerID.steamID;
        /// <summary>
        /// The character's ID
        /// </summary>
        public byte CharacterID => SteamPlayerID.characterID;

        // Steam player information
        /// <summary>
        /// Is the player an admin
        /// </summary>
        public bool IsAdmin => SteamPlayer.isAdmin;
        /// <summary>
        /// player ping
        /// </summary>
        public float Ping => SteamPlayer.ping;
        /// <summary>
        /// Is the player a pro buyer
        /// </summary>
        public bool IsPro => SteamPlayer.isPro;
        /// <summary>
        /// The player skillset
        /// </summary>
        public EPlayerSkillset Skillset => SteamPlayer.skillset;
        /// <summary>
        /// Is the player muted
        /// </summary>
        public bool IsMuted { get { return SteamPlayer.isMuted; } set { SteamPlayer.isMuted = value; } }

        // Player information
        /// <summary>
        /// The player's health
        /// </summary>
        public byte Health
        {
            get
            {
                return Player.life.health;
            }
            set
            {
                // Do checks
                if (Player.life.isDead)
                    return;
                if (value < 1)
                    Player.life.sendSuicide();

                // Set data
                typeof(PlayerLife).GetField("_health", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Player.life, value);

                // Send update
                Player.life.channel.send("tellHealth", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    Health
                });
            }
        }
        /// <summary>
        /// The player's stamina
        /// </summary>
        public byte Stamina => Player.life.stamina;
        /// <summary>
        /// The player's food/hunger
        /// </summary>
        public byte Food
        {
            get
            {
                return Player.life.food;
            }
            set
            {
                // Checks
                if (Player.life.isDead)
                    return;
                if (value < 0)
                    return;

                // Data
                typeof(PlayerLife).GetField("_food", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Player.life, value);

                // Update
                Player.life.channel.send("tellFood", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    Food
                });
            }
        }
        /// <summary>
        /// The player's water/thirst
        /// </summary>
        public byte Thirst
        {
            get
            {
                return Player.life.water;
            }
            set
            {
                // Checks
                if (Player.life.isDead)
                    return;
                if (value < 0)
                    return;

                // Data
                typeof(PlayerLife).GetField("_water", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Player.life, value);

                // Update
                Player.life.channel.send("tellWater", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    Thirst
                });
            }
        }
        /// <summary>
        /// The player's virus/infection
        /// </summary>
        public byte Virus
        {
            get
            {
                return Player.life.virus;
            }
            set
            {
                // Checks
                if (Player.life.isDead)
                    return;
                if (value < 0)
                    return;

                // Data
                typeof(PlayerLife).GetField("_virus", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Player.life, value);

                // Update
                Player.life.channel.send("tellVirus", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    Virus
                });
            }
        }
        /// <summary>
        /// Are the player's legs broken
        /// </summary>
        public bool IsBroken
        {
            get
            {
                return Player.life.isBroken;
            }
            set
            {
                // Checks
                if (Player.life.isDead)
                    return;
                if (value == IsBroken)
                    return;

                // Data
                typeof(PlayerLife).GetField("_isBroken", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Player.life, value);

                // Update
                Player.life.channel.send("tellBroken", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    IsBroken
                });
            }
        }
        /// <summary>
        /// Is the player bleeding
        /// </summary>
        public bool IsBleeding
        {
            get
            {
                return Player.life.isBleeding;
            }
            set
            {
                // Checks
                if (Player.life.isDead)
                    return;
                if (value == IsBleeding)
                    return;

                // Data
                typeof(PlayerLife).GetField("_isBleeding", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Player.life, value);

                // Update
                Player.life.channel.send("tellBleeding", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    IsBleeding
                });
            }
        }
        /// <summary>
        /// Is the player dead
        /// </summary>
        public bool IsDead { get { return Player.life.isDead; } }
        /// <summary>
        /// The player's skill boost
        /// </summary>
        public EPlayerBoost SkillBoost
        {
            get
            {
                return Player.skills.boost;
            }
            set
            {
                // Data
                typeof(PlayerSkills).GetField("_boost", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Player.skills, value);

                // Update
                Player.skills.channel.send("tellBoost", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    SkillBoost
                });
            }
        }
        /// <summary>
        /// The player's skill experience
        /// </summary>
        public uint Experience
        {
            get
            {
                return Player.skills.experience;
            }
            set
            {
                // Data
                typeof(PlayerSkills).GetField("_experience", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Player.skills, value);

                // Update
                Player.skills.channel.send("tellExperience", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    Experience
                });
            }
        }
        /// <summary>
        /// Checks if player is in vehicle
        /// </summary>
        public bool IsInVehicle
        {
            get
            {
                return Player.movement.getVehicle() != null;
            }
        }
        /// <summary>
        /// The player current vehicle
        /// </summary>
        public UnturnedVehicle Vehicle => UnturnedVehicle.Create(Player.movement.getVehicle());
        /// <summary>
        /// The player's reputation
        /// </summary>
        public int Reputation
        {
            get
            {
                return Player.skills.reputation;
            }
            set
            {
                // Data
                typeof(PlayerSkills).GetField("_reputation", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Player.skills, value);

                // Update
                Player.skills.channel.send("tellReputation", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    Reputation
                });
            }
        }
        /// <summary>
        /// The player's skills
        /// </summary>
        public Skill[][] Skills => Player.skills.skills;
        /// <summary>
        /// Has the player got anything equipped
        /// </summary>
        public bool HasAnythingEquipped => (Player.equipment.isEquipped && Player.equipment.asset != null && Player.equipment.useable != null);
        /// <summary>
        /// The currently equipped asset
        /// </summary>
        public ItemAsset EquippedAsset => Player.equipment.asset;
        /// <summary>
        /// The currently equpped useable
        /// </summary>
        public Useable EquippedUseable => Player.equipment.useable;
        /// <summary>
        /// The currently equipped item ID
        /// </summary>
        public ushort EquippedItemID => Player.equipment.itemID;
        /// <summary>
        /// Is the currently equpped item a primary
        /// </summary>
        public bool IsEquippedPrimary => Player.equipment.primary;
        /// <summary>
        /// Is the current equipment busy
        /// </summary>
        public bool IsEquipmentBusy { get { return Player.equipment.isBusy; } set { Player.equipment.isBusy = value; } }
        /// <summary>
        /// Can the equipped item be inspected
        /// </summary>
        public bool CanInspectEquipped => Player.equipment.canInspect;
        /// <summary>
        /// Is the player currently inspecting the equipped item
        /// </summary>
        public bool IsInspectingEquipped => Player.equipment.isInspecting;
        /// <summary>
        /// Current position of the player
        ///</summary>
        public Vector3 Position => Player.transform.position;
        /// <summary>
        /// Current rotation of the player
        ///</summary>
        public Quaternion Rotation => Player.transform.rotation;
        /// <summary>
        /// Is the player moving
        ///</summary>
        public bool IsMoving => Player.movement.isMoving;
        /// <summary>
        /// Is the player grounded
        ///</summary>
        public bool IsGrounded => Player.movement.isGrounded;
        /// <summary>
        /// Is the player safe
        ///</summary>
        public bool IsSafe => Player.movement.isSafe;
        /// <summary>
        /// Is the player loading (Thanks Trojaner)
        ///</summary>
        public bool IsLoading => Provider.pending.Contains(Provider.pending.Find((c) => c.playerID.steamID == SteamID));
        /// <summary>
        /// IP of the player
        ///</summary>
        public string IP
        {
            get
            {
                P2PSessionState_t State;

                SteamGameServerNetworking.GetP2PSessionState(SteamID, out State);

                return Parser.getIPFromUInt32(State.m_nRemoteIP);
            }
        }
        /// <summary>
        /// Array of items in the player's inventory
        ///</summary>
        public Item[] Items
        {
            get
            {
                List<Item> retval = new List<Item>();
                for (byte page = 0; page < (PlayerInventory.PAGES - 1); page++)
                {
                    byte count = Player.inventory.getItemCount(page);
                    if (count > 0)
                    {
                        for (byte index = 0; index < count; index++)
                            retval.Add(Player.inventory.getItem(page, index).item);
                    }
                }
                return retval.ToArray();
            }
        }
        /// <summary>
        /// Is the player member of a quest group
        /// </summary>
        public bool IsInQuestGroup => Player.quests.isMemberOfAGroup;
        /// <summary>
        /// The ID of the quest group
        /// </summary>
        public CSteamID QuestGroupID => Player.quests.groupID;
        /// <summary>
        /// Vehicles the player has locked
        ///</summary>
        public UnturnedVehicle[] LockedVehicles => UnturnedServer.Vehicles.Where(v => v.LockedOwner == SteamID || (IsInQuestGroup && QuestGroupID == v.LockedGroup)).ToArray();

        // Extra data
        /// <summary>
        /// Any custom data you want to attach to the player
        /// </summary>
        public Dictionary<string, object> Metadata { get; private set; }
        /// <summary>
        /// The command cooldown for the player
        /// </summary>
        public int Cooldown { get; set; }
        /// <summary>
        /// The players this player can see are in the server
        /// </summary>
        public UnturnedPlayer[] VisiblePlayers => _VisiblePlayers.ToArray();
        /// <summary>
        /// The groups this player is part of
        /// </summary>
        public Group[] Groups => _Groups.ToArray();
        /// <summary>
        /// The steam groups this player is part of
        /// </summary>
        public RG[] SteamGroups => Steam.Groups;
        /// <summary>
        /// The prefixes of the player
        /// </summary>
        public string[] Prefixes => _Prefixes.ToArray();
        /// <summary>
        /// The suffixes of the player
        /// </summary>
        public string[] Suffixes => _Suffixes.ToArray();
        /// <summary>
        /// The permissions of the player
        /// </summary>
        public string[] Permissions => _Permissions.ToArray();
        #endregion

        private UnturnedPlayer(SPlayer steamplayer)
        {
            // Setup the variables
            Metadata = new Dictionary<string, object>();

            // Set the variables
            this.SteamPlayer = steamplayer;
            this.Steam = new SP(SteamID.m_SteamID);
            this.Cooldown = -1;

            // Run code
            UnturnedServer.AddPlayer(this);
        }

        #region Static Functions
        /// <summary>
        /// Creates the unturned player instance or returns an existing one
        /// </summary>
        /// <param name="steamplayer">The steam player to build from</param>
        /// <returns>An unturned player instance</returns>
        internal static UnturnedPlayer Create(SPlayer steamplayer)
        {
            UnturnedPlayer ply = UnturnedServer.Players.FirstOrDefault(a => a.SteamPlayer == steamplayer);

            if (ply != null)
                return ply;
            return new UnturnedPlayer(steamplayer);
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Make a player visible to this player
        /// </summary>
        /// <param name="player">The player to make visible</param>
        public void AddVisiblePlayer(UnturnedPlayer player)
        {
            if (VisiblePlayers.Contains(player))
                return;

            _VisiblePlayers.Add(player);
            PlayerEvents.RunVisiblePlayerAdd(this, player);
        }
        /// <summary>
        /// Make a player invisible to this player
        /// </summary>
        /// <param name="player">The player to make invisible</param>
        public void RemoveVisiblePlayer(UnturnedPlayer player)
        {
            if (!VisiblePlayers.Contains(player))
                return;

            _VisiblePlayers.Remove(player);
            PlayerEvents.RunVisiblePlayerRemove(this, player);
        }

        /// <summary>
        /// Add the player to a group
        /// </summary>
        /// <param name="group">The group to add the player to</param>
        public void AddGroup(Group group)
        {
            if (Groups.Contains(group))
                return;

            _Groups.Add(group);
            PlayerEvents.RunGroupAdd(this, group);
        }
        /// <summary>
        /// Remove the player from a group
        /// </summary>
        /// <param name="group">The group to remove the player from</param>
        public void RemoveGroup(Group group)
        {
            if (!Groups.Contains(group))
                return;

            _Groups.Remove(group);
            PlayerEvents.RunGroupRemove(this, group);
        }

        /// <summary>
        /// Adds a prefix to the player
        /// </summary>
        /// <param name="prefix">The prefix to add</param>
        public void AddPrefix(string prefix)
        {
            if (Prefixes.Contains(prefix))
                return;

            _Prefixes.Add(prefix);
            PlayerEvents.RunPrefixAdd(this, prefix);
        }
        /// <summary>
        /// Removes a prefix from the player
        /// </summary>
        /// <param name="prefix">The prefix to remove</param>
        public void RemovePrefix(string prefix)
        {
            if (!Prefixes.Contains(prefix))
                return;

            _Prefixes.Remove(prefix);
            PlayerEvents.RunPrefixRemove(this, prefix);
        }

        /// <summary>
        /// Adds a suffix to the player
        /// </summary>
        /// <param name="suffix">The suffix to add</param>
        public void AddSuffix(string suffix)
        {
            if (Suffixes.Contains(suffix))
                return;

            _Suffixes.Add(suffix);
            PlayerEvents.RunSuffixAdd(this, suffix);
        }
        /// <summary>
        /// Removes a suffix from the player
        /// </summary>
        /// <param name="suffix">The suffix to remove</param>
        public void RemoveSuffix(string suffix)
        {
            if (!Suffixes.Contains(suffix))
                return;

            _Suffixes.Remove(suffix);
            PlayerEvents.RunSuffixRemove(this, suffix);
        }

        /// <summary>
        /// Adds a permission to the player
        /// </summary>
        /// <param name="permission">The permission to add</param>
        public void AddPermission(string permission)
        {
            if (Permissions.Contains(permission))
                return;

            _Permissions.Add(permission);
            PlayerEvents.RunPermissionAdd(this, permission);
        }
        /// <summary>
        /// Removes a permission from the player
        /// </summary>
        /// <param name="permission">The permission to remove</param>
        public void RemovePermission(string permission)
        {
            if (!Permissions.Contains(permission))
                return;

            _Permissions.Remove(permission);
            PlayerEvents.RunPermissionRemove(this, permission);
        }
        #endregion

        #region Unturned Functions
        /// <summary>
        /// Checks if the player has a specific item
        /// </summary>
        /// <param name="ID">The item ID to find</param>
        /// <returns>If the player has the specific item in their inventory</returns>
        public bool HasItem(ushort ID)
        {
            if (EquippedItemID == ID) return true;

            return (Player.inventory.search(ID, true, true).Count > 0); // The easy way
        }

        /// <summary>
        /// Checks if the player has a specific item
        /// </summary>
        /// <param name="Item">The item to find</param>
        /// <returns>If the player has the item in the inventory</returns>
        public bool HasItem(Item Item)
        {
            return HasItem(Item.id);
        }

        /// <summary>
        /// Checks if the player has a specific item
        /// </summary>
        /// <param name="Name">The item to find</param>
        /// <returns>If the player has the item in the inventory</returns>
        public bool HasItem(string Name)
        {
            return HasItem(((ItemAsset)Assets.find(EAssetType.ITEM, Name)).id);
        }

        /// <summary>
        /// Gives the player an item
        /// </summary>
        /// <param name="ID">The ID of the item</param>
        /// <returns>If the item was given to the player</returns>
        public bool GiveItem(ushort ID)
        {
            return ItemTool.tryForceGiveItem(Player, ID, 1);
        }

        /// <summary>
        /// Gives the player an item
        /// </summary>
        /// <param name="ID">The item instance to give to the player</param>
        /// <returns>If the item was given to the player</returns>
        public bool GiveItem(Item Item)
        {
            return GiveItem(Item.id);
        }

        /// <summary>
        /// Gives the player an item
        /// </summary>
        /// <param name="ID">The name of the item to give to the player</param>
        /// <returns>If the item was given to the player</returns>
        public bool GiveItem(String Name)
        {
            return GiveItem((Assets.find(EAssetType.ITEM, Name) as ItemAsset).id);
        }

        /// <summary>
        /// Removes an item from the player's inventory
        /// </summary>
        /// <param name="ID">The ID of the item to remove</param>
        /// <returns>If the item was removed</returns>
        public bool RemoveItem(ushort ID)
        {
            if (EquippedItemID == ID)
                Player.equipment.dequip();
            InventorySearch search = Player.inventory.search(ID, true, true).FirstOrDefault();

            if (search == null)
                return false;
            Items items = Player.inventory.items[search.page];

            items.removeItem(items.getIndex(search.jar.x, search.jar.y));
            return true;
        }

        /// <summary>
        /// Removes an item from the player's inventory
        /// </summary>
        /// <param name="Item">The item instance to remove</param>
        /// <returns>If the item was removed</returns>
        public bool RemoveItem(Item Item)
        {
            return RemoveItem(Item.id);
        }

        /// <summary>
        /// Removes an item from the player's inventory
        /// </summary>
        /// <param name="Name">The item's name to remove</param>
        /// <returns>If the item was removed</returns>
        public bool RemoveItem(string Name)
        {
            return RemoveItem((Assets.find(EAssetType.ITEM, Name) as ItemAsset).id);
        }

        /// <summary>
        /// Sends effect to the player
        /// </summary>
        /// <param name="id">The effect id to trigger</param>
        public void SendEffect(ushort id)
        {
            EffectManager.instance.tellEffectPoint(SteamID, id, Position);
        }

        /// <summary>
        /// Clear effect by id
        /// </summary>
        /// <param name="id">The effect id to clear</param>
        public void ClearEffect(ushort id)
        {
            EffectManager.instance.tellEffectClearByID(SteamID, id);
        }

        /// <summary>
        /// Dequip the player's equipped item
        ///</summary>
        public void DequipItem() => Player.equipment.dequip();
        #endregion
    }
}
