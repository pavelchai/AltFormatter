/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Utils;
using NUnit.Framework;

namespace AltFormatter.Formatter
{
	internal sealed partial class ZipStorerTests
	{
		[Test]
    	public void Zip32ReadOnlyStorer_Empty_NoEntries()
    	{
    		IReadOnlyDataStorer storer = ZipReadOnlyStorer.FromData(zip32Empty);
    		Assert.Zero(storer.Entries.Count);
    	}
    	
    	[Test]
    	public void Zip32ReadOnlyStorer_Empty_IsZip32()
    	{
    		ZipReadOnlyStorer storer = ZipReadOnlyStorer.FromData(zip32Empty) as ZipReadOnlyStorer;
    		Assert.False(storer.IsZip64);
    	}
    	
    	[Test]
    	public void Zip32ReadOnlyStorer_StoreOneFile_FileComment()
    	{
    		ZipReadOnlyStorer_OneFile_Test(false, false, f => f.Comment, "");
    	}
    	
    	[Test]
    	public void Zip32ReadOnlyStorer_StoreOneFile_FileCompressedSize()
    	{
    		ZipReadOnlyStorer_OneFile_Test(false, false, f => f.CompressedSize, 96);
    	}
    	
    	[Test]
    	public void Zip32ReadOnlyStorer_StoreOneFile_FileCompressionMethod()
    	{
    		ZipReadOnlyStorer_OneFile_Test(false, false, f => f.CompressionMethod, CompressionMethod.Store);
    	}
    	
    	[Test]
    	public void Zip32ReadOnlyStorer_StoreOneFile_FileCRC32()
    	{
    		ZipReadOnlyStorer_OneFile_Test(false, false, f => f.CRC32, 1383603372);
    	}
    	
    	[Test]
    	public void Zip32ReadOnlyStorer_StoreOneFile_FileOffset()
    	{
    		ZipReadOnlyStorer_OneFile_Test(false, false, f => f.FileOffset, 38);
    	}
    	
    	[Test]
    	public void Zip32ReadOnlyStorer_StoreOneFile_HeaderOffset()
    	{
    		ZipReadOnlyStorer_OneFile_Test(false, false, f => f.HeaderOffset, 0);
    	}
    	
    	[Test]
    	public void Zip32ReadOnlyStorer_StoreOneFile_FilePath()
    	{
    		ZipReadOnlyStorer_OneFile_Test(false, false, f => f.Path, "File.txt");
    	}
    	
    	[Test]
    	public void Zip32ReadOnlyStorer_StoreOneFile_FileSize()
    	{
    		ZipReadOnlyStorer_OneFile_Test(false, false, f => f.Size, 96);
    	}
    	
    	[Test]
    	public void Zip32ReadOnlyStorer_StoreOneFile_OneEntryData()
    	{
    		ZipReadOnlyStorer_OneFile_Test(false, false);
    	}
    	
    	[Test]
    	public void Zip32ReadOnlyStorer_StoreTwoFiles_TwoEntriesData()
    	{
    		ZipReadOnlyStorer_TwoFiles_Test(false, false);
    	}
    	
    	[Test]
    	public void Zip32ReadOnlyStorer_DeflateOneFile_FileComment()
    	{
    		ZipReadOnlyStorer_OneFile_Test(false, true, f => f.Comment, "");
    	}
    	
    	[Test]
    	public void Zip32ReadOnlyStorer_DeflateOneFile_FileCompressedSize()
    	{
    		ZipReadOnlyStorer_OneFile_Test(false, true, f => f.CompressedSize, 86);
    	}
    	
    	[Test]
    	public void Zip32ReadOnlyStorer_DeflateOneFile_FileCompressionMethod()
    	{
    		ZipReadOnlyStorer_OneFile_Test(false, true, f => f.CompressionMethod, CompressionMethod.Deflate);
    	}
    	
    	[Test]
    	public void Zip32ReadOnlyStorer_DeflateOneFile_FileCRC32()
    	{
    		ZipReadOnlyStorer_OneFile_Test(false, true, f => f.CRC32, 1383603372);
    	}
    	
    	[Test]
    	public void Zip32ReadOnlyStorer_DeflateOneFile_FileOffset()
    	{
    		ZipReadOnlyStorer_OneFile_Test(false, true, f => f.FileOffset, 38);
    	}
    	
    	[Test]
    	public void Zip32ReadOnlyStorer_DeflateOneFile_HeaderOffset()
    	{
    		ZipReadOnlyStorer_OneFile_Test(false, true, f => f.HeaderOffset, 0);
    	}
    	
    	[Test]
    	public void Zip32ReadOnlyStorer_DeflateOneFile_FilePath()
    	{
    		ZipReadOnlyStorer_OneFile_Test(false, true, f => f.Path, "File.txt");
    	}
    	
    	[Test]
    	public void Zip32ReadOnlyStorer_DeflateOneFile_FileSize()
    	{
    		ZipReadOnlyStorer_OneFile_Test(false, true, f => f.Size, 96);
    	}
    	
    	[Test]
    	public void Zip32ReadOnlyStorer_DeflateOneFile_OneEntryData()
    	{
    		ZipReadOnlyStorer_OneFile_Test(false, true);
    	}
    	
    	[Test]
    	public void Zip32ReadOnlyStorer_DeflateTwoFiles_TwoEntriesData()
    	{
    		ZipReadOnlyStorer_TwoFiles_Test(false, true);
    	}
	}
}