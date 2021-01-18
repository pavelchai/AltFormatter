/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;
using System;

namespace AltFormatter.Formatter
{
	public sealed partial class AltFormatterTests
	{
		private static readonly object[] PrimitiveObjects = DataSources.PrimitiveTypeValueSource;
		
		[Test, TestCaseSource("PrimitiveObjects")]
        public void Restore_PrimitiveObjects_PrimitiveObjects(Type type, object value)
        {
            var deserialized = SerializeDeserialize(type,value);
            if (type != typeof(DateTime))
            {
            	Assert.AreEqual(value, deserialized);
            }
        }
        
        [Test]
        public void Restore_DateTimeUTC_DateTimeUTC()
        {
        	Restore_DateTime_DateTime(DateTime.Now.ToUniversalTime());
        }
        
        [Test]
        public void Restore_DateTimeLocal_DateTimeLocal()
        {
        	Restore_DateTime_DateTime(DateTime.Now);
        }
        
        [Test]
        public void Restore_DateTimeUnspecified_DateTimeUnspecified()
        {
        	Restore_DateTime_DateTime(new DateTime(2020, 10, 1, 1, 2, 3, 4, DateTimeKind.Unspecified));
        }
        
        private void Restore_DateTime_DateTime(DateTime dateTime)
        {
        	var deserialized = SerializeDeserialize(dateTime);
        	
        	Assert.AreEqual(dateTime.Kind, deserialized.Kind);
        	Assert.True(DateTimeUtils.Equals(dateTime, deserialized));
        }
	}
}