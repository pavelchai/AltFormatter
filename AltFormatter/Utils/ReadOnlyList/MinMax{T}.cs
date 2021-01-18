/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System;

namespace AltFormatter.Utils
{
    /// <summary>
    /// Represents a struct that contains the min and max values.
    /// </summary>
    /// <typeparam name="T"> The type of the values. </typeparam>
    public struct MinMax<T> where T : IComparable
    {
        /// <summary>
        /// Creates a new struct that contains the min and max values.
        /// </summary>
        /// <param name="min"> Min value. </param>
        /// <param name="max"> Max value. </param>
        public MinMax(T min, T max)
        {
            this.Min = min;
            this.Max = max;
        }

        /// <summary>
        /// Min value.
        /// </summary>
        public readonly T Min;

        /// <summary>
        /// Max value.
        /// </summary>
        public readonly T Max;
    }
}