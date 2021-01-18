/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */

namespace AltFormatter.Formatter
{
	/// <summary>
	/// Represents an entry for the <see cref="BinaryWriteOnlyStorer"></see>.
	/// </summary>
	internal sealed class BinaryWriteOnlyStorerEntry : AbstractDataStoreEntry
	{
		/// <summary>
        /// Creates a new entry for the <see cref="BinaryWriteOnlyStorer"></see>.
        /// </summary>
        /// <param name="path"> Path to the file. </param>
        public BinaryWriteOnlyStorerEntry(string path) : base(path)
		{
		}
	}
}