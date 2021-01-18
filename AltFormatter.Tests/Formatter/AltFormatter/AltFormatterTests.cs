/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Utils;
using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AltFormatter.Formatter
{
    public sealed partial class AltFormatterTests
    {   	
    	private static readonly Type[] CollectionTypeGTD = 
        {
        	typeof(List<>),
        	typeof(LinkedList<>),
        	typeof(HashSet<>),
        	typeof(SortedSet<>),
        	typeof(Stack<>),
        	typeof(Queue<>),
        	typeof(ConcurrentStack<>),
        	typeof(ConcurrentQueue<>),
        	typeof(ConcurrentBag<>),
        };
    	
    	private static readonly Type[] DictionaryTypeGTD = 
        {
        	typeof(Dictionary<,>),
        	typeof(SortedDictionary<,>),
        	typeof(ConcurrentDictionary<,>),
        	typeof(SortedList<,>),
        };
    	
    	private static IEnumerable<int> RankSource = new int[30].Select((v,i) => i + 1);
    	
    	private static readonly IFormatter formatter = new AltFormatterZipXmlText(
    		false,
    		null,
    		typeof(ReflectionUtils).Assembly,
    		typeof(AltFormatterTests).Assembly
    	);
    	
    	private static readonly Type formatterType = formatter.GetType();
    	
    	private static readonly MethodInfo serializeMethodGTD = formatterType.GetMethod("Serialize");
    	
    	private static readonly MethodInfo deserializeMethodGTD = formatterType.GetMethod("Deserialize");
    	
    	private T SerializeDeserialize<T>() where T : new()
    	{
    		return SerializeDeserialize<T>(new T());
    	}
    	
    	private T SerializeDeserialize<T>(T instance)
    	{
    		return SerializeDeserialize<T, T>(instance);
    	}
    	
    	private K SerializeDeserialize<T, K>(T instance)
    	{
    		byte[] data = formatter.Serialize<T>(instance);
    		
    		K deserialized = formatter.Deserialize<K>(data);
    		return deserialized;
    	}
        
        private object SerializeDeserialize(Type type, object value)
    	{
        	return SerializeDeserialize(type, type, value);
    	}
        
        private object SerializeDeserialize(Type inputType, Type outputType, object value)
    	{
        	MethodInfo serializeMethod = serializeMethodGTD.MakeGenericMethod(inputType);
        	MethodInfo deserializeMethod = deserializeMethodGTD.MakeGenericMethod(outputType);
        	
        	byte[] data = serializeMethod.Invoke(formatter, new object[] { value }) as byte[];
    		return deserializeMethod.Invoke(formatter, new object[] { data });
    	}
    	
        private void Restore_Object_Test<T>(Func<T, object> valueSelector) where T : new()
    	{
    		Restore_Object_Test(() => new T(), valueSelector);
    	}
    	
    	private void Restore_Object_Test<T>(Func<T> factory, Func<T, object> valueSelector)
    	{
    		var instance = factory();
    		var deserialized = SerializeDeserialize(instance);
    		Assert.AreEqual(valueSelector(instance), valueSelector(deserialized));
    	}
    	
        private void ConvertRestore_CollectionX_CollectionY_Test(bool fill, Type gtdX, Type gtdY)
        {
        	var xType = gtdX.MakeGenericType(typeof(int));
			var yType = gtdY.MakeGenericType(typeof(int));
			
			var collection = CreateCollection(gtdX, fill ? new int [] {1, 2, 3} : new int[0]);

    		
    		var deserialized = SerializeDeserialize(xType, yType, collection) as IEnumerable<int>;
    		
    		if(gtdX.IsBaseInterfaceOrEquals(typeof(ConcurrentBag<>)))
    		{
    			collection = collection.OrderBy(i => i);
    		}
    		
    		if(gtdY.IsBaseInterfaceOrEquals(typeof(ConcurrentBag<>)))
    		{
    			deserialized = deserialized.OrderBy(i => i);
    		}
    		
        	Assert.True(collection.SequenceEqual(deserialized));
        }
    	
    	private void ConvertRestore_DictionaryX_DictionaryY_Test(bool fill, Type gtdX, Type gtdY)
		{
			var xType = gtdX.MakeGenericType(typeof(int), typeof(int));
			var yType = gtdY.MakeGenericType(typeof(int), typeof(int));
			
			var dictionary = ReflectionUtils.CreateDictionary(gtdX, typeof(int), typeof(int), 0).Invoke(new int[]{0}) as IDictionary<int, int>;
    		if(fill)
			{
    			dictionary.Add(1, 10);
    			dictionary.Add(2, 20);
    			dictionary.Add(3, 30);
			}
    		
    		var deserialized = SerializeDeserialize(xType, yType, dictionary) as IDictionary<int, int>;
    		Assert.True(dictionary.SequenceEqual(deserialized));
		}
    	
    	private static IDictionary<K, V> CreateDictionary<K, V>(Type dictionaryGTD)
    	{
    		return ReflectionUtils.CreateDictionary(dictionaryGTD, typeof(K), typeof(V), 0).Invoke(new int[]{0}) as IDictionary<K, V>;
    	}
    	
    	private static IEnumerable<T> CreateCollection<T>(Type collectionGTD, params T[] values)
        {
        	IEnumerable<T> collection = ReflectionUtils.CreateEnumerable(collectionGTD, typeof(T), 0).Invoke(new int[]{0}) as IEnumerable<T>;
    		MethodInfo methodInfo = collection.GetType().GetMethods().FindOrDefault(m => m.Name == "Add" || m.Name == "AddLast" || m.Name == "Push" || m.Name == "Enqueue");
        	if(methodInfo != null)
        	{
        		values.Execute(v => methodInfo.Invoke(collection, new object[] {v}));
        	}
        	else
        	{
        		Assert.Fail();
        	}
    		return collection;
        }
    }
}