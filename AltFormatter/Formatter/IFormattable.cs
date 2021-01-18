/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */

namespace AltFormatter.Formatter
{
    /// <summary>
    /// Extends functionality for serialize/deserialize the 
    /// objects that are marked with <see cref="FormattableAttribute"></see>.
    /// </summary>
    public interface IFormattable
    {
        /// <summary>
        /// Raised before serialization.
        /// </summary>
        void OnSerializing();

        /// <summary>
        /// Raised after serialization.
        /// </summary>
        void OnSerialized();

        /// <summary>
        /// Raised before deserialization.
        /// </summary>
        void OnDeserializing();

        /// <summary>
        /// Raised after deserialization.
        /// </summary>
        void OnDeserialized();

        /// <summary>
        /// Raised after <see cref="OnDeserialized"></see> method and 
        /// allows to change deserialized object by the another (substitutable) object.
        /// </summary>
        /// <returns>
        /// Deserialized object (this) if substitution isn't necessary, 
        /// otherwise substitutable (other) object.
        /// </returns>
        object Substitution();
    }
}