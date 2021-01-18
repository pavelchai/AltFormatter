/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System.Collections;
using System.Collections.Generic;

namespace NUnit.Framework
{
    public static class IReadOnlyListEnumeratorUtils
    {
        public static bool SequenceEqualGeneric<T>(this IReadOnlyList<T> list, IEnumerator<T> enumerator = null)
        {
        	enumerator = enumerator ?? list.GetEnumerator();
        	
        	IEqualityComparer<T> comparer = EqualityComparer<T>.Default;

            int index = 0;
            while (enumerator.MoveNext())
            {
                if (!comparer.Equals(list[index], enumerator.Current)) return false;
                index++;
            }

            return index == list.Count;
        }
        
        public static bool SequenceEqualNonGeneric<T>(this IReadOnlyList<T> list, IEnumerator enumerator = null)
        {
            enumerator = enumerator ?? list.GetEnumerator();
        	
            IEqualityComparer<T> comparer = EqualityComparer<T>.Default;

            int index = 0;
            while (enumerator.MoveNext())
            {
                if (!comparer.Equals(list[index], (T)enumerator.Current)) return false;
                index++;
            }

            return index == list.Count;
        }
    }
}