/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;
using System;
using System.Numerics;
using System.Collections.Generic;
using System.Reflection;

namespace AltFormatter.Utils
{
    public sealed partial class TypeUtilsTests
    {
    	private static readonly MethodInfo convertTo = typeof(TypeUtils).GetMethod("ConvertTo");
    	
        private static readonly object[] PrimitiveObjects =
        {
            new object[]{typeof(bool),true,"True"},
            new object[]{typeof(byte),(byte)127,"127"},
            new object[]{typeof(sbyte),(sbyte)127,"127"},
            new object[]{typeof(short),(short)(-10),"-10"},
            new object[]{typeof(ushort),(ushort)10,"10"},
            new object[]{typeof(int),(int)(-100),"-100"},
            new object[]{typeof(uint),(uint)100,"100"},
            new object[]{typeof(long),(long)(-1000),"-1000"},
            new object[]{typeof(ulong),(ulong)1000,"1000"},
            new object[]{typeof(string),(string)"Test","Test"},
            new object[]{typeof(char),(char)'X',"X"},
            new object[]{typeof(float),(float)1.111,"1.111"},
            new object[]{typeof(double),(double)1.111E3,"1111"},
            new object[]{typeof(DateTime),new DateTime(1,DateTimeKind.Local),"LOCAL 1/1/1 0:0:0.0"},
            new object[]{typeof(DateTime),new DateTime(1,DateTimeKind.Unspecified),"UNSPECIFIED 1/1/1 0:0:0.0"},
            new object[]{typeof(DateTime),new DateTime(1,DateTimeKind.Utc),"UTC 1/1/1 0:0:0.0"},
            new object[]{typeof(TimeSpan),new TimeSpan(1000),"00:00:00.0001000"},
            new object[]{typeof(decimal),(decimal)1000,"1000"},
            new object[]{typeof(Complex),new Complex(10E-3,20E+6),"(0.01,20000000)"},
            new object[]{typeof(AttributeTargets),AttributeTargets.All,"all"},
        };

        [Test, TestCaseSource("PrimitiveObjects")]
        public void GetConvertersToObject_PrimitiveType_Converter(Type type, object obj, string value)
        {
            Assert.NotNull(TypeUtils.GetConverterToObject(type));
        }
        
        [Test]
        public void GetConvertersToObject_InvalidType_Null()
        {
        	Assert.Null(TypeUtils.GetConverterToObject(this.GetType()));
        }

        [Test, TestCaseSource("PrimitiveObjects")]
        public void GetConvertersToString_PrimitiveType_Converter(Type type, object obj, string value)
        {
            Assert.NotNull(TypeUtils.GetConverterToString(type));
        }
        
        [Test]
        public void GetConvertersToString_InvalidType_Null()
        {
        	Assert.Null(TypeUtils.GetConverterToString(this.GetType()));
        }

        [Test, TestCaseSource("PrimitiveObjects")]
        public void ConvertToString_PrimitiveType_SpecifiedValue(Type type, object obj, string value)
        {
            Assert.AreEqual(value, TypeUtils.GetConverterToString(type)(obj));
        }
        
        [Test, TestCaseSource("PrimitiveObjects")]
        public void ConvertToObject_InvariantCultureString_SpecifiedValue(Type type, object obj, string value)
        {
            if (type == typeof(DateTime))
            {
                Assert.AreEqual(((DateTime)obj).ToUniversalTime().ToString(), ((DateTime)TypeUtils.GetConverterToObject(type)(value)).ToUniversalTime().ToString());
            }
            else
            {
                Assert.AreEqual(obj, TypeUtils.GetConverterToObject(type)(value));
            }
        }
        
        [Test, TestCaseSource("PrimitiveObjects")]
        public void TryConvertToObject_InvalidString_Exception(Type type, object obj, string value)
        {
        	if (type == typeof(string) || type == typeof(DateTime) || type.IsEnum)
            {
            	return;
            }
        	
        	var ex = Assert.Throws<StringConversionNotSupportedException>(() => TypeUtils.GetConverterToObject(type)("InvalidValueString"));
        	ex.AssertArguments("InvalidValueString", type.Name);
        }
        
        [Test]
        public void TryConvertToEnum_InvalidString_Exception()
        {
        	Assert.Throws<ArgumentException>(() => TypeUtils.GetConverterToObject(typeof(AttributeTargets))("InvalidValueString"));
        }
        
        [Test]
        public void TryConvertToDateTime_InvalidString_Exception()
        {
        	Assert.Throws<ArgumentOutOfRangeException>(() => TypeUtils.GetConverterToObject(typeof(DateTime))("InvalidValueString"));
        }
        
        [Test]
        public void ConvertTo_TypeTToTypeT_TypeT()
        {
        	const int value = 3;
        	Assert.AreEqual(value, value.ConvertTo<int>());
        }
        
        [Test]
        public void ConvertTo_DerivedTypeToBaseType_BaseType()
        {
        	ClassB obj = new ClassB();
        	Assert.AreEqual(obj, obj.ConvertTo<ClassA>());
        }
        
        [Test]
        public void ConvertTo_BaseTypeToDerivedType_Exception()
        {
        	ClassA obj = new ClassA();
        	
        	var ex = Assert.Throws<TypeConversionNotSupportedException>(() => obj.ConvertTo<ClassB>());
        	ex.AssertArguments(obj.ToString(), typeof(ClassA).Name, typeof(ClassB).Name);
        }
        
        [Test, TestCaseSource("PrimitiveObjects")]
        public void ConvertTo_String_PrimitiveType(Type type, object obj, string value)
        {
            if (type == typeof(DateTime))
            {
            	Assert.AreEqual(((DateTime)obj).ToUniversalTime().ToString(), value.ConvertTo<DateTime>().ToUniversalTime().ToString());
            }
            else
            {
                Assert.AreEqual(obj, convertTo.MakeGenericMethod(type).Invoke(null, new object[] { value }));
            }
        }
        
        [Test, TestCaseSource("PrimitiveObjects")]
        public void ConvertTo_PrimitiveType_String(Type type, object obj, string value)
        {
        	Assert.AreEqual(value, obj.ConvertTo<string>());
        }
        
        [Test]
        public void ConvertTo_InvalidTypeToNotString_Exception()
        {
        	ClassA obj = new ClassA();
        	
        	var ex = Assert.Throws<TypeConversionNotSupportedException>(() => obj.ConvertTo<int>());
        	ex.AssertArguments(obj.ToString(), obj.GetType().Name, typeof(int).Name);
        }
        
        [Test]
        public void ConvertTo_PrimitiveTypeToInvalidType_Exception()
        {
        	var ex = Assert.Throws<TypeConversionNotSupportedException>(() => ((int)1).ConvertTo<ClassA>());
        	ex.AssertArguments(1, typeof(int).Name, typeof(ClassA).Name);
        }
        
        [Test]
        public void ConvertTo_ValueNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (null as string).ConvertTo<string>(),
        		"Value");
        }
        
        [Test]
        public void IsBaseTypeOrEquals_BaseTypeInterfaceDerivedTypeInterface_True()
        {
            Assert.True(typeof(ICollection<int>).IsBaseTypeOrEquals(typeof(IList<int>)));
        }
        
        [Test]
        public void IsBaseTypeOrEquals_BaseTypeInterfaceDerivedTypeClass_True()
        {
            Assert.True(typeof(ICollection<int>).IsBaseTypeOrEquals(typeof(List<int>)));
        }
        
        [Test]
        public void IsBaseTypeOrEquals_BaseTypeClassDerivedTypeClass_True()
        {
            Assert.True(typeof(ClassA).IsBaseTypeOrEquals(typeof(ClassB)));
        }
        
        [Test]
        public void IsBaseTypeOrEquals_TypeInterfaceTypeInterface_True()
        {
            Assert.True(typeof(ICollection<int>).IsBaseTypeOrEquals(typeof(ICollection<int>)));
        }
        
        [Test]
        public void IsBaseTypeOrEquals_TypeClassTypeClass_True()
        {
            Assert.True(typeof(List<int>).IsBaseTypeOrEquals(typeof(List<int>)));
        }
        
        [Test]
        public void IsBaseTypeOrEquals_NotBaseTypeType_False()
        {
            Assert.False(typeof(List<int>).IsBaseTypeOrEquals(typeof(ICollection<int>)));
        }
        
        [Test]
        public void IsBaseTypeOrEquals_TypeNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (null as Type).IsBaseTypeOrEquals(typeof(ClassB)),
        		"Type");
        }
        
        [Test]
        public void IsBaseTypeOrEquals_DerivedTypeNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => typeof(ClassA).IsBaseTypeOrEquals(null),
        		"DerivedType");
        }

        [Test]
        public void IsBaseInterfaceOrEquals_BaseTypeDerivedType_True()
        {
            Assert.True(typeof(IList<int>).IsBaseInterfaceOrEquals(typeof(List<int>)));
        }

        [Test]
        public void IsBaseInterfaceOrEquals_TypeType_True()
        {
            Assert.True(typeof(IList<int>).IsBaseInterfaceOrEquals(typeof(IList<int>)));
        }

        [Test]
        public void IsBaseInterfaceOrEquals_NotBaseTypeType_False()
        {
            Assert.False(typeof(List<int>).IsBaseInterfaceOrEquals(typeof(IList<int>)));
        }

        [Test]
        public void IsBaseInterfaceOrEquals_TypeNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (null as Type).IsBaseInterfaceOrEquals(typeof(ClassB)),
        		"Type");
        }
        
        [Test]
        public void IsBaseInterfaceOrEquals_DerivedTypeNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => typeof(IComparable).IsBaseInterfaceOrEquals(null),
        		"DerivedType");
        }
        
        [Test]
        public void IsBaseClassOrEquals_BaseTypeDerivedType_True()
        {
            Assert.True(typeof(ClassA).IsBaseClassOrEquals(typeof(ClassB)));
        }

        [Test]
        public void IsBaseClassOrEquals_TypeType_True()
        {
            Assert.True(typeof(ClassA).IsBaseClassOrEquals(typeof(ClassA)));
        }

        [Test]
        public void IsBaseClassOrEquals_NotBaseTypeType_False()
        {
            Assert.False(typeof(ClassB).IsBaseClassOrEquals(typeof(ClassA)));
        }
        
        [Test]
        public void IsBaseClassOrEquals_TypeNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (null as Type).IsBaseClassOrEquals(typeof(ClassB)),
        		"Type");
        }
        
        [Test]
        public void IsBaseClassOrEquals_DerivedTypeNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => typeof(ClassA).IsBaseClassOrEquals(null),
        		"DerivedType");
        }
        
        [Test]
        public void GetAttributeAssembly_AttributeExists_Attribute()
        {
        	Assert.NotNull(Assembly.GetExecutingAssembly().GetAttribute<TypeUtilsAssemblyAttribute>());
        }

        [Test]
        public void GetAttributeAssembly_AttributeNotExists_Null()
        {
            Assert.Null(Assembly.GetExecutingAssembly().GetAttribute<NotInheritedAttribute>());
        }
        
        [Test]
        public void GetAttributeAssembly_AssemblyNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (null as Assembly).GetAttribute<TypeUtilsAssemblyAttribute>(),
        		"Assembly");
        }
        
        [Test]
        public void GetAttributeMemberInfo_GetInheritedAttributeBaseClass_Attribute()
        {
            Assert.NotNull(typeof(ClassA).GetAttribute<InheritedAttribute>(false));
        }
        
        [Test]
        public void GetAttributeMemberInfo_BaseInheritedTrue_InheritedAttribute()
        {
            Assert.NotNull(typeof(ClassA).GetAttribute<InheritedAttribute>(true));
        }
        
        [Test]
        public void GetAttributeMemberInfo_BaseInheritedFalse_InheritedAttribute()
        {
            Assert.NotNull(typeof(ClassA).GetAttribute<InheritedAttribute>(false));
        }
        
        [Test]
        public void GetAttributeMemberInfo_DerivedInheritedTrue_InheritedAttribute()
        {
            Assert.NotNull(typeof(ClassB).GetAttribute<InheritedAttribute>(true));
        }
        
        [Test]
        public void GetAttributeMemberInfo_DerivedInheritedFalse_Null()
        {
             Assert.Null(typeof(ClassB).GetAttribute<InheritedAttribute>(false));
        }
        
        [Test]
        public void GetAttributeMemberInfo_NotExistAttributeInheritedTrue_Null()
        {
             Assert.Null(typeof(ClassA).GetAttribute<NotInheritedAttribute>(false));           
        }
        
        [Test]
        public void GetAttributeMemberInfo_NotExistAttributeInheritedFalse_Null()
        {
        	Assert.Null(typeof(ClassA).GetAttribute<NotInheritedAttribute>(true));
        }
        
        [Test]
        public void GetAttribute_MemberInfoNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (null as MemberInfo).GetAttribute<TypeUtilsAssemblyAttribute>(true),
        		"MemberInfo");
        }
        
        [Test, TestCaseSource("PrimitiveObjects")]
        public void GetFriendlyName_PrimitiveType_String(Type type, object obj, string value)
        {
        	Assert.AreEqual(type.Name.ToLowerInvariant(), type.GetFriendlyName().ToLowerInvariant(), type.Name);
        }
        
        [Test, TestCaseSource("PrimitiveObjects")]
        public void GetFriendlyName_GenericType_String(Type type, object obj, string value)
        {
        	string expected = string.Format("icomparable<{0}>",type.Name).ToLowerInvariant();
        	string received = typeof(IComparable<>).MakeGenericType(type).GetFriendlyName().ToLowerInvariant();
        	Assert.AreEqual(expected, received, type.Name);
        }

        [Test, TestCaseSource("PrimitiveObjects")]
        public void GetFriendlyName_ComplexGenericType_String(Type type, object obj, string value)
        {
        	string expected = string.Format("icomparable<icomparable<{0}>>",type.Name).ToLowerInvariant();
        	string received = typeof(IComparable<>).MakeGenericType(typeof(IComparable<>).MakeGenericType(type)).GetFriendlyName().ToLowerInvariant();
        	Assert.AreEqual(expected, received, type.Name);
        }
        
        [Test]
        public void GetFriendlyName_TypeNull_ValidateNotNull()
        {
            AssertValidation.NotNull(
        		() => (null as Type).GetFriendlyName(),
        		"Type");
        }
    }
}