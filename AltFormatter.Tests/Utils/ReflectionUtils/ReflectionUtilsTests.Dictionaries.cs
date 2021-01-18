/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace AltFormatter.Utils
{
	internal sealed partial class ReflectionUtilsTests
	{
        [Test, TestCaseSource("AllObjects")]
        public void CreateDictonary_AllTypesDictonaryNoArgs_Dictonary(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateDictionaryKVAndTest(strategy, typeof(SortedList<,>).MakeGenericType(type, type));
        }

        [Test, TestCaseSource("AllObjects")]
        public void CreateDictonary_AllTypesDictonaryOneArg_Dictionary(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
            CreateDictionaryKVAndTest(strategy, typeof(SortedList<,>).MakeGenericType(type, type), 5);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateDictonary_AllTypesDictonaryTwoArg_Dictionary(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateDictionaryKVAndTest(strategy, typeof(ConcurrentDictionary<,>).MakeGenericType(type, type), 4 * Environment.ProcessorCount, 5);
        }
        
        [Test]
        public void CreateDictonary_GTDNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => ReflectionUtils.CreateDictionary(null, typeof(int), typeof(int), 0),
        		"DictionaryGTD");
        }
        
        [Test]
        public void CreateDictonary_KeyTypeNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => ReflectionUtils.CreateDictionary(typeof(Dictionary<,>), null, typeof(int), 0),
        		"KeyType");
        }
        
        [Test]
        public void CreateDictonary_ValueTypeNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => ReflectionUtils.CreateDictionary(typeof(Dictionary<,>), typeof(int), null, 0),
        		"ValueType");
        }
        
        [Test]
        public void CreateDictonary_GTDInterface_Exception()
        {
        	Type collectionType = typeof(IDictionary<,>);
        	var ex = Assert.Throws<InvalidCollectionGenericTypeDefinitionException>(() => ReflectionUtils.CreateDictionary(collectionType, typeof(int), typeof(int), 0));
        	ex.AssertArguments(typeof(IDictionary).GetFriendlyName(), 2, collectionType.GetFriendlyName());
        }
        
        [Test]
        public void CreateDictonary_GTDNotGTD_Exception()
        {
        	Type collectionType = typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(int));
        	var ex = Assert.Throws<InvalidCollectionGenericTypeDefinitionException>(() => ReflectionUtils.CreateDictionary(collectionType, typeof(int), typeof(int), 0));
        	ex.AssertArguments(typeof(IDictionary).GetFriendlyName(), 2, collectionType.GetFriendlyName());
        }
        
        [Test]
        public void CreateDictonary_GTDNotImplementsIDictionary_Exception()
        {
        	Type collectionType = typeof(List<>);
        	var ex = Assert.Throws<InvalidCollectionGenericTypeDefinitionException>(() => ReflectionUtils.CreateDictionary(collectionType, typeof(int), typeof(int), 0));
        	ex.AssertArguments(typeof(IDictionary).GetFriendlyName(), 2, collectionType.GetFriendlyName());
        }
        
        [Test]
        public void CreateDictonary_GTDArgsNotEqualsTwo_Exception()
        {
        	Type collectionType = typeof(InvalidDictionary<,,>);
        	var ex = Assert.Throws<InvalidCollectionGenericTypeDefinitionException>(() => ReflectionUtils.CreateDictionary(collectionType, typeof(int), typeof(int), 0));
        	ex.AssertArguments(typeof(IDictionary).GetFriendlyName(), 2, collectionType.GetFriendlyName());
        }
        
        [Test]
        public void CreateDictonary_ParameterCountLessThanZero_ValidateMoreThanOrEquals()
        {
        	AssertValidation.MoreThanOrEquals(
        		(s) => ReflectionUtils.CreateDictionary(typeof(Dictionary<,>), typeof(int), typeof(int), s),
        		"ParametersCount", 0);
        }
        
        [Test]
        public void CreateDictonary_InvalidParameterCountOfConstructor_ConstructorNotFoundException()
        {
        	var ex = Assert.Throws<ConstructorNotFoundException>(() => ReflectionUtils.CreateDictionary(typeof(Dictionary<,>), typeof(int), typeof(int), 5));
        	ex.AssertArguments(5, typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(int)).GetFriendlyName());
        }
        
        [Test]
        public void CreateDictonaryCallback_InvalidArgsCountIfInitialCountZero_IgnoreArgs()
        {
        	Assert.DoesNotThrow(() => ReflectionUtils.CreateDictionary(typeof(Dictionary<,>), typeof(int), typeof(int), 0).Invoke(new int[]{0, 2}));
        }
        
        [Test]
        public void CreateDictonaryCallback_InvalidArgsCountIfInitialCountMoreThanZero_Exception()
        {
        	Assert.Throws<TargetParameterCountException>(() => ReflectionUtils.CreateDictionary(typeof(Dictionary<,>), typeof(int), typeof(int), 1).Invoke(new int[]{0, 2}));
        }
        
        private static void CreateDictionaryKVAndTest(ReflectionsUtilsStrategy strategy, Type dictionaryType, params int[] args)
        {
        	if (!TrySetStrategy(strategy))
            {
            	return;
            }
        	
            IFunctionCallback<int[], IEnumerable> dictionaryActivator = ReflectionUtils.CreateDictionary(dictionaryType.GetGenericTypeDefinition(), dictionaryType.GetGenericArguments()[0], dictionaryType.GetGenericArguments()[1], args.Length);
            IDictionary dictionary = dictionaryActivator.Invoke(args) as IDictionary;

            Assert.NotNull(dictionary);
            Assert.AreEqual(0, dictionary.Cast<object>().Count());
        }
	}
}