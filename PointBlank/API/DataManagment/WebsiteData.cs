using System;
using System.Net;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace PointBlank.API.DataManagment
{
    /// <summary>
    /// Allows for easy interaction of websites
    /// </summary>
    public class WebsiteData
    {
        #region Properties
        /// <summary>
        /// The main client used for connecting to websites
        /// </summary>
        public static WeebClient Client { get; private set; } = new WeebClient();
        #endregion

        #region Public Functions
        /// <summary>
        /// Downloads the web page
        /// </summary>
        /// <param name="URL">The URL to the website</param>
        /// <param name="data">The web page as a string</param>
        /// <param name="useNewClient">Should the program create a seperate client for the website</param>
        /// <param name="client">A custom client(leave null to create a new one/use main client)</param>
        /// <returns>The client that was used to download the web page</returns>
        public static WeebClient GetData(string URL, out string data, bool useNewClient = false, WeebClient client = null)
        {
            try
            {
                if (useNewClient)
                    client = new WeebClient();
                else if (client == null)
                    client = Client;

                data = client.DownloadString(URL);
                return client;
            }
            catch (Exception ex)
            {
                data = null;
                return null;
            }
        }

        /// <summary>
        /// Posts data to the website and downloads the response page
        /// </summary>
        /// <param name="URL">The URL to the website</param>
        /// <param name="postData">The data to send as post request</param>
        /// <param name="returnData">The response page as string</param>
        /// <param name="useNewClient">Should the program create a seperate client for the website</param>
        /// <param name="client">A custom client(leave null to create a new one/use main client)</param>
        /// <returns>The client that was used to download the web page</returns>
        public static WeebClient PostData(string URL, NameValueCollection postData, out string returnData, bool useNewClient = false, WeebClient client = null)
        {
            try
            {
                if (useNewClient)
                    client = new WeebClient();
                else if (client == null)
                    client = Client;

                returnData = Encoding.Unicode.GetString(client.UploadValues(URL, postData));
                return client;
            }
            catch (Exception ex)
            {
                returnData = null;
                return null;
            }
        }

        /// <summary>
        /// Downloads a file from the website
        /// </summary>
        /// <param name="URL">The URL to the file</param>
        /// <param name="path">The path to save the file at</param>
        /// <param name="useNewClient">Should the program create a seperate client for the website</param>
        /// <param name="client">A custom client(leave null to create a new one/use main client)</param>
        /// <returns>The client that was used to download the file</returns>
        public static WeebClient DownloadFile(string URL, string path, bool useNewClient = false, WeebClient client = null)
        {
            try
            {
                if (useNewClient)
                    client = new WeebClient();
                else if (client == null)
                    client = Client;

                client.DownloadFile(URL, path);
                return client;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }

    /// <summary>
    /// A website browsing client that supports cookies and redirections
    /// </summary>
    public class WeebClient : WebClient
    {
        #region Properties
        /// <summary>
        /// The current site URL
        /// </summary>
        public Uri URL { get; private set; }
        /// <summary>
        /// The jar of cookies currently stored
        /// </summary>
        public CookieContainer CookieJar { get; private set; }
        #endregion

        /// <summary>
        /// A website browsing client that supports cookies and redirections
        /// </summary>
        /// <param name="cookieJar">The jar of cookies to send to the server</param>
        public WeebClient(CookieContainer cookieJar)
        {
            base.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            this.CookieJar = cookieJar;
        }

        /// <summary>
        /// A website browsing client that supports cookies and redirections
        /// </summary>
        public WeebClient()
        {
            base.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            this.CookieJar = new CookieContainer();
        }

        #region Overrides
        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);

            URL = response.ResponseUri;

            return response;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);

            if (request is HttpWebRequest)
                ((HttpWebRequest)request).CookieContainer = CookieJar;
            ((HttpWebRequest)request).AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            return request;
        }
        #endregion
    }
}
