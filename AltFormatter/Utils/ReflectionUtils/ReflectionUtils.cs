/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace AltFormatter.Utils
{
    /// <summary>
    /// Represents the utils for the reflection.
    /// </summary>
    /// <remarks> 
    /// Allows create callbacks that may be used for creating <see cref="ICollection"></see>s, <see cref="Array"></see>s, instances, etc.
    /// </remarks>
    public static class ReflectionUtils
    {
        /// <summary>
        /// Binding flags for the instances.
        /// </summary>
        private readonly static BindingFlags instanceBindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

        /// <summary>
        /// Binding flags for the static methods.
        /// </summary>
        private readonly static BindingFlags staticMethodBindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;

        /// <summary>
        /// Type of the <see cref="void"></see>.
        /// </summary>
        private readonly static Type voidType = typeof(void);

        /// <summary>
        /// Type of the <see cref="int"></see>.
        /// </summary>
        private readonly static Type intType = typeof(int);

        /// <summary>
        /// Type of the <see cref="object"></see>.
        /// </summary>
        private readonly static Type objectType = typeof(object);
        
        /// <summary>
        /// Type of the <see cref="IEnumerable"></see>.
        /// </summary>
        private readonly static Type iEnumerableType = typeof(IEnumerable);
        
        /// <summary>
        /// Type of the <see cref="IDictionary"></see>.
        /// </summary>
        private readonly static Type iDictionaryType = typeof(IDictionary);

        /// <summary>
        /// Type of the <see cref="Action{object}"></see>.
        /// </summary>
        private readonly static Type actionObjectType = typeof(Action<object>);

        /// <summary>
        /// Type of the <see cref="Action{object, object}"></see>.
        /// </summary>
        private readonly static Type actionObjectObjectType = typeof(Action<object, object>);

        /// <summary>
        /// Type of the <see cref="Func{object}"></see>.
        /// </summary>
        private readonly static Type funcObjectType = typeof(Func<object>);

        /// <summary>
        /// Type of the <see cref="Func{object, object}"></see>.
        /// </summary>
        private readonly static Type funcObjectObjectType = typeof(Func<object, object>);

        /// <summary>
        /// Type of the <see cref="Func{object, object, object}"></see>.
        /// </summary>
        private readonly static Type funcObjectObjectObjectType = typeof(Func<object, object, object>);

        /// <summary>
        /// Types of the arguments for a empty constructor.
        /// </summary>
        private readonly static Type[] argsEmpty = new Type[0];

        /// <summary>
        /// Types of the arguments for a constructor (int[]).
        /// </summary>
        private readonly static Type[] argsIntArray = { typeof(int[]) };

        /// <summary>
        /// Types of the arguments for a constructor (object).
        /// </summary>
        private readonly static Type[] argsObject = { objectType };

        /// <summary>
        /// Types of the arguments for a constructor (object, object).
        /// </summary>
        private readonly static Type[] argsObjectObject = { objectType, objectType };

        /// <summary>
        /// Strategy for the <see cref="ReflectionUtils"></see>.
        /// </summary>
        private static ReflectionsUtilsStrategy strategy;

        /// <summary>
        /// Determines whether code generation is supported.
        /// </summary>
        private static bool isCodeGenerationIsSupported;

        /// <summary>
        /// Initializes the <see cref="ReflectionUtils"></see>.
        /// </summary>
        static ReflectionUtils()
        {
            // Determines whether code generation is supported or not.
            try
            {
                DynamicMethod method = new DynamicMethod(string.Empty, objectType, Type.EmptyTypes, true);
                // Code generation is supported (dynamic methods may be used)
                isCodeGenerationIsSupported = true;
                strategy = ReflectionsUtilsStrategy.Auto;
            }
            catch
            {
                // Code generation isn't supported (reflection should be used)
                isCodeGenerationIsSupported = false;
                strategy = ReflectionsUtilsStrategy.UseOnlyReflection;
            }
        }

        /// <summary>
        /// Sets a new strategy <see cref="ReflectionUtils"></see>.
        /// </summary>
        /// <param name="newStrategy"> New strategy. </param>
        /// <returns> True if strategy has been changed, otherwise false. </returns>
        /// <remarks>
        /// Should be raised only for testing purposes only.
        /// The better strategy for the target environment will be set automatically.
        /// </remarks>
        internal static bool SetStrategy(ReflectionsUtilsStrategy newStrategy)
        {
        	if(isCodeGenerationIsSupported)
        	{
        		strategy = newStrategy;
        		return true;
        	}
        	else
        	{
        		return false;
        	}
        }

        /// <summary>
        /// Creates the callback that creates the instance of the struct or class of the specified type.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <returns> Callback that creates instance of the type. </returns>
        /// <remarks>
        /// Method finds a static method that is marked with the <see cref="FactoryAttribute"></see> and uses it for creating the instance of the type.
        /// If the static method isn't exist in type, tries create instance of the type by empty constructor.
        /// </remarks>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when type is null.
        /// </exception>
        /// <exception cref="DuplicateFactoryMethodException">
        /// The exception that is thrown if factory methods more than one.
        /// </exception>
        /// <exception cref="InvalidMethodException">
        /// The exception that is thrown if factory method is invalid.
        /// </exception>
        /// <exception cref="ConstructorNotFoundException">
        /// The exception that is thrown when constructor with expected count of the parameters hasn't been found.
        /// </exception>
        public static IFunctionCallback<object> CreateInstance(Type type)
        {
        	Validation.NotNull("Type", type);
        	
        	// Function that selects the output callback (name, DM activator, reflection callback)
            Func<string, Func<Func<object>>, Func<object>, IFunctionCallback<object>> selectCallback = (n, dma, rc) =>
            {
            	switch (strategy)
            	{
            		case ReflectionsUtilsStrategy.Auto:
            			return new FunctionCallback<object>(n, dma, rc);
            		case ReflectionsUtilsStrategy.UseOnlyDynamicMethods:
            			return new FunctionCallback<object>(n, dma);
            		default:
            			return new FunctionCallback<object>(n, rc);
            	}
            };
        	
            // Tries to find the factory method
            
            MethodInfo factoryMethod = null;
            MethodInfo staticMethod = null;
            MethodInfo[] staticMethods = type.GetMethods(staticMethodBindingFlags);
            int length = staticMethods.Length;
            
            for (int i = 0; i < length; i++)
            {
                staticMethod = staticMethods[i];

                if (staticMethod.GetAttribute<FactoryAttribute>(true) != null)
                {
                	if (staticMethod.GetParameters().Length != 0 || !type.IsBaseTypeOrEquals(staticMethod.ReturnType))
                    {
                    	throw new InvalidMethodException(staticMethod, type, 0, type);
                    }
                	else
                	{
                		if (factoryMethod == null)
	                    {
	                        factoryMethod = staticMethod;
	                    }
	                    else
	                    {
	                        throw new DuplicateFactoryMethodException(staticMethod, type);
	                    }
                	}
                }
            }

            if (factoryMethod != null)
            {
                // Creates a new callback for the building a new instance from the factory method

                // Creates a name of the callback
                string name = StringUtils.Combine('+', type.GetFriendlyName(), factoryMethod.Name);

                // DM activator
                Func<Func<object>> dmActivator = () =>
                {
                    DynamicMethod dynamicMethod = new DynamicMethod(
                        StringUtils.Combine("INVOKE_FACTORY_METHOD_", name.ToUpper()),
                        objectType,
                        null,
                        factoryMethod.Module,
                        true);
                	
                    ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
                    ilGenerator.Emit(OpCodes.Call, factoryMethod);
                    if (factoryMethod.ReturnType.IsValueType)
                    {
                        ilGenerator.Emit(OpCodes.Box, factoryMethod.ReturnType);
                    }
                    ilGenerator.Emit(OpCodes.Ret);
                    
                    return (Func<object>)dynamicMethod.CreateDelegate(funcObjectType);
                };

                // Reflection callback
                Func<object> reflectionCallback = () => factoryMethod.Invoke(null, null);

                // Selects the callback that associated with the specified strategy
                return selectCallback(name, dmActivator, reflectionCallback);
            }

            if (type.IsValueType)
            {
                // Creates a new callback for the building a new value type

                // Creates a new name of the callback
                string name = type.GetFriendlyName();
                
                // Value type always has parameterless constructor - it can be created without testing

                // DM activator
                Func<Func<object>> dmActivator = () =>
                {
                    DynamicMethod dynamicMethod = new DynamicMethod(
                        StringUtils.Combine("CREATE_INSTANCE_OF_", name.ToUpper()),
                        objectType,
                        null,
                        type,
                        true);

                    ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
                    LocalBuilder localBuilder = ilGenerator.DeclareLocal(type);
                    ilGenerator.Emit(OpCodes.Ldloca_S, localBuilder);
                    ilGenerator.Emit(OpCodes.Initobj, type);
                    ilGenerator.Emit(OpCodes.Ldloc, localBuilder);
                    ilGenerator.Emit(OpCodes.Box, type);
                    ilGenerator.Emit(OpCodes.Ret);
                    
                    return (Func<object>)dynamicMethod.CreateDelegate(funcObjectType);
                };

                // Reflection callback
                // Creating with the Activator.CreateInstance usually has better performance than ConstructorInfo.Invoke for the parameterless constructors
                Func<object> reflectionCallback = () => Activator.CreateInstance(type);

                // Selects the callback that associated with the specified strategy
                return selectCallback(name, dmActivator, reflectionCallback);
            }
            else
            {
                // Creates a new callback for the building a new reference type

                // Creates a new name of the callback
                string name = type.GetFriendlyName();

                // Tests that instance of the specidfied type has parameterless constructor
                ConstructorInfo constructorInfo = type.GetConstructor(instanceBindingFlags, null, argsEmpty, null);
                if (constructorInfo == null)
                {
                	throw new ConstructorNotFoundException(type, 0);
                }

                // DM activator
                Func<Func<object>> dmActivator = () =>
                {
                    DynamicMethod dynamicMethod = new DynamicMethod(
                        StringUtils.Combine("CREATE_INSTANCE_OF_", name.ToUpper()),
                        objectType,
                        argsEmpty,
                        type,
                        true);

                    ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
                    ilGenerator.Emit(OpCodes.Newobj, constructorInfo);
                    ilGenerator.Emit(OpCodes.Ret);
                    
                    return (Func<object>)dynamicMethod.CreateDelegate(funcObjectType);
                };

                // Reflection callback
                // Creating with the Activator.CreateInstance usually has better performance than ConstructorInfo.Invoke for the parameterless constructors
                Func<object> reflectionCallback = () => Activator.CreateInstance(type, true);

                // Selects the callback that associated with the specified strategy
                return selectCallback(name, dmActivator, reflectionCallback);
            }
        }

        /// <summary>
        /// Creates the callback that creates the <see cref="IEnumerable"></see>.
        /// </summary>
        /// <param name="enumerableType"> Generic type definition of the <see cref="IEnumerable"></see>. </param>
        /// <param name="elementType"> Type of the element in the <see cref="IEnumerable"></see>. </param>
        /// <param name="parametersCount">  Expected count of the parameters. </param>
        /// <returns> Callback that creates the <see cref="IEnumerable"></see>. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when enumerable type of type of the elements are null.
        /// </exception>
        /// <exception cref="Localization.ValueLessThanException">
        /// The exception that is thrown when count of the parameters is less than zero.
        /// </exception>
        /// <exception cref="InvalidCollectionGenericTypeDefinitionException">
        /// The exception that is thrown when generic type definition of the enumerable is invalid.
        /// </exception>
        /// <exception cref="ConstructorNotFoundException">
        /// The exception that is thrown when constructor for the collection hasn't been found. 
        /// </exception>
        public static IFunctionCallback<int[], IEnumerable> CreateEnumerable(Type enumerableType, Type elementType, int parametersCount)
        {
        	Validation.NotNull("EnumerableGTD", enumerableType);
        	Validation.NotNull("ElementType", elementType);
        	Validation.MoreThanOrEquals("ParametersCount", parametersCount, 0);
        	
        	if(!enumerableType.IsClass || !enumerableType.IsGenericTypeDefinition || !iEnumerableType.IsBaseInterfaceOrEquals(enumerableType) || enumerableType.GetGenericArguments().Length != 1)
        	{
        		throw new InvalidCollectionGenericTypeDefinitionException(enumerableType, 1, iEnumerableType);
        	}
        	
            Type genericType = enumerableType.MakeGenericType(elementType);
            return CreateCollection<IEnumerable>(genericType, parametersCount);
        }

        /// <summary>
        /// Creates the callback that creates the <see cref="IDictionary"></see>.
        /// </summary>
        /// <param name="dictionaryType"> Generic type definition of the <see cref="IDictionary"></see>. </param>
        /// <param name="keyType"> Type of keys element in the <see cref="IDictionary"></see>. </param>
        /// <param name="valueType"> Type of values in the <see cref="IDictionary"></see>. </param>
        /// <param name="parametersCount">  Expected count of the parameters. </param>
        /// <returns> Callback that creates the <see cref="IDictionary"></see>. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when type of the dictionary, types of the key or values of the dictionary are null.
        /// </exception>
        /// <exception cref="Localization.ValueLessThanException">
        /// The exception that is thrown when count of the parameters is less than zero.
        /// </exception>
        /// <exception cref="InvalidCollectionGenericTypeDefinitionException">
        /// The exception that is thrown when generic type definition of the dictionary is invalid.
        /// </exception>
        /// <exception cref="ConstructorNotFoundException">
        /// The exception that is thrown when constructor for the dictionary hasn't been found.
        /// </exception>
        public static IFunctionCallback<int[], IDictionary> CreateDictionary(Type dictionaryType, Type keyType, Type valueType, int parametersCount)
        {
        	Validation.NotNull("DictionaryGTD", dictionaryType);
        	Validation.NotNull("KeyType", keyType);
        	Validation.NotNull("ValueType", valueType);
        	Validation.MoreThanOrEquals("ParametersCount", parametersCount, 0);
        	
        	if(!dictionaryType.IsClass || !dictionaryType.IsGenericTypeDefinition || !iDictionaryType.IsBaseInterfaceOrEquals(dictionaryType) || dictionaryType.GetGenericArguments().Length != 2)
        	{
        		throw new InvalidCollectionGenericTypeDefinitionException(dictionaryType, 2, iDictionaryType);
        	}
        	
            Type genericType = dictionaryType.MakeGenericType(keyType, valueType);
            return CreateCollection<IDictionary>(genericType, parametersCount);
        }
        
        /// <summary>
        /// Creates the callback that creates multidimensional <see cref="Array"></see>.
        /// </summary>
        /// <param name="elementType"> Type of the elements in the <see cref="Array"></see>. </param>
        /// <param name="rank"> Rank of the <see cref="Array"></see> (from 1 to 32). </param>
        /// <returns> Callback that creates multidimensional <see cref="Array"></see> (argument - lengths of the <see cref="Array"></see>). </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when type of the elements of the array is null.
        /// </exception>
        /// <exception cref="Localization.ValueLessThanOrEqualsException">
        /// The exception that is thrown when rank is less than or equals to zero.
        /// </exception>
        public static IFunctionCallback<int[], Array> CreateMDArray(Type elementType, int rank)
        {
        	Validation.NotNull("ElementType", elementType);
        	Validation.MoreThan("Rank", rank, 0);
            
        	// ConstructorNotFoundException will not be never raised
            // Any array have two constructors and they are .ctor
            // (parameters count = rank, int only) or 
            // .ctor (parameters count = 2 * rank, int only)
            Type arrayType = (rank == 1) ? elementType.MakeArrayType() : elementType.MakeArrayType(rank);
            return CreateCollection<Array>(arrayType, rank);
        }

        /// <summary>
        /// Creates the callback that gets a value of the field.
        /// </summary>
        /// <param name="type"> Type that contains the field. </param>
        /// <param name="fieldInfo"> Information about of the field. </param>
        /// <returns> Callback that gets a value of the field. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when type or field info are null.
        /// </exception>
        public static IFunctionCallback<object, object> CreateGetAccessor(Type type, FieldInfo fieldInfo)
        {
        	Validation.NotNull("Type", type);
        	Validation.NotNull("FieldInfo", fieldInfo);
        	
            // Creates a new name of the callback
            string name = StringUtils.Combine('+', type.GetFriendlyName(), fieldInfo.Name);

            // DM activator
            Func<Func<object, object>> dmActivator = () =>
            {
            	DynamicMethod method = new DynamicMethod(
            		StringUtils.Combine("GET_VALUE_", name.ToUpper()),
            		objectType,
            		argsObject,
            		fieldInfo.Module,
            		true);

            	ILGenerator ilGenerator = method.GetILGenerator();
            	ilGenerator.Emit(OpCodes.Ldarg_0);
            	EmitTypeConversion(ilGenerator, fieldInfo.DeclaringType, true);
            	ilGenerator.Emit(OpCodes.Ldfld, fieldInfo);
            	if (fieldInfo.FieldType.IsValueType)
            	{
            		ilGenerator.Emit(OpCodes.Box, fieldInfo.FieldType);
            	}
            	ilGenerator.Emit(OpCodes.Ret);

            	return (Func<object, object>)method.CreateDelegate(funcObjectObjectType);
            };

            // Reflection callback
            Func<object, object> reflectionCallback = fieldInfo.GetValue;

            // Selects the callback that associated with the specified strategy
            switch (strategy)
            {
                case ReflectionsUtilsStrategy.Auto:
                    return new FunctionCallback<object, object>(name, dmActivator, reflectionCallback);
                case ReflectionsUtilsStrategy.UseOnlyDynamicMethods:
                    return new FunctionCallback<object, object>(name, dmActivator);
                default:
                    return new FunctionCallback<object, object>(name, reflectionCallback);
            }
        }

        /// <summary>
        /// Creates the callback that sets a value of the field.
        /// </summary>
        /// <param name="type"> Type that contains the field. </param>
        /// <param name="fieldInfo"> Information about of the field. </param>
        /// <returns> Callback that sets a value of the field. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when type or field info are null.
        /// </exception>
        public static IActionCallback<object, object> CreateSetAccessor(Type type, FieldInfo fieldInfo)
        {
        	Validation.NotNull("Type", type);
        	Validation.NotNull("FieldInfo", fieldInfo);
        	
            // Creates a new name of the callback
            string name = StringUtils.Combine('+', type.GetFriendlyName(), fieldInfo.Name);

            // DM activator
            Func<Action<object, object>> dmActivator = () =>
            {
            	DynamicMethod method = new DynamicMethod(
            		StringUtils.Combine("SET_VALUE_", name.ToUpper()),
            		voidType,
            		argsObjectObject,
            		fieldInfo.Module,
            		true);
            	
            	ILGenerator ilGenerator = method.GetILGenerator();
            	ilGenerator.Emit(OpCodes.Ldarg_0);
            	EmitTypeConversion(ilGenerator, fieldInfo.DeclaringType, true);
            	ilGenerator.Emit(OpCodes.Ldarg_1);
            	EmitTypeConversion(ilGenerator, fieldInfo.FieldType, false);
            	ilGenerator.Emit(OpCodes.Stfld, fieldInfo);
            	ilGenerator.Emit(OpCodes.Ret);

            	return (Action<object, object>)method.CreateDelegate(actionObjectObjectType);
            };

            // Reflection callback
            Action<object, object> reflectionCallback = fieldInfo.SetValue;

            // Selects the callback that associated with the specified strategy
            switch (strategy)
            {
                case ReflectionsUtilsStrategy.Auto:
                    return new ActionCallback<object, object>(name, dmActivator, reflectionCallback);
                case ReflectionsUtilsStrategy.UseOnlyDynamicMethods:
                    return new ActionCallback<object, object>(name, dmActivator);
                default:
                    return new ActionCallback<object, object>(name, reflectionCallback);
            }
        }

        /// <summary>
        /// Creates the callback that gets a value of the property.
        /// </summary>
        /// <param name="type"> Type that contains the property. </param>
        /// <param name="propertyInfo"> Information about of the property. </param>
        /// <returns> Callback that gets a value of the property. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when type or property info are null.
        /// </exception>
        /// <exception cref="GetAccessorNotFoundException">
        /// The exception that thrown if property doesn't have get accessor.
        /// </exception>
        public static IFunctionCallback<object, object> CreateGetAccessor(Type type, PropertyInfo propertyInfo)
        {
        	Validation.NotNull("Type", type);
        	Validation.NotNull("PropertyInfo", propertyInfo);
        	
            if (!propertyInfo.CanRead)
            {
            	throw new GetAccessorNotFoundException(type, propertyInfo);
            }
            
            // Creates a new name of the callback
            string name = StringUtils.Combine('+', type.GetFriendlyName(), propertyInfo.Name);

            // DM activator
            Func<Func<object, object>> dmActivator = () =>
            {
            	DynamicMethod method = new DynamicMethod(
            		StringUtils.Combine("GET_VALUE_", name.ToUpper()),
            		objectType,
            		argsObject,
            		propertyInfo.Module,
            		true);
            	
            	MethodInfo methodInfo = propertyInfo.GetGetMethod(true);

            	ILGenerator ilGenerator = method.GetILGenerator();
            	ilGenerator.DeclareLocal(objectType);
            	
            	if (!methodInfo.IsStatic)
            	{
            		ilGenerator.Emit(OpCodes.Ldarg_0);
            		EmitTypeConversion(ilGenerator, propertyInfo.DeclaringType, true);
            	}
            	
            	ilGenerator.EmitCall((methodInfo.IsStatic || methodInfo.DeclaringType.IsValueType) ? OpCodes.Call : OpCodes.Callvirt, methodInfo, null);
            	if (propertyInfo.PropertyType.IsValueType)
            	{
            		ilGenerator.Emit(OpCodes.Box, propertyInfo.PropertyType);
            	}
            	ilGenerator.Emit(OpCodes.Ret);

            	return (Func<object, object>)method.CreateDelegate(funcObjectObjectType);
            };

            // Reflection callback
            Func<object, object> reflectionCallback = propertyInfo.GetValue;

            // Selects the callback that associated with the specified strategy
            switch (strategy)
            {
                case ReflectionsUtilsStrategy.Auto:
                    return new FunctionCallback<object, object>(name, dmActivator, reflectionCallback);
                case ReflectionsUtilsStrategy.UseOnlyDynamicMethods:
                    return new FunctionCallback<object, object>(name, dmActivator);
                default:
                    return new FunctionCallback<object, object>(name, reflectionCallback);
            }
        }

        /// <summary>
        /// Creates the callback that sets a value of the property.
        /// </summary>
        /// <param name="type"> Type that contains the property. </param>
        /// <param name="propertyInfo"> Information about of the property. </param>
        /// <returns> Callback that sets a value of the property. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when type or property info are null.
        /// </exception>
        /// <exception cref="SetAccessorNotFoundException">
        /// The exception that thrown if property doesn't have set accessor.
        /// </exception>
        public static IActionCallback<object, object> CreateSetAccessor(Type type, PropertyInfo propertyInfo)
        {
        	Validation.NotNull("Type", type);
        	Validation.NotNull("PropertyInfo", propertyInfo);
        	
            if (!propertyInfo.CanWrite)
            {
            	throw new SetAccessorNotFoundException(type, propertyInfo);
            }

            // Creates a new name of the callback
            string name = StringUtils.Combine('+', type.GetFriendlyName(), propertyInfo.Name);

            // DM activator
            Func<Action<object, object>> dmActivator = () =>
            {
            	DynamicMethod method = new DynamicMethod(
            		StringUtils.Combine("SET_VALUE_", name.ToUpper()),
            		voidType,
            		argsObjectObject,
            		propertyInfo.Module,
            		true);
            	
            	MethodInfo methodInfo = propertyInfo.GetSetMethod(true);
            	
            	ILGenerator ilGenerator = method.GetILGenerator();
            	
            	if (!methodInfo.IsStatic)
            	{
            		ilGenerator.Emit(OpCodes.Ldarg_0);
            		EmitTypeConversion(ilGenerator, propertyInfo.DeclaringType, true);
            	}
            	
            	ilGenerator.Emit(OpCodes.Ldarg_1);
            	EmitTypeConversion(ilGenerator, propertyInfo.PropertyType, false);
            	
            	ilGenerator.EmitCall((methodInfo.IsStatic || methodInfo.DeclaringType.IsValueType) ? OpCodes.Call : OpCodes.Callvirt, methodInfo, null);
            	ilGenerator.Emit(OpCodes.Ret);
            	
            	return (Action<object, object>)method.CreateDelegate(actionObjectObjectType);
            };

            // Reflection callback
            Action<object, object> reflectionCallback = propertyInfo.SetValue;

            // Selects the callback that associated with the specified strategy
            switch (strategy)
            {
                case ReflectionsUtilsStrategy.Auto:
                    return new ActionCallback<object, object>(name, dmActivator, reflectionCallback);
                case ReflectionsUtilsStrategy.UseOnlyDynamicMethods:
                    return new ActionCallback<object, object>(name, dmActivator);
                default:
                    return new ActionCallback<object, object>(name, reflectionCallback);
            }
        }

        /// <summary>
        /// Creates the callback that invokes the method (returnless, one parameter).
        /// </summary>
        /// <param name="type"> Type that contains the method. </param>
        /// <param name="methodInfo"> Information about of the method. </param>
        /// <returns> Callback that invokes the method (returnless, one parameter). </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when type or method info are null.
        /// </exception>
        /// <exception cref="InvalidMethodException">
        /// The exception that is thrown if method is invalid.
        /// </exception>
        public static IActionCallback<object, object> CreateReturnlessOneParameterMethodCallback(Type type, MethodInfo methodInfo)
        {
        	Validation.NotNull("Type", type);
        	Validation.NotNull("MethodInfo", methodInfo);
        	
        	ParameterInfo[] methodParameters = methodInfo.GetParameters();
        	if(methodParameters.Length != 1 || methodInfo.ReturnType != voidType)
        	{
        		throw new InvalidMethodException(methodInfo, type, 1, voidType);
        	}
        	
            // Creates a name of the callback
            string name = StringUtils.Combine('+', type.GetFriendlyName(), methodInfo.Name);

            // DM activator
            Func<Action<object, object>> dmActivator = () =>
            {
            	DynamicMethod method = new DynamicMethod(
            		StringUtils.Combine("INVOKE_METHOD_", name.ToUpper()),
            		voidType,
            		argsObjectObject,
            		type.Module,
            		true);

            	ILGenerator ilGenerator = method.GetILGenerator();
            	
            	if (!methodInfo.IsStatic)
            	{
            		ilGenerator.Emit(OpCodes.Ldarg_0);
            		EmitTypeConversion(ilGenerator, methodInfo.DeclaringType, true);
            	}
            	
            	ilGenerator.Emit(OpCodes.Ldarg_1);
            	EmitTypeConversion(ilGenerator, methodParameters[0].ParameterType, false);
            	ilGenerator.EmitCall((methodInfo.IsStatic || methodInfo.DeclaringType.IsValueType) ? OpCodes.Call : OpCodes.Callvirt, methodInfo, null);
            	ilGenerator.Emit(OpCodes.Ret);
            	
            	return (Action<object, object>)method.CreateDelegate(actionObjectObjectType);
            };

            // Reflection callback
            Action<object, object> reflectionCallback = (obj, v) => methodInfo.Invoke(obj, new[] { v });

            // Selects the callback that associated with the specified strategy
            switch (strategy)
            {
                case ReflectionsUtilsStrategy.Auto:
                    return new ActionCallback<object, object>(name, dmActivator, reflectionCallback);
                case ReflectionsUtilsStrategy.UseOnlyDynamicMethods:
                    return new ActionCallback<object, object>(name, dmActivator);
                default:
                    return new ActionCallback<object, object>(name, reflectionCallback);
            }
        }

        /// <summary>
        /// Creates the callback that invokes the method (with return, 0 parameters).
        /// </summary>
        /// <param name="type"> Type that contains the method. </param>
        /// <param name="methodInfo"> Information about of the method. </param>
        /// <returns> Callback that invokes the method (with return, parameterless). </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when type or method info are null.
        /// </exception>
        /// <exception cref="InvalidMethodException">
        /// The exception that is thrown if method is invalid.
        /// </exception>
        public static IFunctionCallback<object, object> CreateReturnParameterlessMethodCallback(Type type, MethodInfo methodInfo)
        {
        	Validation.NotNull("Type", type);
        	Validation.NotNull("MethodInfo", methodInfo);
        	
        	if(methodInfo.GetParameters().Length != 0 || methodInfo.ReturnType == voidType)
        	{
        		throw new InvalidMethodException(methodInfo, type, 0, objectType);
        	}
        	
            // Creates a name of the callback
            string name = StringUtils.Combine('+', type.GetFriendlyName(), methodInfo.Name);

            // DM activator
            Func<Func<object, object>> dmActivator = () =>
            {
            	DynamicMethod method = new DynamicMethod(
            		StringUtils.Combine("INVOKE_METHOD_", name.ToUpper()),
            		objectType,
            		argsObject,
            		type.Module,
            		true);
            	
            	ILGenerator ilGenerator = method.GetILGenerator();
            	ilGenerator.DeclareLocal(objectType);

            	if (!methodInfo.IsStatic)
            	{
            		ilGenerator.Emit(OpCodes.Ldarg_0);
            		EmitTypeConversion(ilGenerator, methodInfo.DeclaringType, true);
            	}

            	ilGenerator.EmitCall((methodInfo.IsStatic || methodInfo.ReturnType.IsValueType) ? OpCodes.Call : OpCodes.Callvirt, methodInfo, null);
            	if (methodInfo.ReturnType.IsValueType)
            	{
            		ilGenerator.Emit(OpCodes.Box, methodInfo.ReturnType);
            	}
            	ilGenerator.Emit(OpCodes.Ret);
            	
            	return (Func<object, object>)method.CreateDelegate(funcObjectObjectType);
            };

            // Reflection callback
            Func<object, object> reflectionCallback = (obj) => methodInfo.Invoke(obj, new object[0]);

            // Selects the callback that associated with the specified strategy
            switch (strategy)
            {
                case ReflectionsUtilsStrategy.Auto:
                    return new FunctionCallback<object, object>(name, dmActivator, reflectionCallback);
                case ReflectionsUtilsStrategy.UseOnlyDynamicMethods:
                    return new FunctionCallback<object, object>(name, dmActivator);
                default:
                    return new FunctionCallback<object, object>(name, reflectionCallback);
            }
        }

        /// <summary>
        /// Creates the callback that invokes the method (with return, 1 parameters).
        /// </summary>
        /// <param name="type"> Type that contains the method. </param>
        /// <param name="methodInfo"> Information about of the method. </param>
        /// <returns> Callback that invokes the method (return, one parameter). </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when type or method info are null.
        /// </exception>
        /// <exception cref="InvalidMethodException">
        /// The exception that is thrown if method is invalid.
        /// </exception>
        public static IFunctionCallback<object, object, object> CreateReturnOneParameterMethodCallback(Type type, MethodInfo methodInfo)
        {
        	Validation.NotNull("Type", type);
        	Validation.NotNull("MethodInfo", methodInfo);
        	
        	ParameterInfo[] methodParameters = methodInfo.GetParameters();
        	if(methodParameters.Length != 1 || methodInfo.ReturnType == voidType)
        	{
        		throw new InvalidMethodException(methodInfo, type, 1, objectType);
        	}
        	
            // Creates a name of the callback
            string name = StringUtils.Combine('+', type.GetFriendlyName(), methodInfo.Name);

            // DM activator
            Func<Func<object, object, object>> dmActivator = () =>
            {
            	DynamicMethod method = new DynamicMethod(
            		StringUtils.Combine("INVOKE_METHOD_", name.ToUpper()),
            		objectType,
            		argsObjectObject,
            		type.Module,
            		true);
            	
            	ILGenerator ilGenerator = method.GetILGenerator();
            	ilGenerator.DeclareLocal(objectType);

            	if (!methodInfo.IsStatic)
            	{
            		ilGenerator.Emit(OpCodes.Ldarg_0);
            		EmitTypeConversion(ilGenerator, methodInfo.DeclaringType, true);
            	}

            	ilGenerator.Emit(OpCodes.Ldarg_1);
            	EmitTypeConversion(ilGenerator, methodParameters[0].ParameterType, false);
            	ilGenerator.EmitCall((methodInfo.IsStatic || methodInfo.ReturnType.IsValueType) ? OpCodes.Call : OpCodes.Callvirt, methodInfo, null);
            	if (methodInfo.ReturnType.IsValueType)
            	{
            		ilGenerator.Emit(OpCodes.Box, methodInfo.ReturnType);
            	}
            	ilGenerator.Emit(OpCodes.Ret);
            	
            	return (Func<object, object, object>)method.CreateDelegate(funcObjectObjectObjectType);
            };

            // Reflection callback
            Func<object, object, object> reflectionCallback = (obj, v) => methodInfo.Invoke(obj, new object[] { v });

            // Selects the callback that associated with the specified strategy
            switch (strategy)
            {
                case ReflectionsUtilsStrategy.Auto:
                    return new FunctionCallback<object, object, object>(name, dmActivator, reflectionCallback);
                case ReflectionsUtilsStrategy.UseOnlyDynamicMethods:
                    return new FunctionCallback<object, object, object>(name, dmActivator);
                default:
                    return new FunctionCallback<object, object, object>(name, reflectionCallback);
            }
        }

        /// <summary>
        /// Creates the callback that creates the collection.
        /// </summary>
        /// <param name="type"> Type of the collection. </param>
        /// <param name="parametersCount">  Expected count of the parameters. </param>
        /// <returns> Callback that creates the collection. </returns>
        /// <typeparam name="T"> Target type of the collection. </typeparam>
        /// <exception cref="ConstructorNotFoundException"> The exception that is thrown when constructor with expected count of the parameters hasn't been found. </exception>
        private static IFunctionCallback<int[], T> CreateCollection<T>(Type type, int parametersCount) where T : class
        {
            // Tests that collection may be build
            Func<ConstructorInfo, bool> match = (c) =>
            {
            	ParameterInfo[] parametersInfo = c.GetParameters();
            	int length = parametersInfo.Length;
            	
            	if (length != parametersCount)
            	{
            		return false;
            	}
            	
            	for (int i = 0; i < length; i++)
            	{
            		if (parametersInfo[i].ParameterType != intType)
            		{
            			return false;
            		}
            	}
            	return true;
            };
            
            ConstructorInfo constructorInfo = type.GetConstructors(instanceBindingFlags).FindOrDefault(match);
            if (constructorInfo == null)
            {
            	throw new ConstructorNotFoundException(type, parametersCount);
            }

            // Creates the callback for the collection building

            // Creates a new name of the callback
            string name = type.GetFriendlyName();

            // DM activator
            Func<Func<int[], T>> dmActivator = () =>
            {
            	DynamicMethod dynamicMethod = new DynamicMethod(
            		StringUtils.Combine("CREATE_", name.ToUpper()),
            		type,
            		argsIntArray,
            		true);

            	ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            	for (int i = 0; i < parametersCount; i++)
            	{
            		ilGenerator.Emit(OpCodes.Ldarg_0);
            		ilGenerator.Emit(OpCodes.Ldc_I4, i);
            		ilGenerator.Emit(OpCodes.Ldelem_I4);
            	}

            	ilGenerator.Emit(OpCodes.Newobj, constructorInfo);
            	ilGenerator.Emit(OpCodes.Ret);
            	return (Func<int[], T>)dynamicMethod.CreateDelegate(typeof(Func<int[], T>));
            };

            Func<int[], T> reflectionCallback = null;
            if (parametersCount == 0)
            {
                // Creating with the Activator.CreateInstance usually has better performance than ConstructorInfo.Invoke for the parameterless constructors
                reflectionCallback = (args) => Activator.CreateInstance(type) as T;
            }
            else
            {
                reflectionCallback = (args) =>
                {
                    int length = args.Length;
                    object[] objectArgs = new object[length];
                    for (int i = 0; i < length; i++)
                    {
                        objectArgs[i] = args[i];
                    }

                    // Creating with the ConstructorInfo.Invoke usually has better performance than Activator.CreateInstance for the constructors with the parameters
                    return constructorInfo.Invoke(objectArgs) as T;
                };
            }

            // Selects the callback that associated with the specified strategy
            switch (strategy)
            {
                case ReflectionsUtilsStrategy.Auto:
                    return new FunctionCallback<int[], T>(name, dmActivator, reflectionCallback);
                case ReflectionsUtilsStrategy.UseOnlyDynamicMethods:
                    return new FunctionCallback<int[], T>(name, dmActivator);
                default:
                    return new FunctionCallback<int[], T>(name, reflectionCallback);
            }
        }

        /// <summary>
        /// Emits the conversion (unboxing/casting) one type to other type.
        /// </summary>
        /// <param name="ilGenerator"> Instance of the <see cref="ILGenerator"></see>. </param>
        /// <param name="otherType"> Other type. </param>
        /// <param name="isContainer"> Determines whether is container of not. </param>
        private static void EmitTypeConversion(ILGenerator ilGenerator, Type otherType, bool isContainer)
        {
            if (otherType == objectType)
            {
            	return;
            }

            if (otherType.IsValueType)
            {
                ilGenerator.Emit(isContainer ? OpCodes.Unbox : OpCodes.Unbox_Any, otherType);
            }
            else
            {
                ilGenerator.Emit(OpCodes.Castclass, otherType);
            }
        }
    }
}