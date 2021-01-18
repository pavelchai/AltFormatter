/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System;
using System.Collections.Generic;

namespace AltFormatter.Utils
{
    /// <summary>
    /// Represents the utils for the <see cref="IReadOnlyList{T}"></see>.
    /// </summary>
    public static class ReadOnlyListUtils
    {
        /// <summary>
        /// Determines whether any item of a <see cref="IReadOnlyList{T}"></see> match specified predicate.
        /// </summary>
        /// <param name="list"> The list. </param>
        /// <param name="predicate"> A function to test each item for a condition. </param>
        /// <returns> True if the list is not empty and at least one of its elements passes the test in the specified predicate, otherwise false. </returns>
        /// <typeparam name="T"> The type of the elements of the list. </typeparam>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when list or predicate are null.
        /// </exception>
        public static bool Any<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
        {
        	Validation.NotNull("List", list);
        	Validation.NotNull("Predicate", predicate);
        	
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                if (predicate(list[i]))
                {
                	return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Determines whether all item of a <see cref="IReadOnlyList{T}"></see> match specified predicate.
        /// </summary>
        /// <param name="list"> The list. </param>
        /// <param name="predicate"> A function to test each item for a condition. </param>
        /// <returns> True if the list is not empty and all of the elements passes the test in the specified predicate, otherwise false. </returns>
        /// <typeparam name="T"> The type of the elements of the list. </typeparam>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when list or predicate are null.
        /// </exception>
        public static bool All<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
        {
        	Validation.NotNull("List", list);
        	Validation.NotNull("Predicate", predicate);
        	
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                if (!predicate(list[i]))
                {
                	return false;
                }
            }
            
            return true;
        }

        /// <summary>
        /// Determines whether item is exist in the <see cref="IReadOnlyList{T}"></see> or not.
        /// </summary>
        /// <param name="list"> The list. </param>
        /// <param name="item"> The item. </param>
        /// <returns> True if item is exist in the list, otherwise false. </returns>
        /// <typeparam name="T"> The type of the elements of the list. </typeparam>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when list is null.
        /// </exception>
        public static bool Contains<T>(this IReadOnlyList<T> list, T item)
        {
        	Validation.NotNull("List", list);
        	
            IEqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;

            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                if (equalityComparer.Equals(list[i], item))
                {
                	return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a number that represents how many elements in the <see cref="IReadOnlyList{T}"></see> satisfy a condition.
        /// </summary>
        /// <param name="list"> The list. </param>
        /// <param name="predicate"> A function to test each item for a condition. </param>
        /// <returns> A number that represents how many elements in the list satisfy the condition in the predicate function. </returns>
        /// <typeparam name="T"> The type of the elements of the list. </typeparam>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when list or predicate are null.
        /// </exception>
        public static int Count<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
        {
        	Validation.NotNull("List", list);
        	Validation.NotNull("Predicate", predicate);
        	
            int count = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                	count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Executes the action with the all elements of the <see cref="IReadOnlyList{T}"></see>.
        /// </summary>
        /// <param name="list"> The list. </param>
        /// <param name="action"> An action to execute. </param>
        /// <typeparam name="T"> The type of the elements of the list. </typeparam>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when list or action are null.
        /// </exception>
        public static void Execute<T>(this IReadOnlyList<T> list, Action<T> action)
        {
        	Validation.NotNull("List", list);
        	Validation.NotNull("Action", action);
        	
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                action(list[i]);
            }
        }
        
        /// <summary>
        /// Finds item in the <see cref="IReadOnlyList{T}"></see> which match specified predicate.
        /// </summary>
        /// <param name="list"> The list. </param>
        /// <param name="predicate"> A function to test each item for a condition. </param>
        /// <returns> Item in the <see cref="IReadOnlyList{T}"></see> which match specified predicate, otherwise default{T}. </returns>
        /// <typeparam name="T"> The type of the elements of the list. </typeparam>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when list or predicate are null.
        /// </exception>
        public static T FindOrDefault<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
        {
        	Validation.NotNull("List", list);
        	Validation.NotNull("Predicate", predicate);
        	
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                if (predicate(list[i]))
                {
                	return list[i];
                }
            }
            
            return default(T);
        }
        
        /// <summary>
        /// Gets the min and max values of the <see cref="IReadOnlyList{T}"></see>.
        /// </summary>
        /// <param name="list"> The list. </param>
        /// <returns> Min and max values of the list. </returns>
        /// <remarks> If count of the list is zero - default values has been used as min and max values. </remarks>
        /// <typeparam name="T"> The type of the elements of the list. </typeparam>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when list is null.
        /// </exception>
        public static MinMax<T> GetMinMaxOrDefault<T>(this IReadOnlyList<T> list) where T : IComparable
        {
        	Validation.NotNull("List", list);
        	
            int count = list.Count;
            if (count == 0)
            {
            	return new MinMax<T>(default(T), default(T));
            }
            
            T value;
            if (count == 1)
            {
            	value = list[0];
            	return new MinMax<T>(value, value);
            }
            
            T min = list[0];
            T max = list[0];
            
            for (int i = 1; i < count; i++)
            {
                value = list[i];
                
                if (value.CompareTo(min) < 0)
                {
                	min = value;
                }
                
                if (value.CompareTo(max) > 0)
                {
                	max = value;
                }
            }

            return new MinMax<T>(min, max);
        }

        /// <summary>
        /// Gets the min and max values of the <see cref="IReadOnlyList{T}"></see>.
        /// </summary>
        /// <param name="list"> The list. </param>
        /// <param name="selector"> A transform function to apply to each source element; the second and third parameters of the function represents the indexes of the source element. </param>
        /// <returns> Min and max values of the list. </returns>
        /// <remarks> If count of the list is zero - default values has been used as min and max values. </remarks>
        /// <typeparam name="T"> The type of the elements of the list. </typeparam>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when list or selector are null.
        /// </exception>
        public static MinMax<K> GetMinMaxOrDefault<T, K>(this IReadOnlyList<T> list, Func<T, K> selector) where K : IComparable
        {
        	Validation.NotNull("List", list);
        	Validation.NotNull("Selector", selector);
        	
            int count = list.Count;
            
            if (count == 0)
            {
            	return new MinMax<K>(default(K), default(K));
            }
            
            if (count == 1)
            {
            	K selectedValue = selector(list[0]);
            	return new MinMax<K>(selectedValue, selectedValue);
            }

            K min = selector(list[0]);
            K max = selector(list[0]);
            K value;

            for (int i = 1; i < count; i++)
            {
                value = selector(list[i]);
                
                if (value.CompareTo(min) < 0)
                {
                	min = value;
                }
                
                if (value.CompareTo(max) > 0)
                {
                	max = value;
                }
            }

            return new MinMax<K>(min, max);
        }
        
        /// <summary>
        /// Searches for the specified object and returns the index of its first occurrence in the <see cref="IReadOnlyList{T}"></see>.
        /// </summary>
        /// <param name="list"> The list. </param>
        /// <param name="value"> The object to locate in list. </param>
        /// <returns> The index of the first occurrence of value in list if value has been found, otherwise the -1. </returns>
        /// <typeparam name="T"> The type of the elements of the list. </typeparam>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when list is null.
        /// </exception>
        public static int IndexOf<T>(this IReadOnlyList<T> list, T value)
        {
        	Validation.NotNull("List", list);
        	
            IEqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;

            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                if (equalityComparer.Equals(list[i], value))
                {
                	return i;
                }
            }
            
            return -1;
        }
    }
}