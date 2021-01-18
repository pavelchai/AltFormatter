/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;

namespace AltFormatter.Utils
{
    public sealed partial class StringUtilsTests
    {
        [Test]
        public void GetBytes_UTF8String_Bytes()
        {
            const string testString = "Test";

            byte[] data = testString.GetBytes();
            Assert.AreEqual(data[0], 0x54);
            Assert.AreEqual(data[1], 0x65);
            Assert.AreEqual(data[2], 0x73);
            Assert.AreEqual(data[3], 0x74);
        }
        
        [Test]
        public void GetBytes_NullInputString_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => StringUtils.GetBytes(null),
        		"InputString");
        }

        [Test]
        public void GetString_Bytes_String()
        {
            Assert.AreEqual(new byte[] { 0x54, 0x65, 0x73, 0x74 }.GetString(), "Test");
        }
        
        [Test]
        public void GetString_NullBytes_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => StringUtils.GetString(null),
        		"Data");
        }
    }
}