/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;
using System;

namespace AltFormatter.Utils
{
	internal sealed partial class ReflectionUtilsTests
	{
		private const string InstanceMethodValueName = "InstanceMethodValue";

        private const string StaticMethodValueName = "StaticMethodValue";

        private const string InstanceMethodName = "InstanceMethod";

        private const string StaticMethodName = "StaticMethod";
        
        private const string InvalidMethodName = "InvalidMethod";
		
		[Test, TestCaseSource("AllObjects")]
        public void CreateReturnlessOneParameterMethod_StaticMethodValueType(Type type, object value, ReflectionsUtilsStrategy strategy)
        {
        	CreateAndInvokeReturnlessOneParameterMethod(strategy, true, type, value);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateReturnlessOneParameterMethod_InstanceMethodValueType(Type type, object value, ReflectionsUtilsStrategy strategy)
        {
        	CreateAndInvokeReturnlessOneParameterMethod(strategy, false, type, value);
        }
        
        [Test]
        public void CreateReturnlessOneParameterMethod_TypeNull_ValidateIsNull()
        {
        	Type type = typeof(ClassWithReturnlessOneParameterMethods<>).MakeGenericType(typeof(int));
        	
        	AssertValidation.NotNull(
        		() => ReflectionUtils.CreateReturnlessOneParameterMethodCallback(null, type.GetMethod(StaticMethodName)),
        		"Type");
        }
        
        [Test]
        public void CreateReturnlessOneParameterMethod_MethodInfoNull_ValidateIsNull()
        {
        	Type type = typeof(ClassWithReturnlessOneParameterMethods<>).MakeGenericType(typeof(int));
        	
        	AssertValidation.NotNull(
        		() => ReflectionUtils.CreateReturnlessOneParameterMethodCallback(type, null),
        		"MethodInfo");
        }
        
        [Test, TestCaseSource("Strategies")]
        public void CreateReturnlessOneParameterMethod_InvalidMethod_Exception(ReflectionsUtilsStrategy strategy)
        {
        	Type type = typeof(ClassWithReturnlessOneParameterMethods<>).MakeGenericType(typeof(int));
        	string friendlyName = type.GetFriendlyName();
        	string voidTypeName = typeof(void).GetFriendlyName();
        	
        	var ex = Assert.Throws<InvalidMethodException>(() => ReflectionUtils.CreateReturnlessOneParameterMethodCallback(type, type.GetMethod(InvalidMethodName)));
        	ex.AssertArguments("Static", InvalidMethodName, friendlyName, 1, voidTypeName, 0, voidTypeName);
        }
		
        [Test, TestCaseSource("AllObjects")]
        public void CreateReturnParameterlessMethod_StaticMethodValueType_value(Type type, object value, ReflectionsUtilsStrategy strategy)
        {
            CreateAndInvokeReturnParameterlessMethod(strategy, true, type, value);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateReturnParameterlessMethod_StaticMethodObjectType_Value(Type type, object value, ReflectionsUtilsStrategy strategy)
        {
        	CreateAndInvokeReturnParameterlessMethod(strategy, true, typeof(object), value);
        }

        [Test, TestCaseSource("AllObjects")]
        public void CreateReturnParameterlessMethod_InstanceMethodValueType_value(Type type, object value, ReflectionsUtilsStrategy strategy)
        {
            CreateAndInvokeReturnParameterlessMethod(strategy, false, type, value);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateReturnParameterlessMethod_InstanceMethodObjectType_Value(Type type, object value, ReflectionsUtilsStrategy strategy)
        {
        	CreateAndInvokeReturnParameterlessMethod(strategy, false, typeof(object), value);
        }
        
        [Test]
        public void CreateReturnParameterlessMethod_TypeNull_ValidateIsNull()
        {
        	Type type = typeof(ClassWithReturnParameterlessMethods<>).MakeGenericType(typeof(int));
        	
        	AssertValidation.NotNull(
        		() => ReflectionUtils.CreateReturnParameterlessMethodCallback(null, type.GetMethod(StaticMethodName)),
        		"Type");
        }
        
        [Test]
        public void CreateReturnParameterlessMethod_MethodInfoNull_ValidateIsNull()
        {
        	Type type = typeof(ClassWithReturnParameterlessMethods<>).MakeGenericType(typeof(int));
        	
        	AssertValidation.NotNull(
        		() => ReflectionUtils.CreateReturnParameterlessMethodCallback(type, null),
        		"MethodInfo");
        }
        
        [Test, TestCaseSource("Strategies")]
        public void CreateReturnParameterlessMethod_InvalidMethod_Exception(ReflectionsUtilsStrategy strategy)
        {
        	Type type = typeof(ClassWithReturnParameterlessMethods<>).MakeGenericType(typeof(int));
        	
        	string friendlyName = type.GetFriendlyName();
        	string objectTypeName = typeof(object).GetFriendlyName();
        	string voidTypeName = typeof(void).GetFriendlyName();
        	
        	var ex = Assert.Throws<InvalidMethodException>(() => ReflectionUtils.CreateReturnParameterlessMethodCallback(type, type.GetMethod(InvalidMethodName)));
        	ex.AssertArguments("Static", InvalidMethodName, friendlyName, 0, objectTypeName, 1, typeof(int).GetFriendlyName());
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateReturnOneParameterMethod_StaticMethodValueType_value(Type type, object value, ReflectionsUtilsStrategy strategy)
        {
            CreateAndInvokeReturnOneParameterMethod(strategy, true, type, value);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateReturnOneParameterMethod_StaticMethodValueType_Object(Type type, object value, ReflectionsUtilsStrategy strategy)
        {
        	CreateAndInvokeReturnOneParameterMethod(strategy, true, typeof(object), value);
        }

        [Test, TestCaseSource("AllObjects")]
        public void CreateReturnOneParameterMethod_InstanceMethodValueType_value(Type type, object value, ReflectionsUtilsStrategy strategy)
        {
            CreateAndInvokeReturnOneParameterMethod(strategy, false, type, value);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateReturnOneParameterMethod_InstanceMethodValueType_Object(Type type, object value, ReflectionsUtilsStrategy strategy)
        {
        	CreateAndInvokeReturnOneParameterMethod(strategy, false, typeof(object), value);
        }
        
        [Test]
        public void CreateReturnOneParameterMethod_TypeNull_ValidateIsNull()
        {
        	Type type = typeof(ClassWithReturnOneParameterMethods<>).MakeGenericType(typeof(int));
        	
        	AssertValidation.NotNull(
        		() => ReflectionUtils.CreateReturnOneParameterMethodCallback(null, type.GetMethod(StaticMethodName)),
        		"Type");
        }
        
        [Test]
        public void CreateReturnOneParameterMethod_MethodInfoNull_ValidateIsNull()
        {
        	Type type = typeof(ClassWithReturnOneParameterMethods<>).MakeGenericType(typeof(int));
        	
        	AssertValidation.NotNull(
        		() => ReflectionUtils.CreateReturnOneParameterMethodCallback(type, null),
        		"MethodInfo");
        }
        
        [Test, TestCaseSource("Strategies")]
        public void CreateReturnOneParameterMethod_InvalidMethod_Exception(ReflectionsUtilsStrategy strategy)
        {
        	Type type = typeof(ClassWithReturnOneParameterMethods<>).MakeGenericType(typeof(int));
        	
        	string friendlyName = type.GetFriendlyName();
        	string objectTypeName = typeof(object).GetFriendlyName();
        	string voidTypeName = typeof(void).GetFriendlyName();
        	
        	var ex = Assert.Throws<InvalidMethodException>(() => ReflectionUtils.CreateReturnOneParameterMethodCallback(type, type.GetMethod(InvalidMethodName)));
        	ex.AssertArguments("Static", InvalidMethodName, friendlyName, 1, objectTypeName, 0, voidTypeName);
        }
        
        private static void CreateAndInvokeReturnlessOneParameterMethod(ReflectionsUtilsStrategy strategy, bool isStaticMethod, Type type, object value)
        {
            if (!TrySetStrategy(strategy))
            {
            	return;
            }
            
            Type classType = typeof(ClassWithReturnlessOneParameterMethods<>).MakeGenericType(type);
            
            if(isStaticMethod)
            {
            	InvokeReturnlessOneParameterMethod(classType, null, StaticMethodName, value);
            	Assert.AreEqual(value, classType.GetField(StaticMethodValueName).GetValue(null));
            }
            else
            {
            	object instance = ReflectionUtils.CreateInstance(classType).Invoke();
	            InvokeReturnlessOneParameterMethod(classType, instance, InstanceMethodName, value);
	            Assert.AreEqual(value, classType.GetField(InstanceMethodValueName).GetValue(instance));
            }
        }
        
        private static void CreateAndInvokeReturnParameterlessMethod(ReflectionsUtilsStrategy strategy, bool isStaticMethod, Type type, object value)
        {
            if (!TrySetStrategy(strategy))
            {
            	return;
            }

            Type classType = typeof(ClassWithReturnParameterlessMethods<>).MakeGenericType(type);
            
            if(isStaticMethod)
            {
            	classType.GetField(StaticMethodValueName).SetValue(null, value);
            	object staticValue = InvokeReturnParameterlessMethod(classType, null, StaticMethodName);
            	Assert.AreEqual(value, staticValue);
            }
            else
            {
            	object instance = ReflectionUtils.CreateInstance(classType).Invoke();
            	classType.GetField(InstanceMethodValueName).SetValue(instance, value);
            	object instanceValue = InvokeReturnParameterlessMethod(classType, instance, InstanceMethodName);
            	Assert.AreEqual(value, instanceValue);
            }
        }

        private static void CreateAndInvokeReturnOneParameterMethod(ReflectionsUtilsStrategy strategy, bool isStaticMethod, Type type, object value)
        {
        	if (!TrySetStrategy(strategy))
            {
            	return;
            }

            Type classType = typeof(ClassWithReturnOneParameterMethods<>).MakeGenericType(type);
            
            if(isStaticMethod)
            {
            	object staticValue = InvokeReturnOneParameterMethod(classType, null, StaticMethodName, value);
            	Assert.AreEqual(value, staticValue);
            }
            else
            {
            	object instance = ReflectionUtils.CreateInstance(classType).Invoke();
            	object instanceValue = InvokeReturnOneParameterMethod(classType, instance, InstanceMethodName, value);
            	Assert.AreEqual(value, instanceValue);
            }
        }
        
        private static void InvokeReturnlessOneParameterMethod(Type type, object obj, string name, object input)
        {
            IActionCallback<object, object> methodActivator = ReflectionUtils.CreateReturnlessOneParameterMethodCallback(type, type.GetMethod(name));
            methodActivator.Invoke(obj, input);
        }

        private static object InvokeReturnParameterlessMethod(Type type, object obj, string name)
        {
            IFunctionCallback<object, object> methodActivator = ReflectionUtils.CreateReturnParameterlessMethodCallback(type, type.GetMethod(name));
            return methodActivator.Invoke(obj);
        }

        private static object InvokeReturnOneParameterMethod(Type type, object obj, string name, object value)
        {
            IFunctionCallback<object, object, object> methodActivator = ReflectionUtils.CreateReturnOneParameterMethodCallback(type, type.GetMethod(name));
            return methodActivator.Invoke(obj, value);
        }
	}
}