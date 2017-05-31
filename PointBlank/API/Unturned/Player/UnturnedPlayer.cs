using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UPlayer = SDG.Unturned.Player;
using SPlayer = SDG.Unturned.SteamPlayer;
using Steamworks;

namespace PointBlank.API.Unturned.Player
{
    /// <summary>
    /// The unturned player instance
    /// </summary>
    public class UnturnedPlayer
    {
        #region Properties
        // Important information
        /// <summary>
        /// The unturned player instance
        /// </summary>
        public UPlayer Player { get { return SteamPlayer.player; } }
        /// <summary>
        /// The steam player instance
        /// </summary>
        public SPlayer SteamPlayer { get; private set; }
        /// <summary>
        /// The steam player ID instance
        /// </summary>
        public SteamPlayerID SteamPlayerID { get { return SteamPlayer.playerID; } }

        // Steam player ID information
        /// <summary>
        /// The player's name
        /// </summary>
        public string PlayerName { get { return SteamPlayerID.playerName; } }
        /// <summary>
        /// The character's name
        /// </summary>
        public string CharacterName { get { return SteamPlayerID.characterName; } }
        /// <summary>
        /// The player's steam ID
        /// </summary>
        public CSteamID SteamID { get { return SteamPlayerID.steamID; } }
        /// <summary>
        /// The character's ID
        /// </summary>
        public byte CharacterID { get { return SteamPlayerID.characterID; } }

        // Steam player information
        /// <summary>
        /// Is the player an admin
        /// </summary>
        public bool IsAdmin { get { return SteamPlayer.isAdmin; } }
        /// <summary>
        /// Is the player a pro buyer
        /// </summary>
        public bool IsPro { get { return SteamPlayer.isPro; } }
        /// <summary>
        /// The player skillset
        /// </summary>
        public EPlayerSkillset Skillset { get { return SteamPlayer.skillset; } }
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
        public byte Stamina
        {
            get
            {
                return Player.life.stamina;
            }
        }
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
        public Skill[][] Skills { get { return Player.skills.skills; } }
        /// <summary>
        /// Has the player got anything equipped
        /// </summary>
        public bool HasAnythingEquipped { get { return (Player.equipment.isEquipped && Player.equipment.asset != null && Player.equipment.useable != null); } }
        /// <summary>
        /// The currently equipped asset
        /// </summary>
        public ItemAsset EquippedAsset { get { return Player.equipment.asset; } }
        /// <summary>
        /// The currently equpped useable
        /// </summary>
        public Useable EquippedUseable { get { return Player.equipment.useable; } }
        /// <summary>
        /// The currently equipped item ID
        /// </summary>
        public ushort EquippedItemID { get { return Player.equipment.itemID; } }
        /// <summary>
        /// Is the currently equpped item a primary
        /// </summary>
        public bool IsEquippedPrimary { get { return Player.equipment.primary; } }
        /// <summary>
        /// Is the current equipment busy
        /// </summary>
        public bool IsEquipmentBusy { get { return Player.equipment.isBusy; } set { Player.equipment.isBusy = value; } }
        /// <summary>
        /// Can the equipped item be inspected
        /// </summary>
        public bool CanInspectEquipped { get { return Player.equipment.canInspect; } }
        /// <summary>
        /// Is the player currently inspecting the equipped item
        /// </summary>
        public bool IsInspectingEqupped { get { return Player.equipment.isInspecting; } }
        #endregion
    }
}
