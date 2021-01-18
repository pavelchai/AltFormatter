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
        public void Restore_CircularReferencesT1_OneObject()
        {
        	CircularReferenceT1 instance = new CircularReferenceT1();
            instance.Reference = instance;

            var data = formatter.Serialize<CircularReferenceT1>(instance);
            var restored = formatter.Deserialize<CircularReferenceT1>(data);

            Assert.True(restored == restored.Reference);
        }
        
        [Test]
        public void Restore_CircularReferencesT2_TwoObjects()
        {
            CircularReferenceT2One instanceOne = new CircularReferenceT2One();
            CircularReferenceT2Two instanceTwo = new CircularReferenceT2Two();

            instanceOne.Two = instanceTwo;
            instanceTwo.One = instanceOne;

            object[] objects = { instanceOne, instanceTwo };

            var data = formatter.Serialize<object[]>(objects);
            var references = formatter.Deserialize<object[]>(data);

            var referenceOne = references[0] as CircularReferenceT2One;
            var referenceTwo = references[1] as CircularReferenceT2Two;

            Assert.True(referenceOne == referenceTwo.One);
            Assert.True(referenceTwo == referenceOne.Two);
        }
		
		[Test]
    	public void Restore_NonGenericStruct_NonGenericStruct()
        {
    		Restore_StructOrClass_Test<NonGenericStruct, NonGenericStruct>();
        }

        [Test]
        public void Restore_NonGenericClass_NonGenericClass()
        {
        	Restore_StructOrClass_Test<NonGenericClass, NonGenericClass>();
        }
        
        [Test]
    	public void Restore_NonGenericStructAsInterface_NonGenericStructAsInterface()
        {
    		Restore_StructOrClass_Test<NonGenericStruct, INonGenericInterface>();
        }

        [Test]
        public void Restore_NonGenericClassAsInterface_NonGenericClassAsInterface()
        {
        	Restore_StructOrClass_Test<NonGenericClass, INonGenericInterface>();
        }
        
        [Test]
    	public void Restore_GenericStruct_GenericStruct()
        {
    		Restore_StructOrClass_Test<GenericStruct<int>, GenericStruct<int>>();
        }

        [Test]
        public void Restore_GenericClass_GenericClass()
        {
        	Restore_StructOrClass_Test<GenericClass<int>, GenericClass<int>>();
        }
        
        [Test]
    	public void Restore_GenericStructAsInterface_GenericStructAsInterface()
        {
    		Restore_StructOrClass_Test<GenericStruct<long>, IGenericInterface<long>>();
        }

        [Test]
        public void Restore_GenericClassAsInterface_GenericGenericAsInterface()
        {
        	Restore_StructOrClass_Test<GenericClass<long>, IGenericInterface<long>>();
        }
        
        private void Restore_StructOrClass_Test<TF, TT>() where TF : TT, INonGenericInterface, new() where TT : INonGenericInterface
    	{
        	var instance = new TF();
        	instance.Value = 100;
        	Restore_Object_Test<TT>(() => (TT)instance, v => v.Value);
    	}
	}
}