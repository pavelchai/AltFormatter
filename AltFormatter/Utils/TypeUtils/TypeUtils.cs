/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System;
using System.Numerics;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace AltFormatter.Utils
{
    /// <summary>
    /// Represents the utils for the <see cref="Type"/> and <see cref="object"/>.
    /// </summary>
    public static class TypeUtils
    {
        /// <summary>
        /// String that represents a null value
        /// </summary>
        private const string StringNullValue = "null_value";

        /// <summary>
        /// Invariant culture.
        /// </summary>
        private readonly static CultureInfo invariantCulture = CultureInfo.InvariantCulture;

        /// <summary>
        /// Converters from object to string.
        /// </summary>
        private readonly static Dictionary<Type, Func<object, string>> convertersToString;

        /// <summary>
        /// Converters from string to object.
        /// </summary>
        private readonly static Dictionary<Type, Func<string, object>> convertersToObject;

        /// <summary>
        /// Initializes the the <see cref="TypeUtils"></see>.
        /// </summary>
        static TypeUtils()
        {
            // Converters to string

            Func<object, string> floatConverterToString = (o) =>
             {
                 float val = (float)o;
                 
                 if (val.Equals(float.MinValue))
                 {
                 	return "MinValue";
                 }
                 
                 if (val.Equals(float.MaxValue))
                 {
                 	return "MaxValue";
                 }
                 
                 return val.ToString(invariantCulture);
             };

            Func<double, string> fromDoubleConverter = (o) =>
             {
                 double val = (double)o;
                 
                 if (val.Equals(double.MinValue))
                 {
                 	return "MinValue";
                 }
                 
                 if (val.Equals(double.MaxValue))
                 {
                 	return "MaxValue";
                 }
                 
                 return val.ToString(invariantCulture);
             };

            Func<object, string> doubleConverterToString = (o) => fromDoubleConverter((double)o);

            Func<object, string> decimalConverterToString = (o) =>
             {
                 decimal val = (decimal)o;
                 
                 if (val.Equals(decimal.MinValue))
                 {
                 	return "MinValue";
                 }
                 
                 if (val.Equals(decimal.MaxValue))
                 {
                 	return "MaxValue";
                 }
                 
                 return val.ToString(invariantCulture);
             };

            Func<object, string> complexConverterToString = (o) =>
             {
                 Complex val = (Complex)o;
                 return StringUtils.Combine("(", fromDoubleConverter(val.Real), ",", fromDoubleConverter(val.Imaginary), ")");
             };

            Func<object, string> dateTimeConverterToString = (o) =>
             {
                 DateTime val = (DateTime)o;
                 string label = null;

                 switch (val.Kind)
                 {
                     case DateTimeKind.Local:
                         val = val.ToUniversalTime();
                         label = "LOCAL";
                         break;
                     case DateTimeKind.Unspecified:
                         label = "UNSPECIFIED";
                         break;
                     case DateTimeKind.Utc:
                         label = "UTC";
                         break;
                 }

                 return string.Format(invariantCulture, "{0} {1:D2}/{2:D2}/{3:D4} {4:D2}:{5:D2}:{6:D2}.{7:D3}",
                                      label,
                                      val.Month.ToString(invariantCulture),
                                      val.Day.ToString(invariantCulture),
                                      val.Year.ToString(invariantCulture),
                                      val.Hour.ToString(invariantCulture),
                                      val.Minute.ToString(invariantCulture),
                                      val.Second.ToString(invariantCulture),
                                      val.Millisecond.ToString(invariantCulture)
                 );
             };

            convertersToString = new Dictionary<Type, Func<object, string>>(17);
            convertersToString.Add(typeof(bool), (o) => ((bool)o).ToString(invariantCulture));
            convertersToString.Add(typeof(byte), (o) => ((byte)o).ToString(invariantCulture));
            convertersToString.Add(typeof(sbyte), (o) => ((sbyte)o).ToString(invariantCulture));
            convertersToString.Add(typeof(short), (o) => ((short)o).ToString(invariantCulture));
            convertersToString.Add(typeof(ushort), (o) => ((ushort)o).ToString(invariantCulture));
            convertersToString.Add(typeof(int), (o) => ((int)o).ToString(invariantCulture));
            convertersToString.Add(typeof(uint), (o) => ((uint)o).ToString(invariantCulture));
            convertersToString.Add(typeof(long), (o) => ((long)o).ToString(invariantCulture));
            convertersToString.Add(typeof(ulong), (o) => ((ulong)o).ToString(invariantCulture));
            convertersToString.Add(typeof(string), (o) => { if (o == null) return StringNullValue; else return ((string)o).ToString(invariantCulture); });
            convertersToString.Add(typeof(char), (o) => ((char)o).ToString(invariantCulture));
            convertersToString.Add(typeof(DateTime), dateTimeConverterToString);
            convertersToString.Add(typeof(TimeSpan), (o) => ((TimeSpan)o).ToString(null, invariantCulture));
            convertersToString.Add(typeof(float), floatConverterToString);
            convertersToString.Add(typeof(double), doubleConverterToString);
            convertersToString.Add(typeof(decimal), decimalConverterToString);
            convertersToString.Add(typeof(Complex), complexConverterToString);

            // Converters to object

            Func<string, object> boolConverterToObject = (s) =>
             {
                 bool val;
                 
                 if (bool.TryParse(s, out val))
                 {
                 	return val;
                 }
                 
                 return ConversionFromStringRaiseError<bool>(s);
             };

            Func<string, object> byteConverterToObject = (s) =>
             {
                 byte val;
                 
                 if (byte.TryParse(s, NumberStyles.Any, invariantCulture, out val))
                 {
                 	return val;
                 }
                 
                 return ConversionFromStringRaiseError<byte>(s);
             };

            Func<string, object> sbyteConverterToObject = (s) =>
             {
                 sbyte val;
                 
                 if (sbyte.TryParse(s, NumberStyles.Any, invariantCulture, out val))
                 {
                 	return val;
                 }
                 
                 return ConversionFromStringRaiseError<sbyte>(s);
             };

            Func<string, object> shortConverterToObject = (s) =>
             {
                 short val;
                 
                 if (short.TryParse(s, NumberStyles.Any, invariantCulture, out val))
                 {
                 	return val;
                 }
                 
                 return ConversionFromStringRaiseError<short>(s);
             };

            Func<string, object> ushortConverterToObject = (s) =>
             {
                 ushort val;
                 
                 if (ushort.TryParse(s, NumberStyles.Any, invariantCulture, out val))
                 {
                 	return val;
                 }
                 
                 return ConversionFromStringRaiseError<ushort>(s);
             };

            Func<string, object> intConverterToObject = (s) =>
             {
                 int val;
                 
                 if (int.TryParse(s, NumberStyles.Any, invariantCulture, out val))
                 {
                 	return val;
                 }
                 
                 return ConversionFromStringRaiseError<int>(s);
             };

            Func<string, object> uintConverterToObject = (s) =>
             {
                 uint val;
                 
                 if (uint.TryParse(s, NumberStyles.Any, invariantCulture, out val))
                 {
                 	return val;
                 }
                 
                 return ConversionFromStringRaiseError<uint>(s);
             };

            Func<string, object> longConverterToObject = (s) =>
             {
                 long val;
                 
                 if (long.TryParse(s, NumberStyles.Any, invariantCulture, out val))
                 {
                 	return val;
                 }
                 
                 return ConversionFromStringRaiseError<long>(s);
             };

            Func<string, object> ulongConverterToObject = (s) =>
             {
                 ulong val;
                 
                 if (ulong.TryParse(s, NumberStyles.Any, invariantCulture, out val))
                 {
                 	return val;
                 }
                 
                 return ConversionFromStringRaiseError<ulong>(s);
             };

            Func<string, object> charConverterToObject = (s) =>
             {
                 char val;
                 
                 if (char.TryParse(s, out val))
                 {
                 	return val;
                 }
                 
                 return ConversionFromStringRaiseError<char>(s);
             };

            Func<string, object> dateTimeConverterToObject = (s) =>
             {
                 IEnumerable<string> splitted = StringUtils.Split(s, ' ', '/', ':', '.');

                 bool isFirst = true;
                 DateTimeKind targetKind = DateTimeKind.Utc;
                 int[] values = new int[7];
                 int index = 0;

                 foreach (var split in splitted)
                 {
                     if (isFirst)
                     {
                         if (split == "LOCAL")
                         {
                         	targetKind = DateTimeKind.Local;
                         }
                         
                         if (split == "UNSPECIFIED")
                         {
                         	targetKind = DateTimeKind.Unspecified;
                         }
                         
                         if (split == "UTC")
                         {
                         	targetKind = DateTimeKind.Utc;
                         }
                         
                         isFirst = false;
                     }
                     else
                     {
                         values[index++] = int.Parse(split, invariantCulture);
                     }
                 }

                 DateTime val = default(DateTime);
                 if (targetKind == DateTimeKind.Unspecified)
                 {
                     val = new DateTime(values[2], values[0], values[1], values[3], values[4], values[5], values[6], DateTimeKind.Unspecified);
                 }
                 else
                 {
                     val = new DateTime(values[2], values[0], values[1], values[3], values[4], values[5], values[6], DateTimeKind.Utc);
                     if (targetKind == DateTimeKind.Local)
                     {
                     	val = val.ToLocalTime();
                     }
                 }

                 return val;
             };

            Func<string, object> timeSpanConverterToObject = (s) =>
             {
                 TimeSpan val;
                 
                 if (TimeSpan.TryParse(s, invariantCulture, out val))
                 {
                 	return val;
                 }
                 
                 return ConversionFromStringRaiseError<TimeSpan>(s);
             };

            Func<string, object> floatConverterToObject = (s) =>
             {
                 if (s == "MinValue")
                 {
                 	return float.MinValue;
                 }
                 
                 if (s == "MaxValue")
                 {
                 	return float.MaxValue;
                 }

                 float val;
                 
                 if (float.TryParse(s, NumberStyles.Any, invariantCulture, out val))
                 {
                 	return val;
                 }
                 
                 return ConversionFromStringRaiseError<float>(s);
             };

            Func<string, double> toDoubleConverter = (s) =>
             {
                 if (s == "MinValue")
                 {
                 	return double.MinValue;
                 }
                 
                 if (s == "MaxValue")
                 {
                 	return double.MaxValue;
                 }

                 double val;
                 
                 if (double.TryParse(s, NumberStyles.Any, invariantCulture, out val))
                 {
                 	return val;
                 }
                 
                 return ConversionFromStringRaiseError<double>(s);
             };
            Func<string, object> doubleConverterToObject = (s) => toDoubleConverter(s);

            Func<string, object> decimalConverterToObject = (s) =>
             {
                 if (s == "MinValue")
                 {
                 	return decimal.MinValue;
                 }
                 
                 if (s == "MaxValue")
                 {
                 	return decimal.MaxValue;
                 }

                 decimal val;
                 if (decimal.TryParse(s, NumberStyles.Any, invariantCulture, out val))
                 {
                 	return val;
                 }
                 
                 return ConversionFromStringRaiseError<decimal>(s);
             };

            Func<string, object> complexConverterToObject = (s) =>
             {
                 string[] data = s.Split(',');
                 if (data.Length != 2)
                 {
                 	ConversionFromStringRaiseError<Complex>(s);
                 }
                 
                 return new Complex(toDoubleConverter(data[0].Replace("(", "")), toDoubleConverter(data[1].Replace(")", "")));
             };

            convertersToObject = new Dictionary<Type, Func<string, object>>(17);
            convertersToObject.Add(typeof(bool), boolConverterToObject);
            convertersToObject.Add(typeof(byte), byteConverterToObject);
            convertersToObject.Add(typeof(sbyte), sbyteConverterToObject);
            convertersToObject.Add(typeof(short), shortConverterToObject);
            convertersToObject.Add(typeof(ushort), ushortConverterToObject);
            convertersToObject.Add(typeof(int), intConverterToObject);
            convertersToObject.Add(typeof(uint), uintConverterToObject);
            convertersToObject.Add(typeof(long), longConverterToObject);
            convertersToObject.Add(typeof(ulong), ulongConverterToObject);
            convertersToObject.Add(typeof(char), charConverterToObject);
            convertersToObject.Add(typeof(string), (s) => (s != StringNullValue) ? s : null);
            convertersToObject.Add(typeof(DateTime), dateTimeConverterToObject);
            convertersToObject.Add(typeof(TimeSpan), timeSpanConverterToObject);
            convertersToObject.Add(typeof(float), floatConverterToObject);
            convertersToObject.Add(typeof(double), doubleConverterToObject);
            convertersToObject.Add(typeof(decimal), decimalConverterToObject);
            convertersToObject.Add(typeof(Complex), complexConverterToObject);
        }
        
        /// <summary>
        /// Casts the value of the complex type to required type or converts the value of the primitive type to the value of the specified primitive type.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <returns> Value of the specified type. </returns>
        /// <typeparam name="T"> Specified type. </typeparam>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when value is null.
        /// </exception>
        /// <exception cref="TypeConversionNotSupportedException">
        /// The exception that is thrown when conversion between types isn't supported.
        /// </exception>
        public static T ConvertTo<T>(this object value)
        {
        	Validation.NotNull("Value", value);
        	
            Type typeT = typeof(T);
            Type typeV = value.GetType();

            if (typeT == typeV)
            {
            	return (T)value;
            }

            Func<object, string> converterToString = GetConverterToString(typeV);
            Func<string, object> converterToObject = GetConverterToObject(typeT);

            if (converterToString == null || converterToObject == null)
            {
                try
                {
                    return (T)value;
                }
                catch
                {
                    throw new TypeConversionNotSupportedException(value, typeV, typeT);
                }
            }
            else
            {
            	return (T)(converterToObject(converterToString(value)));
            }
        }

        /// <summary>
        /// Determines whether type is base type for derived type or it types are equals.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <param name="derivedType"> The derived type. </param>
        /// <returns> True if type is base type for derived type or it types are equals, otherwise false. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when type or derived type are null.
        /// </exception>
        public static bool IsBaseTypeOrEquals(this Type type, Type derivedType)
        {
        	Validation.NotNull("Type", type);
        	Validation.NotNull("DerivedType", derivedType);
        	
            if (type.IsClass)
            {
                return IsBaseClassOrEqualsInner(type, derivedType);
            }
            else
            {
            	return IsBaseInterfaceOrEqualsInner(type, derivedType);
            }
        }

        /// <summary>
        /// Determines whether type is base interface for derived type or it types are equals.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <param name="derivedType"> The derived type. </param>
        /// <returns> True if type is base interface for derived type or it types are equals, otherwise false. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when type or derived type are null.
        /// </exception>
        public static bool IsBaseInterfaceOrEquals(this Type type, Type derivedType)
        {
        	Validation.NotNull("Type", type);
        	Validation.NotNull("DerivedType", derivedType);
        	
            return IsBaseInterfaceOrEqualsInner(type, derivedType);
        }

        /// <summary>
        /// Determines whether type is base class for derived type or it types are equals.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <param name="derivedType"> The derived type. </param>
        /// <returns> True if type is base class for derived type or it types are equals, otherwise false. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when type or derived type are null.
        /// </exception>
        public static bool IsBaseClassOrEquals(this Type type, Type derivedType)
        {
        	Validation.NotNull("Type", type);
        	Validation.NotNull("DerivedType", derivedType);
        	
            return IsBaseClassOrEqualsInner(type, derivedType);
        }
        
        /// <summary>
        /// Retrieves a custom <see cref="Attribute"></see> applied to an <see cref="Assembly"></see>. 
        /// </summary>
        /// <param name="assembly"> The <see cref="Assembly"></see>. </param>
        /// <returns> <see cref="Attribute"></see> if attribute has been found, otherwise null. </returns>
        /// <typeparam name="T"> Type of the custom <see cref="Attribute"></see>. </typeparam>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when assembly is null.
        /// </exception>
        public static T GetAttribute<T>(this Assembly assembly) where T : Attribute
        {
        	Validation.NotNull("Assembly", assembly);
        	
            object[] attributeTypes = assembly.GetCustomAttributes(true);
            int length = attributeTypes.Length;
            T attribute;
            
            for (int i = 0; i < length; i++)
            {
                attribute = attributeTypes[i] as T;
                if (attribute != null)
                {
                	return attribute;
                }
            }
            
            return null;
        }

        /// <summary>
        /// Retrieves a custom <see cref="Attribute"></see> applied to a member of the type.
        /// </summary>
        /// <param name="memberInfo"> The <see cref="MemberInfo"></see>. </param>
        /// <param name="inherit"> If true, specifies to also search the ancestors of the member info for custom attribute. </param>
        /// <returns> <see cref="Attribute"></see> if attribute has been found, otherwise null. </returns>
        /// <typeparam name="T"> Type of the custom <see cref="Attribute"></see>. </typeparam>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when member info is null.
        /// </exception>
        public static T GetAttribute<T>(this MemberInfo memberInfo, bool inherit) where T : Attribute
        {
        	Validation.NotNull("MemberInfo", memberInfo);
        	
            object[] attributeTypes = memberInfo.GetCustomAttributes(inherit);
            int length = attributeTypes.Length;
            T attribute;
            
            for (int i = 0; i < length; i++)
            {
                attribute = attributeTypes[i] as T;
                if (attribute != null)
                {
                	return attribute;
                }
            }
            
            return null;
        }

        /// <summary>
        /// Gets a friendly name of the type.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <returns> Friendly name of the type. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when type is null.
        /// </exception>
        public static string GetFriendlyName(this Type type)
        {
        	Validation.NotNull("Type", type);
        	
            StringBuilder builder = new StringBuilder();
            BuildFriendlyName(type, builder);
            return builder.ToString();
        }
        
        /// <summary>
        /// Gets a converter that converts a value of the specified type to string.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <returns> Converter if value of the specified type can be converted to string, otherwise null. </returns>
        internal static Func<object, string> GetConverterToString(Type type)
        {
            if (type.IsEnum)
            {
            	return o => o.ToString().ToLowerInvariant();
            }

            Func<object, string> converter;
            if (convertersToString.TryGetValue(type, out converter))
            {
            	return converter;
            }

            return null;
        }

        /// <summary>
        /// Gets a converter that converts string to the value of the specified type.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <returns> Converter if string can be converted to the value of the specified type, otherwise null. </returns>
        internal static Func<string, object> GetConverterToObject(Type type)
        {
            if (type.IsEnum)
            {
            	return s => Enum.Parse(type, s, true);
            }

            Func<string, object> converter;
            if (convertersToObject.TryGetValue(type, out converter))
            {
            	return converter;
            }

            return null;
        }

        /// <summary>
        /// Builds a friendly name of the type with the <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <param name="builder"> The builder. </param>
        private static void BuildFriendlyName(this Type type, StringBuilder builder)
        {
            string friendlyName = type.Name;
            
            if (type.IsGenericType)
            {
                int iBacktick = friendlyName.IndexOf('`');
                
                if (iBacktick > 0)
                {
                	builder.Append(friendlyName.Remove(iBacktick));
                }
                
                builder.Append("<");
                
                Type[] typeParameters = type.GetGenericArguments();
                int length = typeParameters.Length;
                Type typeParameter = null;
                
                for (int i = 0; i < length; ++i)
                {
                	typeParameter = typeParameters[i];
                	
                    if (typeParameter.IsGenericType)
                    {
                        BuildFriendlyName(typeParameter, builder);
                    }
                    else
                    {
                        if (i != 0)
                        {
                        	builder.Append(",");
                        }
                        
                        builder.Append(typeParameter.Name);
                    }
                    
                }
                builder.Append(">");
            }
            else
            {
                builder.Append(friendlyName);
            }
        }

        /// <summary>
        /// Throws an exception if conversion from string isn't available.
        /// </summary>
        /// <param name="value"> Value. </param>
        /// <returns> Not returns anything. </returns>
        /// <typeparam name="T"> Target type of the value. </typeparam>
        /// <exception cref="StringConversionNotSupportedException">
        /// The exception that is thrown when string conversion to the value of the specified type isn't supported.
        /// </exception>
        private static T ConversionFromStringRaiseError<T>(string value)
        {
            throw new StringConversionNotSupportedException(value, typeof(T));
        }
        
        /// <summary>
        /// Determines whether type is base interface for derived type or it types are equals.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <param name="derivedType"> The derived type. </param>
        /// <returns> True if type is base interface for derived type or it types are equals, otherwise false. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when type or derived type are null.
        /// </exception>
        private static bool IsBaseInterfaceOrEqualsInner(this Type type, Type derivedType)
        {
            string typeName = type.ToString();
            
            if (type.Assembly == derivedType.Assembly && type.Namespace == derivedType.Namespace && type.ToString() == derivedType.ToString())
            {
            	return true;
            }

            Type[] interfaceTypes = derivedType.GetInterfaces();
            Type interfaceType = null;
            int length = interfaceTypes.Length;
            
            for (int i = 0; i < length; i++)
            {
                interfaceType = interfaceTypes[i];
                if (type.Assembly == interfaceType.Assembly && type.Namespace == interfaceType.Namespace && typeName == interfaceType.ToString())
                {
                	return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether type is base class for derived type or it types are equals.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <param name="derivedType"> The derived type. </param>
        /// <returns> True if type is base class for derived type or it types are equals, otherwise false. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when type or derived type are null.
        /// </exception>
        private static bool IsBaseClassOrEqualsInner(this Type type, Type derivedType)
        {
            if (type.Assembly == derivedType.Assembly && type.Namespace == derivedType.Namespace && type.ToString() == derivedType.ToString())
            {
            	return true;
            }
            
            Type baseTypeOfDerived = derivedType.BaseType;
            if (baseTypeOfDerived == null)
            {
            	return false;
            }
            
            return type.IsBaseClassOrEqualsInner(baseTypeOfDerived);
        }
    }
}