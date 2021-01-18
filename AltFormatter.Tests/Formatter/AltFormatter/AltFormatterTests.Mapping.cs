/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;
using System;

namespace AltFormatter.Formatter
{
	public sealed partial class AltFormatterTests
	{
        [Test]
        public void Mapping_StructStruct2SameKeys_Struct2Keys()
        {
        	MappingTestValues<MappingStruct2ValuesT1, MappingStruct2ValuesT2>(
        		new MappingStruct2ValuesT1(100,"XXX"));
        }
        
        [Test]
        public void Mapping_StructClass2SameKeys_Class2Keys()
        {
        	MappingTestValues<MappingStruct2ValuesT1, MappingClass2ValuesT1>(
        		new MappingStruct2ValuesT1(100,"XXX"));
        }
        
        [Test]
        public void Mapping_ClassStruct2SameKeys_Struct2Keys()
        {
        	MappingTestValues<MappingClass2ValuesT1, MappingStruct2ValuesT2>(
        		new MappingClass2ValuesT1(100,"XXX"));
        }
        
        [Test]
        public void Mapping_ClassClass2SameKeys_Class2Keys()
        {
        	MappingTestValues<MappingClass2ValuesT1, MappingClass2ValuesT2>(
        		new MappingClass2ValuesT1(100,"XXX"));
        }
        
        [Test]
        public void Mapping_Type23KeysWithOptional_Type3Keys()
        {
        	MappingTestValues<MappingStruct2ValuesT1, MappingStruct3ValuesWithOptional>(
        		new MappingStruct2ValuesT1(100,"XXX"), v => Assert.Null(v.Value3));
        }
        
        [Test]
        public void Mapping_Type23KeysNoOptional_DefaultType3Keys()
        {
        	MappingTestValues<MappingStruct2ValuesT1, MappingStruct3ValuesWithoutOptional>(
        		new MappingStruct2ValuesT1(100,"XXX"), v => Assert.AreEqual(default(MappingStruct3ValuesWithoutOptional), v));
        }
        
        [Test]
        public void Mapping_Type32Keys_Type2Keys()
        {
        	MappingTestValues<MappingStruct3ValuesWithoutOptional, MappingStruct2ValuesT1>(
        		new MappingStruct3ValuesWithoutOptional(100,"XXX"));
        }
        
        private void MappingTestValues<T, K>(T instance, Action<K> callback = null) where T : IMappingType2Values where K : IMappingType2Values
        {
        	var deserialized = SerializeDeserialize<T, K>(instance);
        	
        	if(callback != null)
        	{
        		callback(deserialized);
        	}
        	else
        	{
        		Assert.AreEqual(instance.Value1, deserialized.Value1);
        		Assert.AreEqual(instance.Value2, deserialized.Value2);
        	}
        }
	}
}