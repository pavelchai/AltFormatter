/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Utils;
using System;
using System.Text;
using System.Collections.Generic;

namespace AltFormatter.Formatter
{
	/// <summary>
    /// Represents a read-only binary storer.
    /// </summary>
	public sealed class BinaryReadOnlyStorer : IReadOnlyDataStorer
	{
		/// <summary>
        /// UTF-8 Encoding.
        /// </summary>
        private static readonly Encoding utf8Encoding = Encoding.UTF8;
        
        /// <summary>
        /// Data of storage file.
        /// </summary>
        private byte[] binaryData;
        
        /// <summary>
        /// Creates a new read-only binary storer.
        /// </summary>
        /// <param name="data"> Data of the binary file. </param>
        /// <param name="entries"> Entries in the storer. </param>
    	private BinaryReadOnlyStorer(byte[] data, IReadOnlyList<IDataStorerEntry> entries)
        {
        	this.binaryData = data;
        	this.Entries = entries;
        }
        
        /// <inheritdoc/>
    	public IReadOnlyList<IDataStorerEntry> Entries { get; private set;}
        
        /// <inheritdoc/>
        public byte[] Read(IDataStorerEntry entry)
        {
        	Validation.NotNull("Entry", entry);
        	
            BinaryReadOnlyStorerEntry binaryEntry = entry as BinaryReadOnlyStorerEntry;
            if(binaryEntry == null)
            {
            	throw new InvalidEntryException(entry.Path);
            }

            byte[] data = null;
            int count = (int)binaryEntry.CompressedSize;
            
            if (binaryEntry.CompressionMethod == CompressionMethod.Store)
            {
                data = new byte[count];
                Buffer.BlockCopy(this.binaryData, binaryEntry.FileOffset, data, 0, count);
            }
            else
            {
                data = ZLibUtils.Inflate(this.binaryData, binaryEntry.FileOffset, binaryEntry.CompressedSize);
            }

            return data;
        }
        
        /// <summary>
        /// Creates a new read-only binary storer from the data.
        /// </summary>
        /// <param name="data"> The data. </param>
        /// <returns> New read-only binary storer. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when data is null.
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// The exception that is thrown when data is invalid.
        /// </exception>
        public static IReadOnlyDataStorer FromData(params byte[] data)
        {
        	Validation.NotNull("Data", data);
        	
        	int length = data.Length;
        	int offset = 0;
        	int compressedSize = 0;
        	int pathSize = 0;
        	CompressionMethod compressionMethod;
        	string path;
        	
        	LinkedList<IDataStorerEntry> entries = new LinkedList<IDataStorerEntry>();
        	
        	while(offset < length)
        	{
        		compressedSize = (int)BytesConverter.ToUInt32(data, offset);
        		pathSize = (int)BytesConverter.ToUInt32(data, offset + 4);
        		compressionMethod = (CompressionMethod)data[offset + 8];
        		
        		offset += 9;
        		path = utf8Encoding.GetString(data, offset, pathSize);
        		
        		offset += pathSize;
        		entries.AddLast(new BinaryReadOnlyStorerEntry(path, compressionMethod, offset, compressedSize));
        		
        		offset += compressedSize;
        	}
        	
        	return new BinaryReadOnlyStorer(data, entries.ToArray());
        }
	}
}
