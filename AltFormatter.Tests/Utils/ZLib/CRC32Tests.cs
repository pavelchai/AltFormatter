/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;

namespace AltFormatter.Utils.ZLib
{
    public sealed class CRC32UtilsTests
    {
        [Test]
        public void CRC32_NoArgs_Zero()
        {
            Assert.AreEqual(0, CRC32.Calculate());
        }

        [Test]
        public void CRC32_1EmptyArg_Zero()
        {
            Assert.AreEqual(0, CRC32.Calculate(new byte[0]));
        }
        
        [Test]
        public void CRC32_4EmptyArg_Zero()
        {
            Assert.AreEqual(0, CRC32.Calculate(new byte[0], new byte[0], new byte[0], new byte[0]));
        }
        
        [Test]
        public void CRC32_1Arg_Checksum()
        {
            
            Assert.AreEqual(64128641, CRC32.Calculate(ArrayUtils.CreateArray((byte)255, 100)));
        }

        [Test]
        public void CRC32_4Args_Checksum()
        {
            byte[] data = ArrayUtils.CreateArray((byte)255, 100);
            Assert.AreEqual(2307493135, CRC32.Calculate(data, data, data, data));
        }
    }
}