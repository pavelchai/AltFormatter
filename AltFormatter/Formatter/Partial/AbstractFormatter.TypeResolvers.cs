/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace AltFormatter.Formatter
{
    /// <summary>
    /// Represents a base class for the formatter.
    /// </summary>
    public abstract partial class AbstractFormatter
    {
        /// <summary>
        /// Identifier of the collections.
        /// </summary>
        private static IDictionary<Type, string> collectionTypeIdentifiers;

        /// <summary>
        /// Types of the collections.
        /// </summary>
        private static IDictionary<string, Type> collectionIdentifierTypes;

        /// <summary>
        /// Identifier of the dictionaries.
        /// </summary>
        private static IDictionary<Type, string> dictionaryTypeIdentifiers;

        /// <summary>
        /// Types of the dictionaries.
        /// </summary>
        private static IDictionary<string, Type> dictionaryIdentifierTypes;

        /// <summary>
        /// Primitive types.
        /// </summary>
        private readonly static HashSet<Type> primitiveTypes = new HashSet<Type>();

        /// <summary>
        /// Initializes the <see cref="AbstractFormatter"></see>.
        /// </summary>
        static AbstractFormatter()
        {
            // Primitive types

            primitiveTypes.Add(typeof(bool));
            primitiveTypes.Add(typeof(byte));
            primitiveTypes.Add(typeof(sbyte));
            primitiveTypes.Add(typeof(short));
            primitiveTypes.Add(typeof(ushort));
            primitiveTypes.Add(typeof(int));
            primitiveTypes.Add(typeof(uint));
            primitiveTypes.Add(typeof(long));
            primitiveTypes.Add(typeof(ulong));
            primitiveTypes.Add(typeof(string));
            primitiveTypes.Add(typeof(char));
            primitiveTypes.Add(typeof(DateTime));
            primitiveTypes.Add(typeof(TimeSpan));
            primitiveTypes.Add(typeof(float));
            primitiveTypes.Add(typeof(double));
            primitiveTypes.Add(typeof(decimal));
            primitiveTypes.Add(typeof(Complex));

            // Collections

            collectionTypeIdentifiers = new Dictionary<Type, string>(9);
            collectionTypeIdentifiers.Add(listType, ListIdentifier);
            collectionTypeIdentifiers.Add(linkedListType, LinkedListIdentifier);
            collectionTypeIdentifiers.Add(hashSetType, HashSetIdentifier);
            collectionTypeIdentifiers.Add(sortedSetType, SortedSetIdentifier);
            collectionTypeIdentifiers.Add(queueType, QueueIdentifier);
            collectionTypeIdentifiers.Add(concurrentQueueType, ConcurrentQueueIdentifier);
            collectionTypeIdentifiers.Add(stackType, StackIdentifier);
            collectionTypeIdentifiers.Add(concurrentStackType, ConcurrentStackIdentifier);
            collectionTypeIdentifiers.Add(concurrentBagType, ConcurrentBagIdentifier);

            collectionIdentifierTypes = new Dictionary<string, Type>(9);
            collectionIdentifierTypes.Add(ListIdentifier, listType);
            collectionIdentifierTypes.Add(LinkedListIdentifier, linkedListType);
            collectionIdentifierTypes.Add(HashSetIdentifier, hashSetType);
            collectionIdentifierTypes.Add(SortedSetIdentifier, sortedSetType);
            collectionIdentifierTypes.Add(QueueIdentifier, queueType);
            collectionIdentifierTypes.Add(ConcurrentQueueIdentifier, concurrentQueueType);
            collectionIdentifierTypes.Add(StackIdentifier, stackType);
            collectionIdentifierTypes.Add(ConcurrentStackIdentifier, concurrentStackType);
            collectionIdentifierTypes.Add(ConcurrentBagIdentifier, concurrentBagType);

            // Dictionaries

            dictionaryTypeIdentifiers = new Dictionary<Type, string>(4);
            dictionaryTypeIdentifiers.Add(dictionaryType, DictionaryIdentifier);
            dictionaryTypeIdentifiers.Add(concurrentDictionaryType, ConcurrentDictionaryIdentifier);
            dictionaryTypeIdentifiers.Add(sortedDictionaryType, SortedDictionaryIdentifier);
            dictionaryTypeIdentifiers.Add(sortedListType, SortedListIdentifier);

            dictionaryIdentifierTypes = new Dictionary<string, Type>(4);
            dictionaryIdentifierTypes.Add(DictionaryIdentifier, dictionaryType);
            dictionaryIdentifierTypes.Add(ConcurrentDictionaryIdentifier, concurrentDictionaryType);
            dictionaryIdentifierTypes.Add(SortedDictionaryIdentifier, sortedDictionaryType);
            dictionaryIdentifierTypes.Add(SortedListIdentifier, sortedListType);
        }

        /// <summary>
        /// Gets an identifier of the type of the multidimensional array.
        /// </summary>
        /// <param name="declaredType"> Declared type. </param>
        /// <returns> Identifier of the type of the multidimensional array if array is supported, otherwise null. </returns>
        private static string GetMDArrayTypeIndentifier(Type declaredType)
        {
            return (declaredType.IsArray && declaredType.GetArrayRank() != 1) ? MDArrayIdentifier : null;
        }

        /// <summary>
        /// Gets an identifier of the type of the collection.
        /// </summary>
        /// <param name="declaredType"> Declared type. </param>
        /// <returns> Identifier of the type of the collection if collection is supported, otherwise null. </returns>
        private static string GetCollectionTypeIndentifier(Type declaredType)
        {
            if (declaredType.IsArray && declaredType.GetArrayRank() == 1)
            {
                return ArrayIdentifier;
            }

            if (!declaredType.IsGenericType || declaredType.GetGenericArguments().Length != 1)
            {
                return null;
            }

            string identifier = null;
            Type declaredTypeGTD = declaredType.GetGenericTypeDefinition();
            if (!collectionTypeIdentifiers.TryGetValue(declaredTypeGTD, out identifier))
            {
                return null;
            }

            return identifier;
        }

        /// <summary>
        /// Gets an identifier of the type of the dictionary.
        /// </summary>
        /// <param name="declaredType"> Declared type. </param>
        /// <returns> Identifier of the type of the dictionay if dictionary is supported, otherwise null. </returns>
        private static string GetDictionaryTypeIndentifier(Type declaredType)
        {
            if (!declaredType.IsGenericType || declaredType.GetGenericArguments().Length != 2)
            {
                return null;
            }

            string identifier = null;
            Type declaredTypeGTD = declaredType.GetGenericTypeDefinition();
            if (!dictionaryTypeIdentifiers.TryGetValue(declaredTypeGTD, out identifier))
            {
                return null;
            }

            return identifier;
        }

        /// <summary>
        /// Gets the type of the items of the multidimentional array.
        /// </summary>
        /// <param name="declaredType"> Declared type. </param>
        /// <param name="identifier"> Identifier of the multidimensional array. </param>
        /// <returns> Type of the items of the multidimentional array if multidimensional array is supported, otherwise null. </returns>
        private static Type GetMDArrayElementType(Type declaredType, string identifier)
        {
            if (!declaredType.IsArray || declaredType.GetArrayRank() == 1)
            {
                return null;
            }

            return (identifier == MDArrayIdentifier) ? declaredType.GetElementType() : null;
        }

        /// <summary>
        /// Gets the type of the collection and type of the items of the collection
        /// </summary>
        /// <param name="declaredType"> Declared type. </param>
        /// <param name="declaredTypeInterfaces"> Interfaces of the type. </param>
        /// <param name="identifier"> Identifier of the collection. </param>
        /// <returns> Array of the types that contains type of the collection (index = 0) and type of the items of the collection (index = 1) if collection is supported, otherwise null. </returns>
        private static Type[] GetCollectionTypesCA(Type declaredType, Type[] declaredTypeInterfaces, string identifier)
        {
            if (declaredType.IsArray && declaredType.GetArrayRank() == 1)
            {
                return new[] { arrayType, declaredType.GetElementType() };
            }

            if (!declaredType.IsGenericType)
            {
                return null;
            }

            Type declaredTypeGTD = declaredType.GetGenericTypeDefinition();

            if (declaredTypeGTD != iEnumerableType && declaredTypeInterfaces.FindOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == iEnumerableType) == null)
            {
                return null;
            }

            if (declaredTypeGTD == iDictionaryType || declaredTypeInterfaces.FindOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == iDictionaryType) != null)
            {
                return null;
            }

            Type outputType = null;
            if (declaredType.IsInterface)
            {
                if (identifier == ArrayIdentifier)
                {
                    outputType = arrayType;
                }
                else
                {
                    if (!collectionIdentifierTypes.TryGetValue(identifier, out outputType))
                    {
                        return null;
                    }
                }
            }
            else
            {
                if (collectionTypeIdentifiers.ContainsKey(declaredTypeGTD))
                {
                    outputType = declaredTypeGTD;
                }
                else
                {
                    return null;
                }
            }

            return new[] { outputType, (outputType.IsArray) ? declaredType.GetElementType() : declaredType.GetGenericArguments()[0] };
        }

        /// <summary>
        /// Gets the type of the dictionary and types of the key and value of the dictionary
        /// </summary>
        /// <param name="declaredType"> Declared type. </param>
        /// <param name="declaredTypeInterfaces"> Interfaces of the type. </param>
        /// <param name="identifier"> Identifier of the collection. </param>
        /// <returns> Array of the types that contains type of the dictionary (index = 0) and types of the key (index = 1) and value (index = 2) of the dictionary if dictionary is supported, otherwise null. </returns>
        private static Type[] GetDictionaryTypesDKV(Type declaredType, Type[] declaredTypeInterfaces, string identifier)
        {
            if (!declaredType.IsGenericType)
            {
                return null;
            }

            Type[] genericArgs = declaredType.GetGenericArguments();
            if (genericArgs.Length != 2)
            {
                return null;
            }

            Type declaredTypeGTD = declaredType.GetGenericTypeDefinition();
            if (declaredTypeGTD != iDictionaryType && declaredTypeInterfaces.FindOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == iDictionaryType) == null)
            {
                return null;
            }

            Type outputType = null;
            if (declaredType.IsInterface)
            {
                if (!dictionaryIdentifierTypes.TryGetValue(identifier, out outputType))
                {
                    return null;
                }
            }
            else
            {
                if (dictionaryTypeIdentifiers.ContainsKey(declaredTypeGTD))
                {
                    outputType = declaredTypeGTD;
                }
                else
                {
                    return null;
                }
            }

            return new[] { outputType, genericArgs[0], genericArgs[1] };
        }

        /// <summary>
        /// Gets a primitive converter that converts string to object.
        /// </summary>
        /// <param name="type"> Type. </param>
        /// <returns> Converter if type is primitive, otherwise null. </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual Func<string, object> GetPrimitiveConverterToObject(Type type)
        {
            return TypeUtils.GetConverterToObject(type);
        }

        /// <summary>
        /// Gets a primitive converter that converts object to string.
        /// </summary>
        /// <param name="type"> Type. </param>
        /// <returns> Converter if type is primitive, otherwise null. </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual Func<object, string> GetPrimitiveConverterToString(Type type)
        {
            return TypeUtils.GetConverterToString(type);
        }

        /// <summary>
        /// Determines whether type of the element is primitive.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <returns> True if type of the element is primitive. </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual bool IsPrimitive(Type type)
        {
            return type.IsEnum || primitiveTypes.Contains(type);
        }
    }
}