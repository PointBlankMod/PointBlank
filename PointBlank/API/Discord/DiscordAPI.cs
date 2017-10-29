using Newtonsoft.Json.Linq;

namespace PointBlank.API.Discord
{
    /// <summary>
    /// Used for generating JSON messages to send to discord for interaction
    /// </summary>
    public static class DiscordAPI
    {
        /// <summary>
        /// Creates a rich embed for the message to send
        /// </summary>
        /// <param name="title">The title of the embed</param>
        /// <param name="description">The description shown below the title</param>
        /// <param name="url">The url of the message(clickable title)</param>
        /// <param name="color">The color of the embed</param>
        /// <param name="footer">The footer of the embed</param>
        /// <param name="author">The author of the embed</param>
        /// <param name="fields">The field list of the embed</param>
        /// <param name="image">The image inside the embed</param>
        /// <param name="thumbnail">The thumbnail of the embed</param>
        /// <param name="video">The video inside the embed</param>
        /// <param name="provider">The embed provider</param>
        /// <returns>The generated JObject of the rich embed</returns>
        public static JObject CreateRichEmbed(string title = "", string description = "", string url = "", int color = -1, JObject footer = null,
            JObject author = null, JObject[] fields = null, JObject image = null, JObject thumbnail = null, JObject video = null, JObject provider = null)
        {
            // Setup the variables
            JObject obj = new JObject();

            // Set the information
            if (!string.IsNullOrEmpty(title))
                obj.Add("title", title);
            if (!string.IsNullOrEmpty(description))
                obj.Add("description", description);
            if (!string.IsNullOrEmpty(url))
                obj.Add("url", url);
            if (color > -1)
                obj.Add("color", color);
            if (footer != null)
                obj.Add("footer", footer);
            if (author != null)
                obj.Add("author", author);
            if (image != null)
                obj.Add("image", image);
            if (thumbnail != null)
                obj.Add("thumbnail", thumbnail);
            if (video != null)
                obj.Add("video", video);
            if (provider != null)
                obj.Add("provider", provider);
            if (fields == null || fields.Length <= 0) return obj;
            JArray arr = new JArray();

            for(int i = 0; i < fields.Length; i++)
                arr.Add(fields[i]);
            obj.Add("fields", arr);

            // Return the data
            return obj;
        }

        /// <summary>
        /// Creates a footer for the rich emebed
        /// </summary>
        /// <param name="message">The message/text of the footer</param>
        /// <param name="icon_url">The icon of the footer</param>
        /// <param name="alt_icon_url">The alternative icon of the footer</param>
        /// <returns>The footer object</returns>
        public static JObject CreateFooter(string message = "", string icon_url = "", string alt_icon_url = "")
        {
            // Setup the variables
            JObject obj = new JObject();

            // Set the data
            if (!string.IsNullOrEmpty(message))
                obj.Add("text", message);
            if (!string.IsNullOrEmpty(icon_url))
                obj.Add("icon_url", icon_url);
            if (!string.IsNullOrEmpty(alt_icon_url))
                obj.Add("proxy_icon_url", alt_icon_url);

            // Return the data
            return obj;
        }

        /// <summary>
        /// Creates an author object for the rich embed
        /// </summary>
        /// <param name="name">The name of the author</param>
        /// <param name="url">The URL of the author(clickable name)</param>
        /// <param name="icon_url">The icon of the author</param>
        /// <param name="alt_icon_url">The alternative icon of the author</param>
        /// <returns>The author object</returns>
        public static JObject CreateAuthor(string name = "", string url = "", string icon_url = "", string alt_icon_url = "")
        {
            // Setup the variables
            JObject obj = new JObject();

            // Set the data
            if (!string.IsNullOrEmpty(name))
                obj.Add("name", name);
            if (!string.IsNullOrEmpty(url))
                obj.Add("url", url);
            if (!string.IsNullOrEmpty(icon_url))
                obj.Add("icon_url", icon_url);
            if (!string.IsNullOrEmpty(alt_icon_url))
                obj.Add("proxy_icon_url", alt_icon_url);

            // Return the data
            return obj;
        }

        /// <summary>
        /// Creates a field for the field array of the rich embed
        /// </summary>
        /// <param name="name">The name of the field</param>
        /// <param name="value">The field text/message</param>
        /// <param name="inline">Should the field use inlining</param>
        /// <returns>The field object</returns>
        public static JObject CreateField(string name = "", string value = "", bool inline = true)
        {
            // Setup the variables
            JObject obj = new JObject();

            // Set the data
            if (!string.IsNullOrEmpty(name))
                obj.Add("name", name);
            if (!string.IsNullOrEmpty(value))
                obj.Add("value", value);
            obj.Add("inline", inline);

            // Return the data
            return obj;
        }

        /// <summary>
        /// Creates an image for the rich embed
        /// </summary>
        /// <param name="url">The URL to the image</param>
        /// <param name="alt_url">The alternative URL to the image</param>
        /// <param name="width">The width of the image</param>
        /// <param name="height">The height of the image</param>
        /// <returns>The image object</returns>
        public static JObject CreateImage(string url = "", string alt_url = "", int width = -1, int height = -1)
        {
            // Setup the variables
            JObject obj = new JObject();

            // Set the data
            if (!string.IsNullOrEmpty(url))
                obj.Add("url", url);
            if (!string.IsNullOrEmpty(alt_url))
                obj.Add("proxy_url", alt_url);
            if (width > -1)
                obj.Add("width", width);
            if (height > -1)
                obj.Add("height", height);

            // Return the data
            return obj;
        }

        /// <summary>
        /// Creates a thumbnail for the rich embed
        /// </summary>
        /// <param name="url">The URL to the thumbnail</param>
        /// <param name="alt_url">The alternative URL to the thumbnail</param>
        /// <param name="width">The width of the thumbnail</param>
        /// <param name="height">The height of the thumbnail</param>
        /// <returns>The thumbnail object</returns>
        public static JObject CreateThumbnail(string url = "", string alt_url = "", int width = -1, int height = -1)
        {
            // Setup the variables
            JObject obj = new JObject();

            // Set the data
            if (!string.IsNullOrEmpty(url))
                obj.Add("url", url);
            if (!string.IsNullOrEmpty(alt_url))
                obj.Add("proxy_url", alt_url);
            if (width > -1)
                obj.Add("width", width);
            if (height > -1)
                obj.Add("height", height);

            // Return the data
            return obj;
        }

        /// <summary>
        /// Creates a video for the rich embed
        /// </summary>
        /// <param name="url">The URL to the video</param>
        /// <param name="width">The width of the video</param>
        /// <param name="height">The height of the video</param>
        /// <returns>The video object</returns>
        public static JObject CreateVideo(string url = "", int width = -1, int height = -1)
        {
            // Setup the variables
            JObject obj = new JObject();

            // Set the data
            if (!string.IsNullOrEmpty(url))
                obj.Add("url", url);
            if (width > -1)
                obj.Add("width", width);
            if (height > -1)
                obj.Add("height", height);

            // Return the data
            return obj;
        }

        /// <summary>
        /// Creates a provider for the rich embed
        /// </summary>
        /// <param name="name">The name of the provider</param>
        /// <param name="url">The URL to the provider</param>
        /// <returns>The provider object</returns>
        public static JObject CreateProvider(string name = "", string url = "")
        {
            // Setup the variables
            JObject obj = new JObject();

            // Set the data
            if (!string.IsNullOrEmpty(name))
                obj.Add("name", name);
            if (!string.IsNullOrEmpty(url))
                obj.Add("url", url);

            // Return the data
            return obj;
        }
    }
}
