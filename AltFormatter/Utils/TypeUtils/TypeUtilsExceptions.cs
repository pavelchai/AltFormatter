/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Localization;
using System;

namespace AltFormatter.Utils
{
    /// <summary>
    /// The exception that is thrown when conversion between types isn't supported.
    /// </summary>
    internal sealed class TypeConversionNotSupportedException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when conversion between types isn't supported.
        /// </summary>
        /// <param name="value"> Value. </param>
        /// <param name="typeV"> Type of the value. </param>
        /// <param name="typeT"> Target type. </param>
        public TypeConversionNotSupportedException(object value, Type typeV, Type typeT) : base("Conversion <{0}> from type <{1}> to type <{2}> isn't supported.", !object.ReferenceEquals(value, null) ? value :"null", typeV.Name, typeT.Name) { }
    }

    /// <summary>
    /// The exception that is thrown when string conversion to the value of the specified type isn't supported.
    /// </summary>
    internal sealed class StringConversionNotSupportedException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when conversion isn't supported.
        /// </summary>
        /// <param name="value"> Value. </param>
        /// <param name="type"> Target type. </param>
        public StringConversionNotSupportedException(string value, Type type) : base("String value <{0}> can't be converted to <{1}>.", !object.ReferenceEquals(value, null) ? value: "null", type.Name) { }
    }
}