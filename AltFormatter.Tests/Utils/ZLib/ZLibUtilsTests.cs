/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;
using System.Linq;

namespace AltFormatter.Utils.ZLib
{
    public sealed class ZLibUtilsTests
    {
        [Test]
        public void Deflate_WithoutOffsetAndCount_Bytes()
        {
            byte[] data = new byte[] { 255, 255, 255, 255 };
            byte[] expected = new byte[] { 251, 15, 4, 0 };
            byte[] deflated = ZLibUtils.Deflate(data);

            Assert.True(expected.SequenceEqual(deflated));
        }

        [Test]
        public void Deflate_WithOffsetAndCount_Bytes()
        {
            byte[] data = new byte[] { 255, 255, 255, 255 };
            byte[] expected = new byte[] { 251, 255, 31, 0 };
            byte[] deflated = ZLibUtils.Deflate(data, 1, 2);

            Assert.True(expected.SequenceEqual(deflated));
        }

        [Test]
        public void Inflate_WithoutOffsetAndCount_Bytes()
        {
            byte[] data = new byte[] { 251, 15, 4, 0 };
            byte[] expected = new byte[] { 255, 255, 255, 255 };
            byte[] inflated = ZLibUtils.Inflate(data);

            Assert.True(expected.SequenceEqual(inflated));
        }

        [Test]
        public void Inflate_WithOffsetAndCount_Bytes()
        {
            byte[] data = new byte[] { 0, 0, 251, 255, 31, 0, 0, 0 };
            byte[] expected = new byte[] { 255, 255 };
            byte[] inflated = ZLibUtils.Inflate(data, 2, 4);

            Assert.True(expected.SequenceEqual(inflated));
        }

        [Test]
        public void DeflateInflate_WithoutOffsetAndCount_Bytes()
        {
            byte[] data = ArrayUtils.CreateArray((byte)255, 100);
            Assert.True(data.SequenceEqual(ZLibUtils.Inflate(ZLibUtils.Deflate(data))));
        }

        [Test]
        public void DeflateInflate_WithOffsetAndCount_Bytes()
        {
            byte[] data = ArrayUtils.CreateArray((byte)255, 100);
            Assert.True(data.Skip(10).Take(80).SequenceEqual(ZLibUtils.Inflate(ZLibUtils.Deflate(data, 10, 80))));
        }
    }
}