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
        /// Gets a collection with the primitive elements from the data.
        /// </summary>
        /// <param name="data"> Data of the collection. </param>
        /// <param name="collectionType"> Type of the collection. </param>
        /// <param name="elementType"> Type of the element. </param>
        /// <param name="count"> Count of the elements. </param>
        /// <returns> The collection. </returns>
        protected virtual IEnumerable GetPrimitiveCollection(byte[] data, Type collectionType, Type elementType, int count)
        {
            IEnumerable enumerable = this.CreateCollection(collectionType, elementType, count);
            Func<string, object> reader = GetValueReader(elementType);

            if (reader != null)
            {
                IEnumerable<string> lines = StringUtils.SplitLines(data.GetString());

                Array array = enumerable as Array;
                if (array != null)
                {
                    int index = -1;
                    foreach (string line in lines)
                    {
                        if (++index == count)
                        {
                            break;
                        }

                        array.SetValue(reader(line), index);
                    }
                    return enumerable;
                }

                IList list = enumerable as IList;
                if (list != null)
                {
                    int index = 0;
                    foreach (string line in lines)
                    {
                        if (index++ == count)
                        {
                            break;
                        }

                        list.Add(reader(line));
                    }
                    return enumerable;
                }

                using (CollectionWriter writer = new CollectionWriter(this, enumerable))
                {
                    int index = 0;
                    foreach (string line in lines)
                    {
                        if (index++ == count)
                        {
                            break;
                        }

                        writer.Add(reader(line));
                    }
                    return enumerable;
                }
            }

            return enumerable;
        }

        /// <summary>
        /// Gets a dictionary with primitive keys and values from the data.
        /// </summary>
        /// <param name="data"> Data of the dictionary. </param>
        /// <param name="dictionaryType"> Type of the dictionary. </param>
        /// <param name="keyType"> Type of the key. </param>
        /// <param name="valueType"> Type of the value. </param>
        /// <param name="count"> Count of the elements. </param>
        /// <returns> The dictionary. </returns>
        protected virtual IDictionary GetPrimitiveDictionary(byte[] data, Type dictionaryType, Type keyType, Type valueType, int count)
        {
            IDictionary dictionary = this.CreateDictionary(dictionaryType, keyType, valueType, count);

            Func<string, object> keyReader = GetValueReader(keyType);
            Func<string, object> valueReader = GetValueReader(valueType);

            if (keyReader != null && valueReader != null)
            {
                string[] keyValue;
                int index = 0;
                IEnumerable<string> lines = StringUtils.SplitLines(data.GetString());

                foreach (string line in lines)
                {
                    if (index++ == count)
                    {
                        break;
                    }

                    keyValue = line.Split('\t');
                    dictionary.Add(keyReader(keyValue[0]), valueReader(keyValue[1]));
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Gets the primitive keys of the dictionary from the data.
        /// </summary>
        /// <param name="data"> Data. </param>
        /// <param name="keyType"> Type of the key. </param>
        /// <param name="count"> Count of the elements. </param>
        /// <returns> The keys. </returns>
        protected virtual IEnumerable<object> GetPrimitiveDictionaryKeys(byte[] data, Type keyType, int count)
        {
            return this.GetPrimitiveDictionaryKeysValues(data, keyType, count);
        }

        /// <summary>
        /// Gets the primitive values of the dictionary from the data.
        /// </summary>
        /// <param name="data"> Data. </param>
        /// <param name="valueType"> Type of the values. </param>
        /// <param name="count"> Count of the elements. </param>
        /// <returns> The values. </returns>
        protected virtual IEnumerable<object> GetPrimitiveDictionaryValues(byte[] data, Type valueType, int count)
        {
            return this.GetPrimitiveDictionaryKeysValues(data, valueType, count);
        }

        /// <summary>
        /// Gets an array with the primitive elements from data.
        /// </summary>
        /// <param name="data"> Data of the array. </param>
        /// <param name="elementType"> Type of the element. </param>
        /// <param name="lengths"> Length of the array. </param>
        /// <param name="totalLength"> Total length. </param>
        /// <returns> The array. </returns>
        protected virtual Array GetPrimitiveArray(byte[] data, Type elementType, int[] lengths, int totalLength)
        {
            Array array = this.CreateArray(elementType, lengths);
            Func<string, object> reader = GetValueReader(elementType);

            if (reader != null)
            {
                IEnumerable<string> lines = StringUtils.SplitLines(data.GetString());
                int rank = lengths.Length;
                IEnumerator<string> values = null;
                int index = 0;

                int[] indices = new int[rank];
                foreach (string line in lines)
                {
                    if (index++ == totalLength)
                    {
                        break;
                    }

                    values = StringUtils.Split(line, '\t').GetEnumerator();
                    for (int i = 0; i < rank; i++)
                    {
                        values.MoveNext();
                        indices[i] = Convert.ToInt32(values.Current);
                    }

                    values.MoveNext();
                    array.SetValue(reader(values.Current), indices);
                }
                return array;
            }

            return array;
        }

        /// <summary>
        /// Desrializes an information from the array of the bytes.
        /// </summary>
        /// <param name="data"> The array. </param>
        /// <param name="className"> Name of the class. </param>
        /// <returns> Values of the class. </returns>
        protected virtual IDictionary<string, string> DeserializeInfo(byte[] data, out string className)
        {
            AltXmlDocument document = new AltXmlDocument(data.GetString());

            AltXmlNode classNode = document.RootNode[ClassNodeString];
            AltXmlAttribute nameAttribute = classNode.GetAttribute(NameAttributeString);
            
            if (nameAttribute == null)
            {
                throw new DataNotFoundException(NameAttributeString);
            }

            className = nameAttribute.Value;

            LinkedList<AltXmlNode> subNodes = classNode.SubNodes;

            IDictionary<string, string> values = new Dictionary<string, string>(subNodes.Count);

            AltXmlNode xmlNode;
            for (var node = classNode.SubNodes.First; node != null; node = node.Next)
            {
                xmlNode = node.Value;
                values.Add(xmlNode.Name, xmlNode.Value != NullIdentifier ? xmlNode.Value : null);
            }

            return values;
        }

        /// <summary>
        /// Gets the primitive keys/values of the dictionary from the data.
        /// </summary>
        /// <param name="data"> Data. </param>
        /// <param name="type"> Type of the keys/values. </param>
        /// <param name="count"> Count of the elements. </param>
        /// <returns> The keys/values. </returns>
        private IEnumerable<object> GetPrimitiveDictionaryKeysValues(byte[] data, Type type, int count)
        {
            Func<string, object> reader = GetValueReader(type);

            IEnumerable<string> lines = StringUtils.SplitLines(data.GetString());
            int index = 0;

            if (reader != null)
            {
                foreach (string line in lines)
                {
                    if (index++ == count)
                    {
                        break;
                    }

                    yield return reader(line);
                }
            }
        }

        /// <summary>
        /// Gets a reader for the value.
        /// </summary>
        /// <param name="type"> Type of the value. </param>
        /// <returns> Reader for the value if value of the specified type may be read, otherwise null. </returns>
        private unsafe Func<string, object> GetValueReader(Type type)
        {
            Func<string, object> converter = this.GetPrimitiveConverterToObject(type);

            if (converter != null)
            {
                return v =>
                {
                    StringBuilder builder = new StringBuilder();
                    int length = v.Length;
                    int lengthN4 = length - 4;
                    int index = 0, start = 0;
                    char c0, c3;

                    fixed (char* sPtr = v)
                    {
                        char* ptr = sPtr;

                        while (index < lengthN4)
                        {
                            c0 = *ptr++;

                            if (c0 != '&')
                            {
                                index++;
                                continue;
                            }

                            if (*ptr != '#' || *(ptr + 1) != 'x' || *(ptr + 3) != ';')
                            {
                                index++;
                                continue;
                            }

                            c3 = *(ptr + 2);

                            if (c3 >= '0' && c3 <= '3')
                            {
                                builder.Append(v, start, index - start);

                                if (c3 == '0')
                                {
                                    builder.Append("\r\n");
                                }
                                else
                                {
                                    if (c3 == '1')
                                    {
                                        builder.Append('\n');
                                    }
                                    else
                                    {
                                        builder.Append(c3 == '3' ? '\t' : '\r');
                                    }
                                }

                                index += 5;
                                start = index;
                                ptr = ptr + 4;
                            }
                            else
                            {
                                index++;
                            }
                        }

                        if (start < length)
                        {
                            builder.Append(v, start, length - start);
                        }
                    }

                    return converter(builder.ToString());
                };
            }
            else
            {
                return null;
            }
        }
    }
}