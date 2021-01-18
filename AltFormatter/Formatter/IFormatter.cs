/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System;
using System.Numerics;

namespace AltFormatter.Formatter
{
    /// <summary>
    /// Provides functionality for formatting the objects [Not extendable].
    /// </summary>
    /// <remarks>
    /// Uses types from assemblies that have <see cref="FormattableAttribute"></see>.
    /// 
    /// Supported types:
    /// 1) Primitive types (<see cref="bool"></see>, <see cref="byte"></see>, <see cref="sbyte"></see>, <see cref="short"></see>, <see cref="ushort"></see>, <see cref="int"></see>, <see cref="uint"></see>, <see cref="long"></see>, <see cref="ulong"></see>, <see cref="float"></see>, <see cref="double"></see>, <see cref="decimal"></see>, <see cref="char"></see>, <see cref="string"></see>, <see cref="DateTime"></see>, <see cref="TimeSpan"></see> and <see cref="Complex"></see>.
    /// Boxed primitive types are not supported.
    /// 2) <see cref="Enum"></see>.
    /// 3) Types that have <see cref="FormattableAttribute"></see>. 
    /// The types should have parameterless constructors or static factory methods (that have <see cref="Utils.FactoryAttribute"></see>). 
    /// Serializing/Deserializing fields and properties should have <see cref="KeyAttribute"></see>.
    /// Serialization/deserialization processes may be managed with the methods that may be implemented by using <see cref="IFormatter"></see> interface.
    /// 4) Multidimensional dimensional arrays of (1-6) types (rank of array may be from 1 to 32).
    /// 5) Collections of (1-6) types (<see cref="System.Collections.Generic.List{T}"></see>, <see cref="System.Collections.Generic.LinkedList{T}"></see>, <see cref="System.Collections.Generic.HashSet{T}"></see>, <see cref="System.Collections.Generic.SortedSet{T}"></see>, <see cref="System.Collections.Generic.Queue{T}"></see>, <see cref="System.Collections.Generic.Stack{T}"></see>, <see cref="System.Collections.Concurrent.ConcurrentQueue{T}"></see>, <see cref="System.Collections.Concurrent.ConcurrentStack{T}"></see> and <see cref="System.Collections.Concurrent.ConcurrentBag{T}"></see>).
    /// 6) Dictionaries of (1-6) types (<see cref="System.Collections.Generic.Dictionary{K,V}"></see>, <see cref="System.Collections.Generic.SortedDictionary{K,V}"></see>, <see cref="System.Collections.Generic.SortedList{K,V}"></see> and <see cref="System.Collections.Concurrent.ConcurrentDictionary{K,V}"></see>).
    /// 
    /// Supported functionality:
    /// 1) Formatter supports conversion between primitive types (each other), collection / dictionary types (each other) if the conversion is allowable.
    /// 2) Formatter supports conversion (mapping) between types (3). The combination of the keys of the types should be equals, but names of these types may be different.
    /// 3) Formatter supports curcular references.
    /// 4) May be extended.
    /// Different formatters that implements the functionality described before may be exchanged by each other without modification of the types/assemblies,
    /// but due to different implementation of the formatters serialized objects by the one serializer may not be deserialized by the other.
    /// </remarks>
    public interface IFormatter
    {
        /// <summary>
        /// Serializes an object, or graph of objects with the given root to the array of bytes. </summary>
        /// <param name="graph"> 
        /// The object, or root of the object graph, to serialize. 
        /// All child objects of this root object are automatically serialized.
        /// </param>
        /// <returns> Array of bytes that associated with the graph if serialization has been finished successfully, otherwise null. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when graph is null.
        /// </exception>
        byte[] Serialize<T>(T graph);

        /// <summary>
        /// Deserializes the data of the byte array and reconstitutes the graph of objects.
        /// </summary>
        /// <param name="data"> Array of bytes that contains the data to deserialize. </param>
        /// <returns> Deserialized object or graph of objects if deserialization has been finished successfully, otherwise default(T). </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when data is null.
        /// </exception>
        T Deserialize<T>(byte[] data);
    }
}