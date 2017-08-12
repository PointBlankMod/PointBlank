using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Storage
{
    /// <summary>
    /// Types of compressions that can be used to compress your file
    /// </summary>
    public enum ECompression
    {
        /// <summary>
        /// Use no compression
        /// </summary>
        NONE,
        /// <summary>
        /// Use GZIP for compressing the file
        /// </summary>
        GZIP,
        /// <summary>
        /// Use the huffman algorithm to compress the file
        /// </summary>
        HUFFMAN,
        /// <summary>
        /// Check for similar words and convert them to bytes
        /// </summary>
        SIMILARITY,
        /// <summary>
        /// Use both Huffman and Similarity compressions
        /// </summary>
        DUAL
    }
}
