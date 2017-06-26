using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PointBlank.API.Discord
{
    /// <summary>
    /// Used for interacting with discord webhooks
    /// </summary>
    public class DiscordWebhook : IDisposable
    {
        #region Variables
        private Queue<JObject> QueuedMessages = new Queue<JObject>();
        #endregion

        #region Properties
        /// <summary>
        /// The webhook URL for interaction with discord
        /// </summary>
        public Uri URL { get; private set; }
        /// <summary>
        /// The name of the webhook(it will be displayed when sending messages)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The client responsible for interacting with discord
        /// </summary>
        public DiscordClient Client { get; private set; }
        /// <summary>
        /// The amount of messages you can send before time reset
        /// </summary>
        public int Limit { get; private set; }
        /// <summary>
        /// The time until the limit resets
        /// </summary>
        public DateTime LastLimit { get; private set; }
        #endregion

        /// <summary>
        /// Creates and instance of the discord webhook
        /// </summary>
        /// <param name="URL">The URL of the discord webhook</param>
        /// <param name="Name">The name of the discord webhook(can be changed later)</param>
        public DiscordWebhook(Uri URL, string Name = "Unturned Server")
        {
            // Set the variables
            this.URL = URL;
            this.Name = Name;

            // Setup the variables
            Client = new DiscordClient();
            LastLimit = DateTime.Now;
        }

        #region Public Functions
        /// <summary>
        /// Sends the generated message to the discord webhook
        /// </summary>
        /// <param name="message">The generated message to send</param>
        /// <param name="Async">Should sending be asynced</param>
        /// <returns>Was the message sent successfully</returns>
        public bool Send(JObject message, bool Async = true)
        {
            if (message == null)
                return false;
            if((DateTime.Now - LastLimit).TotalMilliseconds < 0)
            {
                QueuedMessages.Enqueue(message);
                return false;
            }

            FlushQueue(Async);
            if (Async)
                Client.UploadStringAsync(URL, message.ToString(Formatting.None));
            else
                Client.UploadString(URL.AbsoluteUri, message.ToString(Formatting.None));

            if (Client.LastHTTPCode != EDiscordHttpCodes.OK || Client.LastHTTPCode != EDiscordHttpCodes.NO_CONTENT)
                return false;
            if(int.Parse(Client.ResponseHeaders["X-RateLimit-Remaining"]) < 1)
                LastLimit = PBTools.FromUnixTime(long.Parse(Client.ResponseHeaders["X-RateLimit-Reset"]));

            return true;
        }

        /// <summary>
        /// Sends all the messages stuck in queue to discord
        /// </summary>
        /// <param name="Async">Should sending be asynced</param>
        public void FlushQueue(bool Async = true)
        {
            while(QueuedMessages.Count > 0)
            {
                bool result = Send(QueuedMessages.Dequeue(), Async);

                if(!result && Client.LastHTTPCode == EDiscordHttpCodes.TOO_MANY_REQUESTS)
                    break;
            }
        }

        /// <summary>
        /// Called to dispose the webhook
        /// </summary>
        public void Dispose()
        {
            Client.Dispose();
        }
        #endregion
    }
}
