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
        public void Serialize_IFormattable_RaiseOnSerializing()
        {
        	var instance = new ClassImplementsIFormattable();
        	var data = formatter.Serialize(instance);
        	Assert.AreEqual(1, instance.OnSerializingRaisedCount);
        }
        
        [Test]
        public void Serialize_IFormattable_RaiseOnSerialized()
        {
        	var instance = new ClassImplementsIFormattable();
        	var data = formatter.Serialize(instance);
        	Assert.AreEqual(1, instance.OnSerializedRaisedCount);
        }
        
        [Test]
        public void Deserialize_IFormattable_RaiseOnDeserializing()
        {
        	var instance = SerializeDeserialize<ClassImplementsIFormattable>();
        	Assert.AreEqual(1, instance.OnDeserializingRaisedCount);
        }
        
        [Test]
        public void Deserialize_IFormattable_RaiseOnDeserialized()
        {
        	var instance = SerializeDeserialize<ClassImplementsIFormattable>();
        	Assert.AreEqual(1, instance.OnDeserializedRaisedCount);
        }
        
        [Test]
        public void Deserialize_IFormattable_RaiseSubstitution()
        {
        	var instance = SerializeDeserialize<ClassImplementsIFormattable>();
        	Assert.AreEqual(1, instance.OnSubstitutionRaisedCount);
        }
        
        [Test]
        public void Deserialize_InvalidOutputInterfaceType_Null()
        {
        	var data = formatter.Serialize(new GenericClass<int>());
        	Assert.Null(formatter.Deserialize<IComparable>(data));
        }
        
        [Test]
        public void Deserialize_InvalidOutputStructType_Default()
        {
        	var data = formatter.Serialize(new GenericClass<int>());
        	Assert.AreEqual(default(int), formatter.Deserialize<int>(data));
        }
	}
}