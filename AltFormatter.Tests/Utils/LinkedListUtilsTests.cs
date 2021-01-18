/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace AltFormatter.Utils
{
    public sealed class LinkedListUtils
    {
        [Test]
        public void ExecuteAndClear_CloneAction_SequenceEquals()
        {
            LinkedList<int> source = GetSourceList();
            int[] sourceArray = source.ToArray();

            int[] data = new int[sourceArray.Length];
            int index = 0;

            source.ExecuteAndClear(v => data[index++] = v);

            Assert.AreEqual(0, source.Count);
            Assert.True(sourceArray.SequenceEqual(data));
        }
        
        [Test]
        public void Find_MoreThan2_Value()
        {
            LinkedList<int> source = GetSourceList();
            Assert.AreEqual(3, source.Find(v => v > 2).Value);
            Assert.AreEqual(null, source.Find(v => v > 10));
        }

        [Test]
        public void ToArray_SequenceEquals()
        {
            LinkedList<int> source = GetSourceList();
            int[] array = source.ToArray();
            Assert.True(source.SequenceEqual(array));
        }
        
        [Test]
        public void ExecuteAndClear_ListNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (null as LinkedList<int>).ExecuteAndClear(i => {}),
        		"List");
        }
        
        [Test]
        public void ExecuteAndClear_ActionNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (new LinkedList<int>()).ExecuteAndClear(null),
        		"Action");
        }
        
        [Test]
        public void Find_ListNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (null as LinkedList<int>).Find(i => true),
        		"List");
        }
        
        [Test]
        public void Find_PredicateNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (new LinkedList<int>()).Find(null),
        		"Predicate");
        }
        
        [Test]
        public void ToArray_ListNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (null as LinkedList<int>).ToArray(),
        		"List");
        }

        private static LinkedList<int> GetSourceList()
        {
            LinkedList<int> list = new LinkedList<int>();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(3);
            list.AddLast(4);
            return list;
        }
    }
}