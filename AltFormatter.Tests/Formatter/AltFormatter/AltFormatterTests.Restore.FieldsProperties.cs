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
    	public void Restore_BaseNotOverridenVirtualPropertyNotUpdated_InitialValue()
    	{
    		Restore_Object_Test<ClassWithFieldsAndProperties>(
    			s => s.BaseNotOverridenVirtualProperty);
    	}
    	
    	[Test]
    	public void Restore_BaseOverridenVirtualPropertyNotUpdated_InitialValue()
    	{
    		Restore_Object_Test<ClassWithFieldsAndProperties>(
    			s => s.BaseOverridenVirtualProperty);
    	}
    	
    	[Test]
    	public void Restore_ThisVirtualPropertyNotUpdated_InitialValue()
    	{
    		Restore_Object_Test<ClassWithFieldsAndProperties>(
    			s => s.ThisVirtualProperty);
    	}
    	
    	[Test]
    	public void Restore_NonReadOnlyFieldNotUpdated_InitialValue()
    	{
    		Restore_Object_Test<ClassWithFieldsAndProperties>(
    			s => s.NonReadOnlyField);
    	}
    	
    	[Test]
    	public void Restore_NonVirtualPropertyNotUpdated_InitialValue()
    	{
    		Restore_Object_Test<ClassWithFieldsAndProperties>(
    			s => s.NonVirtualProperty);
    	}
    	
    	[Test]
    	public void Restore_ReadOnlyFieldNotUpdated_InitialValue()
    	{
    		Restore_Object_Test<ClassWithFieldsAndProperties>(
    			s => s.ReadOnlyField);
    	}
    	
    	[Test]
    	public void Restore_BaseNotOverridenVirtualPropertyUpdated_NewValue()
    	{
    		Restore_Object_Test<ClassWithFieldsAndProperties>(
    			() => new ClassWithFieldsAndProperties() {BaseNotOverridenVirtualProperty = 3000},
    			s => s.BaseNotOverridenVirtualProperty);
    	}
    	
    	[Test]
    	public void Restore_BaseOverridenVirtualPropertyUpdated_NewValue()
    	{
    		Restore_Object_Test<ClassWithFieldsAndProperties>(
    			() => new ClassWithFieldsAndProperties() {BaseOverridenVirtualProperty = 4000},
    			s => s.BaseOverridenVirtualProperty);
    	}
    	
    	[Test]
    	public void Restore_ThisVirtualPropertyUpdated_NewValue()
    	{
    		Restore_Object_Test<ClassWithFieldsAndProperties>(
    			() => new ClassWithFieldsAndProperties() {ThisVirtualProperty = 2000},
    			s => s.ThisVirtualProperty);
    	}
    	
    	[Test]
    	public void Restore_NotReadOnlyFieldUpdated_NewValue()
    	{
    		Restore_Object_Test<ClassWithFieldsAndProperties>(
    			() => new ClassWithFieldsAndProperties() {NonReadOnlyField = 60.0},
    			s => s.NonReadOnlyField);
    	}
    	
    	[Test]
    	public void Restore_NonVirtualPropertyUpdated_NewValue()
    	{
    		Restore_Object_Test<ClassWithFieldsAndProperties>(
    			() => new ClassWithFieldsAndProperties() {NonVirtualProperty = "newString"},
    			s => s.NonVirtualProperty);
    	}
    	
    	[Test]
    	public void Restore_ReadOnlyFieldUpdated_NewValue()
    	{
    		Restore_Object_Test<ClassWithFieldsAndProperties>(
    			() => new ClassWithFieldsAndProperties(-10),
    			s => s.ReadOnlyField);
    	}
	}
}