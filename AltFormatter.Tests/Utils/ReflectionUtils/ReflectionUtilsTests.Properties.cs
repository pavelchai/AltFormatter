/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;
using System;
using System.Reflection;

namespace AltFormatter.Utils
{
	internal sealed partial class ReflectionUtilsTests
	{
		private const string InstancePropertyName = "InstanceProperty";

        private const string StaticPropertyName = "StaticProperty";
        
        private const string InvalidPropertyWithoutGet = "PropertyWithoutGet";

        private const string InvalidPropertyWithoutSet = "PropertyWithoutSet";
		
		[Test, TestCaseSource("AllObjects")]
        public void CreateGetAccessorProperty_StaticPropertyValueType_Value(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
            CreateAndInvokeGetAccessorProperty(strategy, true, type, obj);
        }

        [Test, TestCaseSource("AllObjects")]
        public void CreateGetAccessorProperty_StaticPropertyObjectType_Value(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
            CreateAndInvokeGetAccessorProperty(strategy, true, typeof(object), obj);
        }
        
		[Test, TestCaseSource("AllObjects")]
        public void CreateGetAccessorProperty_InstancePropertyValueType_Value(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
            CreateAndInvokeGetAccessorProperty(strategy, false, type, obj);
        }

        [Test, TestCaseSource("AllObjects")]
        public void CreateGetAccessorProperty_InstancePropertyObjectType_Value(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
            CreateAndInvokeGetAccessorProperty(strategy, false, typeof(object), obj);
        }
        
        [Test]
        public void CreateGetAccessorProperty_PropertyWithoutGetAccessor_Exception()
        {
        	Type type = typeof(ClassWithProperties<>).MakeGenericType(typeof(int));
        	var ex = Assert.Throws<GetAccessorNotFoundException>(() => GetPropertyValue(type, null, InvalidPropertyWithoutGet));
        	ex.AssertArguments(InvalidPropertyWithoutGet, type.GetFriendlyName());
        }
        
        [Test]
        public void CreateGetAccessorProperty_TypeNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => ReflectionUtils.CreateGetAccessor(null, typeof(ClassWithProperties<>).MakeGenericType(typeof(object)).GetProperty(StaticFieldName)),
        		"Type");
        }
        
        [Test]
        public void CreateGetAccessorProperty_PropertyInfoNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => ReflectionUtils.CreateGetAccessor(this.GetType(), null as PropertyInfo),
        		"PropertyInfo");
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateSetAccessorProperty_StaticPropertyValueType_Value(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
            CreateAndInvokeSetAccessorProperty(strategy, true, type, obj);
        }
        
		[Test, TestCaseSource("AllObjects")]
        public void CreateSetAccessorProperty_InstancePropertyValueType_Value(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
            CreateAndInvokeSetAccessorProperty(strategy, false, type, obj);
        }
        
        [Test]
        public void CreateSetAccessorProperty_PropertyWithoutSetAccessor_Exception()
        {
        	Type type = typeof(ClassWithProperties<>).MakeGenericType(typeof(int));
        	var ex = Assert.Throws<SetAccessorNotFoundException>(() => SetPropertyValue(type, null, InvalidPropertyWithoutSet, 10));
        	ex.AssertArguments(InvalidPropertyWithoutSet, type.GetFriendlyName());
        }
        
        [Test]
        public void CreateSetAccessorProperty_TypeNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => ReflectionUtils.CreateSetAccessor(null, typeof(ClassWithProperties<>).MakeGenericType(typeof(object)).GetProperty(StaticFieldName)),
        		"Type");
        }
        
        [Test]
        public void CreateSetAccessorProperty_PropertyInfoNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => ReflectionUtils.CreateSetAccessor(this.GetType(), null as PropertyInfo),
        		"PropertyInfo");
        }
        
        private static void CreateAndInvokeGetAccessorProperty(ReflectionsUtilsStrategy strategy, bool isStaticProperty, Type type, object obj)
        {
            if (!TrySetStrategy(strategy))
            {
            	return;
            }

            Type classType = typeof(ClassWithProperties<>).MakeGenericType(type);
            
            if(isStaticProperty)
            {
            	classType.GetProperty(StaticPropertyName).SetValue(null, obj);
            	Assert.AreEqual(obj, GetPropertyValue(classType, null, StaticPropertyName));
            }
            else
            {
            	object instance = ReflectionUtils.CreateInstance(classType).Invoke();
            	classType.GetProperty(InstancePropertyName).SetValue(instance, obj);
            	Assert.AreEqual(obj, GetPropertyValue(classType, instance, InstancePropertyName));
            }
        }
        
        private static void CreateAndInvokeSetAccessorProperty(ReflectionsUtilsStrategy strategy, bool isStaticProperty, Type type, object obj)
        {
        	if (!TrySetStrategy(strategy))
            {
            	return;
            }
        	
        	Type classType = typeof(ClassWithProperties<>).MakeGenericType(type);          
            
            if(isStaticProperty)
            {
            	SetPropertyValue(classType, null, StaticPropertyName, obj);
            	Assert.AreEqual(obj, classType.GetProperty(StaticPropertyName).GetValue(null));
            }
            else
            {
            	object instance = ReflectionUtils.CreateInstance(classType).Invoke();
            	SetPropertyValue(classType, instance, InstancePropertyName, obj);
            	Assert.AreEqual(obj, classType.GetProperty(InstancePropertyName).GetValue(instance));
            }
        }
        
        private static object GetPropertyValue(Type type, object obj, string name)
        {
            IFunctionCallback<object, object> getAccessor = ReflectionUtils.CreateGetAccessor(type, type.GetProperty(name));
            return getAccessor.Invoke(obj);
        }

        private static void SetPropertyValue(Type type, object obj, string name, object input)
        {
            IActionCallback<object, object> setAccessor = ReflectionUtils.CreateSetAccessor(type, type.GetProperty(name));
            setAccessor.Invoke(obj, input);
        }
	}
}