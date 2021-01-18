/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Utils;
using System.Collections.Generic;

namespace AltFormatter.Formatter
{
	public sealed partial class AltFormatterTests
	{
		[Formattable("ClassImplementsIFormattable")]
	    private sealed class ClassImplementsIFormattable : IFormattable
	    {
	        public int OnSerializingRaisedCount = 0;
	        
	        public int OnSerializedRaisedCount = 0;
	        
	        public int OnDeserializingRaisedCount = 0;
	        
	        public int OnDeserializedRaisedCount = 0;
	        
	        public int OnSubstitutionRaisedCount = 0;
	
	        public void OnSerializing()
	        {
	        	this.OnSerializingRaisedCount++;
	        }
	
	        public void OnSerialized()
	        {
	            this.OnSerializedRaisedCount++;
	        }
	
	        public void OnDeserializing()
	        {
	            this.OnDeserializingRaisedCount++;
	        }
	
	        public void OnDeserialized()
	        {
	            this.OnDeserializedRaisedCount++;
	        }
	
	        public object Substitution()
	        {
	            this.OnSubstitutionRaisedCount++;
	            return this;
	        }
	    }
		
		private interface IMappingType2Values
	    {
			double Value1 {get;}
	
	        string Value2 {get;}
	    }
		
		[Formattable("MappingClass2ValuesT1")]
		private sealed class MappingClass2ValuesT1 : IMappingType2Values
	    {
	        [Key("Value1")]
	        public double Value1 {get; private set;}
	
	        [Key("Value2")]
	        public string Value2 {get; private set;}
	        
	        public MappingClass2ValuesT1()
	        {	
	        }
	        
	        public MappingClass2ValuesT1(double value1, string value2)
	        {
	            this.Value1 = value1;
	            this.Value2 = value2;
	        }
	    }
	    
	    [Formattable("MappingClass2ValuesT2")]
	    private sealed class MappingClass2ValuesT2 : IMappingType2Values
	    {
	        [Key("Value1")]
	        public double Value1 {get; private set;}
	
	        [Key("Value2")]
	        public string Value2 {get; private set;}
	        
	        public MappingClass2ValuesT2()
	        {	
	        }
	        
	        public MappingClass2ValuesT2(double value1, string value2)
	        {
	            this.Value1 = value1;
	            this.Value2 = value2;
	        }
	    }
	
	    [Formattable("MappingStruct2ValuesT1")]
	    private struct MappingStruct2ValuesT1 : IMappingType2Values
	    {
	        [Key("Value1")]
	        public double Value1 {get; private set;}
	
	        [Key("Value2")]
	        public string Value2 {get; private set;}
	        
	        public MappingStruct2ValuesT1(double value1, string value2) : this()
	        {
	            this.Value1 = value1;
	            this.Value2 = value2;
	        }
	    }
	    
	    [Formattable("MappingStruct2ValuesT2")]
	    private struct MappingStruct2ValuesT2 : IMappingType2Values
	    {
	        [Key("Value1")]
	        public double Value1 {get; private set;}
	
	        [Key("Value2")]
	        public string Value2 {get; private set;}
	        
	        public MappingStruct2ValuesT2(double value1, string value2) : this()
	        {
	            this.Value1 = value1;
	            this.Value2 = value2;
	        }
	    }
	    
	    private interface IMappingType3Values : IMappingType2Values
	    {
	        string Value3 {get;}
	    }
	
	    [Formattable("MappingStruct3KeyWithoutOptional")]
	    private struct MappingStruct3ValuesWithoutOptional : IMappingType3Values
	    {
	        [Key("Value1")]
	        public double Value1 {get; private set;}
	
	        [Key("Value2")]
	        public string Value2 {get; private set;}
	
	        [Key("Value3")]
	        public string Value3 {get; private set;}
	
	        public MappingStruct3ValuesWithoutOptional(double value1, string value2) : this()
	        {
	            this.Value1 = value1;
	            this.Value2 = value2;
	            this.Value3 = null;
	        }
	    }
	    
	    [Formattable("MappingStruct3KeyWithOptional")]
	    private struct MappingStruct3ValuesWithOptional : IMappingType3Values
	    {
	        [Key("Value1")]
	        public double Value1 {get; private set;}
	
	        [Key("Value2")]
	        public string Value2 {get; private set;}
	
	        [Key("Value3", optional : true)]
	        public string Value3 {get; private set;}
	
	        public MappingStruct3ValuesWithOptional(double value1, string value2) : this()
	        {
	            this.Value1 = value1;
	            this.Value2 = value2;
	            this.Value3 = null;
	        }
	    }
	    
	    [Formattable("ClassWithOrderedKeys")]
	    private sealed class ClassWithOrderedKeys
	    {
	        public readonly Queue<string> Queue = new Queue<string>(2);
	    	
	        [Key("Key1", order: 0)]
	        public int Key1
	        {
	            get
	            {
	            	this.Queue.Enqueue("Key1");
	                return 0;
	            }
	            set
	            {
	                this.Queue.Enqueue("Key1");
	            }
	        }
	
	        [Key("Key2", order: 1)]
	        public int Key2
	        {
	            get
	            {
	                this.Queue.Enqueue("Key2");
	                return 0;
	            }
	            set
	            {
	                this.Queue.Enqueue("Key2");
	            }
	        }
	    }
	
	    [Formattable("ClassWithoutOrderedKeys")]
	    private sealed class ClassWithoutOrderedKeys
	    {
	    	public readonly Queue<string> Queue = new Queue<string>(2);
	    	
	        [Key("Key1", order: 1)]
	        public int Key1
	        {
	            get
	            {
	            	this.Queue.Enqueue("Key1");
	                return 0;
	            }
	            set
	            {
	                this.Queue.Enqueue("Key1");
	            }
	        }
	
	        [Key("Key2", order: 0)]
	        public int Key2
	        {
	            get
	            {
	                this.Queue.Enqueue("Key2");
	                return 0;
	            }
	            set
	            {
	                this.Queue.Enqueue("Key2");
	            }
	        }
	    }
		
	    [Formattable("CircularReferenceT1")]
	    private sealed class CircularReferenceT1
	    {
	        [Key("Reference")]
	        public CircularReferenceT1 Reference { get; set; }
	    }
	    
	    [Formattable("CircularReferenceT2One")]
	    private sealed class CircularReferenceT2One
	    {
	        [Key("Two")]
	        public CircularReferenceT2Two Two { get; set; }
	    }
	
	    [Formattable("CircularReferenceT2Two")]
	    private sealed class CircularReferenceT2Two
	    {
	        [Key("One")]
	        public CircularReferenceT2One One { get; set; }
	    }
	
	    private interface INonGenericInterface
	    {
	        object Value { get; set; }
	    }
	
	    private interface IGenericInterface<T> : INonGenericInterface
	    {
	        new T Value { get; set; }
	    }
	    
	    [Formattable("NonGenericStruct")]
	    private struct NonGenericStruct : INonGenericInterface
	    {
	        object value;
	
	        object INonGenericInterface.Value
	        {
	            get
	            {
	                return this.value;
	            }
	            set
	            {
	                this.value = value;
	            }
	        }
	
	        [Key("Value")]
	        public int Value
	        {
	            get
	            {
	                return (int)this.value;
	            }
	            set
	            {
	                this.value = value;
	            }
	        }
	    }
	
	    [Formattable("NonGenericClass")]
	    private sealed class NonGenericClass : INonGenericInterface
	    {
	        object value = null;
	
	        object INonGenericInterface.Value
	        {
	            get
	            {
	                return this.value;
	            }
	            set
	            {
	                this.value = value;
	            }
	        }
	
	        [Key("Value")]
	        public int Value
	        {
	            get
	            {
	                return (int)this.value;
	            }
	            set
	            {
	                this.value = value;
	            }
	        }
	    }
	    
	    [Formattable("GenericStruct")]
	    private struct GenericStruct<T> : IGenericInterface<T>
	    {
	        [Key("Value")]
	        public T Value { get; set; }
	
	        object INonGenericInterface.Value
	        {
	            get
	            {
	                return this.Value;
	            }
	            set
	            {
	                this.Value = value.ConvertTo<T>();
	            }
	        }
	    }
	
	    [Formattable("GenericClass")]
	    private sealed class GenericClass<T> : IGenericInterface<T>
	    {
	        [Key("Value")]
	        public T Value { get; set; }
	
	        object INonGenericInterface.Value
	        {
	            get
	            {
	                return this.Value;
	            }
	            set
	            {
	            	this.Value = value.ConvertTo<T>();
	            }
	        }
	    }
	    
	    private abstract class AbstractClassWithFieldsAndProperties
	    {
			protected AbstractClassWithFieldsAndProperties()
			{
				this.BaseNotOverridenVirtualProperty = 1000;
				this.BaseOverridenVirtualProperty = 2000;
			}
			
	    	[Key("0")]
	        public virtual long BaseNotOverridenVirtualProperty { get; set; }
	        
	        [Key("1")]
	        public virtual ulong BaseOverridenVirtualProperty { get; set; }
	    }
	    
	    [Formattable("ClassFieldsAndProperties")]
	    private class ClassWithFieldsAndProperties : AbstractClassWithFieldsAndProperties
	    {
	        [Key("2")]
	        public virtual short ThisVirtualProperty { get; set; }
	    	
	        [Key("3")]
	        public readonly int ReadOnlyField;
	
	        [Key("4")]
	        public double NonReadOnlyField;
	        
	        [Key("5")]
	        public string NonVirtualProperty { get; set; }
	
	        public ClassWithFieldsAndProperties()
	        {
	        	this.ThisVirtualProperty = 10;
	            this.ReadOnlyField = 10;
	            this.NonReadOnlyField = 1E-3;
	            this.NonVirtualProperty = "NonVirtualProperty";
	        }
	        
	        public ClassWithFieldsAndProperties(int readOnlyValue) : this()
	        {
	            this.ReadOnlyField = readOnlyValue;
	        }
	    }
	}
}