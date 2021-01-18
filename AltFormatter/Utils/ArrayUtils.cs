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
    /// Represents the utils for the <see cref="Array"></see>.
    /// </summary>
    public static class ArrayUtils
    {
        /// <summary>
        /// Creates a new array (with fixed step).
        /// </summary>
        /// <param name="start"> Start value. </param>
        /// <param name="end"> End value. </param>
        /// <param name="N"> Number of the points. </param>
        /// <returns> New array. </returns>
        public static double[] CreateArray(double start, double end, int N)
        {
            double[] array = new double[N];
            double delta = (end - start) / (N - 1);
            for (int i = 0; i < N; i++)
            {
                array[i] = start + i * delta;
            }
            return array;
        }

        /// <summary>
        /// Creates a new array (with default value).
        /// </summary>
        /// <param name="value"> Default value. </param>
        /// <param name="N"> Number of points. </param>
        /// <returns> New array. </returns>
        /// <typeparam name="T"> The type of the elements of the array. </typeparam>
        public static T[] CreateArray<T>(T value, int N)
        {
            T[] array = new T[N];
            for (int i = 0; i < N; i++)
            {
                array[i] = value;
            }
            return array;
        }

        /// <summary>
        /// Creates a new array[X,Y] (with default value).
        /// </summary>
        /// <param name="value"> Default value. </param>
        /// <param name="N"> Number of points (X). </param>
        /// <param name="M"> Number of points (Y). </param>
        /// <returns> New array. </returns>
        /// <typeparam name="T"> The type of the elements of the array. </typeparam>
        public static T[,] CreateArray<T>(T value, int N, int M)
        {
            T[,] array = new T[N, M];
            for (int i = 0; i < N; i++)
            {
                for (int k = 0; k < N; k++)
                {
                    array[i, k] = value;
                }
            }
            return array;
        }

        /// <summary>
        /// Clones a source array.
        /// </summary>
        /// <param name="source"> The source. </param>
        /// <returns> Cloned source array. </returns>
        /// <typeparam name="T"> The type of the elements of source. </typeparam>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when source is null.
        /// </exception>
        public static T[] Clone<T>(T[] source)
        {
        	Validation.NotNull("Source", source);
        	
            int length = source.Length;
            T[] output = new T[length];

            for (int i = 0; i < length; i++)
            {
                output[i] = source[i];
            }

            return output;
        }

        /// <summary>
        /// Clones a source array.
        /// </summary>
        /// <param name="source"> The source. </param>
        /// <returns> Cloned source. </returns>
        /// <typeparam name="T"> The type of the elements of source. </typeparam>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when source is null.
        /// </exception>
        public static T[,] Clone<T>(T[,] source)
        {
        	Validation.NotNull("Source", source);
        	
            int N = source.GetLength(0);
            int M = source.GetLength(1);

            T[,] output = new T[N, M];
            for (int i = 0; i < N; i++)
            {
                for (int k = 0; k < M; k++)
                {
                    output[i, k] = source[i, k];
                }
            }

            return output;
        }

        /// <summary>
        /// Projects each element of a source into a new form.
        /// </summary>
        /// <param name="source"> The source. </param>
        /// <param name="selector"> A transform function to apply to each source element (value, index). </param>
        /// <returns> An array whose elements are the result of invoking the transform function on each element of source. </returns>
        /// <typeparam name="T"> The type of the elements of the source. </typeparam>
        /// <typeparam name="K"> The type of the value returned by selector. </typeparam>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when source or selector are null.
        /// </exception>
        public static K[] Select<T, K>(this T[] source, Func<T, int, K> selector)
        {
        	Validation.NotNull("Source", source);
        	Validation.NotNull("Selector", selector);
        	
            int length = source.Length;
            K[] output = new K[length];

            for (int i = 0; i < length; i++)
            {
                output[i] = selector(source[i], i);
            }

            return output;
        }

        /// <summary>
        /// Projects each element of a source into a new form.
        /// </summary>
        /// <param name="source"> The source. </param>
        /// <param name="selector"> A transform function to apply to each source element. </param>
        /// <returns> An array whose elements are the result of invoking the transform function on each element of source. </returns>
        /// <typeparam name="T"> The type of the elements of source. </typeparam>
        /// <typeparam name="K"> The type of the value returned by selector. </typeparam>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when source or selector are null.
        /// </exception>
        public static K[,] Select<T, K>(this T[,] source, Func<T, int, int, K> selector)
        {
        	Validation.NotNull("Source", source);
        	Validation.NotNull("Selector", selector);        	
        	
            int N = source.GetLength(0);
            int M = source.GetLength(1);

            K[,] output = new K[N, M];
            for (int i = 0; i < N; i++)
            {
                for (int k = 0; k < M; k++)
                {
                    output[i, k] = selector(source[i, k], i, k);
                }
            }

            return output;
        }
        
        /// <summary>
        /// Determines whether two arrays are equal.
        /// </summary>
        /// <param name="first"> First array. </param>
        /// <param name="second"> Second array. </param>
        /// <param name="equalityComparer"> Equality comparer. If null - default comparer will been used. </param>
        /// <returns> True if first array is equals to second array or both arrays are null, otherwise false. </returns>
        /// <typeparam name="T"> The type of the elements of source. </typeparam>
        public static bool Equals<T>(T[] first, T[] second, IEqualityComparer<T> equalityComparer = null)
        {
            if(object.ReferenceEquals(first, second))
        	{
        		return true;
        	}
        	
            if (first == null || second == null)
            {
                return false;
            }

            int length = first.Length;
            if (length != second.Length)
            {
                return false;
            }

            equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
            
            for (int i = 0; i < length; i++)
            {
                if (!equalityComparer.Equals(first[i], second[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether two arrays are equal.
        /// </summary>
        /// <param name="first"> First array. </param>
        /// <param name="second"> Second array. </param>
        /// <param name="equalityComparer"> Equality comparer. If null - default comparer will been used. </param>
        /// <returns> True if first array is equals to second array or both arrays are null, otherwise false. </returns>
        /// <typeparam name="T"> The type of the elements of source. </typeparam>
        public static bool Equals<T>(T[,] first, T[,] second, IEqualityComparer<T> equalityComparer = null)
        {
        	if(object.ReferenceEquals(first, second))
        	{
        		return true;
        	}
        	
            if (first == null || second == null)
            {
                return false;
            }

            int N = first.GetLength(0);
            int M = first.GetLength(1);

            if (N != second.GetLength(0) || M != second.GetLength(1))
            {
                return false;
            }

            equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
            
            for (int i = 0; i < N; i++)
            {
                for (int k = 0; k < M; k++)
                {
                    if (!equalityComparer.Equals(first[i, k], second[i, k]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}