/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;

namespace AltFormatter.Utils.Other.Bytes
{
    public sealed class BytesConverterTests
    {
    	[Test]
        public void GetBytesBigEndian_ZeroInt32_Bytes()
        {
            byte[] bytes = BytesConverter.GetBytesBigEndian((int)0);
            Assert.AreEqual(4, bytes.Length);
            Assert.AreEqual(0, bytes[0]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(0, bytes[2]);
            Assert.AreEqual(0, bytes[3]);
        }
        
        [Test]
        public void GetBytesBigEndian_HalfMaxInt32_Bytes()
        {
            byte[] bytes = BytesConverter.GetBytesBigEndian(int.MaxValue / 2);
            Assert.AreEqual(4, bytes.Length);
            Assert.AreEqual(63, bytes[0]);
            Assert.AreEqual(255, bytes[1]);
            Assert.AreEqual(255, bytes[2]);
            Assert.AreEqual(255, bytes[3]);
        }
    	
        [Test]
        public void GetBytesBigEndian_MaxInt32_Bytes()
        {
            byte[] bytes = BytesConverter.GetBytesBigEndian(int.MaxValue);
            Assert.AreEqual(4, bytes.Length);
            Assert.AreEqual(127, bytes[0]);
            Assert.AreEqual(255, bytes[1]);
            Assert.AreEqual(255, bytes[2]);
            Assert.AreEqual(255, bytes[3]);
        }
        
        [Test]
        public void GetBytesBigEndian_ZeroUInt32_Bytes()
        {
            byte[] bytes = BytesConverter.GetBytesBigEndian((uint)0);
            Assert.AreEqual(4, bytes.Length);
            Assert.AreEqual(0, bytes[0]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(0, bytes[2]);
            Assert.AreEqual(0, bytes[3]);
        }
        
        [Test]
        public void GetBytesBigEndian_HalfMaxUInt32_Bytes()
        {
            byte[] bytes = BytesConverter.GetBytesBigEndian(uint.MaxValue / 2);
            Assert.AreEqual(4, bytes.Length);
            Assert.AreEqual(127, bytes[0]);
            Assert.AreEqual(255, bytes[1]);
            Assert.AreEqual(255, bytes[2]);
            Assert.AreEqual(255, bytes[3]);
        }

        [Test]
        public void GetBytesBigEndian_MaxUInt32_Bytes()
        {
            byte[] bytes = BytesConverter.GetBytesBigEndian(uint.MaxValue);
            Assert.AreEqual(4, bytes.Length);
            Assert.AreEqual(255, bytes[0]);
            Assert.AreEqual(255, bytes[1]);
            Assert.AreEqual(255, bytes[2]);
            Assert.AreEqual(255, bytes[3]);
        }
        
        [Test]
        public void GetBytes_ZeroUInt16_Bytes()
        {
            byte[] bytes = BytesConverter.GetBytes((ushort)0);
            Assert.AreEqual(2, bytes.Length);
            Assert.AreEqual(0, bytes[0]);
            Assert.AreEqual(0, bytes[1]);
        }
        
        [Test]
        public void GetBytes_HalfMaxUInt16_Bytes()
        {
            byte[] bytes = BytesConverter.GetBytes(ushort.MaxValue / 2);
            Assert.AreEqual(2, bytes.Length);
            Assert.AreEqual(255, bytes[0]);
            Assert.AreEqual(127, bytes[1]);
        }
        
        [Test]
        public void GetBytes_MaxUInt16_Bytes()
        {
            byte[] bytes = BytesConverter.GetBytes(ushort.MaxValue);
            Assert.AreEqual(2, bytes.Length);
            Assert.AreEqual(255, bytes[0]);
            Assert.AreEqual(255, bytes[1]);
        }
        
        [Test]
        public void GetBytes_ZeroUInt32_Bytes()
        {
            byte[] bytes = BytesConverter.GetBytes((uint)0);
            Assert.AreEqual(4, bytes.Length);
            Assert.AreEqual(0, bytes[0]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(0, bytes[2]);
            Assert.AreEqual(0, bytes[3]);
        }
        
        [Test]
        public void GetBytes_HalfMaxUInt32_Bytes()
        {
            byte[] bytes = BytesConverter.GetBytes(uint.MaxValue / 2);
            Assert.AreEqual(4, bytes.Length);
            Assert.AreEqual(255, bytes[0]);
            Assert.AreEqual(255, bytes[1]);
            Assert.AreEqual(255, bytes[2]);
            Assert.AreEqual(127, bytes[3]);
        }
        
        [Test]
        public void GetBytes_MaxUInt32_Bytes()
        {
            byte[] bytes = BytesConverter.GetBytes(uint.MaxValue);
            Assert.AreEqual(4, bytes.Length);
            Assert.AreEqual(255, bytes[0]);
            Assert.AreEqual(255, bytes[1]);
            Assert.AreEqual(255, bytes[2]);
            Assert.AreEqual(255, bytes[3]);
        }
        
        [Test]
        public void GetBytes_ZeroUInt64_Bytes()
        {
            byte[] bytes = BytesConverter.GetBytes((ulong)0);
            Assert.AreEqual(8, bytes.Length);
            Assert.AreEqual(0, bytes[0]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(0, bytes[2]);
            Assert.AreEqual(0, bytes[3]);
            Assert.AreEqual(0, bytes[4]);
            Assert.AreEqual(0, bytes[5]);
            Assert.AreEqual(0, bytes[6]);
            Assert.AreEqual(0, bytes[7]);
        }
        
        [Test]
        public void GetBytes_HalfMaxUInt64_Bytes()
        {
            byte[] bytes = BytesConverter.GetBytes(ulong.MaxValue / 2);
            Assert.AreEqual(8, bytes.Length);
            Assert.AreEqual(255, bytes[0]);
            Assert.AreEqual(255, bytes[1]);
            Assert.AreEqual(255, bytes[2]);
            Assert.AreEqual(255, bytes[3]);
            Assert.AreEqual(255, bytes[4]);
            Assert.AreEqual(255, bytes[5]);
            Assert.AreEqual(255, bytes[6]);
            Assert.AreEqual(127, bytes[7]);
        }
        
        [Test]
        public void GetBytes_MaxUInt64_Bytes()
        {
            byte[] bytes = BytesConverter.GetBytes(ulong.MaxValue);
            Assert.AreEqual(8, bytes.Length);
            Assert.AreEqual(255, bytes[0]);
            Assert.AreEqual(255, bytes[1]);
            Assert.AreEqual(255, bytes[2]);
            Assert.AreEqual(255, bytes[3]);
            Assert.AreEqual(255, bytes[4]);
            Assert.AreEqual(255, bytes[5]);
            Assert.AreEqual(255, bytes[6]);
            Assert.AreEqual(255, bytes[7]);
        }
        
        [Test]
        public void ToInt32BigEndianWithoutOffset_4ZeroBytes_ZeroInt32()
        {
            byte[] data = new byte[] { 0, 0, 0, 0 };
            Assert.AreEqual(0, BytesConverter.ToInt32BigEndian(data, 0));
        }
        
        [Test]
        public void ToInt32BigEndianWithoutOffset_4Bytes_HalfMaxInt32()
        {
            byte[] data = new byte[] { 63, 255, 255, 255 };
            Assert.AreEqual(int.MaxValue / 2, BytesConverter.ToInt32BigEndian(data, 0));
        }
        
        [Test]
        public void ToInt32BigEndianWithoutOffset_4ZeroBytes_MaxInt32()
        {
            byte[] data = new byte[] { 127, 255, 255, 255 };
            Assert.AreEqual(int.MaxValue, BytesConverter.ToInt32BigEndian(data, 0));
        }
         
        [Test]
        public void ToInt32BigEndianWithOffset_4ZeroBytes_ZeroInt32()
        {
            byte[] data = new byte[] { 0, 0, 0, 0, 0 };
            Assert.AreEqual(0, BytesConverter.ToInt32BigEndian(data, 1));
        }
        
        [Test]
        public void ToInt32BigEndianWithOffset_4Bytes_HalfMaxInt32()
        {
            byte[] data = new byte[] { 0, 0, 63, 255, 255, 255 };
            Assert.AreEqual(int.MaxValue / 2, BytesConverter.ToInt32BigEndian(data, 2));
        }
        
        [Test]
        public void ToInt32BigEndianWithOffset_4Bytes_MaxInt32()
        {
            byte[] data = new byte[] { 0, 0, 0, 127, 255, 255, 255 };
            Assert.AreEqual(int.MaxValue, BytesConverter.ToInt32BigEndian(data, 3));
        }
        
        [Test]
        public void ToUInt16WithoutOffset_2ZeroBytes_ZeroUInt16()
        {
            byte[] data = new byte[] { 0, 0 };
            Assert.AreEqual(0, BytesConverter.ToUInt16(data, 0));
        }
        
        [Test]
        public void ToUInt16WithoutOffset_2Bytes_HalfMaxUInt16()
        {
            byte[] data = new byte[] { 255, 127 };
            Assert.AreEqual(ushort.MaxValue / 2, BytesConverter.ToUInt16(data, 0));
        }
        
        [Test]
        public void ToUInt16WithoutOffset_2Bytes_MaxUInt16()
        {
            byte[] data = new byte[] { 255, 255 };
            Assert.AreEqual(ushort.MaxValue, BytesConverter.ToUInt16(data, 0));
        }
        
        [Test]
        public void ToUInt16WithOffset_2ZeroBytes_ZeroUInt16()
        {
            byte[] data = new byte[] { 0, 0, 0 };
            Assert.AreEqual(0, BytesConverter.ToUInt16(data, 1));
        }
        
        [Test]
        public void ToUInt16WithOffset_2Bytes_HalfMaxUInt16()
        {
            byte[] data = new byte[] { 0, 0, 255, 127 };
            Assert.AreEqual(ushort.MaxValue / 2, BytesConverter.ToUInt16(data, 2));
        }
        
        [Test]
        public void ToUInt16WithOffset_2Bytes_MaxUInt16()
        {
            byte[] data = new byte[] { 0, 0, 0, 255, 255 };
            Assert.AreEqual(ushort.MaxValue, BytesConverter.ToUInt16(data, 3));
        }
        
        [Test]
        public void ToUInt32WithoutOffset_4ZeroBytes_ZeroUInt32()
        {
            byte[] data = new byte[] { 0, 0, 0, 0 };
            Assert.AreEqual(0, BytesConverter.ToUInt32(data, 0));
        }
        
        [Test]
        public void ToUInt32WithoutOffset_4Bytes_HalfMaxUInt32()
        {
            byte[] data = new byte[] { 255, 255, 255, 127 };
            Assert.AreEqual(uint.MaxValue / 2, BytesConverter.ToUInt32(data, 0));
        }
        
        [Test]
        public void ToUInt32WithoutOffset_4Bytes_MaxUInt16()
        {
            byte[] data = new byte[] { 255, 255, 255, 255 };
            Assert.AreEqual(uint.MaxValue, BytesConverter.ToUInt32(data, 0));
        }
        
        [Test]
        public void ToUInt32WithOffset_4ZeroBytes_ZeroUInt32()
        {
            byte[] data = new byte[] { 0, 0, 0, 0, 0 };
            Assert.AreEqual(0, BytesConverter.ToUInt32(data, 1));
        }
        
        [Test]
        public void ToUInt32WithOffset_4Bytes_HalfMaxUInt32()
        {
            byte[] data = new byte[] { 0, 0, 255, 255, 255, 127 };
            Assert.AreEqual(uint.MaxValue / 2, BytesConverter.ToUInt32(data, 2));
        }
        
        [Test]
        public void ToUInt32WithOffset_4Bytes_MaxUInt32()
        {
            byte[] data = new byte[] { 0, 0, 0, 255, 255, 255, 255 };
            Assert.AreEqual(uint.MaxValue, BytesConverter.ToUInt32(data, 3));
        }
        
        [Test]
        public void ToUInt64WithoutOffset_8Bytes_ZeroUInt64()
        {
            byte[] data = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            Assert.AreEqual(0, BytesConverter.ToUInt64(data, 0));
        }
        
        [Test]
        public void ToUInt64WithoutOffset_8Bytes_HalfMaxUInt64()
        {
            byte[] data = new byte[] { 255, 255, 255, 255, 255, 255, 255, 127 };
            Assert.AreEqual(ulong.MaxValue / 2, BytesConverter.ToUInt64(data, 0));
        }
        
        [Test]
        public void ToUInt64WithoutOffset_8Bytes_MaxUInt64()
        {
            byte[] data = new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 };
            Assert.AreEqual(ulong.MaxValue, BytesConverter.ToUInt64(data, 0));
        }
        
        
        [Test]
        public void ToUInt64WithOffset_8Bytes_ZeroUInt64()
        {
            byte[] data = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Assert.AreEqual(0, BytesConverter.ToUInt64(data, 1));
        }
        
        [Test]
        public void ToUInt64WithOffset_8Bytes_HalfMaxUInt64()
        {
            byte[] data = new byte[] { 0, 0, 255, 255, 255, 255, 255, 255, 255, 127 };
            Assert.AreEqual(ulong.MaxValue / 2, BytesConverter.ToUInt64(data, 2));
        }


        [Test]
        public void ToUInt64WithOffset_8Bytes_MaxUInt64()
        {
            byte[] data = new byte[] { 0, 0, 0, 255, 255, 255, 255, 255, 255, 255, 255 };
            Assert.AreEqual(ulong.MaxValue, BytesConverter.ToUInt64(data, 3));
        }
    }
}