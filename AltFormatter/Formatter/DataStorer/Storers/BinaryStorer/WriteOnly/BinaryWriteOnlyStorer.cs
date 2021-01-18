/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Utils;
using AltFormatter.ZLib;
using System.Text;

namespace AltFormatter.Formatter
{
	/// <summary>
    /// Represents a write-only binary storer.
    /// </summary>
	internal sealed class BinaryWriteOnlyStorer : IWriteOnlyDataStorer
	{
		/// <summary>
        /// UTF-8 Encoding.
        /// </summary>
        private static readonly Encoding utf8Encoding = Encoding.UTF8;
		
		/// <summary>
        /// Indicates whether compression is on.
        /// </summary>
        private readonly bool enableCompression;
        
        /// <summary>
        /// Writer of storage file.
        /// </summary>
        private readonly IByteArrayWriter fileWriter = new ByteArrayWriter(true);
		
		/// <summary>
        /// Creates a new write-only binary storer.
        /// </summary>
        /// <param name="enableCompression"> Indicates whether compression is on. </param>
        public BinaryWriteOnlyStorer(bool enableCompression)
        {
            this.enableCompression = enableCompression;
        }
		
		/// <inheritdoc/>
        public IDataStorerEntry Add(string path, params byte[] data)
        {
        	Validation.NotNull("Path", path);
        	Validation.NotNull("Data", data);
        	
            int uncompressedLength = data.Length;
            int compressedLength;
            byte[] compressedData;
            CompressionMethod compressionMethod;
            
            if (this.enableCompression)
            {
                Deflater deflater = new Deflater(Deflater.DEFLATED);
                deflater.SetInput(data, 0, uncompressedLength);
                deflater.Finish();

                compressedData = new byte[uncompressedLength];
                compressedLength = deflater.Deflate(compressedData, 0, uncompressedLength);
                
                if (deflater.IsFinished && compressedLength <= uncompressedLength)
                {
                    // Use deflate
                    compressionMethod = CompressionMethod.Deflate;
                }
                else
                {
                    // Force to store
                    compressedData = data;
                    compressedLength = uncompressedLength;
                    compressionMethod = CompressionMethod.Store;
                }
            }
            else
            {
                // Only store
                compressedData = data;
                compressedLength = uncompressedLength;
                compressionMethod = CompressionMethod.Store;
            }
            
            byte[] pathBytes = utf8Encoding.GetBytes(path);
            
            this.fileWriter.WriteBytes(BytesConverter.GetBytes((uint)compressedLength));
            this.fileWriter.WriteBytes(BytesConverter.GetBytes((uint)pathBytes.Length));
            this.fileWriter.WriteBytes((byte)compressionMethod);
            this.fileWriter.WriteBytes(pathBytes);
            this.fileWriter.WriteBytes(compressedData, 0, compressedLength);
            
            return new BinaryWriteOnlyStorerEntry(path);
        }

        /// <inheritdoc/>
        public byte[] Close()
        {
        	return this.fileWriter.GetBytes();
        }
	}
}
