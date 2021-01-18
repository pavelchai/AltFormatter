/*
 * This file is a part of the AltPhotonic Studio (ST).
 * Copyright © 2020 Pavel Chaimardanov. All rights reserved.
 */
using NUnit.Framework;

namespace AltPhotonicST.Core.Services.Formatter
{
	internal sealed partial class BinaryStorerTests
	{
		[Test]
        public void BinaryWriteOnlyStorer_AddPathNull_ValidateNotNull()
        {
        	var storer = new BinaryWriteOnlyStorer(false);
        	
        	AssertValidation.NotNull(
        		() => storer.Add(null, 0, 0),
        		"Path");
        }
        
        [Test]
        public void BinaryWriteOnlyStorer_AddDataNull_ValidateNotNull()
        {
        	var storer = new BinaryWriteOnlyStorer(false);
        	
        	AssertValidation.NotNull(
        		() => storer.Add("File.data", null),
        		"Data");
        }
		
		[Test]
        public void BinaryReadOnlyStorer_FromDataNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => BinaryReadOnlyStorer.FromData(null),
        		"Data");
        }
        
        [Test]
        public void BinaryReadOnlyStorer_ReadEntryNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => BinaryReadOnlyStorer.FromData(binaryEmpty).Read(null),
        		"Entry");
        }
        
        [Test]
        public void BinaryReadOnlyStorer_InvalidEntry_Exception()
        {
        	var entry = new InvalidBinaryEntry()
        	{
        		Path = "File.data"
        	};
        	
        	var ex = Assert.Throws<InvalidEntryException>(() => BinaryReadOnlyStorer.FromData(binaryEmpty).Read(entry));
        	ex.AssertArguments(entry.Path);
        }
	}
}