/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AltFormatter.Formatter
{
	public sealed partial class AltFormatterTests
	{
		[Test]
        public void Restore_Primitive1DArray_Primitive1DArray()
        {
        	int[] array = {0};
        	int[] deserialized = this.SerializeDeserialize(array);
        	
        	Assert.True(array.SequenceEqual(deserialized));
        }
        
        [Test]
        public void Restore_NotPrimitive1DArray_NotPrimitive1DArray()
        {
        	MappingStruct2ValuesT1[] array = {new MappingStruct2ValuesT1()};
        	MappingStruct2ValuesT1[] deserialized = this.SerializeDeserialize(array);
        	
        	Assert.True(array.SequenceEqual(deserialized));
        }
        
        [Test, TestCaseSource("CollectionTypeGTD")]
        public void Restore_PrimitiveCollection_PrimitiveCollection(Type collectionGTD)
        {
        	IEnumerable<int> collection = CreateCollection(collectionGTD, 1);
        	IEnumerable<int> deserialized = this.SerializeDeserialize(collection);
        	
        	Assert.True(collection.SequenceEqual(deserialized));
        }
        
        [Test, TestCaseSource("CollectionTypeGTD")]
        public void Restore_NotPrimitiveCollection_NotPrimitiveCollection(Type collectionGTD)
        {
        	IEnumerable<MappingStruct2ValuesT1> collection = CreateCollection(collectionGTD, new MappingStruct2ValuesT1());
        	IEnumerable<MappingStruct2ValuesT1> deserialized = this.SerializeDeserialize(collection);
        	
        	Assert.True(collection.SequenceEqual(deserialized));
        }
        
        [Test, TestCaseSource("CollectionTypeGTD")]
        public void Restore_StringCollectionWithNewLines_StringCollection(Type collectionGTD)
        {
        	IEnumerable<string> collection = CreateCollection(collectionGTD, "\r\nvalue\rvalue\n");
        	IEnumerable<string> deserialized = this.SerializeDeserialize(collection);
        	
        	Assert.True(collection.SequenceEqual(deserialized));
        }
        
        [Test, TestCaseSource("CollectionTypeGTD")]
        public void Restore_StringCollectionWithTabs_StringCollection(Type collectionGTD)
        {
        	IEnumerable<string> collection = CreateCollection(collectionGTD, "\tvalue\tvalue\t");
        	IEnumerable<string> deserialized = this.SerializeDeserialize(collection);
        	
        	Assert.True(collection.SequenceEqual(deserialized));
        }
	}
}