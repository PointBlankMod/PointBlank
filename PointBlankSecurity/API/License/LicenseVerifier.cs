using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using PointBlank.API.DataManagment;
using PointBlank.API.Security;

namespace PointBlank.API.License
{
    /// <summary>
    /// Allows you to easily verify the license of your plugin
    /// </summary>
    public static class LicenseVerifier
    {
        #region Functions
        public static bool VerifyLicense(string file, string website, EHashType hash = EHashType.NONE)
        {
            if (!File.Exists(file))
                return false;
            string response = "false";

            switch (hash)
            {
                case EHashType.MD5:
                    WebsiteData.PostData(website, new NameValueCollection() { { "License", Hashing.CalculateMD5String(File.ReadAllBytes(file)) } }, out response);
                    break;
                case EHashType.SHA1:
                    WebsiteData.PostData(website, new NameValueCollection() { { "License", Hashing.CalculateSHA1String(File.ReadAllBytes(file)) } }, out response);
                    break;
                case EHashType.SHA256:
                    WebsiteData.PostData(website, new NameValueCollection() { { "License", Hashing.CalculateSHA256String(File.ReadAllBytes(file)) } }, out response);
                    break;
                case EHashType.SHA512:
                    WebsiteData.PostData(website, new NameValueCollection() { { "License", Hashing.CalculateSHA512String(File.ReadAllBytes(file)) } }, out response);
                    break;
                default:
                    if (WebsiteData.UploadFile(website, file, out byte[] data))
                        response = Encoding.UTF8.GetString(data);
                    break;
            }

            if (response == "true")
                return true;
            else
                return false;
        }

        public static bool VerifyLicense(byte[] bytes, string website, EHashType hash = EHashType.NONE)
        {
            string response = "false";

            switch (hash)
            {
                case EHashType.MD5:
                    WebsiteData.PostData(website, new NameValueCollection() { { "License", Hashing.CalculateMD5String(bytes) } }, out response);
                    break;
                case EHashType.SHA1:
                    WebsiteData.PostData(website, new NameValueCollection() { { "License", Hashing.CalculateSHA1String(bytes) } }, out response);
                    break;
                case EHashType.SHA256:
                    WebsiteData.PostData(website, new NameValueCollection() { { "License", Hashing.CalculateSHA256String(bytes) } }, out response);
                    break;
                case EHashType.SHA512:
                    WebsiteData.PostData(website, new NameValueCollection() { { "License", Hashing.CalculateSHA512String(bytes) } }, out response);
                    break;
                default:
                    WebsiteData.PostData(website, new NameValueCollection() { { "License", Encoding.UTF8.GetString(bytes) } }, out response);
                    break;
            }

            if (response == "true")
                return true;
            else
                return false;
        }
        #endregion
    }
}
