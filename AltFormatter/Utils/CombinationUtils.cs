/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System.Collections.Generic;

namespace AltFormatter.Utils
{
	/// <summary>
    /// Represents the utils for the combinations of the values.
    /// </summary>
    public static class CombinationUtils
    {
        /// <summary>
        /// Creates all possible combinations from the arrays of the values.
        /// </summary>
        /// <param name="values"> The array. </param>
        /// <returns> All possible combinations from the arrays of the values. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when values or any value in the values are null.
        /// </exception>
        /// <exception cref="Localization.ValueLessThanOrEqualsException">
        /// The exception that is thrown when length of the values is less than or equals 0.
        /// </exception>
        /// <typeparam name="T"> The type of the elements of the values. </typeparam>
        public static IEnumerable<T[]> CartesianProduct<T>(params IEnumerable<T>[] values)
        {
        	Validation.NotNull("Values", values);
        	Validation.NotNullAll("Values", values);
        	Validation.SizeMoreThan("Values", values, 0);
        	
            int len = values.Length;
            int lenS1 = len - 1;

            int[] lengths = new int[len];

            IList<T> vArray = null;
            IList<T>[] vArrays = new IList<T>[values.Length];
            LinkedList<T> vList = new LinkedList<T>();
            for (int i = 0; i < len; i++)
            {
                vArray = values[i] as IList<T>;
                if (vArray != null)
                {
                    vArrays[i] = vArray;
                    lengths[i] = vArray.Count;
                }
                else
                {
                    foreach (var v in values[i])
                    {
                        vList.AddLast(v);
                    }
                    vArrays[i] = vList.ToArray();
                    lengths[i] = vArrays[i].Count;
                }
            }

            int[] indexies = new int[len];

            T[] res = null;
            int j = 0;
            while (indexies[0] != lengths[0])
            {
                res = new T[len];

                for (int i = 0; i < len; i++)
                {
                    res[i] = vArrays[i][indexies[i]];
                }

                yield return res;

                j = lenS1;
                indexies[j]++;
                while (j > 0 && indexies[j] == lengths[j])
                {
                    indexies[j--] = 0;
                    indexies[j]++;
                }
            }
        }
    }
}