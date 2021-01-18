/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;

namespace AltFormatter.Utils.Other.Bytes
{
	public sealed class ByteArrayWriterExtensionsTests
	{
		[Test]
        public void WriteGetBytes_4BytesWithoutOffsetCount()
        {
            IByteArrayWriter writer = new ByteArrayWriter(true);
            writer.WriteBytes(10, 20, 30, 40);

            byte[] output = writer.GetBytes();
            Assert.AreEqual(4, output.Length);
            Assert.AreEqual(10, output[0]);
            Assert.AreEqual(20, output[1]);
            Assert.AreEqual(30, output[2]);
            Assert.AreEqual(40, output[3]);
        }
        
        [Test]
        public void GetBytes_AfterWrite1ByteWithoutOffsetCount()
        {
            IByteArrayWriter writer = new ByteArrayWriter(true);
            writer.WriteBytes(30);

            byte[] output = writer.GetBytes();
            Assert.AreEqual(1, output.Length);
            Assert.AreEqual(30, output[0]);
        }
	}
}