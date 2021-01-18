/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Utils;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AltFormatter.Formatter
{
	/// <summary>
    /// Represents an AltFormatter [Not extendable, binary storer, text info, text data].
    /// </summary>
    /// <remarks> Supports all functionality described in the <see cref="IFormatter"></see>. </remarks>
    public sealed class AltFormatterBinaryText : AbstractFormatter
	{
		/// <summary>
        /// Indicates whether compression is on.
        /// </summary>
        private readonly bool enableCompression;
        
        /// <summary>
        /// Identifier of the null class.
        /// </summary>
        private const string NullIdentifier = "null_value";
        
        /// <summary>
        /// Creates a new AltFormatter [Not extendable, binary storer, text info, text data].
        /// </summary>
        /// <param name="enableCompression"> Indicates whether compression is on. </param>
        /// <param name="assemblies"> Array of the used assemblies.</param>
        public AltFormatterBinaryText(bool enableCompression, params Assembly[] assemblies) : base(assemblies)
        {
        	this.enableCompression = enableCompression;
        }

		/// <inheritdoc/>
		protected override IWriteOnlyDataStorer CreateWriteOnlyDataStorer()
		{
			return new BinaryWriteOnlyStorer(this.enableCompression);
		}

		/// <inheritdoc/>
		protected override IReadOnlyDataStorer CreateReadOnlyDataStorer(byte[] data)
		{
			return BinaryReadOnlyStorer.FromData(data);
		}

		/// <inheritdoc/>
		protected override byte[] SerializeInfo(string className, IDictionary<string, string> values)
		{
			StringBuilder info = new StringBuilder();
			
			info.AppendLine(className);
			
			int count = values.Count;
			int index = 0;
			foreach(var pair in values)
            {
				info.AppendLine(pair.Key);
				
				if(index++ != count)
				{
					info.AppendLine(pair.Value ?? NullIdentifier);
				}
				else
				{
					info.Append(pair.Value ?? NullIdentifier);
				}
            }
			
			return info.ToString().GetBytes();
		}
		
		/// <inheritdoc/>
		protected override IDictionary<string, string> DeserializeInfo(byte[] data, out string className)
		{
			IEnumerator<string> lines = StringUtils.SplitLines(data.GetString()).GetEnumerator();
			
			lines.MoveNext();
			className = lines.Current;
			
			IDictionary<string, string> values = new Dictionary<string, string>(5);
			string key = "";
			
			while(lines.MoveNext())
			{
				key = lines.Current;
				
				lines.MoveNext();
				
				values.Add(key, lines.Current);
			}
			
			return values;
		}
	}
}