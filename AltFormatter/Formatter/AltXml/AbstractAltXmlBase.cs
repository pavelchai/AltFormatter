/*
 * Modified and refactored version of the file from NanoXML [based on https://www.codeproject.com/Tips/682245/NanoXML-Simple-and-fast-XML-parser, BrokenEvent, CPOL License]
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System.Collections.Generic;
using System.Text;

namespace AltFormatter.Formatter
{
    /// <summary>
    /// Represents a base class for the all XML classes.
    /// </summary>
    internal abstract class AbstractAltXmlBase
    {
        /// <summary>
    	/// Replace table.
    	/// </summary>
    	protected static IDictionary<char, string> replaceTable = new Dictionary<char, string>()
        {
            {'&', "&amp;"},
            {'\'', "&apos;"},
            {'"', "&quot;"},
            {'<', "&lt;"},
            {'>', "&gt;"},
            {'\t', "&#x9;"},
            {'\n', "&#xA;"},
            {'\r', "&#xD;"}
        };

        /// <summary>
        /// Determines whether character is space or not.
        /// </summary>
        /// <param name="c"> Character. </param>
        /// <returns> True if character is space, otherwise false. </returns>
        protected static bool IsSpace(char c)
        {
            return c == ' ' || c == '\t' || c == '\n' || c == '\r';
        }

        /// <summary>
        /// Determines whether character is quote or not.
        /// </summary>
        /// <param name="c"> Character. </param>
        /// <returns> True if character is quote, otherwise false. </returns>
        protected static bool IsQuote(char c)
        {
            return c == '"' || c == '\'';
        }

        /// <summary>
        /// Skips spaces in the string.
        /// </summary>
        /// <param name="str"> String. </param>
        /// <param name="i"> Index. </param>
        protected static void SkipSpaces(string str, ref int i)
        {
            int length = str.Length;
            char char0;
            while (i < length)
            {
                char0 = str[i];
                if (!IsSpace(char0))
                {
                    if (char0 == '<' && i + 4 < length && str[i + 1] == '!' && str[i + 2] == '-' && str[i + 3] == '-')
                    {
                        i += 4; // skip <!--

                        while (i + 2 < length && !(str[i] == '-' && str[i + 1] == '-'))
                            i++;

                        i += 2; // skip --
                    }
                    else break;
                }
                i++;
            }
        }

        /// <summary>
        /// Gets a value.
        /// </summary>
        /// <param name="str"> String. </param>
        /// <param name="i"> Index. </param>
        /// <param name="endChar"> End character. </param>
        /// <param name="endChar2"> End character 2. </param>
        /// <param name="stopOnSpace"> Indicates whether getting value should be stopped on space. </param>
        /// <returns> Value. </returns>
        protected static string GetValue(string str, ref int i, char endChar, char endChar2, bool stopOnSpace)
        {
            int start = i;
            char char0 = str[start];

            while ((!stopOnSpace || !IsSpace(char0)) && char0 != endChar && char0 != endChar2)
            {
                char0 = str[++i];
            }

            StringBuilder sb = new StringBuilder(i - start);
            char char1, char2, char3, char4, char5;

            for (int k = start; k < i;)
            {
                if (str[k] != '&')
                {
                    sb.Append(str[k]);
                    k++;
                }
                else
                {
                    char1 = str[k + 1];
                    char2 = str[k + 2];
                    char3 = str[k + 3];

                    // &lt; = <
                    if (char1 == 'l' && char2 == 't' && char3 == ';')
                    {
                        sb.Append('<');
                        k = k + 4;
                        continue;
                    }

                    // &gt; = >
                    if (char1 == 'g' && char2 == 't' && char3 == ';')
                    {
                        sb.Append('>');
                        k = k + 4;
                        continue;
                    }

                    char4 = str[k + 4];

                    // &#x9; = \t
                    if (char1 == '#' && char2 == 'x' && char3 == '9' && char4 == ';')
                    {
                        sb.Append('\t');
                        k = k + 5;
                        continue;
                    }

                    // &#xA; = \n
                    if (char1 == '#' && char2 == 'x' && char3 == 'A' && char4 == ';')
                    {
                        sb.Append('\n');
                        k = k + 5;
                        continue;
                    }

                    // &#D; = \r
                    if (char1 == '#' && char2 == 'x' && char3 == 'D' && char4 == ';')
                    {
                        sb.Append('\r');
                        k = k + 5;
                        continue;
                    }

                    // &amp; = &
                    if (char1 == 'a' && char2 == 'm' && char3 == 'p' && char4 == ';')
                    {
                        sb.Append('&');
                        k = k + 5;
                        continue;
                    }

                    char5 = str[k + 5];

                    // &apos; = '
                    if (char1 == 'a' && char2 == 'p' && char3 == 'o' && char4 == 's' && char5 == ';')
                    {
                        sb.Append('\'');
                        k = k + 6;
                        continue;
                    }

                    // &quot; = \
                    if (char1 == 'q' && char2 == 'u' && char3 == 'o' && char4 == 't' && char5 == ';')
                    {
                        sb.Append('\"');
                        k = k + 6;
                        continue;
                    }

                    throw new InvalidPredefinedEntityException(i);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a list of the XML attributes.
        /// </summary>
        /// <param name="str"> String. </param>
        /// <param name="i"> Index. </param>
        /// <param name="endChar"> End char. </param>
        /// <param name="endChar2"> End char 2. </param>
        /// <param name="nodeName"> Name of the XML node. </param>
        /// <returns> List of the parsed XML attributes. </returns>
        protected static LinkedList<AltXmlAttribute> ParseAttributes(string str, ref int i, char endChar, char endChar2, out string nodeName)
        {
            LinkedList<AltXmlAttribute> attributes = new LinkedList<AltXmlAttribute>();

            SkipSpaces(str, ref i);
            string name = GetValue(str, ref i, endChar, endChar2, true);

            SkipSpaces(str, ref i);

            char quote = default(char);
            string attributeName = "", attributeValue = "";
            char char0 = str[i];

            while (char0 != endChar && char0 != endChar2)
            {
                attributeName = GetValue(str, ref i, '=', '\0', true);

                SkipSpaces(str, ref i);
                i++; // skip '='
                SkipSpaces(str, ref i);

                quote = str[i];
                if (!IsQuote(quote))
                {
                    throw new UnexpectedTokenAfterAttributeException(attributeName);
                }

                i++; // skip quote
                attributeValue = GetValue(str, ref i, quote, '\0', false);
                i++; // skip quote

                attributes.AddLast(new AltXmlAttribute(attributeName, attributeValue));

                SkipSpaces(str, ref i);
                char0 = str[i];
            }

            nodeName = name;
            return attributes;
        }

        /// <summary>
        /// Writes the value of the node or attribute.
        /// </summary>
        /// <param name="builder"> Builder of the string. </param>
        /// <param name="value"> The value. </param>
        protected unsafe static void WriteValue(StringBuilder builder, string value)
        {
            int length = value.Length;
            int index = 0, start = 0;
            char c;
            string replace = null;

            fixed (char* sPtr = value)
            {
                char* ptr = sPtr;

                while (index != length)
                {
                    c = *ptr++;

                    if (replaceTable.TryGetValue(c, out replace))
                    {
                        builder.Append(replace);
                        index++;
                        start = index;
                    }
                    else
                    {
                        index++;
                    }
                }

                if (start < length)
                {
                    builder.Append(value, start, length - start);
                }
            }
        }
    }
}