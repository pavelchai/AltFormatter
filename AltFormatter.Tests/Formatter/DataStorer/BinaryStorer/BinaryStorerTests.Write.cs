/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;
using System.Linq;

namespace AltFormatter.Formatter
{
	internal sealed partial class BinaryStorerTests
	{
		[Test]
    	public void BinaryWriteOnlyStorer_StoreZeroEntries_Empty()
    	{
    		BinaryWriteOnlyStorer storer = new BinaryWriteOnlyStorer(false);
    		Assert.True((new byte[0]).SequenceEqual(storer.Close()));
    	}
    	
    	[Test]
    	public void BinaryWriteOnlyStorer_DeflateNoEntries_Empty()
    	{
    		BinaryWriteOnlyStorer storer = new BinaryWriteOnlyStorer(true);
    		Assert.True((new byte[0]).SequenceEqual(storer.Close()));
    	}
    	
    	[Test]
    	public void BinaryWriteOnlyStorer_StoreOneFile_BinaryOneEntry()
    	{
    		BinaryWriteOnlyStorer storer = new BinaryWriteOnlyStorer(false);
    		storer.Add(filePath1, fileData);
    		Assert.True(binaryOneFileStore.SequenceEqual(storer.Close()));
    	}
    	
    	[Test]
    	public void BinaryWriteOnlyStorer_DeflateOneFile_BinaryOneEntry()
    	{
    		BinaryWriteOnlyStorer storer = new BinaryWriteOnlyStorer(true);
    		storer.Add(filePath1, fileData);
    		Assert.True(binaryOneFileDeflate.SequenceEqual(storer.Close()));
    	}
    	
    	[Test]
    	public void BinaryWriteOnlyStorer_StoreTwoFiles_BinaryTwoEntry()
    	{
    		BinaryWriteOnlyStorer storer = new BinaryWriteOnlyStorer(false);
    		storer.Add(filePath1, fileData);
    		storer.Add(filePath2, fileData);
    		
    		Assert.True(binaryTwoFilesStore.SequenceEqual(storer.Close()));
    	}
    	
    	[Test]
    	public void BinaryWriteOnlyStorer_DeflateTwoFiles_BinaryTwoEntryy()
    	{
    		BinaryWriteOnlyStorer storer = new BinaryWriteOnlyStorer(true);
    		storer.Add(filePath1, fileData);
    		storer.Add(filePath2, fileData);
    		
    		Assert.True(binaryTwoFilesDeflate.SequenceEqual(storer.Close()));
    	}
	}
}