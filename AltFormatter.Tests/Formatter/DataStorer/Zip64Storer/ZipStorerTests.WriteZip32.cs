/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;

namespace AltFormatter.Formatter
{
	internal sealed partial class ZipStorerTests
	{
		[Test]
    	public void Zip32WriteOnlyStorer_StoreZeroEntries_EmptyZip32()
    	{
    		ZipWriteOnlyStorer storer = new ZipWriteOnlyStorer(false, false);
    		ZipWriteOnlyStorer_ZipDataEquality_Test(zip32Empty, storer.Close(), 0);
    	}
    	
    	[Test]
    	public void Zip32WriteOnlyStorer_DeflateNoEntries_EmptyZip32()
    	{
    		ZipWriteOnlyStorer storer = new ZipWriteOnlyStorer(true, false);
    		ZipWriteOnlyStorer_ZipDataEquality_Test(zip32Empty, storer.Close(), 0);
    	}
    	
    	[Test]
    	public void Zip32WriteOnlyStorer_StoreOneFile_Zip32OneEntry()
    	{
    		ZipWriteOnlyStorer storer = new ZipWriteOnlyStorer(false, false);
    		storer.Add(filePath1, fileData);
    		
    		ZipWriteOnlyStorer_ZipDataEquality_Test(zip32OneFileStore, storer.Close(), 1);
    	}
    	
    	[Test]
    	public void Zip32WriteOnlyStorer_DeflateOneFile_Zip32OneEntry()
    	{
    		ZipWriteOnlyStorer storer = new ZipWriteOnlyStorer(true, false);
    		storer.Add(filePath1, fileData);
    		
    		ZipWriteOnlyStorer_ZipDataEquality_Test(zip32OneFileDeflate, storer.Close(), 1);
    	}
    	
    	[Test]
    	public void Zip32WriteOnlyStorer_StoreTwoFiles_Zip32TwoEntries()
    	{
    		ZipWriteOnlyStorer storer = new ZipWriteOnlyStorer(false, false);
    		storer.Add(filePath1, fileData);
    		storer.Add(filePath2, fileData);
    		
    		ZipWriteOnlyStorer_ZipDataEquality_Test(zip32TwoFilesStore, storer.Close(), 2);
    	}
    	
    	[Test]
    	public void Zip32WriteOnlyStorer_DeflateTwoFiles_Zip32TwoEntries()
    	{
    		ZipWriteOnlyStorer storer = new ZipWriteOnlyStorer(true, false);
    		storer.Add(filePath1, fileData);
    		storer.Add(filePath2, fileData);
    		
    		ZipWriteOnlyStorer_ZipDataEquality_Test(zip32TwoFilesDeflate, storer.Close(), 2);
    	}
	}
}