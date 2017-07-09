using System;
using System.Net;
using System.Xml;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.DataManagment;

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

        /// <summary>
        /// The player information on steam
        /// </summary>
        /// <param name="id">The Steam64 of the player</param>
        public SteamPlayer(ulong id)
        {
            // Set the variables
            this.ID = id;

            // Setup the XML
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load($"http://steamcommunity.com/profiles/{ID.ToString()}/?xml=1");
                XmlNode root = document.DocumentElement;

                // Set the data
                if (root != null)
                {
                    Name = root.SelectSingleNode("steamID").InnerText.Replace("<![CDATA[ ", "").Replace(" ]]>", "");
                    IsVisible = (int.Parse(root.SelectSingleNode("visibilityState").InnerText) > 0);
                    IsVACBanned = (int.Parse(root.SelectSingleNode("vacBanned").InnerText) > 0);
                    IsTradeBanned = (root.SelectSingleNode("tradeBanState").InnerText != "None");
                    IsLimited = (int.Parse(root.SelectSingleNode("isLimitedAccount").InnerText) > 0);

                    string privacystate = root.SelectSingleNode("privacyState").InnerText;
                    switch (privacystate)
                    {
                        case "public":
                            PrivacyState = EPrivacyState.PUBLIC;
                            break;
                        case "friendsonly":
                            PrivacyState = EPrivacyState.FRIENDS_ONLY;
                            break;
                        case "private":
                            PrivacyState = EPrivacyState.PRIVATE;
                            break;
                        default:
                            PrivacyState = EPrivacyState.NONE;
                            break;
                    }

                    if(PrivacyState == EPrivacyState.PUBLIC)
                    {
                        List<SteamGroup> groups = new List<SteamGroup>();
                        foreach (XmlNode node in root.SelectNodes("groups/group"))
                        {
                            ulong i = ulong.Parse(node.SelectSingleNode("groupID64").InnerText);
                            SteamGroup group = SteamGroupManager.Groups.FirstOrDefault(a => a.ID == i);

                            if (group == null)
                            {
                                group = new SteamGroup(i, -1, false, true);
                                SteamGroupManager.AddSteamGroup(group);
                            }
                            groups.Add(group);
                        }
                        Groups = groups.ToArray();
                    }
                    else
                    {
                        Groups = new SteamGroup[0];
                    }
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
            catch (Exception ex)
            {
                Logging.LogError("Issue occured when getting info for " + ID, ex);
            }
        }
    }
}
