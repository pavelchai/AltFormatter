/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AltFormatter.Formatter
{
    /// <summary>
    /// Represents a base class for the formatter.
    /// </summary>
    public abstract partial class AbstractFormatter
    {
        /// <summary>
        /// Gets the data of the collection with the primitive elements.
        /// </summary>
        /// <param name="collection"> The collection. </param>
        /// <param name="elementType"> Type of the element. </param>
        /// <returns> Array of the bytes. </returns>
        protected virtual byte[] GetPrimitiveCollectionData(IEnumerable collection, Type elementType)
        {
            Action<StringBuilder, object> writer = this.GetValueWriter(elementType);
            StringBuilder builder = new StringBuilder();

            IList list = collection as IList;
            if (list != null)
            {
                int count = list.Count;

                if (count > 0)
                {
                    int countN1 = count - 1;
                    for (int i = 0; i < countN1; i++)
                    {
                        writer(builder, list[i]);
                        builder.Append('\n');
                    }

                    writer(builder, list[countN1]);
                }

                return builder.ToString().GetBytes();
            }
            else
            {
                foreach (var item in collection)
                {
                    writer(builder, item);
                    builder.Append('\n');
                }

                if (builder.Length > 0)
                {
                    builder.Remove(builder.Length - 1, 1);
                }

                return builder.ToString().GetBytes();
            }
        }

        /// <summary>
        /// Gets the data of the dictionary with the primitive keys and values.
        /// </summary>
        /// <param name="dictionary"> The dictionary. </param>
        /// <param name="keyType"> Type of the key. </param>
        /// <param name="valueType"> Type of the value. </param>
        /// <returns> Array of the bytes. </returns>
        protected virtual byte[] GetPrimitiveDictionaryData(IDictionary dictionary, Type keyType, Type valueType)
        {
            Action<StringBuilder, object> keyWriter = this.GetValueWriter(keyType);
            Action<StringBuilder, object> valueWriter = this.GetValueWriter(valueType);
            StringBuilder builder = new StringBuilder();

            foreach (DictionaryEntry pair in dictionary)
            {
                keyWriter(builder, pair.Key);
                builder.Append('\t');
                valueWriter(builder, pair.Value);
                builder.Append('\n');
            }

            if (builder.Length > 0)
            {
                builder.Remove(builder.Length - 1, 1);
            }

            return builder.ToString().GetBytes();
        }

        /// <summary>
        /// Gets the data of the dictionary with the primitive keys (keys only).
        /// </summary>
        /// <param name="dictionary"> The dictionary. </param>
        /// <param name="keyType"> Type of the key. </param>
        /// <returns> Array of the bytes. </returns>
        protected virtual byte[] GetPrimitiveDictionaryKeysData(IDictionary dictionary, Type keyType)
        {
            Action<StringBuilder, object> keyWriter = this.GetValueWriter(keyType);
            StringBuilder builder = new StringBuilder();

            foreach (DictionaryEntry pair in dictionary)
            {
                keyWriter(builder, pair.Key);
                builder.Append('\n');
            }

            if (builder.Length > 0)
            {
                builder.Remove(builder.Length - 1, 1);
            }

            return builder.ToString().GetBytes();
        }

        /// <summary>
        /// Gets the data of the dictionary with the primitive values (values only).
        /// </summary>
        /// <param name="dictionary"> The dictionary. </param>
        /// <param name="valueType"> Type of the value. </param>
        /// <returns> Array of the bytes. </returns>
        protected virtual byte[] GetPrimitiveDictionaryValuesData(IDictionary dictionary, Type valueType)
        {
            Action<StringBuilder, object> valueWriter = this.GetValueWriter(valueType);
            StringBuilder builder = new StringBuilder();

            foreach (DictionaryEntry pair in dictionary)
            {
                valueWriter(builder, pair.Value);
                builder.Append('\n');
            }

            if (builder.Length > 0)
            {
                builder.Remove(builder.Length - 1, 1);
            }

            return builder.ToString().GetBytes();
        }

        /// <summary>
        /// Gets the data of the multidimensional array with the primitive elements.
        /// </summary>
        /// <param name="array"> The array. </param>
        /// <param name="elementType"> Type of the element. </param>
        /// <param name="lengths"> Lengths. </param>
        /// <returns> Array of the bytes. </returns>
        protected virtual byte[] GetPrimitiveMDArrayData(Array array, Type elementType, int[] lengths)
        {
            Action<StringBuilder, object> writer = this.GetValueWriter(elementType);
            StringBuilder builder = new StringBuilder();

            IEnumerable<int[]> product = CreateCartesianProduct(lengths);
            int length;

            foreach (var p in product)
            {
                length = p.Length;

                for (int k = 0; k < length; k++)
                {
                    builder.Append(p[k].ToString(invariantCulture));
                    builder.Append('\t');
                }

                writer(builder, array.GetValue(p));
                builder.Append('\n');
            }

            if (builder.Length > 0)
            {
                builder.Remove(builder.Length - 1, 1);
            }

            return builder.ToString().GetBytes();
        }

        /// <summary>
        /// Serializes an information to the array of the bytes.
        /// </summary>
        /// <param name="className"> Name of the class. </param>
        /// <param name="values"> Values (primitive). </param>
        /// <returns> Array of the bytes. </returns>
        protected virtual byte[] SerializeInfo(string className, IDictionary<string, string> values)
        {
            AltXmlDocument document = new AltXmlDocument(RootNodeString, "");

            AltXmlNode classNode = new AltXmlNode(ClassNodeString, "");
            document.RootNode.SubNodes.AddLast(classNode);

            AltXmlAttribute attribute = new AltXmlAttribute(NameAttributeString, className);
            classNode.Attributes.AddLast(attribute);

            foreach (var pair in values)
            {
                classNode.SubNodes.AddLast(new AltXmlNode(
                    pair.Key,
                    pair.Value ?? NullIdentifier));
            }

            return document.GetXmlData();
        }

        /// <summary>
        /// Gets a writer for the value.
        /// </summary>
        /// <param name="type"> Type of the value. </param>
        /// <returns> Writer for the value. </returns>
        private unsafe Action<StringBuilder, object> GetValueWriter(Type type)
        {
            Func<object, string> converter = this.GetPrimitiveConverterToString(type);

            if (type == StringType)
            {
                return (b, v) =>
                {
                    string converted = converter(v);
                    int length = converted.Length;
                    int index = 0, start = 0;
                    char c;

                    fixed (char* sPtr = converted)
                    {
                        char* ptr = sPtr;

                        while (index != length)
                        {
                            c = *ptr++;

                            if (c == '\r')
                            {
                                if (*(ptr + 1) == '\n')
                                {
                                    // \r\n
                                    b.Append(converted, start, index - start);
                                    b.Append("&#x0;");

                                    index += 2;
                                    ptr++;
                                    start = index;
                                }
                                else
                                {
                                    // \r
                                    b.Append(converted, start, index - start);
                                    b.Append("&#x2;");

                                    index++;
                                    start = index;
                                }
                            }
                            else
                            {
                                if (c == '\t' || c == '\n')
                                {
                                    b.Append(converted, start, index - start);

                                    if (c == '\t')
                                    {
                                        // \t
                                        b.Append("&#x3;");
                                    }
                                    else
                                    {
                                        // \n
                                        b.Append("&#x1;");
                                    }

                                    index++;
                                    start = index;
                                }
                                else
                                {
                                    // other
                                    index++;
                                }
                            }
                        }

                        if (start < length)
                        {
                            b.Append(converted, start, length - start);
                        }
                    }
                };
            }
            else
            {
                return (b, v) => b.Append(converter(v));
            }
        }
    }
}