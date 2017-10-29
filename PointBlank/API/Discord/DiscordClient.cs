using System;
using System.IO;
using System.Net;
using System.Net.Security;
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
        public Uri Url { get; private set; }

        /// <summary>
        /// The last http code received
        /// </summary>
        public EDiscordHttpCodes LastHttpCode { get; private set; }
        /// <summary>
        /// The last Json code received
        /// </summary>
        public EDiscordJsonCodes LastJsonCode { get; private set; }
        #endregion

<<<<<<< HEAD
        public DiscordClient() => base.Headers[HttpRequestHeader.ContentType] = "application/json";
=======
        public DiscordClient() => base.Headers[HttpRequestHeader.ContentType] = "application/Json";
>>>>>>> master

        #region Private Functions
        private void ParseJsonCode(string response)
        {
            JObject obj = JObject.Parse(response);

            if(obj["code"] != null)
            {
                LastJsonCode = (EDiscordJsonCodes)(int)(obj["code"]);
                return;
            }
            LastJsonCode = 0;
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

                LastHttpCode = (EDiscordHttpCodes)((HttpWebResponse)response).StatusCode;
                if(LastHttpCode != EDiscordHttpCodes.NoContent)
                    using (StreamReader reader = new StreamReader(((HttpWebResponse)response).GetResponseStream()))
                        ParseJsonCode(reader.ReadToEnd());
            }
            catch (WebException ex)
            {
                LastHttpCode = (EDiscordHttpCodes)((HttpWebResponse)ex.Response).StatusCode;
                if (LastHttpCode != EDiscordHttpCodes.NoContent)
                    using (StreamReader reader = new StreamReader(((HttpWebResponse)response).GetResponseStream()))
                        ParseJsonCode(reader.ReadToEnd());
            }
            Url = response.ResponseUri;

            return response;
        }

        protected override void OnUploadStringCompleted(UploadStringCompletedEventArgs e)
        {
            if(e.Error != null)
            {

                LastHttpCode = (EDiscordHttpCodes)((HttpWebResponse)((WebException)e.Error).Response).StatusCode;
                if (LastHttpCode == EDiscordHttpCodes.NoContent) return;
                using (StreamReader reader = new StreamReader(((HttpWebResponse)((WebException)e.Error).Response).GetResponseStream()))
                    ParseJsonCode(reader.ReadToEnd());
                return;
            }

            base.OnUploadStringCompleted(e);
        }
        #endregion
    }
}
