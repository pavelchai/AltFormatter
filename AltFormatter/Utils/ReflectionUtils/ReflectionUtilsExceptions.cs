/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Localization;
using System;
using System.Reflection;

namespace AltFormatter.Utils
{
    /// <summary>
    /// The exception that is thrown when duplicate factory methods have been found.
    /// </summary>
    internal sealed class DuplicateFactoryMethodException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when duplicate factory methods have been found.
        /// </summary>
        /// <param name="methodInfo"> Method info. </param>
        /// <param name="type"> Type that contains the method. </param>
        public DuplicateFactoryMethodException(MethodInfo methodInfo, Type type) : base("Duplicate factory method <{0}> has been found in <{1}>.", methodInfo.Name, type.GetFriendlyName())
        {
        }
    }
    
    /// <summary>
    /// The exception that is thrown when constructor with expected count of the parameters hasn't been found.
    /// </summary>
    internal sealed class ConstructorNotFoundException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when constructor with expected count of the parameters hasn't been found.
        /// </summary>
        /// <param name="type"> Type that contains the constructor. </param>
        /// <param name="count"> Expected count of the parameters. </param>
        public ConstructorNotFoundException(Type type, int count) : base("Constructor with the <{0}> parameters hasn't been found in <{1}>.", count, type.GetFriendlyName())
        {
        }
    }
    
    /// <summary>
    /// The exception that is thrown when method is invalid.
    /// </summary>
    internal sealed class InvalidMethodException : LocalizedException
    {
    	/// <summary>
        /// The exception that is thrown when method is invalid.
        /// </summary>
        /// <param name="methodInfo"> Method info. </param>
        /// <param name="type"> Type that contains the method. </param>
        /// <param name="expectedArgumentsCount"> Expected count of the arguments. </param>
        /// <param name="expectedReturnType"> Expected return type. </param>
        public InvalidMethodException(MethodInfo methodInfo, Type type, int expectedArgumentsCount, Type expectedReturnType) : base("<{0}> method <{1}> in <{2}> is invalid. Expected count of the arguments and return type are <{3}> and <{4}>. Received count of the arguments and return type are <{5}> and <{6}>.", methodInfo.IsStatic ? "Static" : "Instance", methodInfo.Name, type.GetFriendlyName(), expectedArgumentsCount, expectedReturnType.GetFriendlyName(), methodInfo.GetParameters().Length, methodInfo.ReturnType.GetFriendlyName())
        {
        }
    }
    
    /// <summary>
    /// The exception that is thrown when generic type definition of the collection is invalid.
    /// </summary>
    internal sealed class InvalidCollectionGenericTypeDefinitionException : LocalizedException
    {
    	/// <summary>
        /// The exception that is thrown when generic type definition of the collection is invalid.
        /// </summary>
        /// <param name="collectionType"> Generic type definition of the collection. </param>
        /// <param name="expectedArgumentsCount"> Expected count of the generic type definition of the collection. </param>
        /// <param name="baseExpectedType"> Expected generic type definition of the collection. </param>
        public InvalidCollectionGenericTypeDefinitionException(Type collectionType, int expectedArgumentsCount, Type baseExpectedType) : base("Expected generic type definintion is definition that implements <{0}> and have <{1}> generic arguments, but received <{2}>.", baseExpectedType.GetFriendlyName(), expectedArgumentsCount, collectionType.GetFriendlyName())
        {
        }
    }
    
    /// <summary>
    /// The exception that is thrown if property doesn't have get accessor.
    /// </summary>
    internal sealed class GetAccessorNotFoundException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown if property doesn't have get accessor.
        /// </summary>
        /// <param name="type"> Type that contains the property. </param>
        /// <param name="property"> Property. </param>
        public GetAccessorNotFoundException(Type type, PropertyInfo property) : base("Property <{0}> of type <{1}>, doesn't have get assessor.", property.Name, type.GetFriendlyName())
        {
        }
    }

    ///<summary>
    /// The exception that is thrown if property doesn't have set accessor.
    /// </summary>
    internal sealed class SetAccessorNotFoundException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown if property doesn't have set accessor.
        /// </summary>
        /// <param name="type"> Type that contains the property. </param>
        /// <param name="property"> Property. </param>
        public SetAccessorNotFoundException(Type type, PropertyInfo property) : base("Property <{0}> of type <{1}> doesn't have set assessor.", property.Name, type.GetFriendlyName())
        {
        }
    }
}