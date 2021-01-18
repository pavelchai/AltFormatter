/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Localization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AltFormatter.Utils
{
    /// <summary>
    /// Represents a collection of static methods that implement the most common validations.
    /// </summary>
    public static class Validation
    {
        /// <summary>
        /// Verifies that a value with the specified name that is passed in is not null.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="value"> The value. </param>
        /// <exception cref="ValueNullException">
        /// The exception that is thrown when value is null.
        /// </exception>
        /// <typeparam name="T"> Type of the value. </typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotNull<T>(string name, T value) where T : class
        {
            if (value == null)
            {
                throw new ValueNullException(name);
            }
        }
        
        /// <summary>
        /// Verifies that any value in the group of the values with the specified name that is passed in is not null.
        /// </summary>
        /// <param name="name"> The name of the group of the values. </param>
        /// <param name="values"> The value. </param>
        /// <exception cref="ValueNullException">
        /// The exception that is thrown when all values are null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotNullAny(string name, params object[] values)
        {
        	int length = values.Length;
        	for(int i = 0; i < length; i++)
        	{
        		if(values[i] != null)
        		{
        			return;
        		}
        	}
        	
            throw new ValueNullException(name);
        }
        
        /// <summary>
        /// Verifies that all values in the group of the values with the specified name that is passed in are not null.
        /// </summary>
        /// <param name="name"> The name of the group of the values. </param>
        /// <param name="values"> The value. </param>
        /// <exception cref="ValueNullException">
        /// The exception that is thrown when any value in the values is null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotNullAll(string name, params object[] values)
        {
        	int length = values.Length;
        	for(int i = 0; i < length; i++)
        	{
        		if(values[i] == null)
        		{
        			throw new ValueNullException(StringUtils.Combine(name, "[", i.ToString(), "]"));
        		}
        	}
        }
        
        /// <summary>
        /// Verifies that a value with the specified name that is passed in is more than expected value.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="value"> The value. </param>
        /// <param name="expectedValue"> The expected value. </param>
        /// <exception cref="ValueLessThanOrEqualsException">
        /// The exception that is thrown when value is less than or equals expected value.
        /// </exception>
        /// <typeparam name="T"> Type of the value. </typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MoreThan<T>(string name, T value, T expectedValue) where T : IComparable<T>
        {
        	if(value.CompareTo(expectedValue) <= 0)
        	{
        		throw new ValueLessThanOrEqualsException(name, value, expectedValue);
        	}
        }
        
        /// <summary>
        /// Verifies that a value with the specified name that is passed in is more than or equals expected value.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="value"> The value. </param>
        /// <param name="expectedValue"> The expected value. </param>
        /// <exception cref="ValueLessThanException">
        /// The exception that is thrown when value is less than expected value.
        /// </exception>
        /// <typeparam name="T"> Type of the value. </typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MoreThanOrEquals<T>(string name, T value, T expectedValue) where T : IComparable<T>
        {
        	if(value.CompareTo(expectedValue) < 0)
        	{
        		throw new ValueLessThanException(name, value, expectedValue);
        	}
        }
        
        /// <summary>
        /// Verifies that a value with the specified name that is passed in is inside the range of the list.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="value"> The value. </param>
        /// <param name="minValue"> The min value of the range. </param>
        /// <param name="maxValue"> The max value of the range. </param>
        /// <exception cref="ValueOutOfRangeException">
        /// The exception that is thrown when the value is outside the allowable range of the values.
        /// </exception>
        /// <typeparam name="T"> Type of the value. </typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InRange<T>(string name, T value, T minValue, T maxValue) where T : IComparable<T>
        {
        	if(value.CompareTo(minValue) < 0 || value.CompareTo(maxValue) > 0)
        	{
        		throw new ValueOutOfRangeException(name, value, minValue, maxValue);
        	}
        }
        
        /// <summary>
        /// Verifies that a value with the specified name that is passed in is inside the range of the allowable indexes of the list.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="value"> The value. </param>
        /// <param name="list"> The list. </param>
        /// <exception cref="ValueOutOfRangeException">
        /// The exception that is thrown when the value is outside the range of the allowable indexes of the list.
        /// </exception>
        /// <typeparam name="T"> Type of the value. </typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InRange<T>(string name, int value, IReadOnlyList<T> list)
        {
        	int maxValue = list.Count - 1;
        	if(value.CompareTo(0) < 0 || value.CompareTo(maxValue) > 0)
        	{
        		throw new ValueOutOfRangeException(name, value, 0, maxValue);
        	}
        }
        
        /// <summary>
        /// Verifies that a count of the list with the specified name that is passed in is more than expected count.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="list"> The list. </param>
        /// <param name="expectedCount"> The expected count. </param>
        /// <exception cref="ValueLessThanOrEqualsException">
        /// The exception that is thrown when count of the list is less than or equals expected count.
        /// </exception>
        /// <typeparam name="T"> Type of the values of the list. </typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SizeMoreThan<T>(string name, IList<T> list, int expectedCount)
        {
        	MoreThan(StringUtils.Combine(name, ".Count"), list.Count, expectedCount);
        }
        
        /// <summary>
        /// Verifies that a count of the list with the specified name that is passed in is more than or equals expected count.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="list"> The list. </param>
        /// <param name="expectedCount"> The expected count. </param>
        /// <exception cref="ValueLessThanException">
        /// The exception that is thrown when count of the list is less than or equals expected count.
        /// </exception>
        /// <typeparam name="T"> Type of the values of the list. </typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SizeMoreThanOrEquals<T>(string name, IList<T> list, int expectedCount)
        {
        	MoreThanOrEquals(StringUtils.Combine(name, ".Count"), list.Count, expectedCount);
        }
        
        /// <summary>
        /// Verifies that a length of the dimension of the array with the specified name that is passed in is more than expected length.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="array"> The array. </param>
        /// <param name="dimension"> The dimension. </param>
        /// <param name="expectedLength"> The expected length of the dimension. </param>
        /// <exception cref="ValueLessThanOrEqualsException">
        /// The exception that is thrown when length of the dimension of the list is less than or equals expected length.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SizeMoreThan(string name, Array array, int dimension, int expectedLength)
        {
        	MoreThan(StringUtils.Combine(name,".Length(",dimension.ToString(),")"), array.GetLength(dimension), expectedLength);
        }
        
        /// <summary>
        /// Verifies that a length of the dimension of the array with the specified name that is passed in is more than expected length.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="array"> The array. </param>
        /// <param name="dimension"> The dimension. </param>
        /// <param name="expectedLength"> The expected length of the dimension. </param>
        /// <exception cref="ValueLessThanException">
        /// The exception that is thrown when length of the dimension of the list is less than expected length.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SizeMoreThanOrEquals(string name, Array array, int dimension, int expectedLength)
        {
        	MoreThanOrEquals(StringUtils.Combine(name,".Length(",dimension.ToString(),")"), array.GetLength(dimension), expectedLength);
        }

        /// <summary>
        /// Verifies that the sizes of the lists with the specified names that are passed in are equals.
        /// </summary>
        /// <param name="name1"> The name of the first list. </param>
        /// <param name="name2"> The name of the second list. </param>
        /// <param name="list1"> The first list. </param>
        /// <param name="list2"> The second list. </param>
        /// <exception cref="ValuesHaveDifferentSizesException">
        /// The exception that is thrown when sizes of the lists are different.
        /// </exception>
        /// <typeparam name="T"> Type of the values of the first list. </typeparam>
        /// <typeparam name="K"> Type of the values if the second list. </typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SizesEquals<T,K>(string name1, string name2, IList<T> list1, IList<K> list2)
        {
        	SizesEquals(name1, name2, list1.Count, list2.Count);
        }
        
        /// <summary>
        /// Verifies that the each size of the list from the group of the lists with the specified name that are passed in are equals the count of the specified list.
        /// </summary>
        /// <param name="name1"> The name of the specified list. </param>
        /// <param name="name2"> The name of the group of the lists. </param>
        /// <param name="list"> The specified list. </param>
        /// <param name="lists"> The lists. </param>
        /// <exception cref="ValuesHaveDifferentSizesException">
        /// The exception that is thrown when sizes of the lists are different.
        /// </exception>
        /// <typeparam name="T"> Type of the values of the list. </typeparam>
        /// <typeparam name="K"> Type of the values if the lists. </typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SizesEquals<T,K>(string name1, string name2, IList<T> list, params IList<K>[] lists)
        {
        	int count = list.Count;
        	int length = lists.Length;
        	for(int i = 0; i < length; i++)
        	{
        		SizesEquals(name1, StringUtils.Combine(name2, "[", i.ToString(), "]"), list.Count, lists[i].Count);
        	}
        }
        
        /// <summary>
        /// Verifies that the lengths of the dimensions of the arrays with the specified names that are passed in are equals.
        /// </summary>
        /// <param name="name1"> The name of the first array. </param>
        /// <param name="name2"> The name of the second array. </param>
        /// <param name="dimension"> The dimension. </param>
        /// <param name="array1"> The first array. </param>
        /// <param name="array2"> The second array. </param>
        /// <exception cref="ValuesHaveDifferentSizesException">
        /// The exception that is thrown when lengths of the dimensions of the arrays are different.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SizesEquals(string name1, string name2, int dimension, Array array1, Array array2)
        {
        	SizesEquals(StringUtils.Combine(name1,".Length(",dimension.ToString(),")"), StringUtils.Combine(name2,".Length(",dimension.ToString(),")"), array1.GetLength(dimension), array2.GetLength(dimension));
        }
        
        /// <summary>
        /// Verifies that the sizes with the specified names that are passed in are equals.
        /// </summary>
        /// <param name="name1"> The first name. </param>
        /// <param name="name2"> The seconf name. </param>
        /// <param name="size1"> The first size. </param>
        /// <param name="size2"> The second size. </param>
        /// <exception cref="ValuesHaveDifferentSizesException">
        /// The exception that is thrown when sizes are different.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SizesEquals(string name1, string name2, int size1, int size2)
        {
        	if (size1 != size2)
            {
                throw new ValuesHaveDifferentSizesException(name1, name2, size1, size2);
            }
        }
    }
}