using System;
using System.Collections.Generic;
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
        private Queue<object> QueuedMessages = new Queue<object>();
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
        /// The avatar image URL for the webhook
        /// </summary>
        public string Avatar { get; set; }

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
        public DiscordWebhook(Uri URL, string Name = "Unturned Server", string Avatar = null)
        {
            // Set the variables
            this.URL = URL;
            this.Name = Name;
            this.Avatar = Avatar;

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
            if ((DateTime.Now - LastLimit).TotalMilliseconds < 0)
            {
                QueuedMessages.Enqueue(message);
                return false;
            }
            JObject obj = new JObject {{"username", Name}};

            if (!string.IsNullOrEmpty(Avatar))
                obj.Add("avatar_url", Avatar);
            obj.Add("tts", false);
            obj.Add("embeds", new JArray() { message });

            FlushQueue(Async);
            if (Async)
                Client.UploadStringAsync(URL, obj.ToString(Formatting.None));
            else
                Client.UploadString(URL.AbsoluteUri, obj.ToString(Formatting.None));

            if (int.Parse(Client.ResponseHeaders["X-RateLimit-Remaining"]) < 1)
                LastLimit = PointBlankTools.FromUnixTime(long.Parse(Client.ResponseHeaders["X-RateLimit-Reset"]));
            return Client.LastHTTPCode == EDiscordHttpCodes.OK && Client.LastHTTPCode == EDiscordHttpCodes.NO_CONTENT;
        }

        /// <summary>
        /// Sends the text message to the discord webhook
        /// </summary>
        /// <param name="message">The text message to send</param>
        /// <param name="Async">Should sending be asynced</param>
        /// <returns>Was the message sent successfully</returns>
        public bool Send(string message, bool Async = true)
        {
            if (message == null)
                return false;
            if ((DateTime.Now - LastLimit).TotalMilliseconds < 0)
            {
                QueuedMessages.Enqueue(message);
                return false;
            }
            JObject obj = new JObject {{"username", Name}};

            if (!string.IsNullOrEmpty(Avatar))
                obj.Add("avatar_url", Avatar);
            obj.Add("tts", false);
            obj.Add("embeds", new JArray() { message });

            FlushQueue(Async);
            if (Async)
                Client.UploadStringAsync(URL, obj.ToString(Formatting.None));
            else
                Client.UploadString(URL.AbsoluteUri, obj.ToString(Formatting.None));

            if (int.Parse(Client.ResponseHeaders["X-RateLimit-Remaining"]) < 1)
                LastLimit = PointBlankTools.FromUnixTime(long.Parse(Client.ResponseHeaders["X-RateLimit-Reset"]));
            return Client.LastHTTPCode == EDiscordHttpCodes.OK && Client.LastHTTPCode == EDiscordHttpCodes.NO_CONTENT;
        }

        /// <summary>
        /// Sends all the messages stuck in queue to discord
        /// </summary>
        /// <param name="Async">Should sending be asynced</param>
        public void FlushQueue(bool Async = true)
        {
            while(QueuedMessages.Count > 0)
            {
                object msg = QueuedMessages.Dequeue();
                bool result = true;
                if (msg is string)
                    result = Send((string)msg, Async);
                else if (msg is JObject)
                    result = Send((JObject)msg, Async);

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
