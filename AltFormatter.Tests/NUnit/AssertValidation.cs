/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Localization;
using AltFormatter.Utils;
using System;

namespace NUnit.Framework
{
	public static class AssertValidation
	{
		public static void NotNull(Action action, string name)
		{
            LocalizedException ex = Assert.Throws<ValueNullException>(
            	() => action());

            ex.AssertArguments(name);
		}
		
		public static void NotNullAny(Action action, string name)
		{
            LocalizedException ex = Assert.Throws<ValueNullException>(
            	() => action());

            ex.AssertArguments(name);
		}
		
		public static void NotNullAll(Action action, string name, int expectedIndex)
		{
            LocalizedException ex = Assert.Throws<ValueNullException>(
            	() => action());

            ex.AssertArguments(StringUtils.Combine(name, "[", expectedIndex.ToString(), "]"));
		}
		
		
		public static void MoreThan<T>(Action<T> action, string name, T expectedValue) where T : IComparable<T>
		{
			var ex1 = Assert.Throws<ValueLessThanOrEqualsException>(
				() => action(expectedValue));
			
			ex1.AssertArguments(name, expectedValue, expectedValue);
			
			T previousValue = (expectedValue.ConvertTo<double>() - 1.0).ConvertTo<T>();
			var ex2 = Assert.Throws<ValueLessThanOrEqualsException>(
				() => action(previousValue));
			
			ex2.AssertArguments(name, previousValue, expectedValue);
		}
		
		public static void MoreThanOrEquals<T>(Action<T> action, string name, T expectedValue) where T : IComparable<T>
		{
			T previousValue = (expectedValue.ConvertTo<double>() - 1.0).ConvertTo<T>();
			var ex = Assert.Throws<ValueLessThanException>(
				() => action(previousValue));
			
			ex.AssertArguments(name, previousValue, expectedValue);
		}
		
		public static void InRange<T>(Action<T> action, string name, T minValue, T maxValue) where T : IComparable<T>
		{
			T lessThanValue = (minValue.ConvertTo<double>() - 1.0).ConvertTo<T>();
			var ex1 = Assert.Throws<ValueOutOfRangeException>(
				() => action(lessThanValue));
			
			ex1.AssertArguments(name, lessThanValue, minValue, maxValue);
			
			T maxThanValue = (maxValue.ConvertTo<double>() + 1.0).ConvertTo<T>();
			var ex2 = Assert.Throws<ValueOutOfRangeException>(
				() => action(maxThanValue));
			
			ex2.AssertArguments(name, maxThanValue, minValue, maxValue);
		}
		
		public static void SizeMoreThan(Action<int> action, string name, int size)
		{
			var ex1 = Assert.Throws<ValueLessThanOrEqualsException>(
				() => action(size));
			
			ex1.AssertArguments(StringUtils.Combine(name,".Count"), size, size);
			
			if(size >= 1)
			{
				int previousSize = size - 1;
				var ex2 = Assert.Throws<ValueLessThanOrEqualsException>(
					() => action(previousSize));
				
				ex2.AssertArguments(StringUtils.Combine(name,".Count"), previousSize, size);
			}
		}
		
		public static void SizeMoreThanOrEquals(Action<int> action, string name, int size)
		{
			if(size >= 1)
			{
				int previousSize = size - 1;
				var ex = Assert.Throws<ValueLessThanException>(
					() => action(previousSize));
				
				ex.AssertArguments(StringUtils.Combine(name,".Count"), previousSize, size);
			}
		}
		
		public static void SizesEquals(Action action, string name1, string name2, int invalidIndex, int size1, int size2)
		{
			var ex = Assert.Throws<ValuesHaveDifferentSizesException>(
				() => action());
			
			string newName2 = StringUtils.Combine(name2, "[", invalidIndex.ToString() , "]");
			
			ex.AssertArguments(name1, newName2, name1, size1, newName2, size2);
		}
	}
}