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
		private readonly static BindingFlags factoryMethodBindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
		
		[Test, TestCaseSource("NotPrimitiveObjects")]
        public void CreateInstance_NotPrimitiveType_Instance(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateInstanceAndTest(strategy, type);
        }
        
        [Test, TestCaseSource("Strategies")]
        public void CreateInstance_StructWithInvalidFactoryNoParameterless_Exception(ReflectionsUtilsStrategy strategy)
        {
        	Type type = typeof(StructWithInvalidFactoryNoParameterless);
        	string friendlyName = type.GetFriendlyName();
        	
        	var ex = Assert.Throws<InvalidMethodException>(() => CreateInstanceAndTest(strategy, type));
        	ex.AssertArguments("Static", "Create", friendlyName, 0, friendlyName, 1, friendlyName);
        }
        
        [Test, TestCaseSource("Strategies")]
        public void CreateInstance_StructWithInvalidFactoryInvalidReturnType_Exception(ReflectionsUtilsStrategy strategy)
        {
        	Type type = typeof(StructWithInvalidFactoryInvalidReturnType);
        	string friendlyName = type.GetFriendlyName();
        	
        	var ex = Assert.Throws<InvalidMethodException>(() => CreateInstanceAndTest(strategy, type));
        	ex.AssertArguments("Static", "Create", friendlyName, 0, friendlyName, 0, typeof(object).GetFriendlyName());
        }
        
        [Test, TestCaseSource("Strategies")]
        public void CreateInstance_StructWithInvalidFactoryMoreThanOneFactory_Exception(ReflectionsUtilsStrategy strategy)
        {
        	Type type = typeof(StructWithInvalidFactoryMoreThanOneFactory);
        	string friendlyName = type.GetFriendlyName();
        	
        	var ex = Assert.Throws<DuplicateFactoryMethodException>(() => CreateInstanceAndTest(strategy, type));
        	ex.AssertArguments("Create2", friendlyName);
        }
        
        [Test, TestCaseSource("Strategies")]
        public void CreateInstance_ClassWithoutFactoryOrParameterlessConstructor_Exception(ReflectionsUtilsStrategy strategy)
        {
        	Type type = typeof(ClassWithoutFactoryOrParameterlessConstructor);
        	string friendlyName = type.GetFriendlyName();
        	
        	var ex = Assert.Throws<ConstructorNotFoundException>(() => CreateInstanceAndTest(strategy, type));
        	ex.AssertArguments(0, friendlyName);
        }
        
        [Test, TestCaseSource("Strategies")]
        public void CreateInstance_ClassWithInvalidFactoryNoParameterless_Exception(ReflectionsUtilsStrategy strategy)
        {
        	Type type = typeof(ClassWithInvalidFactoryNoParameterless);
        	string friendlyName = type.GetFriendlyName();
        	
        	var ex = Assert.Throws<InvalidMethodException>(() => CreateInstanceAndTest(strategy, type));
        	ex.AssertArguments("Static", "Create", friendlyName, 0, friendlyName, 1, friendlyName);
        }
        
        [Test, TestCaseSource("Strategies")]
        public void CreateInstance_ClassWithInvalidFactoryInvalidReturnType_Exception(ReflectionsUtilsStrategy strategy)
        {
        	Type type = typeof(ClassWithInvalidFactoryInvalidReturnType);
        	string friendlyName = type.GetFriendlyName();
        	
        	var ex = Assert.Throws<InvalidMethodException>(() => CreateInstanceAndTest(strategy, type));
        	ex.AssertArguments("Static", "Create", friendlyName, 0, friendlyName, 0, typeof(object).GetFriendlyName());
        }
        
        [Test, TestCaseSource("Strategies")]
        public void CreateInstance_ClassWithInvalidFactoryMoreThanOneFactory_Exception(ReflectionsUtilsStrategy strategy)
        {
        	Type type = typeof(ClassWithInvalidFactoryMoreThanOneFactory);
        	string friendlyName = type.GetFriendlyName();
        	
        	var ex = Assert.Throws<DuplicateFactoryMethodException>(() => CreateInstanceAndTest(strategy, type));
        	ex.AssertArguments("Create2", friendlyName);
        }
        
        private static void CreateInstanceAndTest(ReflectionsUtilsStrategy strategy, Type type)
        {
        	if (!TrySetStrategy(strategy))
            {
            	return;
        	}
        	
            IFunctionCallback<object> instanceActivator = ReflectionUtils.CreateInstance(type);
            IReflectionUtilsType instance = instanceActivator.Invoke() as IReflectionUtilsType;

            Assert.NotNull(instance);

            if (type.GetMethods(factoryMethodBindingFlags).FindOrDefault(m => m.GetAttribute<FactoryAttribute>(false) != null) != null)
            {
                Assert.True(instance.CreatedFromFactory);
            }
            else
            {
                Assert.False(instance.CreatedFromFactory);
            }
        }
	}
}