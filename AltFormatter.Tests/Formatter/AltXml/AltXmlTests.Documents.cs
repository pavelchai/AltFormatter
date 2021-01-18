/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Utils;
using NUnit.Framework;
using System.Linq;
using System.Xml;

namespace AltFormatter.Formatter
{
    public sealed partial class AltXmlTests
    {
        [Test]
        public void ConvertSimpleXML_SystemXML_AltXML()
        {
            XmlDocument document = CreateDocument();
            XmlElement rootElement = CreateElement(document, document, "root");
            XmlNode classNode = CreateNode(document, rootElement, "class");
            CreateAttribute(document, classNode, "name", "Test");

            XmlElement keysElement = CreateElement(document, classNode, "keys");

            XmlNode key1Node = CreateNode(document, keysElement, "key");
            CreateAttribute(document, key1Node, "name", "key1");
            XmlNode key1Value = CreateNode(document, key1Node, "value", "key1_value");

            XmlNode key2Node = CreateNode(document, keysElement, "key");
            CreateAttribute(document, key2Node, "name", "key2");
            XmlNode key2Value = CreateNode(document, key2Node, "value", "key2_value");

            UFT8StringWriter stringWriter = new UFT8StringWriter();
            document.Save(stringWriter);
            string xmlString = stringWriter.ToString();

            AltXmlDocument altDocument = new AltXmlDocument(xmlString);

            // Root node
            AltXmlNode altRootNode = altDocument.RootNode;
            Assert.NotNull(altRootNode);
            Assert.AreEqual("", altRootNode.Value);

            // Class node
            AltXmlNode altClassNode = altRootNode[classNode.Name];
            Assert.NotNull(altClassNode);
            Assert.AreEqual("", altClassNode.Value);

            AltXmlAttribute altClassNodeAttribute = altClassNode.GetAttribute("name");
            Assert.NotNull(altClassNodeAttribute);
            Assert.AreEqual("Test", altClassNodeAttribute.Value);

            // Keys node
            AltXmlNode altKeysNode = altClassNode[keysElement.Name];
            Assert.NotNull(altKeysNode);
            Assert.AreEqual("", altKeysNode.Value);

            // Key1 node
            AltXmlNode altKey1Node = altKeysNode.SubNodes.ElementAt(0);
            Assert.NotNull(altKey1Node);
            Assert.AreEqual("", altKey1Node.Value);

            AltXmlAttribute altKey1Attribute = altKey1Node.GetAttribute("name");
            Assert.NotNull(altKey1Attribute);
            Assert.AreEqual("key1", altKey1Attribute.Value);

            AltXmlNode altKey1Value = altKey1Node[key1Value.Name];
            Assert.NotNull(altKey1Value);
            Assert.AreEqual(key1Value.InnerText, altKey1Value.Value);

            // Key2 node
            AltXmlNode altKey2Node = altKeysNode.SubNodes.ElementAt(1);
            Assert.NotNull(altKey2Node);
            Assert.AreEqual("", altKey2Node.Value);

            AltXmlAttribute altKey2Attribute = altKey2Node.GetAttribute("name");
            Assert.NotNull(altKey2Attribute);
            Assert.AreEqual("key2", altKey2Attribute.Value);

            AltXmlNode altKey2Value = altKey2Node[key2Value.Name];
            Assert.NotNull(altKey2Value);
            Assert.AreEqual(key2Value.InnerText, altKey2Value.Value);
        }

        [Test]
        public void ConvertXMLWithPredefinedEntities_SystemXML_AltXML()
        {
            XmlDocument document = CreateDocument();
            XmlElement rootElement = CreateElement(document, document, "root");
            XmlNode classNode = CreateNode(document, rootElement, "class");
            CreateAttribute(document, classNode, "name", "Test");

            XmlElement keysElement = CreateElement(document, classNode, "keys");

            XmlNode key1Node = CreateNode(document, keysElement, "key");
            CreateAttribute(document, key1Node, "name", "key1");
            XmlNode key1Value = CreateNode(document, key1Node, "value", "key1_value");

            // Key 2 node with predefined entities
            XmlNode key2Node = CreateNode(document, keysElement, "key");
            CreateAttribute(document, key2Node, "name", "\"&'");
            XmlNode key2Value = CreateNode(document, key2Node, "value", "<>");

            UFT8StringWriter stringWriter = new UFT8StringWriter();
            document.Save(stringWriter);
            string xmlString = stringWriter.ToString();

            AltXmlDocument altDocument = new AltXmlDocument(xmlString);

            // Root node
            AltXmlNode altRootNode = altDocument.RootNode;
            Assert.NotNull(altRootNode);
            Assert.AreEqual("", altRootNode.Value);

            // Class node
            AltXmlNode altClassNode = altRootNode[classNode.Name];
            Assert.NotNull(altClassNode);
            Assert.AreEqual("", altClassNode.Value);

            AltXmlAttribute altClassNodeAttribute = altClassNode.GetAttribute("name");
            Assert.NotNull(altClassNodeAttribute);
            Assert.AreEqual("Test", altClassNodeAttribute.Value);

            // Keys node
            AltXmlNode altKeysNode = altClassNode[keysElement.Name];
            Assert.NotNull(altKeysNode);
            Assert.AreEqual("", altKeysNode.Value);

            // Key1 node
            AltXmlNode altKey1Node = altKeysNode.SubNodes.ElementAt(0);
            Assert.NotNull(altKey1Node);
            Assert.AreEqual("", altKey1Node.Value);

            AltXmlAttribute altKey1Attribute = altKey1Node.GetAttribute("name");
            Assert.NotNull(altKey1Attribute);
            Assert.AreEqual("key1", altKey1Attribute.Value);

            AltXmlNode altKey1Value = altKey1Node[key1Value.Name];
            Assert.NotNull(altKey1Value);
            Assert.AreEqual(key1Value.InnerText, altKey1Value.Value);

            // Key2 node
            AltXmlNode altKey2Node = altKeysNode.SubNodes.ElementAt(1);
            Assert.NotNull(altKey2Node);
            Assert.AreEqual("", altKey2Node.Value);

            AltXmlAttribute altKey2Attribute = altKey2Node.GetAttribute("name");
            Assert.NotNull(altKey2Attribute);
            Assert.AreEqual("\"&'", altKey2Attribute.Value);

            AltXmlNode altKey2Value = altKey2Node[key2Value.Name];
            Assert.NotNull(altKey2Value);
            Assert.AreEqual(key2Value.InnerText, altKey2Value.Value);
        }

        [Test]
        public void ConvertXMLWithEscapeChars_SystemXML_AltXML()
        {
            XmlDocument document = CreateDocument();
            XmlElement rootElement = CreateElement(document, document, "root");
            XmlNode classNode = CreateNode(document, rootElement, "class");
            CreateAttribute(document, classNode, "name", "'\"&<>");

            XmlElement keysElement = CreateElement(document, classNode, "keys");

            XmlNode key1Node = CreateNode(document, keysElement, "key");
            CreateAttribute(document, key1Node, "name", "'\"&<>");
            XmlNode key1Value = CreateNode(document, key1Node, "value", "'\"&<>");

            UFT8StringWriter stringWriter = new UFT8StringWriter();
            document.Save(stringWriter);
            string xmlString = stringWriter.ToString();

            AltXmlDocument altDocument = new AltXmlDocument(xmlString);

            // Root node
            AltXmlNode altRootNode = altDocument.RootNode;
            Assert.NotNull(altRootNode);
            Assert.AreEqual("", altRootNode.Value);

            // Class node
            AltXmlNode altClassNode = altRootNode[classNode.Name];
            Assert.NotNull(altClassNode);
            Assert.AreEqual("", altClassNode.Value);

            AltXmlAttribute altClassNodeAttribute = altClassNode.GetAttribute("name");
            Assert.NotNull(altClassNodeAttribute);
            Assert.AreEqual("'\"&<>", altClassNodeAttribute.Value);

            // Keys node
            AltXmlNode altKeysNode = altClassNode[keysElement.Name];
            Assert.NotNull(altKeysNode);
            Assert.AreEqual("", altKeysNode.Value);

            // Key1 node
            AltXmlNode altKey1Node = altKeysNode.SubNodes.ElementAt(0);
            Assert.NotNull(altKey1Node);
            Assert.AreEqual("", altKey1Node.Value);

            AltXmlAttribute altKey1Attribute = altKey1Node.GetAttribute("name");
            Assert.NotNull(altKey1Attribute);
            Assert.AreEqual("'\"&<>", altKey1Attribute.Value);
        }

        [Test]
        public void SaveLoadXMLWithEscapeChars_AltXML_AltXML()
        {
            AltXmlDocument altDocument = new AltXmlDocument("root", "");
            AltXmlNode node = new AltXmlNode("node", "'\"&<>\t\n\r");
            altDocument.RootNode.SubNodes.AddLast(node);

            byte[] data = altDocument.GetXmlData();

            AltXmlDocument loadedDocument = new AltXmlDocument(data.GetString());
            Assert.AreEqual("'\"&<>\t\n\r", loadedDocument.RootNode.SubNodes.First.Value.Value);
        }
    }
}