/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System.Reflection;

namespace AltFormatter.Formatter
{
    /// <summary>
    /// Represents an AltFormatter [Not extendable, ZIP64 storer, XML info, text data].
    /// </summary>
    /// <remarks> Supports all functionality described in the <see cref="IFormatter"></see>. </remarks>
    public sealed class AltFormatterZipXmlText : AbstractFormatter
    {
        /// <summary>
        /// Indicates whether compression is on.
        /// </summary>
        private readonly bool enableCompression;

        /// <summary>
        /// Creates a new AltFormatter [Not extendable, ZIP64 storer, XML info, text data].
        /// </summary>
        /// <param name="enableCompression"> Indicates whether compression is on. </param>
        /// <param name="assemblies"> Array of the used assemblies.</param>
        public AltFormatterZipXmlText(bool enableCompression, params Assembly[] assemblies) : base(assemblies)
        {
            this.enableCompression = enableCompression;
        }

        /// <inheritdoc/>
        protected override IWriteOnlyDataStorer CreateWriteOnlyDataStorer()
        {
            return new ZipWriteOnlyStorer(this.enableCompression, true);
        }

        /// <inheritdoc/>
        protected override IReadOnlyDataStorer CreateReadOnlyDataStorer(byte[] data)
        {
            return ZipReadOnlyStorer.FromData(data);
        }
    }
}