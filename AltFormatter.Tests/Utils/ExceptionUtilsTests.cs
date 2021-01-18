/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;
using System;

namespace AltFormatter.Utils
{
    public sealed class ExceptionUtilsTests
    {
        [Test]
        public void GetFullMessage_NoInnerException_Message()
        {
            Exception exception = new Exception("New exception");
            Assert.AreEqual("New exception\r\nSource: \r\nStack trace: ", exception.GetFullMessage());
        }

        [Test]
        public void GetFullMessage_1LevelInnerException_Message()
        {
            Exception innerException = new Exception("Inner exception");
            Exception outerException = new Exception("Outer exception", innerException);
            Assert.AreEqual("Outer exception\r\n-----------------\r\nInner exception\r\nSource: \r\nStack trace: ", outerException.GetFullMessage());
        }

        [Test]
        public void GetFullMessage_2LevelInnerException_Message()
        {
            Exception innerExceptionL2 = new Exception("Inner exception L2");
            Exception innerExceptionL1 = new Exception("Inner exception L1", innerExceptionL2);
            Exception outerException = new Exception("Outer exception", innerExceptionL1);
            Assert.AreEqual("Outer exception\r\n-----------------\r\nInner exception L1\r\n-----------------\r\nInner exception L2\r\nSource: \r\nStack trace: ", outerException.GetFullMessage());
        }
        
        [Test]
        public void GetFullMessage_ExceptionNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => (null as Exception).GetFullMessage(),
        		"Exception");
        }
    }
}