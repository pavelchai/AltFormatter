/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Localization;

namespace AltFormatter.Formatter
{
    /// <summary>
    /// The exception that is thrown when data is invalid.
    /// </summary>
    internal sealed class InvalidDataException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when data is invalid.
        /// </summary>
        public InvalidDataException() : base("Data is invalid.")
        {
        }
    }
    
    /// <summary>
    /// The exception that is thrown when entry is invalid.
    /// </summary>
    internal sealed class InvalidEntryException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when entry is invalid.
        /// </summary>
    	/// <param name="name"> Name of the entry. </param>
        public InvalidEntryException(string name) : base("Entry <{0}> is invalid.", name)
        {
        }
    }
}