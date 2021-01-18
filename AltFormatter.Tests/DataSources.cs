/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System.Numerics;
using System;

namespace AltFormatter
{
    public static class DataSources
    {
        public static readonly object[] PrimitiveTypeValueSource =
        {
            new object[]{typeof(bool),true},
            new object[]{typeof(byte),(byte)127},
            new object[]{typeof(sbyte),(sbyte)127},
            new object[]{typeof(short),(short)(-10)},
            new object[]{typeof(ushort),(ushort)10},
            new object[]{typeof(int),(int)(-100)},
            new object[]{typeof(uint),(uint)100},
            new object[]{typeof(long),(long)(-1000)},
            new object[]{typeof(ulong),(ulong)1000},
            new object[]{typeof(string),(string)"Test"},
            new object[]{typeof(char),(char)'X'},
            new object[]{typeof(float),(float)1.111},
            new object[]{typeof(double),(double)1.111E3},
            new object[]{typeof(DateTime),new DateTime(10)},
            new object[]{typeof(TimeSpan),new TimeSpan(1000)},
            new object[]{typeof(float),(float)111.111},
            new object[]{typeof(decimal),(decimal)1000},
            new object[]{typeof(Complex),new Complex(10E-3,20E+6)},
        };
    }
}