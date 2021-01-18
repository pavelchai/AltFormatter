/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Utils;
using AltFormatter.Formatter;
using System;

namespace AltFormatter.Examples
{
    [Serializable]
    [Formattable("TestClassWithDoubleArray")]
    public sealed class TestClassWithDoubleArray
    {
        private TestClassWithDoubleArray()
        {
        }

        public TestClassWithDoubleArray(int size)
        {
            Array = ArrayUtils.CreateArray(0.0, 1000.0, size);
        }

        [Key("Array")]
        public double[] Array;
    }
}