/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System;

namespace NUnit.Framework
{
    public static class DateTimeUtils
    {
        public static bool Equals(DateTime dt1, DateTime dt2)
        {
            if (dt1.Kind != dt2.Kind) return false;

            return dt1.Year == dt2.Year && dt1.Month == dt2.Month && dt1.Day == dt2.Day &&
                   dt1.Hour == dt2.Hour && dt1.Minute == dt2.Minute && dt1.Second == dt2.Second && dt1.Millisecond == dt2.Millisecond;
        }
    }
}