/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;
using System.Collections.Generic;

namespace AltFormatter.Utils.Sort
{
    public sealed class SortUtilsIComparerTests
    {
    	private sealed class Element
	    {
	        public readonly int Index;
	
	        public Element(int index)
	        {
	            this.Index = index;
	        }
	    }
	
	    private sealed class ElementComparer : IComparer<Element>
	    {
	        public int Compare(Element first, Element second)
	        {
	            return first.Index.CompareTo(second.Index);
	        }
	    }
    	
        private readonly static ElementComparer elementComparer = new ElementComparer();
        
        [Test]
        public void QuickSortOnlyArray_Ascending()
        {
            var element1 = new Element(0);
            var element2 = new Element(10);
            var element3 = new Element(20);
            
            var array = new []{ element1, element3, element2 };
            
            SortUtils.QuickSort(true, elementComparer, array);

            Assert.AreEqual(element1, array[0]);
            Assert.AreEqual(element2, array[1]);
            Assert.AreEqual(element3, array[2]);
        }

        [Test]
        public void QuickSortArrayAndAssociatedArray_Ascending()
        {
            var element1 = new Element(0);
            var element2 = new Element(10);
            var element3 = new Element(20);
            
            var array = new []{ element1, element3, element2 };
            var associatedArray = new []{element1, element3, element2};

            SortUtils.QuickSort(true, elementComparer, array, associatedArray);

            Assert.AreEqual(element1, array[0]);
            Assert.AreEqual(element2, array[1]);
            Assert.AreEqual(element3, array[2]);

            Assert.AreEqual(element1, associatedArray[0]);
            Assert.AreEqual(element2, associatedArray[1]);
            Assert.AreEqual(element3, associatedArray[2]);
        }

        [Test]
        public void QuickSortOnlyArray_Descending()
        {
            var element1 = new Element(0);
            var element2 = new Element(10);
            var element3 = new Element(20);
            
            var array = new []{ element1, element2, element3 };

            SortUtils.QuickSort(false, elementComparer, array);
            
            Assert.AreEqual(element3, array[0]);
            Assert.AreEqual(element2, array[1]);
            Assert.AreEqual(element1, array[2]);
        }

        [Test]
        public void QuickSortArrayAndAssociatedArray_Descending()
        {
            var element1 = new Element(0);
            var element2 = new Element(10);
            var element3 = new Element(20);
            
            var array = new []{ element1, element2, element3 };
            var associatedArray = new []{element1, element2, element3};

            SortUtils.QuickSort(false, elementComparer, array, associatedArray);

            Assert.AreEqual(element3, array[0]);
            Assert.AreEqual(element2, array[1]);
            Assert.AreEqual(element1, array[2]);

            Assert.AreEqual(element3, associatedArray[0]);
            Assert.AreEqual(element2, associatedArray[1]);
            Assert.AreEqual(element1, associatedArray[2]);
        }
        
        [Test]
        public void QuickSortOnlyArray_ArrayNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => SortUtils.QuickSort(true, elementComparer, null),
        		"List");
        }
        
        [Test]
        public void QuickSortArrayAndAssociatedArray_ArrayNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => SortUtils.QuickSort(true, elementComparer, null as Element[], new Element[6]),
        		"List");
        }
        
        [Test]
        public void QuickSortArrayAndAssociatedArray_AssociatedArrayNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => SortUtils.QuickSort(true, elementComparer, new Element[6], null as IList<Element>[]),
        		"AssociatedLists");
        }
        
        [Test]
        public void QuickSortArrayAndAssociatedArray_AssociatedArray0ArrayNull_ValidateIsNull()
        {
        	AssertValidation.NotNullAll(
        		() => SortUtils.QuickSort(true, elementComparer, new Element[6], null as Element[], new Element[6]),
        		"AssociatedLists", 0);
        }
        
        [Test]
        public void QuickSortArrayAndAssociatedArray_ListDifferentSizes_ValidateIsSizesEquals()
        {
        	AssertValidation.SizesEquals(
        		() => SortUtils.QuickSort(true, elementComparer, new Element[3], new Element[6], new Element[6]),
        		"List", "AssociatedLists", 0, 3, 6);
        }
    }
}