/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AltFormatter.Utils
{
    internal sealed partial class ReflectionUtilsTests
    {
    	private static readonly object[] Strategies = new object[]
        {
        	ReflectionsUtilsStrategy.Auto,
        	ReflectionsUtilsStrategy.UseOnlyDynamicMethods,
        	ReflectionsUtilsStrategy.UseOnlyReflection,
        };
    	
        private static readonly IEnumerable<object> NotPrimitiveTypes = new[]
        {
        	typeof(ClassWithParameterlessConstructor),
        	typeof(ClassWithFactoryBaseType),
        	typeof(ClassWithFactoryDerivedType),
        	typeof(StructWithParameterlessConstructor),
        	typeof(StructWithFactory)
        };

        private static readonly IEnumerable<object[]> PrimitiveObjects = 
        	DataSources.PrimitiveTypeValueSource.
        	Cast<object[]>().
        	SelectMany(
        		s => new object[][]
        		{ 
        			new object[]
        			{
        				s[0],
        				s[1],
        				ReflectionsUtilsStrategy.Auto
        			},
        			new object[] 
        			{ 
        				s[0],
        				s[1],
        				ReflectionsUtilsStrategy.UseOnlyDynamicMethods
        			},
        			new object[]
        			{
        				s[0],
        				s[1],
        				ReflectionsUtilsStrategy.UseOnlyReflection
        			}
        		});

        private static readonly IEnumerable<object[]> NotPrimitiveObjects = 
        	NotPrimitiveTypes.SelectMany(
        		s =>
        		new object[][]
        		{
        			new object[]
        			{
        				s,
        				ReflectionUtils.CreateInstance(s as Type).Invoke(),
        				ReflectionsUtilsStrategy.Auto
        			},
        			new object[]
        			{
        				s,
        				ReflectionUtils.CreateInstance(s as Type).Invoke(),
        				ReflectionsUtilsStrategy.UseOnlyDynamicMethods },
        			new object[]
        			{
        				s,
        				ReflectionUtils.CreateInstance(s as Type).Invoke(),
        				ReflectionsUtilsStrategy.UseOnlyReflection
        			}
        		});

        private static readonly IEnumerable<object[]> AllObjects = 
        	NotPrimitiveObjects.Concat(PrimitiveObjects);
        
        private static bool TrySetStrategy(ReflectionsUtilsStrategy strategy)
        {
        	if (!ReflectionUtils.SetStrategy(strategy))
            {
                Assert.Ignore("{0} strategy not supported on the target environment.", strategy);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}