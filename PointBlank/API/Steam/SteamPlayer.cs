using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Steam
{
    /// <summary>
    /// The player information on steam
    /// </summary>
    public class SteamPlayer
    {
        #region Properties
        /// <summary>
        /// The user's steam name
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The steam groups the player has joined
        /// </summary>
        public SteamGroup[] Groups { get; private set; }
        /// <summary>
        /// The state of the user's profile privacy
        /// </summary>
        public EPrivacyState PrivacyState { get; private set; }
        /// <summary>
        /// The steam64 ID of the user
        /// </summary>
        public ulong ID { get; private set; }

        /// <summary>
        /// Is the profile visible/non private
        /// </summary>
        public bool IsVisible { get; private set; }
        /// <summary>
        /// Is the user VAC banned from any game
        /// </summary>
        public bool IsVACBanned { get; private set; }
        /// <summary>
        /// Is the user trade banned
        /// </summary>
        public bool IsTradeBanned { get; private set; }
        /// <summary>
        /// Is the user a limited account
        /// </summary>
        public bool IsLimited { get; private set; }
        #endregion

        public SteamPlayer(ulong id)
        {
            // Set the variables
            this.ID = id;

            // Setup the XML
            XmlDocument document = new XmlDocument();
            document.Load(string.Format("http://steamcommunity.com/profiles/{0}/?xml=1", ID.ToString()));
            XmlNode root = document.DocumentElement;

            // Set the data
            if(root != null)
            {
                Name = root.SelectSingleNode("steamID").InnerText.Replace("<![CDATA[ ", "").Replace(" ]]>", "");
                IsVisible = (int.Parse(root.SelectSingleNode("visibilityState").InnerText) > 0);
                IsVACBanned = (int.Parse(root.SelectSingleNode("vacBanned").InnerText) > 0);
                IsTradeBanned = (root.SelectSingleNode("tradeBanState").InnerText != "None");
                IsLimited = (int.Parse(root.SelectSingleNode("isLimitedAccount").InnerText) > 0);

                string privacystate = root.SelectSingleNode("privacyState").InnerText;
                if (privacystate == "public")
                    PrivacyState = EPrivacyState.PUBLIC;
                else if (privacystate == "friendsonly")
                    PrivacyState = EPrivacyState.FRIENDS_ONLY;
                else if (privacystate == "private")
                    PrivacyState = EPrivacyState.PRIVATE;
                else
                    PrivacyState = EPrivacyState.NONE;

                List<SteamGroup> groups = new List<SteamGroup>();
                foreach(XmlNode node in root.SelectNodes("groups/group"))
                {
                    ulong i = ulong.Parse(node.InnerText);
                    SteamGroup group = SteamGroupManager.Groups.FirstOrDefault(a => a.ID == i);

                    if (group == null)
                        group = new SteamGroup(i);

                    SteamGroupManager.AddSteamGroup(group);
                    groups.Add(group);
                }
                Groups = groups.ToArray();
            }
            else
            {
                Name = "";
                Groups = new SteamGroup[0];
                PrivacyState = EPrivacyState.NONE;
                IsVisible = false;
                IsVACBanned = false;
                IsTradeBanned = false;
                IsLimited = false;
            }
        }
    }
}
