/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace AltFormatter.Utils
{
	internal sealed partial class ReflectionUtilsTests
	{
        [Test, TestCaseSource("AllObjects")]
        public void CreateArray_AllTypes_Array(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 1);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank2_ArrayRank2(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 2);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank3_ArrayRank3(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 3);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank4_ArrayRank4(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 4);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank5_ArrayRank5(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 5);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank6_ArrayRank6(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 6);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank7_ArrayRank7(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 7);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank8_ArrayRank8(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 8);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank9_ArrayRank9(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 9);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank10_ArrayRank10(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 10);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank11_ArrayRank11(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 11);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank12_ArrayRank12(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 12);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank13_ArrayRank13(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 13);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank14_ArrayRank14(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 14);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank15_ArrayRank15(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 15);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank16_ArrayRank16(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 16);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank17_ArrayRank17(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 17);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank18_ArrayRank18(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 18);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank19_ArrayRank19(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 19);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank20_ArrayRank20(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 20);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank21_ArrayRank21(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 21);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank22_ArrayRank22(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 22);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank23_ArrayRank23(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 23);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank24_ArrayRank24(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 24);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank25_ArrayRank25(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 25);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank26_ArrayRank26(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 26);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank27_ArrayRank27(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 27);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank28_ArrayRank28(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 28);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank29_ArrayRank29(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 29);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank30_ArrayRank30(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 30);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank31_ArrayRank31(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 31);
        }
        
        [Test, TestCaseSource("AllObjects")]
        public void CreateMDArray_AllTypesRank32_ArrayRank32(Type type, object obj, ReflectionsUtilsStrategy strategy)
        {
        	CreateMDArrayAndTest(strategy, type, 32);
        }
        
        [Test]
        public void CreateMDArray_ElementTypeNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => ReflectionUtils.CreateMDArray(null, 1),
        		"ElementType");
        }
        
        [Test]
        public void CreateMDArray_RankLessThanOrEqualsZero_ValidateMoreThan()
        {
        	AssertValidation.MoreThan(
        		(s) => ReflectionUtils.CreateMDArray(typeof(int), s),
        		"Rank", 0);
        }
        
        [Test]
        public void CreateMDArrayCallback_InvalidArgValue_Exception()
        {
        	Assert.Throws<OverflowException>(() => ReflectionUtils.CreateMDArray(typeof(int), 1).Invoke(new int[] {-1}));
        }
        
        [Test]
        public void CreateMDArrayCallback_InvalidArgsCount_Exception()
        {
        	Assert.Throws<TargetParameterCountException>(() => ReflectionUtils.CreateMDArray(typeof(int), 1).Invoke(new int[] {1, 2}));
        }
        
        private static void CreateMDArrayAndTest(ReflectionsUtilsStrategy strategy, Type elementType, int rank)
        {
        	if (!TrySetStrategy(strategy))
            {
            	return;
            }
        	
            IFunctionCallback<int[], IEnumerable> mdArrayActivator = ReflectionUtils.CreateMDArray(elementType, rank);
            Array array = mdArrayActivator.Invoke(new int[rank].Select((v,i) => 1)) as Array;
            
            Assert.AreEqual(rank, array.Rank);
            Assert.AreEqual(1, array.Length);
        }
	}
}