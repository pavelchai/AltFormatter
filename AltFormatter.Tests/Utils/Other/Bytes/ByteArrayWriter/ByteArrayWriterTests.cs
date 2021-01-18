/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;

namespace AltFormatter.Utils.Other.Bytes
{
    public sealed class ByteArrayWriterTests
    {
        private static readonly object[] AlwaysCopyInputData = { true, false };

        [Test, TestCaseSource("AlwaysCopyInputData")]
        public void Create_ByteArrayWriter(bool alwaysCopyInputData)
        {
            IByteArrayWriter writer = new ByteArrayWriter(alwaysCopyInputData);
            Assert.Pass();
        }
        
        [Test, TestCaseSource("AlwaysCopyInputData")]
        public void GetPosition_AfterCreate(bool alwaysCopyInputData)
        {
        	IByteArrayWriter writer = new ByteArrayWriter(alwaysCopyInputData);
            Assert.AreEqual(0, writer.Position);
        }
        
        [Test, TestCaseSource("AlwaysCopyInputData")]
        public void GetPosition_AfterWriteBytes(bool alwaysCopyInputData)
        {
        	IByteArrayWriter writer = new ByteArrayWriter(alwaysCopyInputData);
            writer.WriteBytes(100, 100);
            Assert.AreEqual(2, writer.Position);
        }

        [Test, TestCaseSource("AlwaysCopyInputData")]
        public void WriteBytes_2Bytes2Offset(bool alwaysCopyInputData)
        {
            byte[] data = new byte[] { 1, 2, 3, 4, 5 };
            IByteArrayWriter writer = new ByteArrayWriter(alwaysCopyInputData);
            writer.WriteBytes(data, 2, 2);

            byte[] output = writer.GetBytes();
            Assert.AreEqual(2, output.Length);
            Assert.AreEqual(3, output[0]);
            Assert.AreEqual(4, output[1]);
        }
        
        [Test, TestCaseSource("AlwaysCopyInputData")]
        public void GetBytes_AfterCreate(bool alwaysCopyInputData)
        {
            IByteArrayWriter writer = new ByteArrayWriter(alwaysCopyInputData);
            byte[] output = writer.GetBytes();
            Assert.AreEqual(0, output.Length);
        }
        
        [Test, TestCaseSource("AlwaysCopyInputData")]
        public void GetBytes_AfterWrite2Bytes3Offset(bool alwaysCopyInputData)
        {
        	byte[] data = new byte[] { 1, 2, 3, 4, 5 };
            IByteArrayWriter writer = new ByteArrayWriter(alwaysCopyInputData);
            writer.WriteBytes(data, 3, 2);

            byte[] output = writer.GetBytes();
            Assert.AreEqual(2, output.Length);
            Assert.AreEqual(4, output[0]);
            Assert.AreEqual(5, output[1]);
        }
    }
}