/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;

namespace AltFormatter.Utils
{
	internal sealed partial class ReflectionUtilsTests
	{
		private const string InstanceFieldName = "InstanceField";

        private const string StaticFieldName = "StaticField";
		
        [Test, TestCaseSource("AllObjects")]
        public void CreateGetAccessorField_StaticFieldValueType_Value(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
            CreateAndInvokeGetAccessorField(strategy, true, type, obj);
        }

        [Test, TestCaseSource("AllObjects")]
        public void CreateGetAccessorField_StaticFieldObjectType_Value(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
            CreateAndInvokeGetAccessorField(strategy, true, typeof(object), obj);
        }
        
		[Test, TestCaseSource("AllObjects")]
        public void CreateGetAccessorField_InstanceFieldValueType_Value(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
            CreateAndInvokeGetAccessorField(strategy, false, type, obj);
        }

        [Test, TestCaseSource("AllObjects")]
        public void CreateGetAccessorField_InstanceFieldObjectType_Value(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
            CreateAndInvokeGetAccessorField(strategy, false, typeof(object), obj);
        }
        
        [Test]
        public void CreateGetAccessorField_TypeNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => ReflectionUtils.CreateGetAccessor(null, typeof(ClassWithFields<>).MakeGenericType(typeof(object)).GetField(StaticFieldName)),
        		"Type");
        }
        
        [Test]
        public void CreateGetAccessorField_FieldInfoNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => ReflectionUtils.CreateGetAccessor(this.GetType(), null as FieldInfo),
        		"FieldInfo");
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateSetAccessorField_StaticFieldValueType_Value(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
            CreateAndInvokeSetAccessorField(strategy, true, type, obj);
        }
        
		[Test, TestCaseSource("AllObjects")]
        public void CreateSetAccessorField_InstanceFieldValueType_Value(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
            CreateAndInvokeSetAccessorField(strategy, false, type, obj);
        }
        
        [Test]
        public void CreateSetAccessorField_TypeNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => ReflectionUtils.CreateSetAccessor(null, typeof(ClassWithFields<>).MakeGenericType(typeof(object)).GetField(StaticFieldName)),
        		"Type");
        }
        
        [Test]
        public void CreateSetAccessorField_FieldInfoNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => ReflectionUtils.CreateSetAccessor(this.GetType(), null as FieldInfo),
        		"FieldInfo");
        }
        
        private static void CreateAndInvokeGetAccessorField(ReflectionsUtilsStrategy strategy, bool isStaticField, Type type, object obj)
        {
            if (!TrySetStrategy(strategy))
            {
            	return;
            }

            Type classType = typeof(ClassWithFields<>).MakeGenericType(type);
            
            if(isStaticField)
            {
            	classType.GetField(StaticFieldName).SetValue(null, obj);
            	Assert.AreEqual(obj, GetFieldValue(classType, null, StaticFieldName));
            }
            else
            {
            	object instance = ReflectionUtils.CreateInstance(classType).Invoke();
            	classType.GetField(InstanceFieldName).SetValue(instance, obj);
            	Assert.AreEqual(obj, GetFieldValue(classType, instance, InstanceFieldName));
            }
        }
        
        private static void CreateAndInvokeSetAccessorField(ReflectionsUtilsStrategy strategy, bool isStaticField, Type type, object obj)
        {
            if (!TrySetStrategy(strategy))
            {
            	return;
            }
            
            Type classType = typeof(ClassWithFields<>).MakeGenericType(type);          
            
            if(isStaticField)
            {
            	SetFieldValue(classType, null, StaticFieldName, obj);
            	Assert.AreEqual(obj, classType.GetField(StaticFieldName).GetValue(null));
            }
            else
            {
            	object instance = ReflectionUtils.CreateInstance(classType).Invoke();
            	SetFieldValue(classType, instance, InstanceFieldName, obj);
            	Assert.AreEqual(obj, classType.GetField(InstanceFieldName).GetValue(instance));
            }
        }
        
        private static object GetFieldValue(Type type, object obj, string name)
        {
            IFunctionCallback<object, object> getAccessor = ReflectionUtils.CreateGetAccessor(type, type.GetField(name));
            return getAccessor.Invoke(obj);
        }

        private static void SetFieldValue(Type type, object obj, string name, object input)
        {
            IActionCallback<object, object> setAccessor = ReflectionUtils.CreateSetAccessor(type, type.GetField(name));
            setAccessor.Invoke(obj, input);
        }
	}
}