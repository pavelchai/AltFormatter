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
        public void Combine_0Strings_EmptyString()
        {
            Assert.AreEqual(string.Empty, StringUtils.Combine());
        }

        [Test]
        public void Combine_EmptyString_EmptyString()
        {
            Assert.AreEqual(string.Empty, StringUtils.Combine("", ""));
        }

        [Test]
        public void Combine_0Separators_CombinedStringWithoutSeparators()
        {
            Assert.AreEqual("111222333", StringUtils.Combine("111", "222", "333"));
        }
        
        [Test]
        public void Combine_StringsNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => StringUtils.Combine(null),
        		"Strings");
        }
        
        [Test]
        public void Combine_0StringNull_ValidateIsNull0String()
        {
        	AssertValidation.NotNullAll(
        		() => StringUtils.Combine(null,""),
        		"Strings",0);
        }
        
        [Test]
        public void Combine_1Separator0Strings_EmptyString()
        {
            Assert.AreEqual(string.Empty, StringUtils.Combine('\n'));
        }

        [Test]
        public void Combine_1Separator1String_OneString()
        {
            Assert.AreEqual("111", StringUtils.Combine('\n', "111"));
        }

        [Test]
        public void Combine_1Separator3EmptyStrings_2Separators()
        {
            Assert.AreEqual("\n\n", StringUtils.Combine('\n', "", "", ""));
        }

        [Test]
        public void Combine_1Separator3Strings_CombinedStringWithSeparators()
        {
            Assert.AreEqual("111.222.333", StringUtils.Combine('.', "111", "222", "333"));
        }
        
        [Test]
        public void Combine_1SeparatorStringsNull_ValidateIsNull()
        {
        	AssertValidation.NotNull(
        		() => StringUtils.Combine('\n',null),
        		"Strings");
        }
        
        [Test]
        public void Combine_1Separato0StringNull_ValidateIsNull0String()
        {
        	AssertValidation.NotNullAll(
        		() => StringUtils.Combine('\n', null, ""),
        		"Strings",0);
        }
    }
}