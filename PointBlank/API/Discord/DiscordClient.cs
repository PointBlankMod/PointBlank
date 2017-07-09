using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PointBlank.API.Discord
{
    /// <summary>
    /// Used for interacting with discord via the https code
    /// </summary>
    public class DiscordClient : WebClient
    {
        #region Properties
        /// <summary>
        /// The current URL of the website
        /// </summary>
        public Uri URL { get; private set; }

        /// <summary>
        /// The last http code received
        /// </summary>
        public EDiscordHttpCodes LastHTTPCode { get; private set; }
        /// <summary>
        /// The last json code received
        /// </summary>
        public EDiscordJsonCodes LastJSONCode { get; private set; }
        #endregion

        public DiscordClient()
        {
            base.Headers[HttpRequestHeader.ContentType] = "application/json";
        }

        #region Private Functions
        private void ParseJsonCode(string response)
        {
            JObject obj = JObject.Parse(response);

            if(obj["code"] != null)
            {
                LastJSONCode = (EDiscordJsonCodes)(int)(obj["code"]);
                return;
            }
            LastJSONCode = 0;
        }
        #endregion

        #region Override Functions
        protected override WebRequest GetWebRequest(Uri address)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(PointBlank.ValidateCertificate);

            WebRequest request = base.GetWebRequest(address);

            return request;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = null;

            try
            {
                response = base.GetWebResponse(request);

                LastHTTPCode = (EDiscordHttpCodes)((HttpWebResponse)response).StatusCode;
                if(LastHTTPCode != EDiscordHttpCodes.NO_CONTENT)
                    using (StreamReader reader = new StreamReader(((HttpWebResponse)response).GetResponseStream()))
                        ParseJsonCode(reader.ReadToEnd());
            }
            catch (WebException ex)
            {
                LastHTTPCode = (EDiscordHttpCodes)((HttpWebResponse)ex.Response).StatusCode;
                if (LastHTTPCode != EDiscordHttpCodes.NO_CONTENT)
                    using (StreamReader reader = new StreamReader(((HttpWebResponse)response).GetResponseStream()))
                        ParseJsonCode(reader.ReadToEnd());
            }
            URL = response.ResponseUri;

            return response;
        }

        protected override void OnUploadStringCompleted(UploadStringCompletedEventArgs e)
        {
            if(e.Error != null)
            {

                LastHTTPCode = (EDiscordHttpCodes)((HttpWebResponse)((WebException)e.Error).Response).StatusCode;
                if (LastHTTPCode == EDiscordHttpCodes.NO_CONTENT) return;
                using (StreamReader reader = new StreamReader(((HttpWebResponse)((WebException)e.Error).Response).GetResponseStream()))
                    ParseJsonCode(reader.ReadToEnd());
                return;
            }

            base.OnUploadStringCompleted(e);
        }
        #endregion
    }
}
