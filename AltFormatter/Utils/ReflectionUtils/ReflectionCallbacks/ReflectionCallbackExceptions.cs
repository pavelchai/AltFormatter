/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Localization;

namespace AltFormatter.Utils
{
    ///<summary>
    /// The exception that is thrown when reflection callback isn't allowable.
    /// </summary>
    internal sealed class ReflectionCallbackIsNotAllowableException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when reflection callback isn't allowable.
        /// </summary>
        /// <param name="name"> Name of the callback. </param>
        public ReflectionCallbackIsNotAllowableException(string name) : base("Reflection callback for the <{0}> isn't allowable.", name) { }
    }
}