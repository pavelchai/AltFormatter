/*
 * Modified and refactored version of the file from NanoXML [based on https://www.codeproject.com/Tips/682245/NanoXML-Simple-and-fast-XML-parser, BrokenEvent, CPOL License]
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Utils;
using System.Collections.Generic;
using System.Text;

namespace AltFormatter.Formatter
{
    /// <summary>
    /// Represents a XML document.
    /// </summary>
    internal sealed class AltXmlDocument : AbstractAltXmlBase
    {
        /// <summary>
        /// Root node.
        /// </summary>
        public readonly AltXmlNode RootNode;

        /// <summary>
        /// XML declarations.
        /// </summary>
        public readonly LinkedList<AltXmlAttribute> Declarations = new LinkedList<AltXmlAttribute>();

        /// <summary>
        /// Creates a new XML document.
        /// </summary>
        private AltXmlDocument() { }

        /// <summary>
        /// Creates a new XML document.
        /// </summary>
        /// <param name="name"> Name of the root node. </param>
        /// <param name="value"> Value of the root node. </param>
        public AltXmlDocument(string name, string value)
        {
            this.RootNode = new AltXmlNode(name, value);
        }

        /// <summary>
        /// Creates a new XML document (from XML string).
        /// </summary>
        /// <param name="xmlString"> XML string. </param>
        public AltXmlDocument(string xmlString)
        {
            int i = 0;
            string nodeName;
            char xmlChar;

            while (true)
            {
                SkipSpaces(xmlString, ref i);

                if (xmlString[i] != '<')
                {
                    throw new UnexpectedTokenInsteadLessThanException(xmlString[i]);
                }

                xmlChar = xmlString[++i]; // skip <

                if (xmlChar == '?') // declaration
                {
                    i++; // skip ?
                    this.Declarations = ParseAttributes(xmlString, ref i, '?', '>', out nodeName);
                    i++; // skip ending ?
                    i++; // skip ending >
                    continue;
                }

                if (xmlChar == '!') // doctype
                {
                    while (xmlString[i] != '>') // skip doctype
                        i++;

                    i++; // skip >
                    continue;
                }

                this.RootNode = new AltXmlNode(xmlString, ref i);
                break;
            }
        }

        /// <summary>
        /// Gets a xml data (UTF-8 Encoding).
        /// </summary>
        /// <returns> Xml data. </returns>
        public byte[] GetXmlData()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");

            if (this.RootNode != null)
            {
                this.RootNode.WriteData(sb, 0);
            }

            return sb.ToString().GetBytes();
        }

        /// <summary>
        /// Returns a string that represents the <see cref="AltXmlDocument"></see>.
        /// </summary>
        /// <returns> A string that represents the <see cref="AltXmlDocument"></see>. </returns>
        public override string ToString()
        {
            return StringUtils.Combine(
                "Name of the root node: ",
                (this.RootNode == null || string.IsNullOrEmpty(this.RootNode.Name)) ? "<Empty>" : this.RootNode.Name,
                "; Declarations: ",
                this.Declarations.Count.ToString()
            );
        }
    }
}