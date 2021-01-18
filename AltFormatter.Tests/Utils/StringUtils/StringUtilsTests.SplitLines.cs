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
        private static readonly string[] LineSeparators = { "\n", "\r", "\r\n" };

        [Test]
        public void SplitLines_EmptyInputString_InputString()
        {
            var lines = StringUtils.SplitLines("");
            Assert.AreEqual(1, lines.Count());
            Assert.AreEqual(string.Empty, lines.ElementAt(0));
        }

        [Test, TestCaseSource("LineSeparators")]
        public void SplitLines_1StringWith1SeparatorAtStart1Times_2Strings(string separator)
        {
            var lines = StringUtils.SplitLines(string.Concat(separator, "xxx"));
            Assert.AreEqual(2, lines.Count());
            Assert.AreEqual(string.Empty, lines.ElementAt(0));
            Assert.AreEqual("xxx", lines.ElementAt(1));
        }

        [Test, TestCaseSource("LineSeparators")]
        public void SplitLines_1StringWith1SeparatorAtStart2Times_3Strings(string separator)
        {
            var lines = StringUtils.SplitLines(string.Concat(separator, separator, "xxx"));
            Assert.AreEqual(3, lines.Count());
            Assert.AreEqual(string.Empty, lines.ElementAt(0));
            Assert.AreEqual(string.Empty, lines.ElementAt(1));
            Assert.AreEqual("xxx", lines.ElementAt(2));
        }

        [Test, TestCaseSource("LineSeparators")]
        public void SplitLines_1StringWith1SeparatorAtEnd1Times_2String(string separator)
        {
            var lines = StringUtils.SplitLines(string.Concat("xxx", separator));
            Assert.AreEqual(2, lines.Count());
            Assert.AreEqual("xxx", lines.ElementAt(0));
            Assert.AreEqual(string.Empty, lines.ElementAt(1));
        }

        [Test, TestCaseSource("LineSeparators")]
        public void SplitLines_1StringWith1SeparatorAtEnd2Times_3Strings(string separator)
        {
            var lines = StringUtils.SplitLines(string.Concat("xxx", separator, separator));
            Assert.AreEqual(3, lines.Count());
            Assert.AreEqual("xxx", lines.ElementAt(0));
            Assert.AreEqual(string.Empty, lines.ElementAt(1));
            Assert.AreEqual(string.Empty, lines.ElementAt(2));
        }

        [Test, TestCaseSource("LineSeparators")]
        public void SplitLines_1StringWith1SeparatorInside1Times_2Strings(string separator)
        {
            var lines = StringUtils.SplitLines(string.Concat("xxx", separator, "yyy"));
            Assert.AreEqual(2, lines.Count());
            Assert.AreEqual("xxx", lines.ElementAt(0));
            Assert.AreEqual("yyy", lines.ElementAt(1));
        }

        [Test, TestCaseSource("LineSeparators")]
        public void SplitLines_1StringWith1SeparatorInside1TimesTwise_2Strings(string separator)
        {
            var lines = StringUtils.SplitLines(string.Concat("xxx", separator, separator, "yyy"));
            Assert.AreEqual(3, lines.Count());
            Assert.AreEqual("xxx", lines.ElementAt(0));
            Assert.AreEqual(string.Empty, lines.ElementAt(1));
            Assert.AreEqual("yyy", lines.ElementAt(2));
        }

        [Test, TestCaseSource("LineSeparators")]
        public void SplitLines_1StringWith1SeparatorsInside2Times_3Strings(string separator)
        {
            var lines = StringUtils.SplitLines(string.Concat("xxx", separator, "yyy", separator, "zzz"));
            Assert.AreEqual(3, lines.Count());
            Assert.AreEqual("xxx", lines.ElementAt(0));
            Assert.AreEqual("yyy", lines.ElementAt(1));
            Assert.AreEqual("zzz", lines.ElementAt(2));
        }

        [Test, TestCaseSource("LineSeparators")]
        public void SplitLines_1StringWith1SeparatorInside2TimesTwise_5Strings(string separator)
        {
            var lines = StringUtils.SplitLines(string.Concat("xxx", separator, separator, "yyy", separator, separator, "zzz"));
            Assert.AreEqual(5, lines.Count());
            Assert.AreEqual("xxx", lines.ElementAt(0));
            Assert.AreEqual(string.Empty, lines.ElementAt(1));
            Assert.AreEqual("yyy", lines.ElementAt(2));
            Assert.AreEqual(string.Empty, lines.ElementAt(3));
            Assert.AreEqual("zzz", lines.ElementAt(4));
        }

        [Test, TestCaseSource("LineSeparators")]
        public void SplitLines_AllCases_Strings(string separator)
        {
            var lines = StringUtils.SplitLines(string.Concat(separator, "www", separator, "xxx", separator, separator, "yyy", separator, "zzz", separator));
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
        public void SplitLines_NullInputString_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => StringUtils.Split(null, '\n').Count(),
        		"InputString");
        }
    }
}