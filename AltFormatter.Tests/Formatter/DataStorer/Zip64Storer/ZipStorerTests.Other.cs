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
        public void ZipWriteOnlyStorer_AddPathNull_ValidateNotNull()
        {
        	var storer = new ZipWriteOnlyStorer(false, true);
        	
        	AssertValidation.NotNull(
        		() => storer.Add(null, 0, 0),
        		"Path");
        }
        
        [Test]
        public void ZipWriteOnlyStorer_AddDataNull_ValidateNotNull()
        {
        	var storer = new ZipWriteOnlyStorer(false, true);
        	
        	AssertValidation.NotNull(
        		() => storer.Add("File.data", null),
        		"Data");
        }
		
		[Test]
        public void ZipReadOnlyStorer_FromDataNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => ZipReadOnlyStorer.FromData(null),
        		"Data");
        }
        
        [Test]
        public void ZipReadOnlyStorer_ReadEntryNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => ZipReadOnlyStorer.FromData(zip32Empty).Read(null),
        		"Entry");
        }
        
        [Test]
        public void ZipReadOnlyStorer_ReadDataLessThan22_Exception()
        {
        	Assert.Throws<InvalidDataException>(() => ZipReadOnlyStorer.FromData(new byte[21]));
        }
        
        [Test]
        public void ZipReadOnlyStorer_ReadInvalidLocalFileHeaderSignature_Exception()
        {
        	byte[] data = zip64OneFileStore.Clone() as byte[];
        	data[0] = 0;
        	
        	Assert.Throws<InvalidDataException>(() => ZipReadOnlyStorer.FromData(data));
        }
        
        [Test]
        public void ZipReadOnlyStorer_ReadInvalidCentralDirectoryFileHeaderSignature_Exception()
        {
        	byte[] data = zip64OneFileStore.Clone() as byte[];
        	data[166] = 0;
        	
        	Assert.Throws<InvalidDataException>(() => ZipReadOnlyStorer.FromData(data));
        }
        
        [Test]
        public void ZipReadOnlyStorer_ReadInvalidCentralDirectoryZip64LocatorSignature_Exception()
        {
        	byte[] data = zip64OneFileStore.Clone() as byte[];
        	data[308] = 0;
        	
        	Assert.Throws<InvalidDataException>(() => ZipReadOnlyStorer.FromData(data));
        }
        
        [Test]
        public void ZipReadOnlyStorer_ReadInvalidCentralDirectoryZip64RecordSignature_Exception()
        {
        	byte[] data = zip64OneFileStore.Clone() as byte[];
        	data[252] = 0;
        	
        	Assert.Throws<InvalidDataException>(() => ZipReadOnlyStorer.FromData(data));
        }
        
        [Test]
        public void ZipReadOnlyStorer_ReadNoValidEndOfCentralDirectoryRecordSignature_Exception()
        {
        	byte[] data = zip64OneFileStore.Clone() as byte[];
        	data[328] = 0;
        	
        	Assert.Throws<InvalidDataException>(() => ZipReadOnlyStorer.FromData(data));
        }
        
        [Test]
        public void ZipReadOnlyStorer_InvalidEntry_Exception()
        {
        	var entry = new InvalidZipEntry()
        	{
        		Path = "File.data"
        	};
        	
        	var ex = Assert.Throws<InvalidEntryException>(() => ZipReadOnlyStorer.FromData(zip32Empty).Read(entry));
        	ex.AssertArguments(entry.Path);
        }
	}
}