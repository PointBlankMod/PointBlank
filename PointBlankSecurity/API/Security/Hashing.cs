using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace PointBlank.API.Security
{
    /// <summary>
    /// Used for easier hashing so you won't have to make it yourself
    /// </summary>
    public static class Hashing
    {
        #region MD5
        
        /// <summary>
        /// Calculate the MD5 hash from a string and return the hash in string format
        /// </summary>
        /// <param name="input">The string to hash using MD5</param>
        /// <returns>The MD5 hash of the string</returns>
        [Obsolete("MD5 is highly insecure and should be avoided wherever possible", false)]
        public static string CalculateMD5String(string input) => CalculateMD5String(Encoding.UTF8.GetBytes(input));
        /// <summary>
        /// Calculate the MD5 hash from a byte array and return the hash in string format
        /// </summary>
        /// <param name="bytes">The byte array to hash using MD5</param>
        /// <returns>The MD5 hash of the byte array</returns>
        [Obsolete("MD5 is highly insecure and should be avoided wherever possible", false)]
        public static string CalculateMD5String(byte[] bytes) => BitConverter.ToString(CalculateMD5Bytes(bytes)).Replace("-", string.Empty).ToLower();

        /// <summary>
        /// Calculate the MD5 hash from a string and return the hash in byte array format
        /// </summary>
        /// <param name="input">The string to hash using MD5</param>
        /// <returns>The MD5 hash of the string</returns>
        [Obsolete("MD5 is highly insecure and should be avoided wherever possible", false)]
        public static byte[] CalculateMD5Bytes(string input) => CalculateMD5Bytes(Encoding.UTF8.GetBytes(input));
        /// <summary>
        /// Calculate the MD5 hash from a byte array and return the hash in byte array format
        /// </summary>
        /// <param name="input">The byte array to hash using MD5</param>
        /// <returns>The MD5 hash of the byte array</returns>
        [Obsolete("MD5 is highly insecure and should be avoided wherever possible", false)]
        public static byte[] CalculateMD5Bytes(byte[] bytes) => ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(bytes);
        #endregion

        #region SHA1
        [Obsolete("SHA1 is prone to collisions and should be avoided if possible", false)]
        public static string CalculateSHA1String(string input) => CalculateSHA1String(Encoding.UTF8.GetBytes(input));
        [Obsolete("SHA1 is prone to collisions and should be avoided if possible", false)]
        public static string CalculateSHA1String(byte[] bytes) => string.Join("", CalculateSHA1Bytes(bytes).Select(a => a.ToString("x2")).ToArray());

        [Obsolete("SHA1 is prone to collisions and should be avoided if possible", false)]
        public static byte[] CalculateSHA1Bytes(string input) => CalculateSHA1Bytes(Encoding.UTF8.GetBytes(input));
        [Obsolete("SHA1 is prone to collisions and should be avoided if possible", false)]
        public static byte[] CalculateSHA1Bytes(byte[] bytes)
        {
            using(SHA1Managed sha1 = new SHA1Managed())
                return sha1.ComputeHash(bytes);
        }
        #endregion

        #region SHA2-256
        
        public static string CalculateSHA2_256String(string input) => CalculateSHA2_256String(Encoding.UTF8.GetBytes(input));
        public static string CalculateSHA2_256String(byte[] bytes) => string.Join("", CalculateSHA2_256Bytes(bytes).Select(a => a.ToString("x2")).ToArray());

        public static byte[] CalculateSHA2_256Bytes(string input) => CalculateSHA2_256Bytes(Encoding.UTF8.GetBytes(input));
        public static byte[] CalculateSHA2_256Bytes(byte[] bytes)
        {
            using (SHA256 sha256 = SHA256Managed.Create())
                return sha256.ComputeHash(bytes);
        }
        
        #endregion

        #region SHA2-512
        
        public static string CalculateSHA2_512String(string input) => CalculateSHA2_512String(Encoding.UTF8.GetBytes(input));
        public static string CalculateSHA2_512String(byte[] bytes) => string.Join("", CalculateSHA2_512Bytes(bytes).Select(a => a.ToString("x2")).ToArray());

        public static byte[] CalculateSHA2_512Bytes(string input) => CalculateSHA2_512Bytes(Encoding.UTF8.GetBytes(input));
        public static byte[] CalculateSHA2_512Bytes(byte[] bytes)
        {
            using (SHA512 sha512 = SHA512Managed.Create())
                return sha512.ComputeHash(bytes);
        }
        
        #endregion
        
        #region SHA3-256

        public static Byte[] CalculateSHA3_256Bytes(Byte[] Input) => new PBSHA3Digest(256).CreateDigest(Input);
        
        public static Byte[] CalculateSHA3_256Bytes(String Input) => new PBSHA3Digest(256).CreateDigest(Input);
        
        public static String CalculateSHA3_256String(Byte[] Input) => new PBSHA3Digest(256).CreateStringDigest(Input);
        
        public static String CalculateSHA3_256String(String Input) => new PBSHA3Digest(256).CreateStringDigest(Input);
        
        #endregion
        
        #region SHA3-512
        
        public static Byte[] CalculateSHA3_512Bytes(Byte[] Input) => new PBSHA3Digest(512).CreateDigest(Input);
        
        public static Byte[] CalculateSHA3_512Bytes(String Input) => new PBSHA3Digest(512).CreateDigest(Input);
        
        public static String CalculateSHA3_512String(Byte[] Input) => new PBSHA3Digest(512).CreateStringDigest(Input);
        
        public static String CalculateSHA3_512String(String Input) => new PBSHA3Digest(512).CreateStringDigest(Input);
        
        #endregion
    }
}
