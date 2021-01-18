/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;
using System.Linq;

namespace AltFormatter.Utils
{
    public sealed partial class StringUtilsTests
    {
        private const char separator = '\n';

        [Test]
        public void Split_EmptyInputString_EmptyString()
        {
            var lines = StringUtils.Split("", '\n');
            Assert.AreEqual(1, lines.Count());
            Assert.AreEqual(string.Empty, lines.ElementAt(0));
        }

        [Test]
        public void Split_0Separators1String_1String()
        {
            var lines = StringUtils.Split("xxx");
            Assert.AreEqual(1, lines.Count());
            Assert.AreEqual("xxx", lines.ElementAt(0));
        }

        [Test]
        public void Split_1SeparatorAtStart1Times1String_2Strings()
        {
            var lines = StringUtils.Split(string.Concat(separator, "xxx"), separator);
            Assert.AreEqual(2, lines.Count());
            Assert.AreEqual(string.Empty, lines.ElementAt(0));
            Assert.AreEqual("xxx", lines.ElementAt(1));
        }

        [Test]
        public void Split_1SeparatorAtStart2Times1String_3Strings()
        {
            var lines = StringUtils.Split(string.Concat(separator, separator, "xxx"), separator);
            Assert.AreEqual(3, lines.Count());
            Assert.AreEqual(string.Empty, lines.ElementAt(0));
            Assert.AreEqual(string.Empty, lines.ElementAt(1));
            Assert.AreEqual("xxx", lines.ElementAt(2));
        }

        [Test]
        public void Split_1SeparatorAtEnd1Times1String_2Strings()
        {
            var lines = StringUtils.Split(string.Concat("xxx", separator), separator);
            Assert.AreEqual(2, lines.Count());
            Assert.AreEqual("xxx", lines.ElementAt(0));
            Assert.AreEqual(string.Empty, lines.ElementAt(1));
        }

        [Test]
        public void Split_1SeparatorAtEnd2Times1String_3Add1Strings()
        {
            var lines = StringUtils.Split(string.Concat("xxx", separator, separator), separator);
            Assert.AreEqual(3, lines.Count());
            Assert.AreEqual("xxx", lines.ElementAt(0));
            Assert.AreEqual(string.Empty, lines.ElementAt(1));
            Assert.AreEqual(string.Empty, lines.ElementAt(2));
        }

        [Test]
        public void Split_1SeparatorInside1Times2String_2Strings()
        {
            var lines = StringUtils.Split(string.Concat("xxx", separator, "yyy"), separator);
            Assert.AreEqual(2, lines.Count());
            Assert.AreEqual("xxx", lines.ElementAt(0));
            Assert.AreEqual("yyy", lines.ElementAt(1));
        }

        [Test]
        public void Split_1SeparatorInsideOnce1TimesTwise2String_3Strings()
        {
            var lines = StringUtils.Split(string.Concat("xxx", separator, separator, "yyy"), separator);
            Assert.AreEqual(3, lines.Count());
            Assert.AreEqual("xxx", lines.ElementAt(0));
            Assert.AreEqual(string.Empty, lines.ElementAt(1));
            Assert.AreEqual("yyy", lines.ElementAt(2));
        }

        [Test]
        public void Split_1SeparatorInside2Times3String_3Strings()
        {
            var lines = StringUtils.Split(string.Concat("xxx", separator, "yyy", separator, "zzz"), separator);
            Assert.AreEqual(3, lines.Count());
            Assert.AreEqual("xxx", lines.ElementAt(0));
            Assert.AreEqual("yyy", lines.ElementAt(1));
            Assert.AreEqual("zzz", lines.ElementAt(2));
        }

        [Test]
        public void Split_1SeparatorInside2TimesTwise3String_5Strings()
        {
            var lines = StringUtils.Split(string.Concat("xxx", separator, separator, "yyy", separator, separator, "zzz"), separator);
            Assert.AreEqual(5, lines.Count());
            Assert.AreEqual("xxx", lines.ElementAt(0));
            Assert.AreEqual(string.Empty, lines.ElementAt(1));
            Assert.AreEqual("yyy", lines.ElementAt(2));
            Assert.AreEqual(string.Empty, lines.ElementAt(3));
            Assert.AreEqual("zzz", lines.ElementAt(4));
        }
        
        [Test]
        public void Split_AllCases_Strings()
        {
            var lines = StringUtils.Split(string.Concat(separator, "www", separator, "xxx", separator, separator, "yyy", separator, "zzz", separator), separator);
            Assert.AreEqual(7, lines.Count());
            Assert.AreEqual(string.Empty, lines.ElementAt(0));
            Assert.AreEqual("www", lines.ElementAt(1));
            Assert.AreEqual("xxx", lines.ElementAt(2));
            Assert.AreEqual(string.Empty, lines.ElementAt(3));
            Assert.AreEqual("yyy", lines.ElementAt(4));
            Assert.AreEqual("zzz", lines.ElementAt(5));
            Assert.AreEqual(string.Empty, lines.ElementAt(6));
        }
        
        [Test]
        public void Split_3DiffererentSeparators_Strings()
        {
            var lines = StringUtils.Split(string.Concat("\n", "xxx", "\r\r", "yyy", "\n", "zzz", "\t"), '\n', '\r', '\t').ToArray();
            Assert.AreEqual(6, lines.Count());
            Assert.AreEqual(string.Empty, lines.ElementAt(0));
            Assert.AreEqual("xxx", lines.ElementAt(1));
            Assert.AreEqual(string.Empty, lines.ElementAt(2));
            Assert.AreEqual("yyy", lines.ElementAt(3));
            Assert.AreEqual("zzz", lines.ElementAt(4));
            Assert.AreEqual(string.Empty, lines.ElementAt(5));
        }

        [Test]
        public void Split_NullInputString_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => StringUtils.Split(null, '\n').Count(),
        		"InputString");
        }
        
        [Test]
        public void Split_NullSeparators_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => StringUtils.Split("", null).Count(),
        		"Separators");
        }
    }
}