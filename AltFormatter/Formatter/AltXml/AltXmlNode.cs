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
    /// Represents a XML node.
    /// </summary>
    internal sealed class AltXmlNode : AbstractAltXmlBase
    {
        /// <summary>
        /// Name of the XML node.
        /// </summary>
        public readonly string Name = "";

        /// <summary>
        /// Value of the XML node.
        /// </summary>
        public readonly string Value = "";

        /// <summary>
        /// Subnodes of the <see cref="AltXmlNode"></see>.
        /// </summary>
        public readonly LinkedList<AltXmlNode> SubNodes = new LinkedList<AltXmlNode>();

        /// <summary>
        /// Attributes of the <see cref="AltXmlNode"></see>.
        /// </summary>
        public readonly LinkedList<AltXmlAttribute> Attributes = new LinkedList<AltXmlAttribute>();

        /// <summary>
        /// Creates a new XML node.
        /// </summary>
        /// <param name="name"> Name of the node. </param>
        /// <param name="value"> Value of the node. </param>
        public AltXmlNode(string name, string value = "")
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Creates a XML node (from XML).
        /// </summary>
        /// <param name="str"> String. </param>
        /// <param name="i"> Index. </param>
        public AltXmlNode(string str, ref int i)
        {
            this.Attributes = ParseAttributes(str, ref i, '>', '/', out this.Name);

            if (str[i] == '/') // if this node has nothing inside
            {
                i++; // skip /
                i++; // skip >
                return;
            }

            i++; // skip >

            // temporary to include all whitespaces into value, if any
            int tempI = i;

            SkipSpaces(str, ref tempI);

            int length = str.Length;
            if (str[tempI] == '<')
            {
                i = tempI;

                while (str[i + 1] != '/') // parse subnodes
                {
                    i++; // skip <
                    this.SubNodes.AddLast(new AltXmlNode(str, ref i));

                    SkipSpaces(str, ref i);

                    if (i >= length)
                    {
                        // EOF
                        return;
                    }

                    if (str[i] != '<')
                    {
                        throw new UnexpectedTokenInsteadLessThanException(str[i]);
                    }
                }

                i++; // skip <
            }
            else // parse value
            {
                this.Value = GetValue(str, ref i, '<', '\0', false);
                i++; // skip <

                if (str[i] != '/')
                {
                    throw new InvalidEndingOnTagException(this.Name, '/');
                }
            }

            i++; // skip /
            SkipSpaces(str, ref i);

            string endName = GetValue(str, ref i, '>', '\0', true);
            if (endName != this.Name)
            {
                throw new MismatchStartEndTagNameException(this.Name, endName);
            }

            SkipSpaces(str, ref i);

            if (str[i] != '>')
            {
                throw new InvalidEndingOnTagException(this.Name, '>');
            }

            i++; // skip >
        }

        /// <summary>
        /// Returns a child <see cref="AltXmlNode"/> by given name.
        /// </summary>
        /// <param name="nodeName"> Name of subnode. </param>
        /// <returns> First a child <see cref="AltXmlNode"/> with given name or null if no such element. </returns>
        public AltXmlNode this[string nodeName]
        {
            get
            {
                LinkedListNode<AltXmlNode> node = this.SubNodes.Find(s => s.Name == nodeName);
                return (node != null) ? node.Value : null;
            }
        }

        /// <summary>
        /// Returns an attribute by given name.
        /// </summary>
        /// <param name="attributeName"> Name of the <see cref="AltXmlAttribute"/>. </param>
        /// <returns> <see cref="AltXmlAttribute"/> with given name or null if no such attribute. </returns>
        public AltXmlAttribute GetAttribute(string attributeName)
        {
            LinkedListNode<AltXmlAttribute> node = this.Attributes.Find(s => s.Name == attributeName);
            return (node != null) ? node.Value : null;
        }

        /// <summary>
        /// Writes the data to the xml string.
        /// </summary>
        /// <param name="sb"> String builder. </param>
        /// <param name="offset"> Offset. </param>
        public void WriteData(StringBuilder sb, int offset)
        {
            string offsetString = new string('\t', offset);
            bool hasSubnodes = this.SubNodes.Count != 0;

            sb.Append(offsetString);
            sb.Append('<');
            sb.Append(this.Name);

            if (this.Attributes.Count != 0)
            {
                sb.Append(' ');

                AltXmlAttribute attribute = null;
                for (var node = this.Attributes.First; node != null; node = node.Next)
                {
                    attribute = node.Value;

                    sb.Append(attribute.Name);
                    sb.Append("=\"");
                    WriteValue(sb, attribute.Value);
                    sb.Append('\"');

                    if (node.Next != null)
                    {
                        sb.Append(" ");
                    }
                }
            }

            if (!hasSubnodes)
            {
                if (!string.IsNullOrEmpty(this.Value))
                {
                    sb.Append('>');
                    WriteValue(sb, this.Value);
                    sb.Append("</");
                    sb.Append(this.Name);
                }
                else
                {
                    sb.Append(" /");
                }
            }

            sb.Append(">\r\n");

            if (hasSubnodes)
            {
                int nextOffset = offset + 1;

                for (var subNode = this.SubNodes.First; subNode != null; subNode = subNode.Next)
                {
                    subNode.Value.WriteData(sb, nextOffset);
                }

                sb.Append(offsetString);
                sb.Append("</");
                sb.Append(this.Name);
                sb.Append(">\r\n");
            }
        }

        /// <summary>
        /// Returns a string that represents the <see cref="AltXmlNode"></see>.
        /// </summary>
        /// <returns> A string that represents the <see cref="AltXmlNode"></see>. </returns>
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