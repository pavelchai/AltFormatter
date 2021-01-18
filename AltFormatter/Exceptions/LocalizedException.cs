/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System;

namespace AltFormatter.Localization
{
    /// <summary>
    /// Represents the errors that occur during application execution.
    /// </summary>
    public class LocalizedException : Exception
    {
        /// <summary>
        /// Represents errors that occur during application execution.
        /// </summary>
        /// <param name="format"> A composite format string. </param>
        /// <param name="args"> An object array that contains zero or more objects to format. </param>
        public LocalizedException(string format, params object[] args) : base(string.Format(format, args))
        {
        }
    }
}