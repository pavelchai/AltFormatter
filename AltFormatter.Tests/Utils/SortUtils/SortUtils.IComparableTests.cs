/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;
using System.Collections.Generic;

namespace AltFormatter.Utils.Sort
{
	public sealed class SortUtilsIComparableTests
	{
		[Test]
        public void QuickSortOnlyArray_Ascending()
        {
            int[] array = new int[] { 0, 10, 4, 2, 8 };
            SortUtils.QuickSort(true, array);

            Assert.AreEqual(0, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(4, array[2]);
            Assert.AreEqual(8, array[3]);
            Assert.AreEqual(10, array[4]);
        }
        
        [Test]
        public void QuickSortArrayAndAssociatedArray_Ascending()
        {
            int[] array = new int[] { 0, 10, 4, 2, 8 };
            int[] associatedArray = new int[] { 0, 10, 4, 2, 8 };
            SortUtils.QuickSort(true, array, associatedArray);

            Assert.AreEqual(0, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(4, array[2]);
            Assert.AreEqual(8, array[3]);
            Assert.AreEqual(10, array[4]);

            Assert.AreEqual(0, associatedArray[0]);
            Assert.AreEqual(2, associatedArray[1]);
            Assert.AreEqual(4, associatedArray[2]);
            Assert.AreEqual(8, associatedArray[3]);
            Assert.AreEqual(10, associatedArray[4]);
        }
        
        [Test]
        public void QuickSortOnlyArray_Descending()
        {
            int[] array = new int[] { 0, 10, 4, 2, 8 };
            SortUtils.QuickSort(false, array);

            Assert.AreEqual(10, array[0]);
            Assert.AreEqual(8, array[1]);
            Assert.AreEqual(4, array[2]);
            Assert.AreEqual(2, array[3]);
            Assert.AreEqual(0, array[4]);
        }

        [Test]
        public void QuickSortArrayAndAssociatedArray_Descending()
        {
            int[] array = new int[] { 0, 10, 4, 2, 8 };
            int[] associatedArray = new int[] { 0, 10, 4, 2, 8 };
            SortUtils.QuickSort(false, array, associatedArray);

            Assert.AreEqual(10, array[0]);
            Assert.AreEqual(8, array[1]);
            Assert.AreEqual(4, array[2]);
            Assert.AreEqual(2, array[3]);
            Assert.AreEqual(0, array[4]);

            Assert.AreEqual(10, associatedArray[0]);
            Assert.AreEqual(8, associatedArray[1]);
            Assert.AreEqual(4, associatedArray[2]);
            Assert.AreEqual(2, associatedArray[3]);
            Assert.AreEqual(0, associatedArray[4]);
        }
        
        [Test]
        public void QuickSortOnlyArray_ArrayNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => SortUtils.QuickSort<int>(true, null),
        		"List");
        }
        
        [Test]
        public void QuickSortArrayAndAssociatedArray_ArrayNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => SortUtils.QuickSort<int, int>(true, null as int[], new int[6]),
        		"List");
        }
        
        [Test]
        public void QuickSortArrayAndAssociatedArray_AssociatedArrayNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => SortUtils.QuickSort<int, int>(true, new int[6], null as IList<int>[]),
        		"AssociatedLists");
        }
        
        [Test]
        public void QuickSortArrayAndAssociatedArray_AssociatedArray0ArrayNull_ValidateIsNull()
        {
        	AssertValidation.NotNullAll(
        		() => SortUtils.QuickSort<int, int>(true, new int[6], null, new int[6]),
        		"AssociatedLists", 0);
        }
        
        [Test]
        public void QuickSortArrayAndAssociatedArray_ListDifferentSizes_ValidateIsSizesEquals()
        {
        	AssertValidation.SizesEquals(
        		() => SortUtils.QuickSort<int, int>(true, new int[3], new int[6], new int[6]),
        		"List", "AssociatedLists", 0, 3, 6);
        }
	}
}