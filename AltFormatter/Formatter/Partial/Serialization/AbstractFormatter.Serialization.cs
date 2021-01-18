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
        /// Serializes an object, or graph of objects with the given root to the array of bytes. </summary>
        /// <param name="graph"> 
        /// The object, or root of the object graph, to serialize. 
        /// All child objects of this root object are automatically serialized.
        /// </param>
        /// <returns> Array of bytes that associated with the graph if serialization has been finished successfully, otherwise null. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when graph is null.
        /// </exception>
        public byte[] Serialize<T>(T graph)
        {
            if (object.ReferenceEquals(graph, null))
            {
            	Validation.NotNull("Graph", graph as object);
                return null;
            }

            while (Interlocked.CompareExchange(ref this.formatterIsLoading, 1, 1) == 1)
            {
                // Waits while assemblies have been loaded
                Thread.Sleep(1);
            }

            try
            {
                string graphName = graph.ToString();

                Type type = typeof(T);

                if (this.IsPrimitive(type))
                {
                    // Tries serialize the object/graph of the objects (as primitive type)

                    Func<object, string> primitiveConverter = this.GetPrimitiveConverterToString(typeof(T));
                    return primitiveConverter(graph).GetBytes();
                }
                else
                {
                    // Tries serialize the object/graph of the objects (as non-primitive type)

                    Dictionary<object, string> references = new Dictionary<object, string>(100);
                    IWriteOnlyDataStorer storer = this.CreateWriteOnlyDataStorer();

                    this.Write(graph, "", storer, references);
                    return storer.Close();
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Writes an object (or graph of objects with the given root) in the storer.
        /// </summary>
        /// <param name="graph"> Object (or graph of objects with the given root). </param>
        /// <param name="path"> The path to the file. </param>
        /// <param name="storer"> The storer. </param>
        /// <param name="references"> Dictionary that contains references to the objects. </param>
        private void Write(object graph, string path, IWriteOnlyDataStorer storer, IDictionary<object, string> references)
        {
            if (graph == null)
            {
                this.WriteNull(path, storer);
                return;
            }

            Type graphType = graph.GetType();

            if (this.TryWriteCollection(graph, graphType, path, storer, references))
            {
                return;
            }

            if (this.TryWriteDictionary(graph, graphType, path, storer, references))
            {
                return;
            }

            if (this.TryWriteMDArray(graph, graphType, path, storer, references))
            {
                return;
            }

            if (this.TryWriteFormattable(graph, graphType, path, storer, references))
            {
                return;
            }

            throw new UnsupportedTypeException(graphType);
        }

        /// <summary>
        /// Tries write a collection in the storer.
        /// </summary>
        /// <param name="graph"> Object (or graph of objects with the given root). </param>
        /// <param name="collectionType"> Type of the collection. </param>
        /// <param name="path"> The path to the file. </param>
        /// <param name="storer"> The storer. </param>
        /// <param name="references"> Dictionary that contains references to the objects. </param>
        /// <returns> True if collection has been written (or is exist), otherwise false. </returns>
        private bool TryWriteCollection(object graph, Type collectionType, string path, IWriteOnlyDataStorer storer, IDictionary<object, string> references)
        {
            // Gets an identifier of the collection

            string identifier = GetCollectionTypeIndentifier(collectionType);
            if (identifier == null)
            {
                // If identifier is null -> collection isn't supported -> returns false
                return false;
            }

            // Tries write the collection in the storer

            IEnumerable enumerable = graph as IEnumerable;

            if (this.TryWriteValue(storer, path, references, enumerable, identifier))
            {
                Type elementType = (collectionType.IsArray) ? collectionType.GetElementType() : collectionType.GetGenericArguments()[0];

                bool isPrimitive = this.IsPrimitive(elementType);
                int count = 0;

                ICollection collection = enumerable as ICollection;
                if (collection != null)
                {
                    count = collection.Count;
                }
                else
                {
                    IEnumerator enumerator = enumerable.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        count++;
                    }
                }

                // Writes the information about of the collection in the document

                IDictionary<string, string> values = new Dictionary<string, string>()
                {
                    { PrimitiveString, isPrimitive ? TrueString : FalseString },
                    { CountString, count.ToString() },
                };

                this.WriteInfo(storer, path, identifier, values);

                // Writes the data of the collection in the storer

                if (count != 0)
                {
                    if (isPrimitive)
                    {
                        // Elements of the collection are primitive - writes their in single file

                        storer.Add(
                            StringUtils.Combine(path, CollectionFileName),
                            this.GetPrimitiveCollectionData(enumerable, elementType)
                        );
                    }
                    else
                    {
                        // Elements of the collection aren't primitive - writes their in differerent files

                        IList list = enumerable as IList;
                        if (list != null)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                this.Write(
                                    list[i],
                                    StringUtils.Combine(path, i.ToString(), DirectorySeparatorString),
                                    storer,
                                    references
                                );
                            }
                        }
                        else
                        {
                            int index = 0;
                            foreach (object item in enumerable)
                            {
                                this.Write(
                                    item,
                                    StringUtils.Combine(path, index.ToString(), DirectorySeparatorString),
                                    storer,
                                    references
                                );
                                index++;
                            }
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Tries write a dictionary in the storer.
        /// </summary>
        /// <param name="graph"> Object (or graph of objects with the given root). </param>
        /// <param name="dicType"> Type of the dictionary. </param>
        /// <param name="path"> The path to the file. </param>
        /// <param name="storer"> The storer. </param>
        /// <param name="references"> Dictionary that contains references to the objects. </param>
        /// <returns> True if dictionary has been written (or is exist), otherwise false. </returns>
        private bool TryWriteDictionary(object graph, Type dicType, string path, IWriteOnlyDataStorer storer, IDictionary<object, string> references)
        {
            // Gets an identifier of the dictionary

            string identifier = GetDictionaryTypeIndentifier(dicType);
            if (identifier == null)
            {
                // If identifier is null -> dictionary isn't supported -> returns false
                return false;
            }

            // Tries write the dictionary in the storer

            IDictionary dictionary = graph as IDictionary;

            if (this.TryWriteValue(storer, path, references, dictionary, identifier))
            {
                Type[] genericArgs = dicType.GetGenericArguments();
                Type keyType = genericArgs[0];
                Type valueType = genericArgs[1];

                bool isPrimitiveKeys = IsPrimitive(keyType);
                bool isPrimitiveValues = IsPrimitive(valueType);
                int count = dictionary.Count;

                // Writes the information about of the dictionary in the document

                IDictionary<string, string> values = new Dictionary<string, string>()
                {
                    { PrimitiveKeysString, isPrimitiveKeys ? TrueString : FalseString },
                    { PrimitiveValuesString, isPrimitiveValues ? TrueString : FalseString },
                    { CountString, count.ToString() },
                };

                this.WriteInfo(storer, path, identifier, values);

                // Writes the data of the dictionary in the storer

                if (count != 0)
                {
                    if (isPrimitiveKeys && isPrimitiveValues)
                    {
                        // Keys and values of the dictionary are primitive - writes their in single file

                        storer.Add(
                            StringUtils.Combine(path, DictionaryFileName),
                            this.GetPrimitiveDictionaryData(dictionary, keyType, valueType)
                        );
                    }
                    else
                    {
                        if (isPrimitiveKeys)
                        {
                            // Keys of the dictionary are primitive - writes their in single file

                            storer.Add(
                                StringUtils.Combine(path, DictionaryKeysFileName),
                                this.GetPrimitiveDictionaryKeysData(dictionary, keyType)
                            );
                        }
                        else
                        {
                            // Keys aren't primitive - writes their in different files

                            int index = 0;
                            foreach (DictionaryEntry pair in dictionary)
                            {
                                this.Write(
                                    pair.Key,
                                    StringUtils.Combine(path, index.ToString(), KeyDirectoryString),
                                    storer,
                                    references
                                );
                                index++;
                            }
                        }

                        if (isPrimitiveValues)
                        {
                            // Values of the dictionary are primitive - writes their in single file

                            storer.Add(
                                StringUtils.Combine(path, DictionaryValuesFileName),
                                this.GetPrimitiveDictionaryValuesData(dictionary, valueType)
                            );
                        }
                        else
                        {
                            // Values of the dictionary aren't primitive - writes their in different files

                            int index = 0;
                            foreach (DictionaryEntry pair in dictionary)
                            {
                                this.Write(
                                    pair.Value,
                                    StringUtils.Combine(path, index.ToString(), ValueDirectoryString),
                                    storer,
                                    references
                                );
                                index++;
                            }
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Tries write a multidimensional array in the storer.
        /// </summary>
        /// <param name="graph"> Object (or graph of objects with the given root). </param>
        /// <param name="mdArrayType"> Type of the multidimensional array. </param>
        /// <param name="path"> The path to the file. </param>
        /// <param name="storer"> The storer. </param>
        /// <param name="references"> Dictionary that contains references to the objects. </param>
        /// <returns> True if multidimensional array has been written (or is exist), otherwise false. </returns>
        private bool TryWriteMDArray(object graph, Type mdArrayType, string path, IWriteOnlyDataStorer storer, IDictionary<object, string> references)
        {
            // Gets an identifier of the multidimensional array

            string identifier = GetMDArrayTypeIndentifier(mdArrayType);
            if (identifier == null)
            {
                // If identifier is null -> multidimensional array isn't supported -> returns false
                return false;
            }

            // Tries write the multidimensional array in the storer

            Array array = graph as Array;

            if (this.TryWriteValue(storer, path, references, array, identifier))
            {
                Type elementType = mdArrayType.GetElementType();
                bool isPrimitive = this.IsPrimitive(elementType);

                // Writes the information about of the multidimensional array in the document

                int rank = array.Rank;
                int[] lengths = new int[rank];
                bool isEmpty = false;

                IDictionary<string, string> values = new Dictionary<string, string>(rank + 2)
                {
                    { RankString, array.Rank.ToString() },
                };

                int length = 0;
                for (int i = 0; i < rank; i++)
                {
                    length = array.GetLength(i);
                    lengths[i] = length;

                    if (!isEmpty && length == 0)
                    {
                        isEmpty = true;
                    }

                    values.Add(StringUtils.Combine(LengthWithUnderscoreString, (i + 1).ToString()), length.ToString());
                }

                values.Add(PrimitiveString, isPrimitive ? TrueString : FalseString);

                this.WriteInfo(storer, path, identifier, values);

                if (!isEmpty)
                {
                    if (isPrimitive)
                    {
                        // Elements of the multidimensional array are primitive - writes their in single file

                        storer.Add(
                            StringUtils.Combine(path, MDArrayFileName),
                            this.GetPrimitiveMDArrayData(array, elementType, lengths)
                        );
                    }
                    else
                    {
                        // Elements of the multidimensional array aren't primitive - writes their in different files

                        IEnumerable<int[]> product = CreateCartesianProduct(lengths);

                        int index = 0;
                        string indexString = "";
                        foreach (int[] p in product)
                        {
                            indexString = index.ToString();
                            index++;

                            Write(
                                p,
                                StringUtils.Combine(path, indexString, IndicesDirectoryString),
                                storer,
                                references
                            );

                            Write(
                                array.GetValue(p),
                                StringUtils.Combine(path, indexString, ValueDirectoryString),
                                storer,
                                references
                            );
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Tries write a formattable object in the storer.
        /// </summary>
        /// <param name="graph"> Object (or graph of objects with the given root). </param>
        /// <param name="type"> Type of the object. </param>
        /// <param name="path"> The path to the file. </param>
        /// <param name="storer"> The storer. </param>
        /// <param name="references"> Dictionary that contains references to the objects. </param>
        /// <returns> True if formattable objecthas been written (or is exist), otherwise false. </returns>
        private bool TryWriteFormattable(object graph, Type type, string path, IWriteOnlyDataStorer storer, IDictionary<object, string> references)
        {
            // Gets a formattable type for the specified type

            FormattableType formattableType = this.formattableTypes.FindOrDefault(f => f.IsRelatedType(type, true));
            if (formattableType == null)
            {
                // If formattable type isn't exists -> type isn't supported -> returns false
                return false;
            }

            // Tries write the object in the storer

            string className = formattableType.Name;

            if (this.TryWriteValue(storer, path, references, graph, className))
            {
                formattableType.OnSerializing(graph);

                FormattableValue[] formattableValues = formattableType.GetValues(type);
                FormattableValue formattableValue;
                int length = formattableValues.Length;
                object value;
                Type valueType;

                IDictionary<string, string> values = new Dictionary<string, string>(length);

                for (int i = 0; i < length; i++)
                {
                    formattableValue = formattableValues[i];

                    value = formattableValue.GetValue(graph);
                    valueType = formattableValue.Type;

                    if (this.IsPrimitive(valueType))
                    {
                        // Value of the object is primitive - writes it in the information file

                        values.Add(formattableValue.Name, this.GetPrimitiveConverterToString(valueType)(value));
                    }
                    else
                    {
                        // Value of the object isn't primitive - writes it in new file

                        try
                        {
                            // Tries write a value in the new file
                            this.Write(
                                value,
                                StringUtils.Combine(path, formattableValue.Name, DirectorySeparatorString),
                                storer,
                                references
                            );
                        }
                        catch (Exception ex)
                        {
                            // Thrown error if value isn't optional
                            if (!formattableValue.Optional)
                            {
                                throw new KeyCanNotSavedException(formattableType, formattableType.Name, ex);
                            }
                        }
                    }
                }

                formattableType.OnSerialized(graph);

                this.WriteInfo(storer, path, className, values);
            }

            return true;
        }

        /// <summary>
        /// Writes a null xml file in the storer.
        /// </summary>
        /// <param name="path"> The path to the file. </param>
        /// <param name="storer"> The storer. </param>
        private void WriteNull(string path, IWriteOnlyDataStorer storer)
        {
            this.WriteInfo(storer, path, NullIdentifier, new Dictionary<string, string>());
        }

        /// <summary>
        /// Writes an info xml file in the storer.
        /// </summary>
        /// <param name="storer"> The storer. </param>
        /// <param name="path"> The path to the file. </param>
        /// <param name="className"> Name of the class. </param>
        /// <param name="values"> Values. </param>
        private void WriteInfo(IWriteOnlyDataStorer storer, string path, string className, IDictionary<string, string> values)
        {
            storer.Add(
                StringUtils.Combine(path, InfoFileName),
                this.SerializeInfo(className, values)
            );
        }

        /// <summary>
        /// Tries write reference to object in information file and gets information about of continuation of writing.
        /// </summary>
        /// <param name="storer"> The storer. </param>
        /// <param name="path"> The path to the file. </param>
        /// <param name="references"> Dictionary that contains references to objects.</param>
        /// <param name="graph"> Object (or graph of objects with the given root). </param>
        /// <param name="className"> Name of the class. </param>
        /// <returns> Returns true if writing should be continued (for first object), otherwise false (if object with same reference has been found). </returns>
        /// <remarks> The method adds creates an information file if object has been written before. </remarks>
        private bool TryWriteValue(IWriteOnlyDataStorer storer, string path, IDictionary<object, string> references, object graph, string className)
        {
            if (graph.GetType().IsClass)
            {
                string reference;
                if (references.TryGetValue(graph, out reference))
                {
                    IDictionary<string, string> values = new Dictionary<string, string>()
                    {
                        { ReferenceToString, reference }
                    };

                    this.WriteInfo(storer, path, className, values);
                    return false;
                }
                else
                {
                    references.Add(graph, path);
                    return true;
                }
            }

            return true;
        }

        /// <summary>
        /// Creates a cartesian product from the lengths of the multidimensional array.
        /// </summary>
        /// <param name="lengths"> Lengths of the multidimensional array. </param>
        /// <returns> The cartesian product. </returns>
        private static IEnumerable<int[]> CreateCartesianProduct(params int[] lengths)
        {
            int length = lengths.Length;

            if (length == 0)
            {
                return new int[][] { new int[0] };
            }

            if (length == 1)
            {
                int count = lengths[0];
                int[][] product = new int[count][];
                for (int i = 0; i < count; i++)
                {
                    product[i] = new int[] { i };
                }
                return product;
            }

            int[][] arrays = new int[length][];
            int len;
            for (int i = 0; i < length; i++)
            {
                len = lengths[i];
                arrays[i] = new int[len];
                for (int k = 0; k < len; k++)
                {
                    arrays[i][k] = k;
                }
            }

            return CombinationUtils.CartesianProduct<int>(arrays);
        }
    }
}