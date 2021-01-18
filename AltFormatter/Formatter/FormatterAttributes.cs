/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System;

namespace AltFormatter.Formatter
{
    /// <summary>
    /// Attribute for the assemblies and types that can be 
    /// serialized/deserialized with <see cref="IFormatter"></see>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed class FormattableAttribute : Attribute
    {
        /// <summary>
        /// Name of the type/assembly
        /// (may not equals real name of the type/assembly).
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Creates a new attribute for the assemblies and types 
        /// that can be serialized/deserialized with <see cref="IFormatter"></see>.
        /// </summary>
        /// <param name="name"> 
        /// Name of the type/assembly
        /// (may not equals real name of the type/assembly). 
        /// </param>
        public FormattableAttribute(string name)
        {
            this.Name = name;
        }
    }

    /// <summary>
    /// Attribute for fields/properties that should be 
    /// serialized/deserialized with <see cref="IFormatter"></see>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class KeyAttribute : Attribute
    {
        /// <summary>
        /// Name of the field/property that should be serialized/deserialized
        /// (may not equals real name of the field/property).
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Indicates whether value of the field or property is optional.
        /// Value will be equals default(T) if restoring the value 
        /// in deserialization is impossible.
        /// </summary>
        public readonly bool Optional;

        /// <summary>
        /// Determines order in serialization/deserialization.
        /// </summary>
        public readonly int Order = 0;

        /// <summary>
        /// Creates a new attribute for fields/properties 
        /// that should be serialized/deserialized with <see cref="IFormatter"></see>.
        /// </summary>
        /// <param name="name">
        /// Name of the field/property that should be 
        /// serialized/deserialized (may not equals real name of the field/property).
        /// </param>
        /// <param name="optional">
        /// Indicates whether value of the field or property is optional.
        /// Value will be equals default(T) if restoring in deserialization is impossible.
        /// </param>
        /// <param name="order"> Determines order in serialization/deserialization. </param>
        public KeyAttribute(string name, bool optional = false, int order = 0)
        {
            this.Name = name;
            this.Optional = optional;
            this.Order = order;
        }
    }
}