/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */

namespace AltFormatter.Localization
{
    /// <summary>
    /// The exception that is thrown when value is null.
    /// </summary>
    public sealed class ValueNullException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when value is null.
        /// </summary>
        /// <param name="name"> Name of the value. </param>
        public ValueNullException(string name) : base("<{0}> is null.", name) { }
    }

    /// <summary>
    /// The exception that is thrown when value is less than the specified value.
    /// </summary>
    public sealed class ValueLessThanException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when value is less than the specified value.
        /// </summary>
        /// <param name="name"> Name of the value. </param>
        /// <param name="value"> The value. </param>
        /// <param name="comparandValue"> Value of the comparand. </param>
        public ValueLessThanException(string name, object value, object comparandValue) : base("<{0}> = <{1}> is less than <{2}>.", name, value, comparandValue) { }
    }

    /// <summary>
    /// The exception that is thrown when value is less than or equals the specified value.
    /// </summary>
    public sealed class ValueLessThanOrEqualsException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when value is less than or equals the specified value.
        /// </summary>
        /// <param name="name"> Name of the value. </param>
        /// <param name="value"> The value. </param>
        /// <param name="comparandValue"> Value of the comparand. </param>
        public ValueLessThanOrEqualsException(string name, object value, object comparandValue) : base("<{0}> = <{1}> is less than or equals <{2}>.", name, value, comparandValue) { }
    }
    
    /// <summary>
    /// The exception that is thrown when the value is outside the allowable range of the values.
    /// </summary>
    internal sealed class ValueOutOfRangeException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when the value is outside the allowable range of the values.
        /// </summary>
        /// <param name="name"> Name of the value. </param>
        /// <param name="value"> The value. </param>
        /// <param name="minValue"> The min value. </param>
        /// <param name="maxValue"> The max value. </param>
        public ValueOutOfRangeException(string name, object value, object minValue, object maxValue) : base("<{0}> = <{1}> is outside the allowable range [<{2}>,<{3}>]>.", name, value, minValue, maxValue) { }
    }

    /// <summary>
    /// The exception that is thrown when values have duplicates.
    /// </summary>
    internal sealed class ValuesHaveDuplicatesException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when values have duplicates.
        /// </summary>
        /// <param name="name"> Name of the group of the values. </param>
        /// <param name="index"> Index of the duplicate. </param>
        public ValuesHaveDuplicatesException(string name, int index) : base("<{0}> have duplicates. Index of the duplicate item = <{1}>.", name, index) { }
    }

    /// <summary>
    /// The exception that is thrown when values are not ordered in ascending order.
    /// </summary>
    internal sealed class ValuesNotOrderedException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when values are not ordered in ascending order.
        /// </summary>
        /// <param name="name"> Name of the group of the values. </param>
        /// <param name="index"> Index of the item. </param>
        public ValuesNotOrderedException(string name, int index) : base("<{0}> is not ordered in ascending order. Index of the item = <{1}>.", name, index) { }
    }

    /// <summary>
    /// The exception that is thrown when sizes of the first and second values are different.
    /// </summary>
    internal sealed class ValuesHaveDifferentSizesException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when sizes of the first and second values are different.
        /// </summary>
        /// <param name="name1"> Name of first value. </param>
        /// <param name="name2"> Name of second value. </param>
        /// <param name="length1"> Length of first value. </param>
        /// <param name="length2"> Length of second value. </param>
        public ValuesHaveDifferentSizesException(string name1, string name2, int length1, int length2) : base("Sizes of the arguments <{0}> and <{1}> are different. Length of the <{0}> = <{2}>, length of the <{1}> = <{3}>.", name1, name2, length1, length2) { }
    }
    
    /// <summary>
    /// The exception that is thrown when length of the value is not a power of two.
    /// </summary>
    internal sealed class ValueLengthIsNotPowerOfTwoException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when length of the value is not a power of two.
        /// </summary>
        /// <param name="name"> Name of the value. </param>
        /// <param name="length"> Length of the value. </param>
        public ValueLengthIsNotPowerOfTwoException(string name, int length) : base("Length <{1}> of the <{0}> isn't a power of 2.", name, length) { }
    }
}