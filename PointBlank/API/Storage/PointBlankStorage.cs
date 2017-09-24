using System.IO;
using System.Text;
using PointBlank.API.Server;
using PointBlank.API.Storage.Compressions;

namespace PointBlank.API.Storage
{
    /// <summary>
    /// Used for managing stored data
    /// </summary>
    public class PointBlankStorage
    {
        #region Properties
        /// <summary>
        /// The name of the file
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The path to the file
        /// </summary>
        public string Path { get; private set; }
        /// <summary>
        /// The compression to be use
        /// </summary>
        public ECompression Compression { get; private set; }

        /// <summary>
        /// This is the compressed version of the file
        /// </summary>
        public byte[] FileCompressed { get; private set; }
        /// <summary>
        /// This is the raw version of the file
        /// </summary>
        public byte[] FileDecompressed { get; private set; }
        /// <summary>
        /// This is the file contents. Anything you write here will be written back to the file
        /// </summary>
        public string FileString { get; set; }
        #endregion

        /// <summary>
        /// Create storage to manage stored data
        /// </summary>
        /// <param name="name">The name of the storage/file</param>
        /// <param name="compression">The compression to use</param>
        public PointBlankStorage(string name, ECompression compression)
        {
            // Set the variables
            Name = name;
            Path = PointBlankServer.DataPath + "/" + Name + ".dat";
            Compression = compression;

            // Run the code
            if (File.Exists(Path))
                Read();
        }

        #region Private Functions
        private void Read()
        {
            switch (Compression)
            {
                case ECompression.Gzip:
                    FileCompressed = File.ReadAllBytes(Path);
                    FileDecompressed = GZip.UnZip(FileCompressed);
                    break;
                default:
                    FileDecompressed = File.ReadAllBytes(Path);
                    break;
            }
            FileString = Encoding.Unicode.GetString(FileDecompressed);
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Compresses the data and writes it to the file
        /// </summary>
        public void Write()
        {
            if (!string.IsNullOrEmpty(FileString))
                FileDecompressed = Encoding.Unicode.GetBytes(FileString);
            switch (Compression)
            {
                case ECompression.Gzip:
                    FileCompressed = GZip.Zip(FileDecompressed);
                    File.WriteAllBytes(Path, FileCompressed);
                    break;
                default:
                    File.WriteAllBytes(Path, FileDecompressed);
                    break;
            }
        }
        #endregion
    }
}
