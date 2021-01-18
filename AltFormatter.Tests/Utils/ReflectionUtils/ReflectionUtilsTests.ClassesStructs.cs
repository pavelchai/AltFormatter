/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace AltFormatter.Utils
{
	internal sealed partial class ReflectionUtilsTests
	{
		// Interfaces/classes for the valid/invalid types
		
		private interface IReflectionUtilsType
	    {
	        bool CreatedFromFactory { get; }
	    }
		
		private abstract class AbstractBaseClass : IReflectionUtilsType
	    {
	        public bool CreatedFromFactory { get; protected set; }
	    }
		
		// Valid types (can be created by ReflectionUtils.CreateInstance method)
		
	    private struct StructWithParameterlessConstructor : IReflectionUtilsType
	    {
	        public bool CreatedFromFactory
	        { 
	        	get
	        	{
	        		return false;
	        	}
	        }
	    }
	
	    private struct StructWithFactory : IReflectionUtilsType
	    {
	        public bool CreatedFromFactory { get; private set; }
	
	        [Factory]
	        public static StructWithFactory Create()
	        {
	        	var instance = new StructWithFactory();
	        	instance.CreatedFromFactory = true;
	        	return instance;
	        }
	    }
	    
	    private sealed class ClassWithParameterlessConstructor : AbstractBaseClass
	    {
	    }
	
	    private sealed class ClassWithFactoryBaseType : AbstractBaseClass
	    {
	        [Factory]
	        public static ClassWithFactoryBaseType Create()
	        {
	            var instance = new ClassWithFactoryBaseType();
	            instance.CreatedFromFactory = true;
	            return instance;
	        }
	    }
	    
	    private class ClassWithFactoryDerivedType : AbstractBaseClass
	    {
	        [Factory]
	        public static ClassWithFactoryDerivedTypeDerived Create()
	        {
	            var instance = new ClassWithFactoryDerivedTypeDerived();
	            instance.CreatedFromFactory = true;
	            return instance;
	        }
	    }
	    
	    private sealed class ClassWithFactoryDerivedTypeDerived : ClassWithFactoryDerivedType
	    {
	    }
	    
	    // Invalid types (can't be created by ReflectionUtils.CreateInstance method)
	    
	    private struct StructWithInvalidFactoryNoParameterless : IReflectionUtilsType
	    {
	        public bool CreatedFromFactory { get; private set; }
	
	        [Factory]
	        public static StructWithInvalidFactoryNoParameterless Create(int v)
	        {
	        	var instance = new StructWithInvalidFactoryNoParameterless();
	        	instance.CreatedFromFactory = true;
	        	return instance;
	        }
	    }
	    
	    private struct StructWithInvalidFactoryInvalidReturnType : IReflectionUtilsType
	    {
	        public bool CreatedFromFactory { get; private set; }
	
	        [Factory]
	        public static object Create()
	        {
	        	var instance = new StructWithInvalidFactoryInvalidReturnType();
	        	instance.CreatedFromFactory = true;
	        	return instance;
	        }
	    }
	    
	    private struct StructWithInvalidFactoryMoreThanOneFactory : IReflectionUtilsType
	    {
	        public bool CreatedFromFactory { get; private set; }
	
	        [Factory]
	        public static StructWithInvalidFactoryMoreThanOneFactory Create1()
	        {
	        	var instance = new StructWithInvalidFactoryMoreThanOneFactory();
	        	instance.CreatedFromFactory = true;
	        	return instance;
	        }
	        
	        [Factory]
	        public static StructWithInvalidFactoryMoreThanOneFactory Create2()
	        {
	        	var instance = new StructWithInvalidFactoryMoreThanOneFactory();
	        	instance.CreatedFromFactory = true;
	        	return instance;
	        }
	    }
	    
	    private sealed class ClassWithoutFactoryOrParameterlessConstructor : AbstractBaseClass
	    {
	    	public ClassWithoutFactoryOrParameterlessConstructor(int value)
	    	{	
	    	}
	    }
	    
	    private sealed class ClassWithInvalidFactoryNoParameterless  : AbstractBaseClass
	    {
	    	[Factory]
	        public static ClassWithInvalidFactoryNoParameterless Create(int v)
	        {
	        	var instance = new ClassWithInvalidFactoryNoParameterless();
	        	instance.CreatedFromFactory = true;
	        	return instance;
	        }
	    }
	    
	    private sealed class ClassWithInvalidFactoryInvalidReturnType  : AbstractBaseClass
	    {
	    	[Factory]
	        public static object Create()
	        {
	        	var instance = new ClassWithInvalidFactoryInvalidReturnType();
	        	instance.CreatedFromFactory = true;
	        	return instance;
	        }
	    }
	    
	    private sealed class ClassWithInvalidFactoryMoreThanOneFactory  : AbstractBaseClass
	    {
	    	[Factory]
	        public static ClassWithInvalidFactoryMoreThanOneFactory Create1()
	        {
	        	var instance = new ClassWithInvalidFactoryMoreThanOneFactory();
	        	instance.CreatedFromFactory = true;
	        	return instance;
	        }
	        
	        [Factory]
	        public static ClassWithInvalidFactoryMoreThanOneFactory Create2()
	        {
	        	var instance = new ClassWithInvalidFactoryMoreThanOneFactory();
	        	instance.CreatedFromFactory = true;
	        	return instance;
	        }
	    }
	    
	    // Fields
	    
	    private sealed class ClassWithFields<T>
	    {
	    	#pragma warning disable 0649
	    	
	        public T InstanceField;
	
	        public static T StaticField;
	        
	        #pragma warning restore 0649
	    }
	    
	    // Properties
	    
	    private sealed class ClassWithProperties<T>
	    {
	        public T InstanceProperty { get; set; }
	        
	        public static T StaticProperty { get; set; }
	        
	        public static T PropertyWithoutGet
	        {
	        	set
	        	{
	        	}
	        }
	        
	        public static T PropertyWithoutSet
	        {
	        	get
	        	{
	        		return default(T);
	        	}
	        }
	    }
	    
	    // Methods
	
	    private sealed class ClassWithReturnlessOneParameterMethods<T>
	    {
	        public T InstanceMethodValue = default(T);
	
	        public static T StaticMethodValue = default(T);
	
	        public void InstanceMethod(T value)
	        {
	            this.InstanceMethodValue = value;
	        }
	
	        public static void StaticMethod(T value)
	        {
	            StaticMethodValue = value;
	        }
	        
	        public static void InvalidMethod()
	        {
	        }
	    }
	
	    private sealed class ClassWithReturnParameterlessMethods<T>
	    {
	        public T InstanceMethodValue = default(T);
	
	        public static T StaticMethodValue = default(T);
	
	        public T InstanceMethod()
	        {
	            return this.InstanceMethodValue;
	        }
	
	        public static T StaticMethod()
	        {
	            return StaticMethodValue;
	        }
	        
	        public static T InvalidMethod(int value)
	        {	
	        	return default(T);
	        }
	    }
	
	    private sealed class ClassWithReturnOneParameterMethods<T>
	    {
	        public T InstanceMethod(T value)
	        {
	            return value;
	        }
	
	        public static T StaticMethod(T value)
	        {
	            return value;
	        }
	        
	        public static void InvalidMethod()
	        {
	        }
	    }
	    
	    // Other 
	    
	    private sealed class InvalidList<T1, T2> : List<T1>
        {
        }
	    
	    private sealed class InvalidDictionary<TK, TV1, TV2> : Dictionary<TK, TV1>
        {
        }
	}
}