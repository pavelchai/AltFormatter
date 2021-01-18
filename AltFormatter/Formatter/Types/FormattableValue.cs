/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Utils;
using System;

namespace AltFormatter.Formatter
{
    /// <summary>
    /// Represents a formattable value.
    /// </summary>
    internal sealed class FormattableValue : IComparable<FormattableValue>
    {
        /// <summary>
        /// Callback for the get accessor.
        /// </summary>
        private readonly Func<object, object> getValueCallback;

        /// <summary>
        ///  Callback for the set accessor.
        /// </summary>
        private readonly Action<object, object> setValueCallback;

        /// <summary>
        /// Name of value.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Indicates whether value is optional.
        /// </summary>
        public readonly bool Optional;

        /// <summary>
        /// Determines order in serialization/deserialization.
        /// </summary>
        public readonly int Order;

        /// <summary>
        /// Type of the value.
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// Creates a new formattable value.
        /// </summary>
        /// <param name="name"> Name of value. </param>
        /// <param name="optional"> Indicates whether value is optional. </param>
        /// <param name="order"> Determines order in serialization/deserialization. </param>
        /// <param name="type"> Type of the value. </param>
        /// <param name="getValue"> Callback for the get accessor. </param>
        /// <param name="setValue"> Callback for the set accessor. </param>
        public FormattableValue(string name, bool optional, int order, Type type, IFunctionCallback<object, object> getValue, IActionCallback<object, object> setValue)
        {
            this.Name = name;
            this.Optional = optional;
            this.Order = order;
            this.Type = type;
            this.getValueCallback = getValue.Invoke;
            this.setValueCallback = setValue.Invoke;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="obj"> The object. </param>
        /// <returns> Current value. </returns>
        public object GetValue(object obj)
        {
            return this.getValueCallback(obj);
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="obj"> The object. </param>
        /// <param name="value"> New value. </param>
        public void SetValue(object obj, object value)
        {
            this.setValueCallback(obj, value);
        }

        /// <inheritdoc/>
        public int CompareTo(FormattableValue other)
        {
            if (this.Order == other.Order)
            {
                return 0;
            }

            return (this.Order < other.Order) ? -1 : 1;
        }
    }
}