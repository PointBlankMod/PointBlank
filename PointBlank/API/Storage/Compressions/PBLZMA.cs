using System;
using System.IO;

namespace PointBlank.API.Storage.Compressions
{
    public class PBLZMA
    {
        #region Compression
        
        public static void CompressFile(String UncompressedFile, String CompressedFile)
        {
            SevenZip.Compression.LZMA.Encoder Coder = new SevenZip.Compression.LZMA.Encoder();
            
            using (FileStream Input = new FileStream(UncompressedFile, FileMode.Open))
            using (FileStream Output = new FileStream(CompressedFile, FileMode.Create))
            {
                Coder.WriteCoderProperties(Output);

                Output.Write(BitConverter.GetBytes(Input.Length), 0, 8);

                Coder.Code(Input, Output, Input.Length, -1, null);
                
                Output.Flush();
                Output.Close();
            }
        }

        public static Byte[] CompressBytes(Byte[] UncompressedBytes)
        {
            SevenZip.Compression.LZMA.Encoder Coder = new SevenZip.Compression.LZMA.Encoder();

            Byte[] CompressedBytes;
            
            using (MemoryStream Input = new MemoryStream(UncompressedBytes))
            using (MemoryStream Output = new MemoryStream())
            {
                Coder.WriteCoderProperties(Output);

                Output.Write(BitConverter.GetBytes(Input.Length), 0, 8);
                
                Coder.Code(Input, Output, Input.Length, -1, null);

                CompressedBytes = Output.ToArray();
                
                Output.Flush();
                Output.Close();
            }

            return CompressedBytes;
        }
        
        #endregion
        
        #region Decompression

        public static Byte[] DecompressFile(String CompressedFile)
        {
            SevenZip.Compression.LZMA.Decoder Coder = new SevenZip.Compression.LZMA.Decoder();

            Byte[] DecompressedBytes;
            
            using (FileStream Input = new FileStream(CompressedFile, FileMode.Open))
            using (MemoryStream Output = new MemoryStream())
            {
                byte[] properties = new byte[5];
                Input.Read(properties, 0, 5);

                byte[] fileLengthBytes = new byte[8];
                Input.Read(fileLengthBytes, 0, 8);
                long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

                Coder.SetDecoderProperties(properties);
                Coder.Code(Input, Output, Input.Length, fileLength, null);

                DecompressedBytes = Output.ToArray();
                
                Output.Flush();
                Output.Close();
            }

            return DecompressedBytes;
        }

        public static void DecompressFile(String CompressedFile, String DecompressedFile)
        {
            SevenZip.Compression.LZMA.Decoder Coder = new SevenZip.Compression.LZMA.Decoder();

            using (FileStream Input = new FileStream(CompressedFile, FileMode.Open))
            using (FileStream Output = new FileStream(DecompressedFile, FileMode.Create))
            {
                byte[] properties = new byte[5];
                Input.Read(properties, 0, 5);

                byte[] fileLengthBytes = new byte[8];
                Input.Read(fileLengthBytes, 0, 8);
                long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

                Coder.SetDecoderProperties(properties);
                Coder.Code(Input, Output, Input.Length, fileLength, null);
                
                Output.Flush();
                Output.Close();
            }
        }
        
        #endregion
    }
}