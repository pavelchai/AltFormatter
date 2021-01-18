/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AltFormatter.Formatter
{
	internal sealed partial class BinaryStorerTests
	{
		[Test]
    	public void BinaryReadOnlyStorer_Empty_NoEntries()
    	{
    		IReadOnlyDataStorer storer = BinaryReadOnlyStorer.FromData(new byte[0]);
    		Assert.Zero(storer.Entries.Count);
    	}
    	
    	[Test]
    	public void BinaryReadOnlyStorer_StoreOneFile_OneEntryData()
    	{
    		BinaryReadOnlyStorer_OneFile_Test(false);
    	}
    	
    	[Test]
    	public void BinaryReadOnlyStorer_StoreOneFile_CompressedSize()
    	{
    		BinaryReadOnlyStorer_OneFile_Test(false, f => f.CompressedSize, 96);
    	}
    	
    	[Test]
    	public void BinaryReadOnlyStorer_StoreOneFile_CompressionMethod()
    	{
    		BinaryReadOnlyStorer_OneFile_Test(false, f => f.CompressionMethod, CompressionMethod.Store);
    	}
    	
    	[Test]
    	public void BinaryReadOnlyStorer_StoreOneFile_FileOffset()
    	{
    		BinaryReadOnlyStorer_OneFile_Test(false, f => f.FileOffset, 17);
    	}
    	
    	[Test]
    	public void BinaryReadOnlyStorer_StoreTwoFiles_TwoEntriesData()
    	{
    		BinaryReadOnlyStorer_TwoFiles_Test(false);
    	}
    	
    	[Test]
    	public void BinaryReadOnlyStorer_DeflateOneFile_OneEntryData()
    	{
    		BinaryReadOnlyStorer_OneFile_Test(true);
    	}
    	
    	[Test]
    	public void BinaryReadOnlyStorer_DeflateOneFile_CompressedSize()
    	{
    		BinaryReadOnlyStorer_OneFile_Test(true, f => f.CompressedSize, 86);
    	}
    	
    	[Test]
    	public void BinaryReadOnlyStorer_DeflateOneFile_CompressionMethod()
    	{
    		BinaryReadOnlyStorer_OneFile_Test(true, f => f.CompressionMethod, CompressionMethod.Deflate);
    	}
    	
    	[Test]
    	public void BinaryReadOnlyStorer_DeflateOneFile_FileOffset()
    	{
    		BinaryReadOnlyStorer_OneFile_Test(true, f => f.FileOffset, 17);
    	}
    	
    	[Test]
    	public void BinaryReadOnlyStorer_DeflateTwoFiles_TwoEntriesData()
    	{
    		BinaryReadOnlyStorer_TwoFiles_Test(true);
    	}
    	
    	private void BinaryReadOnlyStorer_OneFile_Test(bool enableCompression, Func<BinaryReadOnlyStorerEntry, object> selectValue, object value)
    	{
    		IReadOnlyDataStorer storer = BinaryReadOnlyStorer.FromData(enableCompression ? binaryOneFileDeflate : binaryOneFileStore);
    		BinaryReadOnlyStorerEntry entry = storer.Entries.FindOrDefault(e => e.Path == filePath1) as BinaryReadOnlyStorerEntry;
    		
    		Assert.AreEqual(value, selectValue(entry));
    	}
    	
    	private void BinaryReadOnlyStorer_OneFile_Test(bool enableCompression)
    	{
    		IReadOnlyDataStorer storer = BinaryReadOnlyStorer.FromData(enableCompression ? binaryOneFileDeflate : binaryOneFileStore);
    		BinaryReadOnlyStorerEntry entry = storer.Entries.FindOrDefault(e => e.Path == filePath1) as BinaryReadOnlyStorerEntry;
    		
    		Assert.NotNull(fileData.SequenceEqual(storer.Read(entry)));
    	}
    	
    	private void BinaryReadOnlyStorer_TwoFiles_Test(bool enableCompression)
    	{
    		IReadOnlyDataStorer storer = BinaryReadOnlyStorer.FromData(enableCompression ? binaryTwoFilesDeflate : binaryTwoFilesStore);
    		
    		IReadOnlyList<IDataStorerEntry> entries = storer.Entries;
    		BinaryReadOnlyStorerEntry entry1 = entries.FindOrDefault(e => e.Path == filePath1) as BinaryReadOnlyStorerEntry;
    		BinaryReadOnlyStorerEntry entry2 = entries.FindOrDefault(e => e.Path == filePath2) as BinaryReadOnlyStorerEntry;
    		
    		Assert.NotNull(fileData.SequenceEqual(storer.Read(entry1)));
    		Assert.NotNull(fileData.SequenceEqual(storer.Read(entry2)));
    	}
	}
}