/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;

namespace AltFormatter.Formatter
{
	public sealed partial class AltFormatterTests
	{
		[Test]
        public void Restore_PrimitiveMDArray_Primitive1DArray()
        {
        	int[,] array = {{1}};
        	int[,] deserialized = this.SerializeDeserialize(array);
        	
        	Assert.AreEqual(array.Rank, deserialized.Rank);
        	Assert.AreEqual(array.GetLength(0), deserialized.GetLength(0));
        	Assert.AreEqual(array.GetLength(1), deserialized.GetLength(1));
        	Assert.AreEqual(array[0,0],deserialized[0,0]);
        }
        
        [Test]
        public void Restore_NotPrimitiveMDArray_NotPrimitiveMDArray()
        {
        	MappingStruct2ValuesT1[,] array = {{new MappingStruct2ValuesT1()}};
        	MappingStruct2ValuesT1[,] deserialized = this.SerializeDeserialize(array);
        	
        	Assert.AreEqual(array.Rank, deserialized.Rank);
        	Assert.AreEqual(array.GetLength(0), deserialized.GetLength(0));
        	Assert.AreEqual(array.GetLength(1), deserialized.GetLength(1));
        	Assert.AreEqual(array[0,0],deserialized[0,0]);
        }
        
        public void Restore_MDArrayStringWithLines_MDArrayString()	
        {
        	string[,] array = {{"\r\nvalue\rvalue\n"}};
        	string[,] deserialized = this.SerializeDeserialize(array);
        	
        	Assert.True(array[0,0] == deserialized[0,0]);
        }
        
        public void Restore_MDArrayStringWithTabs_MDArrayStringg()
        {
        	string[,] array = {{"\tvalue\tvalue\t"}};
        	string[,] deserialized = this.SerializeDeserialize(array);
        	
        	Assert.True(array[0,0] == deserialized[0,0]);
        }
	}
}