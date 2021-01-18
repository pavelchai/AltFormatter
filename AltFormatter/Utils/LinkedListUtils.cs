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
    /// Represents the utils for the <see cref="LinkedList{T}"></see>.
    /// </summary>
    public static class LinkedListExtensions
    {
        /// <summary>
        /// Executes the action with the each item in the <see cref="LinkedList{T}"></see> and clears the list.
        /// </summary>
        /// <param name="list"> The list. </param>
        /// <param name="action"> The action. </param>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when list or action are null.
        /// </exception>
        /// <typeparam name="T"> The type of the elements of the list. </typeparam>
        public static void ExecuteAndClear<T>(this LinkedList<T> list, Action<T> action)
        {
        	Validation.NotNull("List", list);
        	Validation.NotNull("Action", action);
        	
            for (var node = list.First; node != null; node = node.Next)
            {
                action(node.Value);
            }
            
            list.Clear();
        }

        /// <summary>
        /// Finds a node of the item in the <see cref="LinkedList{T}"></see> that match specified predicate.
        /// </summary>
        /// <param name="list"> The list. </param>
        /// <param name="predicate"> A function to test each item for a condition. </param>
        /// <returns> Node of the item in the <see cref="LinkedList{T}"></see> that match specified predicate, otherwise null. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when list or predicate are null.
        /// </exception>
        /// <typeparam name="T"> The type of the elements of the list. </typeparam>
        public static LinkedListNode<T> Find<T>(this LinkedList<T> list, Func<T, bool> predicate)
        {
        	Validation.NotNull("List", list);
        	Validation.NotNull("Predicate", predicate);
        	
            for (var node = list.First; node != null; node = node.Next)
            {
                if (predicate(node.Value)) return node;
            }
            
            return null;
        }

        /// <summary>
        /// Creates an <see cref="Array"></see> from a <see cref="LinkedList{T}"></see>.
        /// </summary>
        /// <param name="list"> The list. </param>
        /// <returns> An array that contains the items from the input <see cref="LinkedList{T}"></see>. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when list is null.
        /// </exception>
        /// <typeparam name="T"> The type of the elements of the list. </typeparam>
        public static T[] ToArray<T>(this LinkedList<T> list)
        {
        	Validation.NotNull("List", list);
        	
            T[] ret = new T[list.Count];
            int i = 0;
            for (var node = list.First; node != null; node = node.Next)
            {
                ret[i] = node.Value;
                i++;
            }
            
            return ret;
        }
    }
}