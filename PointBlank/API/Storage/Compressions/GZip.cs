using System.IO;
using System.IO.Compression;
using PointBlank.API.Implements;

namespace PointBlank.API.Storage.Compressions
{
    internal static class GZip
    {
        #region Public Functions
        public static byte[] Zip(byte[] bytes)
        {
            using(MemoryStream memoryIn = new MemoryStream(bytes))
            {
                using(MemoryStream memoryOut = new MemoryStream())
                {
                    using(GZipStream gzip = new GZipStream(memoryOut, CompressionMode.Compress))
                        memoryIn.CopyTo(gzip);

                    return memoryOut.ToArray();
                }
            }
        }

        public static byte[] UnZip(byte[] bytes)
        {
            using(MemoryStream memoryIn = new MemoryStream(bytes))
            {
                using(MemoryStream memoryOut = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(memoryIn, CompressionMode.Decompress))
                        gzip.CopyTo(memoryOut);

                    return memoryOut.ToArray();
                }
            }
        }
        #endregion
    }
}
