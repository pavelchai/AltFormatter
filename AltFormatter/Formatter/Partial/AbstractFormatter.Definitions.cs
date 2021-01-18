/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;

namespace AltFormatter.Formatter
{
    /// <summary>
    /// Represents a base class for the formatter.
    /// </summary>
    public abstract partial class AbstractFormatter
    {
        /// <summary>
        /// Constant for the "True" string
        /// </summary>
        private const string TrueString = "true";

        /// <summary>
        /// Constant for the "False" string
        /// </summary>
        private const string FalseString = "false";

        /// <summary>
        /// Constant for the root node
        /// </summary>
        private const string RootNodeString = "root";

        /// <summary>
        /// Constant for the class node
        /// </summary>
        private const string ClassNodeString = "class";

        /// <summary>
        /// Constant for the value node
        /// </summary>
        private const string ValueNodeString = "value";

        /// <summary>
        /// Constant for the key node
        /// </summary>
        private const string KeyNodeString = "key";

        /// <summary>
        /// Constant for the keys node
        /// </summary>
        private const string KeysNodeString = "keys";

        /// <summary>
        /// Constant for the name attribute
        /// </summary>
        private const string NameAttributeString = "name";

        /// <summary>
        /// Constant for the primitive type
        /// </summary>
        private const string PrimitiveString = "primitive";

        /// <summary>
        /// Constant for the primitive type of the keys
        /// </summary>
        private const string PrimitiveKeysString = "primitive_keys";

        /// <summary>
        /// Constant for the primitive type of the values
        /// </summary>
        private const string PrimitiveValuesString = "primitive_values";

        /// <summary>
        /// Constant for the count of the collections/dictionaries
        /// </summary>
        private const string CountString = "count";

        /// <summary>
        /// Constant for the length of multidimensional array (with _)
        /// </summary>
        private const string LengthWithUnderscoreString = "length_";

        /// <summary>
        ///  Constant for the directory separator
        /// </summary>
        private const string DirectorySeparatorString = "/";

        /// <summary>
        /// Constant for the key directory
        /// </summary>
        private const string KeyDirectoryString = "/Key/";

        /// <summary>
        /// Constant for the value directory
        /// </summary>
        private const string ValueDirectoryString = "/Value/";

        /// <summary>
        /// Constant for the indices directory
        /// </summary>
        private const string IndicesDirectoryString = "/Indices/";

        /// <summary>
        /// Constant for the rank of the multidimensional array
        /// </summary>
        private const string RankString = "rank";

        /// <summary>
        /// Constant for the "Reference to" string
        /// </summary>
        private const string ReferenceToString = "reference_to";

        /// <summary>
        /// Name of the information file.
        /// </summary>
        private const string InfoFileName = "information.xml";

        /// <summary>
        /// Name of the collection file.
        /// </summary>
        private const string CollectionFileName = "collection.data";

        /// <summary>
        /// Name of the multidimensional array file.
        /// </summary>
        private const string MDArrayFileName = "multidimensional_array.data";

        /// <summary>
        /// Name of the dictionary file.
        /// </summary>
        private const string DictionaryFileName = "dictionary.data";

        /// <summary>
        /// Name of the dictionary keys file.
        /// </summary>
        private const string DictionaryKeysFileName = "dictionary_keys.data";

        /// <summary>
        /// Name of the dictionary values file.
        /// </summary>
        private const string DictionaryValuesFileName = "dictionary_values.data";

        /// <summary>
        /// Identifier of the null class.
        /// </summary>
        private const string NullIdentifier = "null_value";

        /// <summary>
        /// Identifier of the one dimensional <see cref="Array"></see> class.
        /// </summary>
        private const string ArrayIdentifier = "array";

        /// <summary>
        /// Identifier of the multidimensional <see cref="Array"></see> class.
        /// </summary>
        private const string MDArrayIdentifier = "multidimensional_array";

        /// <summary>
        /// Identifier of the <see cref="List{T}"></see> class.
        /// </summary>
        private const string ListIdentifier = "list";

        /// <summary>
        /// Identifier of the <see cref="LinkedList{T}"></see> class.
        /// </summary>
        private const string LinkedListIdentifier = "linked_list";

        /// <summary>
        /// Identifier of the <see cref="HashSet{T}"></see> class.
        /// </summary>
        private const string HashSetIdentifier = "hash_set";

        /// <summary>
        /// Identifier of the <see cref="SortedSet{T}"></see> class.
        /// </summary>
        private const string SortedSetIdentifier = "sorted_set";

        /// <summary>
        /// Identifier of the <see cref="Queue{T}"></see> class.
        /// </summary>
        private const string QueueIdentifier = "queue";

        /// <summary>
        /// Identifier of the <see cref="SortedSet{T}"></see> class.
        /// </summary>
        private const string StackIdentifier = "stack";

        /// <summary>
        /// Identifier of the <see cref="ConcurrentQueue{T}"></see> class.
        /// </summary>
        private const string ConcurrentQueueIdentifier = "concurrent_queue";

        /// <summary>
        /// Identifier of the <see cref="ConcurrentStack{T}"></see> class.
        /// </summary>
        private const string ConcurrentStackIdentifier = "concurrent_stack";

        /// <summary>
        /// Identifier of the <see cref="ConcurrentBag{T}"></see> class.
        /// </summary>
        private const string ConcurrentBagIdentifier = "concurrent_bag";

        /// <summary>
        /// Identifier of the <see cref="Dictionary{K,V}"></see> class.
        /// </summary>
        private const string DictionaryIdentifier = "dictionary";

        /// <summary>
        /// Identifier of the <see cref="SortedDictionary{K,V}"></see> class.
        /// </summary>
        private const string SortedDictionaryIdentifier = "sorted_dictionary";

        /// <summary>
        /// Identifier of the <see cref="SortedList{K,V}"></see> class.
        /// </summary>
        private const string SortedListIdentifier = "sorted_list";

        /// <summary>
        /// Identifier of the <see cref="ConcurrentDictionary{K,V}"></see> class.
        /// </summary>
        private const string ConcurrentDictionaryIdentifier = "concurrent_dictionary";

        /// <summary>
        /// Invariant culture.
        /// </summary>
        private static readonly CultureInfo invariantCulture = CultureInfo.InvariantCulture;

        /// <summary>
        /// Concurrency level.
        /// </summary>
        private static int concurrencyLevel = 4 * Environment.ProcessorCount;

        /// <summary>
        /// Void type.
        /// </summary>
        private readonly static Type voidType = typeof(void);

        /// <summary>
        /// Type of the <see cref="string"></see>.
        /// </summary>
        private readonly static Type StringType = typeof(string);

        /// <summary>
        /// Type of the <see cref="IEnumerable{T}"></see>.
        /// </summary>
        private readonly static Type iEnumerableType = typeof(IEnumerable<>);

        /// <summary>
        /// Type of the <see cref="Array"></see>.
        /// </summary>
        private readonly static Type arrayType = typeof(Array);

        /// <summary>
        /// Type of the <see cref="List{T}"></see>.
        /// </summary>
        private readonly static Type listType = typeof(List<>);

        /// <summary>
        /// Type of the <see cref="LinkedList{T}"></see>.
        /// </summary>
        private readonly static Type linkedListType = typeof(LinkedList<>);

        /// <summary>
        /// Type of the <see cref="HashSet{T}"></see>.
        /// </summary>
        private readonly static Type hashSetType = typeof(HashSet<>);

        /// <summary>
        /// Type of the <see cref="SortedSet{T}"></see>.
        /// </summary>
        private readonly static Type sortedSetType = typeof(SortedSet<>);

        /// <summary>
        /// Type of the <see cref="Queue{T}"></see>.
        /// </summary>
        private readonly static Type queueType = typeof(Queue<>);

        /// <summary>
        /// Type of the <see cref="Stack{T}"></see>.
        /// </summary>
        private readonly static Type stackType = typeof(Stack<>);

        /// <summary>
        /// Type of the <see cref="ConcurrentQueue{T}"></see>.
        /// </summary>
        private readonly static Type concurrentQueueType = typeof(ConcurrentQueue<>);

        /// <summary>
        /// Type of the <see cref="Stack{T}"></see>.
        /// </summary>
        private readonly static Type concurrentStackType = typeof(ConcurrentStack<>);

        /// <summary>
        /// Type of the <see cref="Stack{T}"></see>.
        /// </summary>
        private readonly static Type concurrentBagType = typeof(ConcurrentBag<>);

        /// <summary>
        /// Type of the <see cref="IDictionary{TKey,TValue}"></see>.
        /// </summary>
        private readonly static Type iDictionaryType = typeof(IDictionary<,>);

        /// <summary>
        /// Type of the <see cref="Dictionary{TKey,TValue}"></see>.
        /// </summary>
        private readonly static Type dictionaryType = typeof(Dictionary<,>);

        /// <summary>
        /// Type of the <see cref="SortedDictionary{TKey,TValue}"></see>.
        /// </summary>
        private readonly static Type sortedDictionaryType = typeof(SortedDictionary<,>);

        /// <summary>
        /// Type of the <see cref="SortedList{TKey,TValue}"></see>.
        /// </summary>
        private readonly static Type sortedListType = typeof(SortedList<,>);

        /// <summary>
        /// Type of the <see cref="ConcurrentDictionary{TKey,TValue}"></see>.
        /// </summary>
        private readonly static Type concurrentDictionaryType = typeof(ConcurrentDictionary<,>);
    }
}