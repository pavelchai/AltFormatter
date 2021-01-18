/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System;

namespace AltFormatter.Utils
{
    public sealed partial class TypeUtilsTests
    {
    	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	    private sealed class InheritedAttribute : Attribute
	    {
	    }
	
	    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	    private sealed class NotInheritedAttribute : Attribute
	    {
	    }
	
	    [Inherited]
	    private class ClassA
	    {
	    }
	
	    [NotInherited]
	    private sealed class ClassB : ClassA
	    {
	    }
    }
}