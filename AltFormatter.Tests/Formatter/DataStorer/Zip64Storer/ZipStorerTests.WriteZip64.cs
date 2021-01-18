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
    	public void Zip64WriteOnlyStorer_StoreZeroEntries_EmptyZip64()
    	{
    		ZipWriteOnlyStorer storer = new ZipWriteOnlyStorer(false, true);
    		ZipWriteOnlyStorer_ZipDataEquality_Test(zip64Empty, storer.Close(), 0);
    	}
    	
    	[Test]
    	public void Zip64WriteOnlyStorer_DeflateNoEntries_EmptyZip64()
    	{
    		ZipWriteOnlyStorer storer = new ZipWriteOnlyStorer(true, true);
    		ZipWriteOnlyStorer_ZipDataEquality_Test(zip64Empty, storer.Close(), 0);
    	}
    	
    	[Test]
    	public void Zip64WriteOnlyStorer_StoreOneFile_Zip64OneEntry()
    	{
    		ZipWriteOnlyStorer storer = new ZipWriteOnlyStorer(false, true);
    		storer.Add(filePath1, fileData);
    		
    		ZipWriteOnlyStorer_ZipDataEquality_Test(zip64OneFileStore, storer.Close(), 1);
    	}
    	
    	[Test]
    	public void Zip64WriteOnlyStorer_DeflateOneFile_Zip64OneEntry()
    	{
    		ZipWriteOnlyStorer storer = new ZipWriteOnlyStorer(true, true);
    		storer.Add(filePath1, fileData);
    		
    		ZipWriteOnlyStorer_ZipDataEquality_Test(zip64OneFileDeflate, storer.Close(), 1);
    	}
    	
    	[Test]
    	public void Zip64WriteOnlyStorer_StoreTwoFiles_Zip64TwoEntries()
    	{
    		ZipWriteOnlyStorer storer = new ZipWriteOnlyStorer(false, true);
    		storer.Add(filePath1, fileData);
    		storer.Add(filePath2, fileData);
    		
    		ZipWriteOnlyStorer_ZipDataEquality_Test(zip64TwoFilesStore, storer.Close(), 2);
    	}
    	
    	[Test]
    	public void Zip64WriteOnlyStorer_DeflateTwoFiles_Zip64TwoEntries()
    	{
    		ZipWriteOnlyStorer storer = new ZipWriteOnlyStorer(true, true);
    		storer.Add(filePath1, fileData);
    		storer.Add(filePath2, fileData);
    		
    		ZipWriteOnlyStorer_ZipDataEquality_Test(zip64TwoFilesDeflate, storer.Close(), 2);
    	}
	}
}