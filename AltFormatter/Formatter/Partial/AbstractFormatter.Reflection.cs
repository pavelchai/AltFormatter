/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace AltFormatter.Formatter
{
    /// <summary>
    /// Represents a base class for the formatter.
    /// </summary>
    public abstract partial class AbstractFormatter
    {
        /// <summary>
        /// Represents a writer for the <see cref="ICollection"></see>.
        /// </summary>
        protected sealed class CollectionWriter : IDisposable
        {
            /// <summary>
            /// Determines whether collection is <see cref="Stack{T}"></see>/<see cref="System.Collections.Concurrent.ConcurrentStack{T}"></see> or not.
            /// </summary>
            private readonly bool collectionIsStack = false;

            /// <summary>
            /// Collected values in reverse order (used if <see cref="collectionIsStack"></see> is true).
            /// </summary>
            private readonly LinkedList<object> collectedValues = new LinkedList<object>();

            /// <summary>
            /// Function for adding a new values (equals <see cref="collectionAddAction"></see> if <see cref="collectionIsStack"></see> is false).
            /// </summary>
            private readonly Action<object> addAction;

            /// <summary>
            /// Function for adding the new values in specified collection.
            /// </summary>
            private readonly Action<object> collectionAddAction;

            /// <summary>
            /// Creates a new writer for the <see cref="ICollection"></see>.
            /// </summary>
            /// <param name="formatter"> The formatter. </param>
            /// <param name="enumerable"> The collection. </param>
            public CollectionWriter(AbstractFormatter formatter, IEnumerable enumerable)
            {
                // Gets type and generic type definition of the collection

                Type collectionType = enumerable.GetType();
                Type elementType = collectionType.GetGenericArguments()[0];
                Type collectionGTD = collectionType.GetGenericTypeDefinition();

                // Gets the methods of the collection

                MethodInfo methodInfo = null;
                MethodInfo[] methodsInfo = collectionType.GetMethods(BindingFlags.Instance | BindingFlags.Public);

                Func<string, Action<object>> getCallbackReturnMethod = (name) =>
                {
                    // Builds a callback for the public method with specified name
                    // (one parameter, return something)

                    Func<MethodInfo, bool> predicate = (m) =>
                    {
                        if (m.Name != name || m.ReturnType == voidType)
                        {
                            return false;
                        }

                        ParameterInfo[] parameters = m.GetParameters();
                        return parameters.Length == 1 && parameters[0].ParameterType.IsBaseTypeOrEquals(elementType);
                    };

                    methodInfo = methodsInfo.FindOrDefault(predicate);
                    IFunctionCallback<object, object, object> callback = formatter.CreateReturnOneParameterMethodCallback(collectionType, methodInfo);
                    return (o) => callback.Invoke(enumerable, o);
                };

                Func<string, Action<object>> getCallbackReturnlessMethod = (name) =>
                {
                    // Builds a callback for the public method with specified name
                    // (one parameter, returnless)

                    Func<MethodInfo, bool> predicate = (m) =>
                    {
                        if (m.Name != name || m.ReturnType != voidType)
                        {
                            return false;
                        }

                        ParameterInfo[] parameters = m.GetParameters();
                        return parameters.Length == 1 && parameters[0].ParameterType.IsBaseTypeOrEquals(elementType);
                    };

                    methodInfo = methodsInfo.FindOrDefault(predicate);
                    IActionCallback<object, object> callback = formatter.CreateReturnlessOneParameterMethodCallback(collectionType, methodInfo);
                    return (o) => callback.Invoke(enumerable, o);
                };

                // Finds the specific methods for the adding a new values of the different collections

                if (collectionGTD == linkedListType)
                {
                    // Linked list
                    this.collectionAddAction = getCallbackReturnMethod("AddLast");
                }

                if (collectionGTD == listType)
                {
                    // List
                    IList list = enumerable as IList;
                    this.collectionAddAction = (o) => list.Add(o);
                }

                if (collectionGTD == hashSetType || collectionGTD == sortedSetType)
                {
                    // Hash set, sorted set
                    this.collectionAddAction = getCallbackReturnMethod("Add");
                }

                if (collectionGTD == concurrentBagType)
                {
                    // Concurrent bag
                    this.collectionAddAction = getCallbackReturnlessMethod("Add");
                }

                if (collectionGTD == queueType || collectionGTD == concurrentQueueType)
                {
                    // Queue, concurrent queue
                    this.collectionAddAction = getCallbackReturnlessMethod("Enqueue");
                }

                if (collectionGTD == stackType || collectionGTD == concurrentStackType)
                {
                    // Stack, concurrent stack
                    this.collectionAddAction = getCallbackReturnlessMethod("Push");
                    this.collectionIsStack = true;
                }

                // Saves the callback for the found method

                if (this.collectionIsStack)
                {
                    this.addAction = (v) => collectedValues.AddFirst(v);
                }
                else
                {
                    this.addAction = this.collectionAddAction;
                }
            }

            /// <summary>
            /// Adds a new value to the collection.
            /// </summary>
            /// <param name="value"> New value. </param>
            public void Add(object value)
            {
                this.addAction.Invoke(value);
            }

            /// <summary>
            /// Disposes the writer.
            /// </summary>
            public void Dispose()
            {
                if (this.collectionIsStack)
                {
                    // Adds the values in normal (not reversed) order
                    for (var node = this.collectedValues.First; node != null; node = node.Next)
                    {
                        this.collectionAddAction(node.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Callbacks that raises the methods that contains one parameter and returns the value.
        /// </summary>
        private readonly IDictionary<Type, IDictionary<MethodInfo, IFunctionCallback<object, object, object>>> oneParameterReturnMethodCallbacks = new Dictionary<Type, IDictionary<MethodInfo, IFunctionCallback<object, object, object>>>(10);

        /// <summary>
        /// Callbacks that raises the methods that contains one parameter and doesn't return the value.
        /// </summary>
        private readonly IDictionary<Type, IDictionary<MethodInfo, IActionCallback<object, object>>> oneParameterReturnlessMethodCallbacks = new Dictionary<Type, IDictionary<MethodInfo, IActionCallback<object, object>>>(10);

        /// <summary>
        /// Callbacks that creates the arrays.
        /// </summary>
        private readonly IDictionary<Type, IDictionary<int, IFunctionCallback<int[], Array>>> arrayCreateCallbacks = new Dictionary<Type, IDictionary<int, IFunctionCallback<int[], Array>>>(10);

        /// <summary>
        /// Callbacks that creates the collections.
        /// </summary>
        private readonly IDictionary<Type, IDictionary<Type, IFunctionCallback<int[], IEnumerable>>> collectionCreateCallbacks = new Dictionary<Type, IDictionary<Type, IFunctionCallback<int[], IEnumerable>>>(10);

        /// <summary>
        /// Callbacks that creates the dictionaries.
        /// </summary>
        private readonly IDictionary<Type, IDictionary<Type, IFunctionCallback<int[], IDictionary>>> dictionaryCreateCallbacks = new Dictionary<Type, IDictionary<Type, IFunctionCallback<int[], IDictionary>>>(10);

        /// <summary>
        /// Callbacks that creates the sorted dictionaries.
        /// </summary>
        private readonly IDictionary<Type, IDictionary<Type, IFunctionCallback<int[], IDictionary>>> sortedDictionaryCreateCallbacks = new Dictionary<Type, IDictionary<Type, IFunctionCallback<int[], IDictionary>>>(10);

        /// <summary>
        /// Callbacks that creates the concurrent dictionaries.
        /// </summary>
        private readonly IDictionary<Type, IDictionary<Type, IFunctionCallback<int[], IDictionary>>> concurrentDictionaryCreateCallbacks = new Dictionary<Type, IDictionary<Type, IFunctionCallback<int[], IDictionary>>>(10);

        /// <summary>
        /// Callbacks that creates the sorted lists.
        /// </summary>
        private readonly IDictionary<Type, IDictionary<Type, IFunctionCallback<int[], IDictionary>>> sortedListCreateCallbacks = new Dictionary<Type, IDictionary<Type, IFunctionCallback<int[], IDictionary>>>(10);

        /// <summary>
        /// Creates a new callback that raises a method that contains one parameter and returns the value.
        /// </summary>
        /// <param name="type"> Type. </param>
        /// <param name="methodInfo"> Method info. </param>
        /// <returns> Callback that raises the method that contains one parameter and returns the value. </returns>
        private IFunctionCallback<object, object, object> CreateReturnOneParameterMethodCallback(Type type, MethodInfo methodInfo)
        {
            return GetDictionaryValue(
                this.oneParameterReturnMethodCallbacks,
                type,
                methodInfo,
                () =>
                {
                    IFunctionCallback<object, object, object> callback = ReflectionUtils.CreateReturnOneParameterMethodCallback(type, methodInfo);
                    return callback;
                });
        }

        /// <summary>
        /// Creates a new callback that raises a method that contains one parameter and doesn't return the value.
        /// </summary>
        /// <param name="type"> Type. </param>
        /// <param name="methodInfo"> Method info. </param>
        /// <returns> Callback that raises the method that contains one parameter and doesn't returns the value. </returns>
        private IActionCallback<object, object> CreateReturnlessOneParameterMethodCallback(Type type, MethodInfo methodInfo)
        {
            return GetDictionaryValue(
                this.oneParameterReturnlessMethodCallbacks,
                type,
                methodInfo,
                () =>
                {
                    IActionCallback<object, object> callback = ReflectionUtils.CreateReturnlessOneParameterMethodCallback(type, methodInfo);
                    return callback;
                });
        }

        /// <summary>
        /// Creates a new dictionary.
        /// </summary>
        /// <param name="type"> Type of the dictionary. </param>
        /// <param name="keyType"> Type of the key. </param>
        /// <param name="valueType"> Type of the value. </param>
        /// <param name="capacity"> Capacity of the dictionary. </param>
        /// <returns> New <see cref="IDictionary"></see>. </returns>
        protected IDictionary CreateDictionary(Type type, Type keyType, Type valueType, int capacity)
        {
            IDictionary dictionary = null;

            if (type == dictionaryType)
            {
                dictionary = this.GetCreateDictionaryCallback(this.dictionaryCreateCallbacks, type, keyType, valueType, 1).Invoke(new[] { capacity });
            }

            if (type == sortedDictionaryType)
            {
                dictionary = this.GetCreateDictionaryCallback(this.sortedDictionaryCreateCallbacks, type, keyType, valueType, 0).Invoke(null);
            }

            if (type == concurrentDictionaryType)
            {
                dictionary = this.GetCreateDictionaryCallback(this.concurrentDictionaryCreateCallbacks, type, keyType, valueType, 2).Invoke(new[] { concurrencyLevel, capacity });
            }

            if (type == sortedListType)
            {
                dictionary = this.GetCreateDictionaryCallback(this.sortedListCreateCallbacks, type, keyType, valueType, 1).Invoke(new[] { capacity });
            }

            if (dictionary != null)
            {
                return dictionary;
            }
            else
            {
                throw new UnsupportedTypeException(type.MakeGenericType(keyType, valueType));
            }
        }

        /// <summary>
        /// Creates a new collection.
        /// </summary>
        /// <param name="collectionType"> Type of the collection. </param>
        /// <param name="elementType"> Type of the element. </param>
        /// <param name="capacity"> Capacity of the collection. </param>
        /// <returns> New <see cref="ICollection"></see>. </returns>
        protected IEnumerable CreateCollection(Type collectionType, Type elementType, int capacity)
        {
            if (collectionType == arrayType)
            {
            	return this.CreateArray(elementType, new[] { capacity });
            }

            int[] args = (collectionType == listType || collectionType == queueType || collectionType == stackType) ? new[] { capacity } : null;

            return GetDictionaryValue(
                this.collectionCreateCallbacks,
                collectionType,
                elementType,
                () =>
                {
                    IFunctionCallback<int[], IEnumerable> callback = ReflectionUtils.CreateEnumerable(collectionType, elementType, (args == null) ? 0 : args.Length);
                    return callback;
                }).Invoke(args);
        }

        /// <summary>
        /// Creates a new array.
        /// </summary>
        /// <param name="elementType"> Type of the element. </param>
        /// <param name="lengths"> Lenghts of the array. </param>
        /// <returns> New <see cref="Array"></see>. </returns>
        protected Array CreateArray(Type elementType, params int[] lengths)
        {
            int rank = lengths.Length;

            return GetDictionaryValue(
                this.arrayCreateCallbacks,
                elementType,
                rank,
                () =>
                {
                    IFunctionCallback<int[], Array> callback = ReflectionUtils.CreateMDArray(elementType, rank);
                    return callback;
                }).Invoke(lengths);
        }

        /// <summary>
        /// Gets an activation function for the dictionary.
        /// </summary>
        /// <param name="createFunctions"> Dictionary that contains activation functions for the dictionaries of specified type. </param>
        /// <param name="type"> Type of the dictionary. </param>
        /// <param name="keyType"> Type of the key. </param>
        /// <param name="valueType"> Type of the values. </param>
        /// <param name="argsCount"> Count of the arguments. </param>
        /// <returns> Activation function for the dictionary. </returns>
        private IFunctionCallback<int[], IDictionary> GetCreateDictionaryCallback(IDictionary<Type, IDictionary<Type, IFunctionCallback<int[], IDictionary>>> createFunctions, Type type, Type keyType, Type valueType, int argsCount)
        {
            return GetDictionaryValue(
                createFunctions,
                keyType,
                valueType,
                () =>
                {
                    IFunctionCallback<int[], IDictionary> callback = ReflectionUtils.CreateDictionary(type, keyType, valueType, argsCount);
                    return callback;
                });
        }

        /// <summary>
        /// Gets a value from the composite dictionary.
        /// </summary>
        /// <param name="compositeDictionary"> Composite dictionary. </param>
        /// <param name="arg1"> First argument. </param>
        /// <param name="arg2"> Second argument. </param>
        /// <param name="builder"> Builder for the value (if value isn't exist). </param>
        /// <returns> Value from the complex dictionary. </returns>
        private static TOut GetDictionaryValue<TIn1, TIn2, TOut>(IDictionary<TIn1, IDictionary<TIn2, TOut>> compositeDictionary, TIn1 arg1, TIn2 arg2, Func<TOut> builder)
        {
            IDictionary<TIn2, TOut> dictionary;
            TOut value;

            if (!compositeDictionary.TryGetValue(arg1, out dictionary))
            {
                value = builder();
                dictionary = new Dictionary<TIn2, TOut>();
                dictionary.Add(arg2, value);
                compositeDictionary.Add(arg1, dictionary);
            }
            else
            {
                if (!dictionary.TryGetValue(arg2, out value))
                {
                    value = builder();
                    dictionary.Add(arg2, value);
                }
            }

            return value;
        }
    }
}