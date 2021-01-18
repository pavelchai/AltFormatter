/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace AltFormatter.Formatter
{
	public sealed partial class AltFormatterTests
	{
		[Test, TestCaseSource("CollectionTypeGTD")]
        public void Convert_EmptyArray_EmptyCollection(Type collectionGTD)
        {
        	Convert_ArrayX_CollectionY_Test(false, collectionGTD);
        }
        
        [Test, TestCaseSource("CollectionTypeGTD")]
        public void Convert_EmptyCollection_EmptyArray(Type collectionGTD)
        {
        	Convert_CollectionX_ArrayY_Test(false, collectionGTD);
        }
        
        [Test, TestCaseSource("CollectionTypeGTD")]
        public void Convert_Array_Collection(Type collectionGTD)
        {
        	Convert_ArrayX_CollectionY_Test(true, collectionGTD);
        }
        
        [Test, TestCaseSource("CollectionTypeGTD")]
        public void Convert_Collection_Array(Type collectionGTD)
        {
        	Convert_CollectionX_ArrayY_Test(true, collectionGTD);
        }
        
        [Test, TestCaseSource("CollectionTypeGTD")]
        public void Convert_CollectionX_CollectionY(Type collectionGTD)
        {
        	ConvertRestore_CollectionX_CollectionY_Test(true, typeof(List<>), collectionGTD);
        }
		
		[Test, TestCaseSource("DictionaryTypeGTD")]
        public void Convert_DictionaryX_DictionaryY(Type dictionaryGTD)
        {
        	ConvertRestore_DictionaryX_DictionaryY_Test(true, typeof(Dictionary<,>), dictionaryGTD);
        }
		
		[Test]
    	public void Convert_GenericStructX_GenericStructY()
        {
    		Convert_GenericStructOrClass_Test<GenericStruct<long>, GenericStruct<int>>(
    			() => new GenericStruct<long>());
        }

        [Test]
        public void Convert_GenericClassX_GenericClassY()
        {
        	Convert_GenericStructOrClass_Test<GenericClass<long>, GenericClass<int>>(
    			() => new GenericClass<long>());
        }
        
        [Test]
    	public void Convert_GenericStructX_GenericStructAsInterfaceY()
        {
    		Convert_GenericStructOrClass_Test<GenericStruct<long>, IGenericInterface<int>>(
    			() => new GenericStruct<long>());
        }

        [Test]
        public void Convert_GenericClassX_GenericClassAsInterfaceY()
        {
        	Convert_GenericStructOrClass_Test<GenericClass<long>, IGenericInterface<int>>(
    			() => new GenericClass<long>());
        }
         
        [Test]
    	public void Convert_GenericStructAsInterfaceX_GenericStructAsInterfaceY()
        {
    		Convert_GenericStructOrClass_Test<IGenericInterface<long>, IGenericInterface<int>>(
    			() => new GenericStruct<long>());
        }

        [Test]
        public void Convert_GenericClassAsInterfaceX_GenericClassAsInterfaceY()
        {
        	Convert_GenericStructOrClass_Test<IGenericInterface<long>, IGenericInterface<int>>(
    			() => new GenericClass<long>() as IGenericInterface<long>);
        }
        
        private void Convert_ArrayX_CollectionY_Test(bool fill, Type gtdY)
        {
			var yType = gtdY.MakeGenericType(typeof(int));
			
			var array = fill ? new int[] {1, 2, 3}:new int[0];

			var deserialized = SerializeDeserialize(array.GetType(), yType, array) as IEnumerable<int>;
    		
    		if(gtdY.IsBaseInterfaceOrEquals(typeof(ConcurrentBag<>)))
    		{
    			deserialized = deserialized.OrderBy(i => i);
    		}
    		
        	Assert.True(array.SequenceEqual(deserialized));
        }
        
        private void Convert_CollectionX_ArrayY_Test(bool fill, Type gtdX)
        {
        	var collection = CreateCollection(gtdX, fill ? new int [] {1, 2, 3} : new int[0]);
    		var deserialized = SerializeDeserialize<IEnumerable<int>,int[]>(collection);
    		
        	Assert.True(collection.SequenceEqual(deserialized));
        }
        
        private void Convert_GenericStructOrClass_Test<TIn, TOut>(Func<TIn> factory) where TIn : INonGenericInterface where TOut : INonGenericInterface
    	{
        	var instance = factory();
        	instance.Value = 100;
        	
        	var deserialized = SerializeDeserialize<TIn, TOut>(instance);
        	Assert.AreEqual(instance.GetType().GetGenericTypeDefinition(), deserialized.GetType().GetGenericTypeDefinition());
        	Assert.AreEqual(instance.Value, deserialized.Value);
    	}
	}
}