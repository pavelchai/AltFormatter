/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AltFormatter.Utils
{
	internal sealed partial class ReflectionUtilsTests
	{
		[Test, TestCaseSource("AllObjects")]
        public void CreateEnumerable_AllTypesEnumerableNoArgs_Collection(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateEnumerableTAndTest(strategy, typeof(LinkedList<>).MakeGenericType(type));
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateEnumerable_AllTypesEnumerableOneArg_Collection(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateEnumerableTAndTest(strategy, typeof(List<>).MakeGenericType(type), 5);
        }
        
        [Test]
        public void CreateEnumerable_EnumerableGTDNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => ReflectionUtils.CreateEnumerable(null, typeof(int), 0),
        		"EnumerableGTD");
        }
        
        [Test]
        public void CreateEnumerable_ElementTypeNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => ReflectionUtils.CreateEnumerable(typeof(List<>), null, 0),
        		"ElementType");
        }
        
        [Test]
        public void CreateEnumerable_ParameterCountLessThanZero_ValidateMoreThanOrEquals()
        {
        	AssertValidation.MoreThanOrEquals(
        		(s) => ReflectionUtils.CreateEnumerable(typeof(List<>), typeof(int), s),
        		"ParametersCount", 0);
        }
        
        [Test]
        public void CreateEnumerable_GTDInterface_Exception()
        {
        	Type collectionType = typeof(IList<>);
        	var ex = Assert.Throws<InvalidCollectionGenericTypeDefinitionException>(() => ReflectionUtils.CreateEnumerable(collectionType, typeof(int), 0));
        	ex.AssertArguments(typeof(IEnumerable).GetFriendlyName(), 1, collectionType.GetFriendlyName());
        }
        
        [Test]
        public void CreateEnumerable_GTDNotGTD_Exception()
        {
        	Type collectionType = typeof(List<>).MakeGenericType(typeof(int));
        	var ex = Assert.Throws<InvalidCollectionGenericTypeDefinitionException>(() => ReflectionUtils.CreateEnumerable(collectionType, typeof(int), 0));
        	ex.AssertArguments(typeof(IEnumerable).GetFriendlyName(), 1, collectionType.GetFriendlyName());
        }
        
        [Test]
        public void CreateEnumerable_GTDNotImplementsIEnumerable_Exception()
        {
        	Type collectionType = typeof(ClassWithReturnParameterlessMethods<>);
        	var ex = Assert.Throws<InvalidCollectionGenericTypeDefinitionException>(() => ReflectionUtils.CreateEnumerable(collectionType, typeof(int), 0));
        	ex.AssertArguments(typeof(IEnumerable).GetFriendlyName(), 1, collectionType.GetFriendlyName());
        }
        
        [Test]
        public void CreateEnumerable_GTDArgsCountNotEqualsOne_Exception()
        {
        	Type collectionType = typeof(InvalidList<,>);
        	var ex = Assert.Throws<InvalidCollectionGenericTypeDefinitionException>(() => ReflectionUtils.CreateEnumerable(collectionType, typeof(int), 0));
        	ex.AssertArguments(typeof(IEnumerable).GetFriendlyName(), 1, collectionType.GetFriendlyName());
        }
        
        [Test]
        public void CreateEnumerable_InvalidParameterCountOfConstructor_ConstructorNotFoundException()
        {
        	var ex = Assert.Throws<ConstructorNotFoundException>(() => ReflectionUtils.CreateEnumerable(typeof(List<>), typeof(int), 15));
        	ex.AssertArguments(15, typeof(List<>).MakeGenericType(typeof(int)).GetFriendlyName());
        }
        
        [Test]
        public void CreateEnumerableCallback_InvalidArgsCountIfInitialCountZero_IgnoreArgs()
        {
        	Assert.DoesNotThrow(() => ReflectionUtils.CreateEnumerable(typeof(List<>), typeof(int), 0).Invoke(new int[]{0, 2}));
        }
        
        [Test]
        public void CreateEnumerableCallback_InvalidArgsCountIfInitialCountMoreThanZero_Exception()
        {
        	Assert.Throws<TargetParameterCountException>(() => ReflectionUtils.CreateEnumerable(typeof(List<>), typeof(int), 1).Invoke(new int[]{0, 2}));
        }
        
        private static void CreateEnumerableTAndTest(ReflectionsUtilsStrategy strategy, Type collectionType, params int[] args)
        {
        	if (!TrySetStrategy(strategy))
            {
            	return;
            }
        	
        	IFunctionCallback<int[], IEnumerable> enumerableActivator = ReflectionUtils.CreateEnumerable(collectionType.GetGenericTypeDefinition(), collectionType.GetGenericArguments()[0], args.Length);
        	IEnumerable enumerable = enumerableActivator.Invoke(args) as IEnumerable;
            
            Assert.NotNull(enumerable);
            Assert.AreEqual(0, enumerable.Cast<object>().Count());
        }
	}
}