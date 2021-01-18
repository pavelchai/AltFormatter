/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Localization;
using System;

namespace AltFormatter.Formatter
{
    /// <summary>
    /// The exception that is thrown when type have the attribute.
    /// </summary>
    internal sealed class MissingAttributeException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when type have the attribute.
        /// </summary>
        /// <param name="type"> Type. </param>
        /// <param name="attribute"> Attribute. </param>
        public MissingAttributeException(Type type, Type attribute) : base("Type <{0}> isn't marked with <{1}>.", type, attribute.Name) { }
    }

    /// <summary>
    /// The exception that is thrown when data isn't found.
    /// </summary>
    internal sealed class DataNotFoundException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when data hasn't been found.
        /// </summary>
        /// <param name="name"> Name. </param>
        public DataNotFoundException(string name) : base("Data <{0}> isn't exist.", name) { }
    }

    /// <summary>
    /// The exception that is thrown when key can't be loaded.
    /// </summary>
    internal sealed class KeyCanNotLoadedException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when key can't be loaded.
        /// </summary>
        /// <param name="formattableType"> Type. </param>
        /// <param name="keyName"> Name of key. </param>
        public KeyCanNotLoadedException(FormattableType formattableType, string keyName) : base("Key <{0}> in <{1}> can't be loaded.", keyName, formattableType.Name) { }

        /// <summary>
        /// The exception that is thrown when key can't be loaded.
        /// </summary>
        /// <param name="formattableType"> Type. </param>
        /// <param name="keyName"> Name of key. </param>
        /// <param name="exception"> Exception. </param>
        public KeyCanNotLoadedException(FormattableType formattableType, string keyName, Exception exception) : base("Key <{0}> in <{1}> can't be loaded.", exception, keyName, formattableType.Name) { }
    }

    /// <summary>
    /// The exception that is thrown when key can't be saved.
    /// </summary>
    internal sealed class KeyCanNotSavedException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when key can't be saved.
        /// </summary>
        /// <param name="formattableType"> Type. </param>
        /// <param name="keyName"> Name of key. </param>
        /// <param name="exception"> Exception. </param>
        public KeyCanNotSavedException(FormattableType formattableType, string keyName, Exception exception) : base("Key <{0}> in <{1}> can't be saved.", exception, keyName, formattableType.Name) { }
    }

    /// <summary>
    /// The exception that is thrown when type is unsupported.
    /// </summary>
    internal sealed class UnsupportedTypeException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when type is unsupported.
        /// </summary>
        /// <param name="type"> Type. </param>
        public UnsupportedTypeException(Type type) : base("Type <{0}> is unsupported.", type.Name) { }

        /// <summary>
        /// The exception that is thrown when type is unsupported.
        /// </summary>
        /// <param name="type"> Type. </param>
        /// <param name="className"> Class name. </param>
        public UnsupportedTypeException(Type type, string className) : base("Type <{0}> is unsupported. Class name: <{1}>", type.Name, className) { }
    }

    /// <summary>
    /// The exception that is thrown when type is abstract and can't be used with the formatter.
    /// </summary>
    internal sealed class AbstractTypeException : LocalizedException
    {
        /// <summary> The exception that is thrown when type is abstract and can't be used with the formatter. </summary>
        /// <param name="type"> Type. </param>
        public AbstractTypeException(Type type) : base("Type <{0}> is abstract and can't be used with the formatter.", type.Name) { }
    }
}