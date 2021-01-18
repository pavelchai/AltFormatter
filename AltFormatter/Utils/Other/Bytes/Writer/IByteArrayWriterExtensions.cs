/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */

namespace AltFormatter.Utils
{
    /// <summary>
    /// Represents the extensions for the <see cref="IByteArrayWriter"></see>.
    /// </summary>
    internal static class IByteArrayWriterExtensions
    {
        /// <summary>
        /// Writes the array of the bytes with the <see cref="IByteArrayWriter"></see>.
        /// </summary>
        /// <param name="writer"> The writer. </param>
        /// <param name="data"> Array of the bytes. </param>
        public static void WriteBytes(this IByteArrayWriter writer, params byte[] data)
        {
            writer.WriteBytes(data, 0, data.Length);
        }
    }
}