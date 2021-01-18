﻿/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System.Collections.Generic;

namespace AltFormatter.Formatter
{
    /// <summary>
    /// Represents a read-only data storer.
    /// </summary>
    public interface IReadOnlyDataStorer
    {
        /// <summary>
    	/// Gets the entries in the storer.
    	/// </summary>
    	/// <returns> Entries in the storer. </returns>
    	IReadOnlyList<IDataStorerEntry> Entries { get; }

        /// <summary>
        /// Reads the data of the specified entry.
        /// </summary>
        /// <param name="entry"> The entry. </param>
        /// <returns> Array of bytes containing the data of the entry. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when entry is null.
        /// </exception>
        /// <exception cref="InvalidEntryException">
        /// The exception that is thrown when entry is invalid.
        /// </exception>
        byte[] Read(IDataStorerEntry entry);
    }
}