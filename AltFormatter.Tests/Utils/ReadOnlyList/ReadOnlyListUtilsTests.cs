/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;

namespace AltFormatter.Utils.ReadOnlyList
{
    public sealed class ReadOnlyListUtilsTests
    {
        [Test]
        public void Any_True()
        {
            int[] source = GetSource();
            Assert.True(source.Any(v => v > 5));
        }
        
        [Test]
        public void Any_False()
        {
            int[] source = GetSource();
            Assert.True(source.Any(v => v > 5));
        }
        
        [Test]
        public void Any_ListNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (null as int[]).Any(i => true),
        		"List");
        }
        
        [Test]
        public void Any_PredicateNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (new int[0]).Any(null),
        		"Predicate");
        }

        [Test]
        public void All_True()
        {
            int[] source = GetSource();
            Assert.True(source.All(v => v < 11));;
        }
        
        [Test]
        public void All_False()
        {
            int[] source = GetSource();
            Assert.False(source.All(v => v < 10));
        }
        
        [Test]
        public void All_ListNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (null as int[]).All(i => true),
        		"List");
        }
        
        [Test]
        public void All_PredicateNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (new int[0]).All(null),
        		"Predicate");
        }

        [Test]
        public void Contains_True()
        {
            int[] source = GetSource();
            Assert.True(source.Contains(5));
        }
        
        [Test]
        public void Contains_False()
        {
            int[] source = GetSource();
            Assert.False(source.Contains(20));
        }
        
        [Test]
        public void Contains_ListNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (null as int[]).Contains(0),
        		"List");
        }
        
        [Test]
        public void Count_Zero()
        {
            int[] source = GetSource();
            Assert.AreEqual(0, source.Count(v => v > 20));
        }

        [Test]
        public void Count_NotZero()
        {
            int[] source = GetSource();
            Assert.AreEqual(5, source.Count(v => v % 2 != 0));
        }
        
        [Test]
        public void Count_ListNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (null as int[]).Count(i => true),
        		"List");
        }
        
        [Test]
        public void Count_PredicateNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (new int[0]).Count(null),
        		"Predicate");
        }

        [Test]
        public void Execute()
        {
            int[] source = GetSource();
            int sum = 0;
            source.Execute(v => sum += v);
            Assert.AreEqual(55, sum);
        }
        
        [Test]
        public void Execute_ListNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (null as int[]).Execute(i => {}),
        		"List");
        }
        
        [Test]
        public void Execute_ActionNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (new int[0]).Execute(null),
        		"Action");
        }
        
        [Test]
        public void FindOrDefault_ItemExist_Item()
        {
            int[] source = GetSource();
            Assert.AreEqual(5, source.FindOrDefault(v => v == 5));
        }
        
        [Test]
        public void FindOrDefault_ItemNotExist_Default()
        {
            int[] source = GetSource();
            Assert.AreEqual(0, source.FindOrDefault(v => v > 20));
        }
        
        [Test]
        public void FindOrDefault_ListNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (null as int[]).FindOrDefault(v => v == 0),
        		"List");
        }
        
        [Test]
        public void Find_PredicateNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (new int[0]).FindOrDefault(null),
        		"Predicate");
        }
        
        public void GetMinMaxOrDefault_EmptyArrayWithoutSelector_DefaultMinMax()
        {
            MinMax<string> minMax = (new string[0]).GetMinMaxOrDefault();
            Assert.AreEqual(null, minMax.Min);
            Assert.AreEqual(null, minMax.Max);
        }
        
        public void GetMinMaxOrDefault_ArraySize1WithoutSelector_FirstItemMinMax()
        {
        	const string value = "XXX";
            MinMax<string> minMax = (new [] {value}).GetMinMaxOrDefault();
            Assert.AreEqual(value, minMax.Min);
            Assert.AreEqual(value, minMax.Max);
        }
        
        public void GetMinMaxOrDefault_BigArrayWithoutSelector_ArrayMinMax()
        {
        	int[] source = GetSource();
            MinMax<int> minMax = source.GetMinMaxOrDefault();
            Assert.AreEqual(0, minMax.Min);
            Assert.AreEqual(10, minMax.Max);
        }
        
        [Test]
        public void GetMinMaxOrDefault_ArrayNullWithoutSelector_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (null as int[]).GetMinMaxOrDefault(),
        		"List");
        }
        
        public void GetMinMaxOrDefault_EmptyArrayWithSelector_DefaultMinMax()
        {
        	MinMax<string> minMax = (new int[0]).GetMinMaxOrDefault(s => s.ToString());
            Assert.AreEqual(null, minMax.Min);
            Assert.AreEqual(null, minMax.Max);
        }
        
        public void GetMinMaxOrDefault_ArraySize1WithSelector_FirstItemMinMax()
        {
        	const int value = 1;
        	const string selectedValue = "1";
        	
            MinMax<string> minMax = (new [] {value}).GetMinMaxOrDefault(s => s.ToString());
            Assert.AreEqual(selectedValue, minMax.Min);
            Assert.AreEqual(selectedValue, minMax.Max);
        }
        
        public void GetMinMaxOrDefault_BigArrayWithSelector_ArrayMinMax()
        {
        	int[] source = GetSource();
            MinMax<string> minMax = source.GetMinMaxOrDefault(s => s.ToString());
            Assert.AreEqual("0", minMax.Min);
            Assert.AreEqual("10", minMax.Max);
        }
        
        [Test]
        public void GetMinMaxOrDefault_ArrayNullWithSelector_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (null as int[]).GetMinMaxOrDefault(s => s.ToString()),
        		"List");
        }
        
        [Test]
        public void GetMinMaxOrDefault_SelectorNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (new int[2]).GetMinMaxOrDefault<int, string>(null),
        		"Selector");
        }
        
        [Test]
        public void IndexOf_ItemExists_ItemIndex()
        {
            int[] source = GetSource();
            Assert.AreEqual(5, source.IndexOf(5));
        }
        
        [Test]
        public void IndexOf_ItemNotExists_NegativeOneIndex()
        {
            int[] source = GetSource();
            Assert.AreEqual(-1, source.IndexOf(20));
        }
        
        [Test]
        public void IndexOf_ListNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (null as int[]).IndexOf(0),
        		"List");
        }

        private static int[] GetSource()
        {
            return new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        }
    }
}