/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */

namespace AltFormatter.Formatter
{
    /// <summary>
    /// Represents an entry of the data in the file storer.
    /// </summary>
    public interface IDataStorerEntry
    {
        /// <summary>
        /// Gets the path to the data in the file storer.
        /// </summary>
        /// <returns> Path to the data in the file storer. </returns>
        string Path { get; }
    }
}