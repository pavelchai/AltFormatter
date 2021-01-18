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
		[Test, TestCaseSource("DictionaryTypeGTD")]
        public void Restore_PrimitiveKeyAndValueDictionary_PrimitiveKeyAndValueDictionary(Type dictionaryGTD)
        {
        	IDictionary<int, int> dictionary = CreateDictionary<int, int>(dictionaryGTD);
        	dictionary.Add(1, 1);
        	
        	IDictionary<int, int> deserialized = this.SerializeDeserialize(dictionary);
        	
        	Assert.True(dictionary.SequenceEqual(deserialized));
        }
        
        [Test, TestCaseSource("DictionaryTypeGTD")]
        public void Restore_NotPrimitiveKeyPrimitiveValueDictionary_PrimitiveKeyNotPrimitiveValueDictionary(Type dictionaryGTD)
        {
        	IDictionary<MappingStruct2ValuesT1, int> dictionary = CreateDictionary<MappingStruct2ValuesT1, int>(dictionaryGTD);
        	dictionary.Add(new MappingStruct2ValuesT1(), 1);
        	
        	IDictionary<MappingStruct2ValuesT1, int> deserialized = this.SerializeDeserialize(dictionary);
        	
        	Assert.True(dictionary.SequenceEqual(deserialized));
        }
        
        [Test, TestCaseSource("DictionaryTypeGTD")]
        public void Restore_PrimitiveKeyNotPrimitiveValueDictionary_PrimitiveKeyNotPrimitiveValueDictionary(Type dictionaryGTD)
        {
        	IDictionary<int, MappingStruct2ValuesT1> dictionary = CreateDictionary<int, MappingStruct2ValuesT1>(dictionaryGTD);
        	dictionary.Add(1, new MappingStruct2ValuesT1());
        	
        	IDictionary<int, MappingStruct2ValuesT1> deserialized = this.SerializeDeserialize(dictionary);
        	
        	Assert.True(dictionary.SequenceEqual(deserialized));
        }
        
        [Test, TestCaseSource("DictionaryTypeGTD")]
        public void Restore_NotPrimitiveKeyNotPrimitiveValueDictionary_PrimitiveKeyNotPrimitiveValueDictionary(Type dictionaryGTD)
        {
        	IDictionary<MappingStruct2ValuesT1, MappingStruct2ValuesT1> dictionary = CreateDictionary<MappingStruct2ValuesT1, MappingStruct2ValuesT1>(dictionaryGTD);
        	dictionary.Add(new MappingStruct2ValuesT1(), new MappingStruct2ValuesT1());
        	
        	IDictionary<MappingStruct2ValuesT1, MappingStruct2ValuesT1> deserialized = this.SerializeDeserialize(dictionary);
        	
        	Assert.True(dictionary.SequenceEqual(deserialized));
        }
        
        [Test, TestCaseSource("DictionaryTypeGTD")]
        public void Restore_StringKeyValueDictionaryWithNewLines_StringKeyValueDictionary(Type dictionaryGTD)
        {
        	IDictionary<string, string> dictionary = CreateDictionary<string, string>(dictionaryGTD);
        	dictionary.Add("\r\nvalue\rvalue\n", "\r\nvalue\rvalue\n");
        	
        	IDictionary<string, string> deserialized = this.SerializeDeserialize(dictionary);
        	
        	Assert.True(dictionary.SequenceEqual(deserialized));
        }
        
        [Test, TestCaseSource("DictionaryTypeGTD")]
        public void Restore_StringKeyValueDictionaryWithTabs_StringKeyValueDictionary(Type dictionaryGTD)
        {
        	IDictionary<string, string> dictionary = CreateDictionary<string, string>(dictionaryGTD);
        	dictionary.Add("\tvalue\tvalue\t", "\tvalue\tvalue\t");
        	
        	IDictionary<string, string> deserialized = this.SerializeDeserialize(dictionary);
        	
        	Assert.True(dictionary.SequenceEqual(deserialized));
        }
        
        [Test, TestCaseSource("DictionaryTypeGTD")]
        public void Restore_StringKeyDictionaryWithNewLines_StringKeyValueDictionary(Type dictionaryGTD)
        {
        	IDictionary<string, int> dictionary = CreateDictionary<string, int>(dictionaryGTD);
        	dictionary.Add("\r\nvalue\rvalue\n", 0);
        	
        	IDictionary<string, int> deserialized = this.SerializeDeserialize(dictionary);
        	
        	Assert.True(dictionary.SequenceEqual(deserialized));
        }
        
        [Test, TestCaseSource("DictionaryTypeGTD")]
        public void Restore_StringKeyDictionaryWithTabs_StringKeyValueDictionary(Type dictionaryGTD)
        {
        	IDictionary<string, int> dictionary = CreateDictionary<string, int>(dictionaryGTD);
        	dictionary.Add("\tvalue\tvalue\t", 0);
        	
        	IDictionary<string, int> deserialized = this.SerializeDeserialize(dictionary);
        	
        	Assert.True(dictionary.SequenceEqual(deserialized));
        }
        
        
        [Test, TestCaseSource("DictionaryTypeGTD")]
        public void Restore_StringValuesDictionaryWithNewLines_StringKeyValueDictionary(Type dictionaryGTD)
        {
        	IDictionary<int, string> dictionary = CreateDictionary<int, string>(dictionaryGTD);
        	dictionary.Add(0, "\r\nvalue\rvalue\n");
        	
        	IDictionary<int, string> deserialized = this.SerializeDeserialize(dictionary);
        	
        	Assert.True(dictionary.SequenceEqual(deserialized));
        }
        
        [Test, TestCaseSource("DictionaryTypeGTD")]
        public void Restore_StringValuesDictionaryWithTabs_StringKeyValueDictionary(Type dictionaryGTD)
        {
        	IDictionary<int, string> dictionary = CreateDictionary<int, string>(dictionaryGTD);
        	dictionary.Add(0, "\tvalue\tvalue\t");
        	
        	IDictionary<int, string> deserialized = this.SerializeDeserialize(dictionary);
        	
        	Assert.True(dictionary.SequenceEqual(deserialized));
        }
        
	}
}