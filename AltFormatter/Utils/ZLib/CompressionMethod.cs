/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */

namespace AltFormatter.Utils
{
    /// <summary>
    /// Represents the compression method.
    /// </summary>
    internal enum CompressionMethod : ushort
    {
        /// <summary>
        /// Store (without compression).
        /// </summary>
        Store = 0,

        /// <summary>
        /// Deflate compression method.
        /// </summary>
        Deflate = 8
    }
}