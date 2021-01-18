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
    public sealed class ArrayUtilsTests
    {
        [Test]
        public void CreateArray1D_FixedStep1To3Step1_1DArraySize3()
        {
            double[] array = ArrayUtils.CreateArray(1.0, 3.0, 3);
            double[] targetArray = { 1.0, 2.0, 3.0 };
            Assert.True(targetArray.SequenceEqual(array));
        }

        [Test]
        public void CreateArray1D_DefaultValueSize3_1DArraySize3()
        {
            int[] array = ArrayUtils.CreateArray(10, 3);
            int[] targetArray = { 10, 10, 10 };
            Assert.True(targetArray.SequenceEqual(array));
        }

        [Test]
        public void CreateArray1D_DefaultValueSize2x2_2DArraySize4()
        {
            int[,] array = ArrayUtils.CreateArray(10, 2, 2);
            int[,] targetArray = { { 10, 10 }, { 10, 10 } };
            Assert.AreEqual(targetArray[0, 0], array[0, 0]);
            Assert.AreEqual(targetArray[0, 1], array[0, 1]);
            Assert.AreEqual(targetArray[1, 0], array[1, 0]);
            Assert.AreEqual(targetArray[1, 1], array[1, 1]);
        }

        [Test]
        public void Clone1D_1DArraySize4_1DArraySize4()
        {
            int[] array = { 1, 2, 3, 4 };
            int[] clonedArray = ArrayUtils.Clone(array);
            Assert.True(array.SequenceEqual(clonedArray));
        }
        
        [Test]
        public void Clone1D_ArrayNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => ArrayUtils.Clone(null as int[]),
        		"Source");
        }

        [Test]
        public void Clone2D_2DArraySize2x2_2DArraySize2x2()
        {
            int[,] array = { { 10, 20 }, { 30, 40 } };
            int[,] clonedArray = ArrayUtils.Clone(array);
            Assert.AreEqual(array[0, 0], clonedArray[0, 0]);
            Assert.AreEqual(array[0, 1], clonedArray[0, 1]);
            Assert.AreEqual(array[1, 0], clonedArray[1, 0]);
            Assert.AreEqual(array[1, 1], clonedArray[1, 1]);
        }
        
        [Test]
        public void Clone2D_ArrayNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => ArrayUtils.Clone(null as int[,]),
        		"Source");
        }

        [Test]
        public void Select1D_ArraySize4_ArraySize4()
        {
            int[] array = { 1, 2, 3, 4 };
            string[] selectedArray = array.Select((v, i) => (2 * v).ToString());
            string[] targetArray = { "2", "4", "6", "8" };
            Assert.True(targetArray.SequenceEqual(selectedArray));
        }
        
        [Test]
        public void Select1D_ArrayNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (null as int[]).Select((v,i) => v),
        		"Source");
        }
        
        [Test]
        public void Select1D_SelectorNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (new int[0]).Select<int,long>(null),
        		"Selector");
        }

        [Test]
        public void Select2D_ArraySize2x2_ArraySize2x2()
        {
            int[,] array = { { 10, 20 }, { 30, 40 } };
            string[,] selectedArray = array.Select((v, i1, i2) => (2 * v).ToString());
            Assert.AreEqual("20", selectedArray[0, 0]);
            Assert.AreEqual("40", selectedArray[0, 1]);
            Assert.AreEqual("60", selectedArray[1, 0]);
            Assert.AreEqual("80", selectedArray[1, 1]);
        }
        
        [Test]
        public void Select2D_ArrayNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (null as int[,]).Select((v,i1,i2) => v),
        		"Source");
        }
        
        [Test]
        public void Select2D_SelectorNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (new int[0,0]).Select<int,long>(null),
        		"Selector");
        }
        
        [Test]
        public void Equals1D_ReferenceEqualsBothNotNull_True()
        {
            int[] array1 = { 1, 2, 3, 4 };
            Assert.True(ArrayUtils.Equals(array1, array1));
        }

        [Test]
        public void Equals1D_ReferenceEqualsBothAreNull_False()
        {
            Assert.True(ArrayUtils.Equals<int>(null as int[], null as int[]));
        }
        
        [Test]
        public void Equals1D_FirstArrayNullSecondArrayNotNull_False()
        {
            int[] array = { 1, 2, 3, 4 };
            Assert.False(ArrayUtils.Equals(array, null));
        }
        
        [Test]
        public void Equals1D_FirstArrayNotNullSecondArrayNull_False()
        {
            int[] array = { 1, 2, 3, 4 };
            Assert.False(ArrayUtils.Equals(null, array));
        }

        [Test]
        public void Equals1D_ArraysHaveDifferentLength_False()
        {
            int[] array1 = { 1, 2, 3, 4 };
            int[] array2 = { 1, 2 };
            Assert.False(ArrayUtils.Equals(array1, array2));
        }

        [Test]
        public void Equals1D_ArraysHaveDifferentElements_Fallse()
        {
            int[] array1 = { 1, 2, 3, 4 };
            int[] array2 = { 1, 2, 0, 4 };
            Assert.False(ArrayUtils.Equals(array1, array2));
        }
        
        [Test]
        public void Equals1D_EqualsArrayNullComparer_True()
        {
            int[] array1 = { 1, 2, 3, 4 };
            int[] array2 = { 1, 2, 3, 4 };
            Assert.True(ArrayUtils.Equals(array1, array1, null));
        }

        [Test]
        public void Equals1D_EqualsArraysCustomComparer_True()
        {
            int[] array1 = { 1, 2, 3, 4 };
            int[] array2 = { 1, 2, 3, 4 };
            Assert.True(ArrayUtils.Equals(array1, array1, EqualityComparer<int>.Default));
        }
        
        [Test]
        public void Equals2D_ReferenceEqualsBothNotNull_True()
        {
            int[,] array = { { 10, 20 }, { 30, 40 } };
            Assert.True(ArrayUtils.Equals(array, array));
        }

        [Test]
        public void Equals2D_ReferenceEqualsBothAreNull_False()
        {
             Assert.True(ArrayUtils.Equals<int>(null as int[,], null as int[,]));
        }
        
        [Test]
        public void Equals2D_FirstArrayNullSecondArrayNotNull_False()
        {
            int[,] array = { { 10, 20 }, { 30, 40 } };
            Assert.False(ArrayUtils.Equals(array, null));
        }
        
        [Test]
        public void Equals2D_FirstArrayNotNullSecondArrayNull_False()
        {
            int[,] array = { { 10, 20 }, { 30, 40 } };
            Assert.False(ArrayUtils.Equals(null, array));
        }
        
        [Test]
        public void Equals2D_ArraysHaveDifferentLength0_False()
        {
            int[,] array1 = { { 10, 20 }, { 30, 40 } };
            int[,] array2 = { { 10, 20, 30 }, { 50, 50, 60 } };
            Assert.False(ArrayUtils.Equals(array1, array2));
        }
        
        [Test]
        public void Equals2D_ArraysHaveDifferentLength1_False()
        {
            int[,] array1 = { { 10, 20 }, { 30, 40 } };
            int[,] array2 = { { 10, 20 }, { 30, 40 }, { 50, 60 } };
            Assert.False(ArrayUtils.Equals(array1, array2));
        }
        
        [Test]
        public void Equals2D_ArraysHaveDifferentElementDimension0_False()
        {
            int[,] array1 = { { 10, 20 }, { 30, 40 } };
            int[,] array2 = { { 10, 20 }, { 30, 0 } };
            Assert.False(ArrayUtils.Equals(array1, array2));
        }
        
        [Test]
        public void Equals2D_ArraysHaveDifferentElementsDimension1_False()
        {
            int[,] array1 = { { 10, 20 }, { 30, 40 } };
            int[,] array2 = { { 0, 20 }, { 30, 40 } };
            Assert.False(ArrayUtils.Equals(array1, array2));
        }
        
        [Test]
        public void Equals2D_EqualsArraysNullComparer_True()
        {
            int[,] array1 = { { 10, 20 }, { 30, 40 } };
            int[,] array2 = { { 10, 20 }, { 30, 40 } };
            Assert.True(ArrayUtils.Equals(array1, array2, null));
        }

        [Test]
        public void Equals2D_EqualsArraysCustomComparer_True()
        {
            int[,] array1 = { { 10, 20 }, { 30, 40 } };
            int[,] array2 = { { 10, 20 }, { 30, 40 } };
            Assert.True(ArrayUtils.Equals(array1, array2, EqualityComparer<int>.Default));
        }
    }
}