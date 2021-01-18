/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace AltFormatter.Formatter
{
    /// <summary>
    /// Represents a base class for the formatter.
    /// </summary>
    public abstract partial class AbstractFormatter
    {
        /// <summary>
        /// Deserializes the data of the byte array and reconstitutes the graph of objects.
        /// </summary>
        /// <param name="data"> Array of bytes that contains the data to deserialize. </param>
        /// <returns> Deserialized object or graph of objects if deserialization has been finished successfully, otherwise default(T). </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when data is null.
        /// </exception>
        public T Deserialize<T>(byte[] data)
        {
        	Validation.NotNull("Data", data);

            while (Interlocked.CompareExchange(ref this.formatterIsLoading, 1, 1) == 1)
            {
                // Waits while assemblies have been loaded
                Thread.Sleep(1);
            }

            try
            {
                // Gets a type of the object
                Type type = typeof(T);
                string typeName = type.GetFriendlyName();

                if (this.IsPrimitive(type))
                {
                    Func<string, object> primitiveConverter = this.GetPrimitiveConverterToObject(type);

                    // Tries deserialize the data (as primitive type)

                    return (T)primitiveConverter(data.GetString());
                }
                else
                {
                    // Tries deserialize the data (as non-primitive type)

                    Dictionary<string, object> references = new Dictionary<string, object>(100);
                    IReadOnlyDataStorer storer = this.CreateReadOnlyDataStorer(data);

                    IReadOnlyList<IDataStorerEntry> entries = storer.Entries;
                    LinkedList<IDataStorerEntry> storerEntries = new LinkedList<IDataStorerEntry>();
                    int count = entries.Count;
                    for (int i = 0; i < count; i++)
                    {
                        storerEntries.AddLast(entries[i]);
                    }

                    return (T)this.Read(type, "", storer, storerEntries, references);
                }
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Reads an entry of the file by the path and creates a new object.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <param name="path"> The path. </param>
        /// <param name="storer"> The storer. </param>
        /// <param name="entries"> List of the entries of the storer. </param>
        /// <param name="references"> Dictionary that contains references to the objects. </param>
        /// <returns> New object. </returns>
        private object Read(Type type, string path, IReadOnlyDataStorer storer, LinkedList<IDataStorerEntry> entries, Dictionary<string, object> references)
        {
            string className = null;
            IDataStorerEntry entry = FindEntryNode(path, InfoFileName, entries).Value;
            IDictionary<string, string> values = DeserializeInfo(storer.Read(entry), out className);

            if (className == NullIdentifier)
            {
                // Graph is null -> returns null
                return null;
            }

            // Reads the graph

            object graph = null;
            string referenceTo = values.ContainsKey(ReferenceToString) ? values[ReferenceToString] : null;
            if (referenceTo != null)
            {
                // Tries get the reference to to the previously deserialized graph

                if (!references.TryGetValue(referenceTo, out graph))
                {
                    // Returns new (not deserialized before) graph
                    return this.Read(type, referenceTo, storer, entries, references);
                }
                else
                {
                    // Returns previously deserialized graph
                    return graph;
                }
            }
            else
            {
                // Tests whether graph has been deserialized before 
                if (references.TryGetValue(path, out graph))
                {
                    // Returns previously deserialized graph
                    return graph;
                }

                // Tries reads the new graph

                Type[] typeInterfaces = type.GetInterfaces();

                graph = TryReadCollection(path, type, typeInterfaces, className, values, storer, entries, references);
                if (graph != null)
                {
                    return graph;
                }

                graph = TryReadDictionary(path, type, typeInterfaces, className, values, storer, entries, references);
                if (graph != null)
                {
                    return graph;
                }

                graph = TryReadMDArray(path, type, className, values, storer, entries, references);
                if (graph != null)
                {
                    return graph;
                }

                graph = TryReadFormattable(path, type, className, values, storer, entries, references);
                if (graph != null)
                {
                    return graph;
                }

                throw new UnsupportedTypeException(type, className);
            }
        }

        /// <summary>
        /// Tries read the entries of the data by the path and creates a new collection.
        /// </summary>
        /// <param name="path"> The path. </param>
        /// <param name="type"> The type. </param>
        /// <param name="typeInterfaces"> Interfaces of the type. </param>
        /// <param name="className"> Name of the class. </param>
        /// <param name="values"> Values of the class. </param>
        /// <param name="storer"> The storer. </param>
        /// <param name="entries"> List of the entries of the storer. </param>
        /// <param name="references"> Dictionary that contains references to the objects. </param>
        /// <returns> Collection if collection has been created successfully, otherwise null. </returns>
        private object TryReadCollection(string path, Type type, Type[] typeInterfaces, string className, IDictionary<string, string> values, IReadOnlyDataStorer storer, LinkedList<IDataStorerEntry> entries, Dictionary<string, object> references)
        {
            // Tries get types of the collection and elements of the collection

            Type[] targetTypes = GetCollectionTypesCA(type, typeInterfaces, className);
            if (targetTypes == null)
            {
                // Target types are null -> collection isn't supported -> returns null
                return null;
            }

            // Reads the information about of the collection

            Type collectionType = targetTypes[0];
            Type elementType = targetTypes[1];

            int count = Convert.ToInt32(values[CountString]);

            if (count != 0)
            {
                bool isPrimitive = values[PrimitiveString] == TrueString;

                if (!isPrimitive)
                {
                    // Elements of the array/collection aren't primitive - reads the elements from different files

                    IEnumerable enumerable = this.CreateCollection(collectionType, elementType, count);
                    references.Add(path, enumerable);

                    Array array = enumerable as Array;
                    if (array != null)
                    {
                        // Reads the array

                        for (int i = 0; i < count; i++)
                        {
                            array.SetValue(this.Read(
                                elementType,
                                StringUtils.Combine(path, i.ToString(), DirectorySeparatorString),
                                storer,
                                entries,
                                references), i
                            );
                        }

                        return enumerable;
                    }
                    else
                    {
                        // Reads the collection

                        IList list = enumerable as IList;
                        if (list != null)
                        {
                            // Collection implements IList interface - the interface should be used for the fast writing the elements of the collection

                            for (int i = 0; i < count; i++)
                            {
                                list.Add(this.Read(
                                    elementType,
                                    StringUtils.Combine(path, i.ToString(), DirectorySeparatorString),
                                    storer,
                                    entries,
                                    references)
                                );
                            }

                            return enumerable;
                        }
                        else
                        {
                            // Collection not implements IList interface - the collection writer should be used for the writing the elements of the collection

                            using (CollectionWriter writer = new CollectionWriter(this, enumerable))
                            {
                                for (int i = 0; i < count; i++)
                                {
                                    writer.Add(this.Read(
                                        elementType,
                                        StringUtils.Combine(path, i.ToString(), DirectorySeparatorString),
                                        storer,
                                        entries,
                                        references)
                                    );
                                }
                            }

                            return enumerable;
                        }
                    }
                }
                else
                {
                    // Elements of the array/collection are primitive - reads the elements from single file

                    byte[] data = ReadAndRemoveEntry(path, CollectionFileName, storer, entries);

                    IEnumerable enumerable = this.GetPrimitiveCollection(data, collectionType, elementType, count);
                    references.Add(path, enumerable);

                    return enumerable;
                }
            }
            else
            {
                // Collection is empty -> returns empty collection

                IEnumerable enumerable = this.CreateCollection(collectionType, elementType, 0);
                references.Add(path, enumerable);
                return enumerable;
            }
        }

        /// <summary>
        /// Tries read the entries of the data by the path and creates a new dictionary.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <param name="typeInterfaces"> Interfaces of the type. </param>
        /// <param name="className"> Name of the class. </param>
        /// <param name="values"> Values of the class. </param>
        /// <param name="path"> The path. </param>
        /// <param name="storer"> The storer. </param>
        /// <param name="entries"> List of the entries of the storer. </param>
        /// <param name="references"> Dictionary that contains references to the objects. </param>
        /// <returns> Dictionary if dictionary has been created successfully, otherwise null. </returns>
        private object TryReadDictionary(string path, Type type, Type[] typeInterfaces, string className, IDictionary<string, string> values, IReadOnlyDataStorer storer, LinkedList<IDataStorerEntry> entries, Dictionary<string, object> references)
        {
            // Tries get the type of the dictionary and types of the key and value of the dictionary

            Type[] targetTypes = GetDictionaryTypesDKV(type, typeInterfaces, className);
            if (targetTypes == null)
            {
                // Target types are null -> dictionary isn't supported -> returns null
                return null;
            }

            // Reads the information about of the dictionary

            Type dictionaryType = targetTypes[0];
            Type keyType = targetTypes[1];
            Type valueType = targetTypes[2];

            int count = Convert.ToInt32(values[CountString]);

            if (count != 0)
            {
                bool isPrimitiveKeys = values[PrimitiveKeysString] == TrueString;
                bool isPrimitiveValues = values[PrimitiveValuesString] == TrueString;

                if (!isPrimitiveKeys && !isPrimitiveValues)
                {
                    // Keys and values of the dictionary aren't primitive - reads their from different files

                    IDictionary dictionary = this.CreateDictionary(dictionaryType, keyType, valueType, count);
                    references.Add(path, dictionary);

                    for (int i = 0; i < count; i++)
                    {
                        object key = this.Read(
                            keyType,
                            StringUtils.Combine(path, i.ToString(), KeyDirectoryString),
                            storer,
                            entries,
                            references);

                        object value = this.Read(
                            valueType,
                            StringUtils.Combine(path, i.ToString(), ValueDirectoryString),
                            storer,
                            entries,
                            references);

                        dictionary.Add(key, value);
                    }

                    return dictionary;
                }
                else
                {
                    if (isPrimitiveKeys && isPrimitiveValues)
                    {
                        // Keys and values of the dictionary are primitive - reads their from single file

                        byte[] data = ReadAndRemoveEntry(path, DictionaryFileName, storer, entries);

                        IDictionary dictionary = this.GetPrimitiveDictionary(data, dictionaryType, keyType, valueType, count);
                        references.Add(path, dictionary);
                        return dictionary;
                    }
                    else
                    {
                        if (isPrimitiveKeys)
                        {
                            // Keys are primitive and values aren't primitive - reads keys from the single file and values from different files

                            IDictionary dictionary = this.CreateDictionary(dictionaryType, keyType, valueType, count);
                            references.Add(path, dictionary);

                            byte[] data = ReadAndRemoveEntry(path, DictionaryKeysFileName, storer, entries);

                            int index = 0;
                            IEnumerable<object> keys = GetPrimitiveDictionaryKeys(data, keyType, count);
                            foreach (object key in keys)
                            {
                                if (index == count)
                                {
                                    break;
                                }

                                object value = this.Read(
                                    valueType,
                                    StringUtils.Combine(path, index.ToString(), ValueDirectoryString),
                                    storer,
                                    entries,
                                    references);

                                dictionary.Add(key, value);

                                index++;
                            }

                            return dictionary;
                        }
                        else
                        {
                            // Keys are primitive and values aren't primitive - reads keys from the single file and values from different files

                            IDictionary dictionary = this.CreateDictionary(dictionaryType, keyType, valueType, count);
                            references.Add(path, dictionary);

                            byte[] data = ReadAndRemoveEntry(path, DictionaryValuesFileName, storer, entries);

                            int index = 0;
                            IEnumerable<object> dictionaryValues = GetPrimitiveDictionaryKeys(data, valueType, count);
                            foreach (object value in dictionaryValues)
                            {
                                if (index == count)
                                {
                                    break;
                                }

                                object key = this.Read(
                                    keyType,
                                    StringUtils.Combine(path, index.ToString(), KeyDirectoryString),
                                    storer,
                                    entries,
                                    references);

                                dictionary.Add(key, value);

                                index++;
                            }

                            return dictionary;
                        }
                    }
                }
            }
            else
            {
                // Dictionary is empty -> returns empty dictionary

                IDictionary dictionary = this.CreateDictionary(dictionaryType, keyType, valueType, count);
                references.Add(path, dictionary);
                return dictionary;
            }
        }

        /// <summary>
        /// Tries read the entries of the data by the path and creates a new multidimensional array.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <param name="className"> Name of the class. </param>
        /// <param name="values"> Values of the class. </param>
        /// <param name="path"> The pathy. </param>
        /// <param name="storer"> The storer. </param>
        /// <param name="entries"> List of the entries of the storer. </param>
        /// <param name="references"> Dictionary that contains references to the objects. </param>
        /// <returns> Multidimensional array if multidimensional array has been created successfully, otherwise null. </returns>
        private object TryReadMDArray(string path, Type type, string className, IDictionary<string, string> values, IReadOnlyDataStorer storer, LinkedList<IDataStorerEntry> entries, Dictionary<string, object> references)
        {
            // Tries get the type of the elements of the multidimensional array

            Type elementType = GetMDArrayElementType(type, className);
            if (elementType == null)
            {
                // Type of the element is null -> multidimensional array isn't supported -> returns null
                return null;
            }

            // Reads the information about of the multidimensional array

            int rank = Convert.ToInt32(values[RankString]);

            int[] lengths = new int[rank];
            int totalLength = 1;
            for (int i = 0; i < rank; i++)
            {
                lengths[i] = Convert.ToInt32(values[StringUtils.Combine(LengthWithUnderscoreString, (i + 1).ToString())]);
                totalLength *= lengths[i];
            }

            if (totalLength != 0)
            {
                bool isPrimitive = values[PrimitiveString] == TrueString;

                if (!isPrimitive)
                {
                    // Elements of the multidimensional array aren't primitive - reads their from different files

                    Array array = this.CreateArray(elementType, lengths);
                    references.Add(path, array);

                    for (int i = 0; i < totalLength; i++)
                    {
                        int[] indices = this.Read(
                            typeof(int[]),
                            StringUtils.Combine(path, i.ToString(), IndicesDirectoryString),
                            storer,
                            entries,
                            references) as int[];

                        object value = this.Read(
                                elementType,
                                StringUtils.Combine(path, i.ToString(), ValueDirectoryString),
                                storer,
                                entries,
                                references);

                        array.SetValue(value, indices);
                    }

                    return array;
                }
                else
                {
                    // Elements of the multidimensional array are primitive - reads their from single file

                    byte[] data = ReadAndRemoveEntry(path, MDArrayFileName, storer, entries);

                    Array array = this.GetPrimitiveArray(data, elementType, lengths, totalLength);
                    references.Add(path, array);

                    return array;
                }
            }
            else
            {
                // Multidimensional array is empty -> returns empty multidimensional array

                Array array = this.CreateArray(elementType, lengths);
                references.Add(path, array);
                return array;
            }
        }

        /// <summary>
        /// Tries read an entry of the data by the path and creates a new class/struct.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <param name="className"> Name of the class. </param>
        /// <param name="values"> Values of the class. </param>
        /// <param name="path"> The path. </param>
        /// <param name="storer"> The storer. </param>
        /// <param name="entries"> List of the entries of the storer. </param>
        /// <param name="references"> Dictionary that contains references to the objects. </param>
        /// <returns> Graph if graph has been created successfully, otherwise null. </returns>
        private object TryReadFormattable(string path, Type type, string className, IDictionary<string, string> values, IReadOnlyDataStorer storer, LinkedList<IDataStorerEntry> entries, Dictionary<string, object> references)
        {
            // Tries get the formattable type that is related (same) with the declared type

            FormattableType formattableType = this.formattableTypes.FindOrDefault(f => f.Name == className && f.IsRelatedType(type, false));
            if (formattableType == null)
            {
                // Tries get the formattable type that is related (not same, derived) with the declared type
                formattableType = this.formattableTypes.FindOrDefault(f => f.IsRelatedType(type, true));
            }

            if (formattableType == null)
            {
                // Formattable type is null -> type isn't supported -> returns null                
                return null;
            }

            object graph = formattableType.CreateInstance(type);
            references.Add(path, graph);

            formattableType.OnDeserializing(graph);

            // Restores the keys and values of the objects

            FormattableValue[] formattableValues = formattableType.GetValues(type);
            int length = formattableValues.Length;

            FormattableValue value = null;
            Type valueType = null;
            string valueName = "";

            for (int i = 0; i < length; i++)
            {
                value = formattableValues[i];
                valueType = value.Type;
                valueName = value.Name;

                if (this.IsPrimitive(valueType))
                {
                    // Value of the object is primitive - reads it from information file

                    Func<string, object> primitiveConverter = this.GetPrimitiveConverterToObject(valueType);

                    if (values.ContainsKey(value.Name))
                    {
                        value.SetValue(graph, primitiveConverter(values[valueName]));
                    }
                    else
                    {
                        if (!value.Optional)
                        {
                            throw new KeyCanNotLoadedException(formattableType, valueName);
                        }
                    }
                }
                else
                {
                    // Value of the object isn't primitive - reads it from special file

                    try
                    {
                        value.SetValue(graph,
                            this.Read(
                                 value.Type,
                                 StringUtils.Combine(path, valueName, DirectorySeparatorString),
                                 storer,
                                 entries,
                                 references
                        ));
                    }
                    catch (Exception ex)
                    {
                        // Thrown error if value isn't optional

                        if (!value.Optional)
                        {
                            throw new KeyCanNotLoadedException(formattableType, valueName, ex);
                        }
                    }
                }
            }

            formattableType.OnDeserialized(graph);

            // Makes the substitution (if required)

            object substitution = formattableType.Substitution(graph);
            if (substitution != graph)
            {
                references.Remove(path);
                references.Add(path, substitution);

                // Returns the substituted graph

                return substitution;
            }
            else
            {
                // Returns the created graph

                return graph;
            }
        }

        /// <summary>
        /// Reads a entry and remove associated with the path the entry. </summary>
        /// <param name="path"> The path. </param>
        /// <param name="name"> Name of data. </param>
        /// <param name="storer"> The storer. </param>
        /// <param name="entries"> List of the entries of the storer. </param>
        /// <returns> Data of the entry. </returns>
        private static byte[] ReadAndRemoveEntry(string path, string name, IReadOnlyDataStorer storer, LinkedList<IDataStorerEntry> entries)
        {
            LinkedListNode<IDataStorerEntry> entryNode = FindEntryNode(path, name, entries);
            entries.Remove(entryNode);
            return storer.Read(entryNode.Value);
        }

        /// <summary>
        /// Finds an entry node. </summary>
        /// <param name="path"> The path. </param>
        /// <param name="name"> Name of file. </param>
        /// <param name="entries"> List of the entries of the storer. </param>
        /// <returns> Node of the entry. </returns>
        private static LinkedListNode<IDataStorerEntry> FindEntryNode(string path, string name, LinkedList<IDataStorerEntry> entries)
        {
            string infoFile = StringUtils.Combine(path, name);

            LinkedListNode<IDataStorerEntry> entryNode = entries.Find(e => e.Path == infoFile);
            if (entryNode == null)
            {
                throw new DataNotFoundException(infoFile);
            }

            return entryNode;
        }
    }
}