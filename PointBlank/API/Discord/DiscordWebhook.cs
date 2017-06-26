using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PointBlank.API.Discord
{
    /// <summary>
    /// Used for interacting with discord webhooks
    /// </summary>
    public class DiscordWebhook
    {
        #region Properties
        /// <summary>
        /// The webhook URL for interaction with discord
        /// </summary>
        public string URL { get; private set; }
        /// <summary>
        /// The name of the webhook(it will be displayed when sending messages)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The client responsible for interacting with discord
        /// </summary>
        public DiscordClient Client { get; private set; }
        #endregion

        public DiscordWebhook(string URL, string Name)
        {
            this.URL = URL;
            this.Name = Name;
        }

        #region Public Functions
        public bool Send(JObject message, bool Async = true)
        {
            return false;
        }
        #endregion
    }
}
