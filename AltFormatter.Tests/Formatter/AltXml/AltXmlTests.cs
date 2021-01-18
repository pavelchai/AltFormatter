/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System.IO;
using System.Text;
using System.Xml;

namespace AltFormatter.Formatter
{
	public sealed partial class AltXmlTests
	{
		private sealed class UFT8StringWriter : StringWriter
        {
            public override Encoding Encoding
            {
                get
                {
                    return Encoding.UTF8;
                }
            }
        }
		
        private static XmlAttribute CreateAttribute(XmlDocument document, XmlNode node, string name, string value)
        {
            XmlAttribute attributeName = document.CreateAttribute(name);
            attributeName.Value = value;
            return node.Attributes.Append(attributeName);
        }

        private static XmlDocument CreateDocument()
        {
            XmlDocument document = new XmlDocument();

            XmlDeclaration xmlDeclaration = document.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = document.DocumentElement;
            document.InsertBefore(xmlDeclaration, root);

            return document;
        }

        private static XmlElement CreateElement(XmlDocument document, XmlNode node, string name)
        {
            XmlElement rootElement = document.CreateElement(string.Empty, name, string.Empty);
            node.AppendChild(rootElement);
            return rootElement;
        }

        private static XmlNode CreateNode(XmlDocument document, XmlNode node, string name, string value = "")
        {
            XmlNode childNode = document.CreateElement(name);
            childNode.InnerText = value;
            return node.AppendChild(childNode);
        }
	}
}