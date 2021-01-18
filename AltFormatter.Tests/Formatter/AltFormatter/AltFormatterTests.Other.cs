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
		public void Serialize_WithoutOrderedKeys_Key2Key1()
        {
        	var instance = new ClassWithoutOrderedKeys();
        	formatter.Serialize(instance);
        	
        	Assert.AreEqual("Key2", instance.Queue.Dequeue());
        	Assert.AreEqual("Key1", instance.Queue.Dequeue());
        }
        
        public void Serialize_WithOrderedKeys_Key1Key2()
        {
        	var instance = new ClassWithoutOrderedKeys();
        	formatter.Serialize(instance);
        	
        	Assert.AreEqual("Key1", instance.Queue.Dequeue());
        	Assert.AreEqual("Key2", instance.Queue.Dequeue());
        }
        
        [Test]
        public void Serialize_GraphNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => formatter.Serialize<string>(null),
        		"Graph");
        }
        
        public void Deserialize_WithoutOrderedKeys_Key2Key1()
        {
        	var instance = SerializeDeserialize<ClassWithoutOrderedKeys>();
        	
        	Assert.AreEqual("Key2", instance.Queue.Dequeue());
        	Assert.AreEqual("Key1", instance.Queue.Dequeue());
        }
        
        public void Deserialize_WithOrderedKeys_Key1Key2()
        {
        	var instance = SerializeDeserialize<ClassWithoutOrderedKeys>();
        	
        	Assert.AreEqual("Key1", instance.Queue.Dequeue());
        	Assert.AreEqual("Key2", instance.Queue.Dequeue());
        }
		
		[Test]
        public void Deserialize_InvalidOutputClassType_Null()
        {
        	var data = formatter.Serialize(new GenericClass<int>());
        	Assert.Null(formatter.Deserialize<WeakReference>(data));
        }
        
        [Test]
        public void Deserialize_DataNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => formatter.Deserialize<int>(null),
        		"Data");
        }
	}
}