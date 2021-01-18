/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Utils;

namespace AltFormatter.Formatter
{
	/// <summary>
	/// Represents an entry for the <see cref="BinaryReadOnlyStorer"></see>.
	/// </summary>
	internal sealed class BinaryReadOnlyStorerEntry : AbstractDataStoreEntry
	{
		/// <summary>
        /// Creates a new entry for the <see cref="BinaryReadOnlyStorer"></see>.
        /// </summary>
        /// <param name="path"> Path to the data. </param>
        /// <param name="compressionMethod"> Compression method for the data. </param>
        /// <param name="fileOffset"> Offset of the data. </param>
        /// <param name="compressedSize"> Size of the compressed data. </param>
        public BinaryReadOnlyStorerEntry(string path, CompressionMethod compressionMethod, int fileOffset, int compressedSize) : base(path)
		{
        	this.FileOffset = fileOffset;
        	this.CompressedSize = compressedSize;
        	this.CompressionMethod = compressionMethod;
		}
        
        /// <summary>
        /// Compression method for the data.
        /// </summary>
        public readonly CompressionMethod CompressionMethod;
        
        /// <summary>
        /// Offset of the data.
        /// </summary>
        public readonly int FileOffset;
        
        /// <summary>
        /// Gets a size of the compressed data.
        /// </summary>
        /// <returns> Size of the compressed data. </returns>
        public readonly int CompressedSize;
	}
}