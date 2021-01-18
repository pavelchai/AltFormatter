/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System.Collections.Generic;

namespace AltFormatter.Utils
{
    /// <summary>
    /// Represents the utils for the sorting.
    /// </summary>
    public static partial class SortUtils
    {
    	/// <summary>
        /// Sorts the <see cref="IList{T}"></see> with the <see cref="IComparer{T}"></see>.
        /// </summary>
        /// <param name="isAscendingOrder"> Indicates whether sorting order is ascending. </param>
        /// <param name="comparer"> The comparer. </param>
        /// <param name="list"> The list. </param>
        /// <typeparam name="T"> The type of the elements of the list. </typeparam>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when list is null.
        /// </exception>
        public static void QuickSort<T>(bool isAscendingOrder, IComparer<T> comparer, IList<T> list)
        {
        	Validation.NotNull("List", list);
        	
        	int count = list.Count;
            if (count < 2)
            {
            	return;
            }
            
            if (isAscendingOrder)
            {
                QuickSortAscending<T, T>(0, count - 1, comparer, list);
            }
            else
            {
                QuickSortDescending<T, T>(0, count - 1, comparer, list);
            }
        }
        
        /// <summary>
        /// Sorts the <see cref="IList{T}"></see> (and other <see cref="IList{T}"></see>s that are associated with the one) with the <see cref="IComparer{T}"></see>.
        /// </summary>
        /// <param name="isAscendingOrder"> Indicates whether sorting order is ascending. </param>
        /// <param name="comparer"> The comparer. </param>
        /// <param name="list"> The list. </param>
        /// <param name="associatedLists"> Array of the lists that are associated with the list. </param>
        /// <typeparam name="T"> The type of the elements of the list. </typeparam>
        /// <typeparam name="K"> The type of the elements of the associated lists. </typeparam>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when list, or array of the associated lists or any associated list in the array are null.
        /// </exception>
        /// <exception cref="Localization.ValuesHaveDifferentSizesException">
        /// The exception that is thrown when sizes of the list are different.
        /// </exception>
        public static void QuickSort<T, K>(bool isAscendingOrder, IComparer<T> comparer, IList<T> list, params IList<K>[] associatedLists)
        {
        	Validation.NotNull("List", list);
        	Validation.NotNull("AssociatedLists", associatedLists);
        	Validation.NotNullAll("AssociatedLists", associatedLists);
        	Validation.SizesEquals("List", "AssociatedLists", list, associatedLists);
        	
        	int count = list.Count;
            if (count < 2)
            {
            	return;
            }
            
            if (isAscendingOrder)
            {
                QuickSortAscending<T, K>(0, count - 1, comparer, list, associatedLists);
            }
            else
            {
                QuickSortDescending<T, K>(0, count - 1, comparer, list, associatedLists);
            }
        }

        /// <summary>
        /// Sorts the <see cref="IList{T}"></see> (and other <see cref="IList{T}"></see>s that are associated with the one) with the <see cref="IComparer{T}"></see> in ascending order.
        /// </summary>
        /// <param name="l"> l. </param>
        /// <param name="r"> r. </param>
        /// <param name="comparer"> The comparer. </param>
        /// <param name="list"> The list. </param>
        /// <param name="associatedLists"> Array of the lists that are associated with the list. </param>
        /// <typeparam name="T"> The type of the elements of the list. </typeparam>
        /// <typeparam name="K"> The type of the elements of the associated lists. </typeparam>
        private static void QuickSortAscending<T, K>(int l, int r, IComparer<T> comparer, IList<T> list, params IList<K>[] associatedLists)
        {
            T value = list[l + (r - l) / 2];
            int i = l;
            int j = r;

            while (i <= j)
            {
                while (comparer.Compare(list[i], value) < 0)
                {
                    i++;
                }

                while (comparer.Compare(list[j], value) > 0)
                {
                    j--;
                }

                if (i <= j)
                {
                    T temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;

                    K tmp;
                    int length = associatedLists.Length;
                    for (int k = 0; k < length; k++)
                    {
                        tmp = associatedLists[k][i];
                        associatedLists[k][i] = associatedLists[k][j];
                        associatedLists[k][j] = tmp;
                    }

                    i++;
                    j--;
                }
            }

            if (i < r)
            {
                QuickSortAscending(i, r, comparer, list, associatedLists);
            }

            if (l < j)
            {
                QuickSortAscending(l, j, comparer, list, associatedLists);
            }
        }

        /// <summary>
        /// Sorts the <see cref="IList{T}"></see> (and other <see cref="IList{T}"></see>s that are associated with the one) with the <see cref="IComparer{T}"></see> in descending order.
        /// </summary>
        /// <param name="l"> l. </param>
        /// <param name="r"> r. </param>
        /// <param name="comparer"> The comparer. </param>
        /// <param name="list"> The list. </param>
        /// <param name="associatedLists"> Array of the lists that are associated with the list. </param>
        /// <typeparam name="T"> The type of the elements of the list. </typeparam>
        /// <typeparam name="K"> The type of the elements of the associated lists. </typeparam>
        private static void QuickSortDescending<T, K>(int l, int r, IComparer<T> comparer, IList<T> list, params IList<K>[] associatedLists)
        {
            T value = list[l + (r - l) / 2];
            int i = l;
            int j = r;

            while (i <= j)
            {
                while (comparer.Compare(list[i], value) > 0)
                {
                    i++;
                }

                while (comparer.Compare(list[j], value) < 0)
                {
                    j--;
                }

                if (i <= j)
                {
                    T temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;

                    K tmp;
                    int length = associatedLists.Length;
                    for (int k = 0; k < length; k++)
                    {
                        tmp = associatedLists[k][i];
                        associatedLists[k][i] = associatedLists[k][j];
                        associatedLists[k][j] = tmp;
                    }

                    i++;
                    j--;
                }
            }

            if (i < r)
            {
                QuickSortDescending(i, r, comparer, list, associatedLists);
            }

            if (l < j)
            {
                QuickSortDescending(l, j, comparer, list, associatedLists);
            }
        }
    }
}