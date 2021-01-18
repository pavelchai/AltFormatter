/*
 * Modified and refactored version of the file from NanoXML [based on https://www.codeproject.com/Tips/682245/NanoXML-Simple-and-fast-XML-parser, BrokenEvent, CPOL License]
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Utils;

namespace AltFormatter.Formatter
{
    /// <summary>
    /// Represents a XML attribute.
    /// </summary>
    internal sealed class AltXmlAttribute
    {
        /// <summary>
        /// Creates a new XML attribute.
        /// </summary>
        /// <param name="name"> Name of the XML attribute. </param>
        /// <param name="value"> Value of the XML attribute. </param>
        public AltXmlAttribute(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Name of the XML attribute.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Value of the XML attribute.
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// Returns a string that represents the <see cref="AltXmlAttribute"></see>.
        /// </summary>
        /// <returns> A string that represents the <see cref="AltXmlAttribute"></see>. </returns>
        public override string ToString()
        {
            return StringUtils.Combine(
                "Name: ",
                string.IsNullOrEmpty(this.Name) ? "<Empty>" : this.Name,
                "; Value: ",
                string.IsNullOrEmpty(this.Value) ? "<Empty>" : this.Value
            );
        }
    }
}