/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AltFormatter.Formatter
{
	public sealed partial class AltFormatterTests
	{
        [Test, TestCaseSource("CollectionTypeGTD")]
        public void Restore_EmptyCollection_EmptyCollection(Type collectionGTD)
        {
        	ConvertRestore_CollectionX_CollectionY_Test(false, collectionGTD, collectionGTD);
        }
        
        [Test, TestCaseSource("CollectionTypeGTD")]
        public void Restore_Collection_Collection(Type collectionGTD)
        {
        	ConvertRestore_CollectionX_CollectionY_Test(true, collectionGTD, collectionGTD);
        }
        
        [Test, TestCaseSource("DictionaryTypeGTD")]
        public void Restore_EmptyDictionary_EmptyDictionary(Type dictionaryGTD)
        {
        	ConvertRestore_DictionaryX_DictionaryY_Test(false, dictionaryGTD, dictionaryGTD);
        }
        
        [Test, TestCaseSource("DictionaryTypeGTD")]
        public void Restore_Dictionary_Dictionary(Type dictionaryGTD)
        {
        	ConvertRestore_DictionaryX_DictionaryY_Test(true, dictionaryGTD, dictionaryGTD);
        }
        
        [Test, TestCaseSource("RankSource")]
        public void Restore_EmptyArray_EmptyArray(int rank)
        {
        	Restore_Array_Array(false, rank);
        }
        
        [Test, TestCaseSource("RankSource")]
        public void Restore_Array_Array(int rank)
        {
        	Restore_Array_Array(true, rank);
        }
        
        [Test, TestCaseSource("DictionaryTypeGTD")]
        public void Restore_CollectionWithDictionaryItem_CollectionWithDictionaryItem(Type dictionaryGTD)
        {
        	var dictionary = ReflectionUtils.CreateDictionary(dictionaryGTD, typeof(int), typeof(int), 0).Invoke(new int[]{0}) as IDictionary<int, int>;
        	dictionary.Add(1, 10);
        	dictionary.Add(2, 20);
        	
        	var list = new List<IDictionary<int, int>>();
        	list.Add(dictionary);
        	
        	var deserialized = SerializeDeserialize(list);
        	
        	Assert.AreEqual(list.Count, deserialized.Count);
        	Assert.True(list[0].SequenceEqual(dictionary));
        }
        
        [Test, TestCaseSource("CollectionTypeGTD")]
        public void Restore_DictionaryWithCollectionItem_DictionaryWithCollectionItem(Type collectionGTD)
        {
        	var collection = CreateCollection(collectionGTD, 1, 2);
        	
        	var dictionary = new Dictionary<IEnumerable<int>, IEnumerable<int>>();
        	dictionary.Add(collection, collection);
        	
        	var deserialized = SerializeDeserialize(dictionary);
        	Assert.AreEqual(dictionary.Count, deserialized.Count);
        	
        	Assert.True(dictionary.First().Key.SequenceEqual(collection));
        	Assert.True(dictionary.First().Value.SequenceEqual(collection));
        }
        
        private void Restore_Array_Array(bool fill, int rank)
        {
        	var elementType = typeof(int);
        	var arrayType = elementType.MakeArrayType(rank);
        	var indexes = ArrayUtils.CreateArray(0, rank);
        	var sizes = (fill) ? ArrayUtils.CreateArray(1, rank): ArrayUtils.CreateArray(0, rank);
        	
        	var array = ReflectionUtils.CreateMDArray(elementType, rank).Invoke(sizes);
        	
        	if(fill)
        	{
        		array.SetValue(1, indexes);
        	}
        	
        	var deserialized = SerializeDeserialize(arrayType, array) as Array;
        	
        	if(fill)
        	{
        		Assert.AreEqual(1, deserialized.Length);
        		Assert.AreEqual(1, deserialized.GetValue(indexes));
        	}
        	else
        	{
        		Assert.AreEqual(0, deserialized.Length);
        	}
        }
	}
}
