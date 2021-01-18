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
    	public void Zip64ReadOnlyStorer_Empty_NoEntries()
    	{
    		IReadOnlyDataStorer storer = ZipReadOnlyStorer.FromData(zip64Empty);
    		Assert.Zero(storer.Entries.Count);
    	}
    	
    	[Test]
    	public void Zip64ReadOnlyStorer_Empty_IsZip64()
    	{
    		ZipReadOnlyStorer storer = ZipReadOnlyStorer.FromData(zip64Empty) as ZipReadOnlyStorer;
    		Assert.True(storer.IsZip64);
    	}
    	
    	[Test]
    	public void Zip64ReadOnlyStorer_StoreOneFile_FileComment()
    	{
    		ZipReadOnlyStorer_OneFile_Test(true, false, f => f.Comment, "");
    	}
    	
    	[Test]
    	public void Zip64ReadOnlyStorer_StoreOneFile_FileCompressedSize()
    	{
    		ZipReadOnlyStorer_OneFile_Test(true, false, f => f.CompressedSize, 96);
    	}
    	
    	[Test]
    	public void Zip64ReadOnlyStorer_StoreOneFile_FileCompressionMethod()
    	{
    		ZipReadOnlyStorer_OneFile_Test(true, false, f => f.CompressionMethod, CompressionMethod.Store);
    	}
    	
    	[Test]
    	public void Zip64ReadOnlyStorer_StoreOneFile_FileCRC32()
    	{
    		ZipReadOnlyStorer_OneFile_Test(true, false, f => f.CRC32, 1383603372);
    	}
    	
    	[Test]
    	public void Zip64ReadOnlyStorer_StoreOneFile_FileOffset()
    	{
    		ZipReadOnlyStorer_OneFile_Test(true, false, f => f.FileOffset, 70);
    	}
    	
    	[Test]
    	public void Zip64ReadOnlyStorer_StoreOneFile_HeaderOffset()
    	{
    		ZipReadOnlyStorer_OneFile_Test(true, false, f => f.HeaderOffset, 0);
    	}
    	
    	[Test]
    	public void Zip64ReadOnlyStorer_StoreOneFile_FilePath()
    	{
    		ZipReadOnlyStorer_OneFile_Test(true, false, f => f.Path, "File.txt");
    	}
    	
    	[Test]
    	public void Zip64ReadOnlyStorer_StoreOneFile_FileSize()
    	{
    		ZipReadOnlyStorer_OneFile_Test(true, false, f => f.Size, 96);
    	}
    	
    	[Test]
    	public void Zip64ReadOnlyStorer_StoreOneFile_OneEntryData()
    	{
    		ZipReadOnlyStorer_OneFile_Test(true, false);
    	}
    	
    	[Test]
    	public void Zip64ReadOnlyStorer_StoreTwoFiles_TwoEntriesData()
    	{
    		ZipReadOnlyStorer_TwoFiles_Test(true, false);
    	}
    	
    	[Test]
    	public void Zip64ReadOnlyStorer_DeflateOneFile_FileComment()
    	{
    		ZipReadOnlyStorer_OneFile_Test(true, true, f => f.Comment, "");
    	}
    	
    	[Test]
    	public void Zip64ReadOnlyStorer_DeflateOneFile_FileCompressedSize()
    	{
    		ZipReadOnlyStorer_OneFile_Test(true, true, f => f.CompressedSize, 86);
    	}
    	
    	[Test]
    	public void Zip64ReadOnlyStorer_DeflateOneFile_FileCompressionMethod()
    	{
    		ZipReadOnlyStorer_OneFile_Test(true, true, f => f.CompressionMethod, CompressionMethod.Deflate);
    	}
    	
    	[Test]
    	public void Zip64ReadOnlyStorer_DeflateOneFile_FileCRC32()
    	{
    		ZipReadOnlyStorer_OneFile_Test(true, true, f => f.CRC32, 1383603372);
    	}
    	
    	[Test]
    	public void Zip64ReadOnlyStorer_DeflateOneFile_FileOffset()
    	{
    		ZipReadOnlyStorer_OneFile_Test(true, true, f => f.FileOffset, 70);
    	}
    	
    	[Test]
    	public void Zip64ReadOnlyStorer_DeflateOneFile_HeaderOffset()
    	{
    		ZipReadOnlyStorer_OneFile_Test(true, true, f => f.HeaderOffset, 0);
    	}
    	
    	[Test]
    	public void Zip64ReadOnlyStorer_DeflateOneFile_FilePath()
    	{
    		ZipReadOnlyStorer_OneFile_Test(true, true, f => f.Path, "File.txt");
    	}
    	
    	[Test]
    	public void Zip64ReadOnlyStorer_DeflateOneFile_FileSize()
    	{
    		ZipReadOnlyStorer_OneFile_Test(true, true, f => f.Size, 96);
    	}
    	
    	[Test]
    	public void Zip64ReadOnlyStorer_DeflateOneFile_OneEntryData()
    	{
    		ZipReadOnlyStorer_OneFile_Test(true, true);
    	}
    	
    	[Test]
    	public void Zip64ReadOnlyStorer_DeflateTwoFiles_TwoEntriesData()
    	{
    		ZipReadOnlyStorer_TwoFiles_Test(true, true);
    	}
	}
}