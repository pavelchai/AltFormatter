/*
 * Modified and refactored version of the file from SharpZipLib [based on https://github.com/icsharpcode/SharpZipLib, MIT License]
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
namespace AltFormatter.ZLib
{
    /// <summary>
    /// This class stores the pending output of the Deflater.
    ///
    /// author of the original java version : Jochen Hoenicke
    /// </summary>
    internal sealed class DeflaterPending : PendingBuffer
    {
        /// <summary>
        /// Construct instance with default buffer size
        /// </summary>
        public DeflaterPending() : base(DeflaterConstants.PENDING_BUF_SIZE) { }
    }
}