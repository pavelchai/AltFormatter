# AltFormatter

Objects serialization library into ZIP64 file (with XML + plain text).

Target: NET Framework 4.5+

1. Storer: ZIP64.
2. Information: XML files.
3. Data: files with plain text.

# Built-in support types

1. Primitive types: `bool`, `byte`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`, `float`, `double`, `decimal`, `char`, `string`, `DateTime`, `TimeSpan` and `Complex`.
2. Enums.
3. Types that have `FormattableAttribute` (class / struct). 
The types should have parameterless constructors or static factory methods (that have `FactoryAttribute`). 
Serializing/Deserializing fields and properties should have `KeyAttribute`.
Serialization/deserialization processes may be managed with the methods that may be implemented by using `IFormatter` interface.
4. Multidimensional dimensional arrays of (1-6) types (rank of array may be from 1 to 32).
5. Collections of (1-6) types (`List<>`, `LinkedList<>`, `HashSet<>`, `SortedSet<>`, `Queue<>`, `Stack<>`, `ConcurrentQueue<>`, `ConcurrentStack<>` and `ConcurrentBag{T}`).
6. Dictionaries of (1-6) types (`Dictionary<,>`, `SortedDictionary<,>`, `SortedList<,>` and `ConcurrentDictionary<,>`).

# Supported functionality
1. Formatter supports conversion between primitive types (each other), collection / dictionary types (each other) if the conversion is allowable.
2. Formatter supports conversion between types (3). The combination of the keys of the types should be equals, but names of these types may be different.
3. Formatter supports curcular references.

# Advantages
1. Serialized data may be opened, read and changed by user with using default tools.
2. Detects invalid zip file or data of the files inside zip storage (CRC-32 algorithm).
3. Can enable or disable compression (Store only / mixed Deflate + Store).
4. Supports conversion between primitive types (each other), collection / dictionary types (each other) if the conversion is allowable.
5. Supports conversion between types (3). The combination of the keys of the types should be equals, but names of these types may be different.
6. Supports curcular references.
7. May be modified. Storer, primitive types, type of output information file or file with data may be changed (see `AltFormatterBinaryText` in examples).  

# Disadvantages

1. Performance of the solution is near of the XMLSerializer, SOAPFormatter (see in examples). If you want more performance, you should select another serializer.

# How to use

1. Add `FormattableAttribute` to the assembly with types

2. Add `FormattableAttribute` to the types and `KeyAttribute` to the fields / properties, and `FactoryAttribute` for the factory methods (if required)

```csharp
[Formattable("Version 1")]
public class Version1
{
    [Key("Property 1")]
    public virtual int Property1 { get; set; }
    
    [Key("Property 2")]
    public virtual int[] Property2 { get; set; }
}

[Formattable("Version 2")]
public class Version2
{
    [Key("Property 1")]
    public virtual int Property1 { get; set; }
    
    [Key("Property 2")]
    public virtual int[] Property2 { get; set; }
    
    private Version2()
    {
    }
    
    [Factory]
    public static Version2 Create()
    {
        return new Version2();
    }
}
```

3. Use formatter

```csharp
IFormatter formatter = new AltFormatterZipXmlText(true, assemblies);

Version1 version1 = new Version1();
version1.Property1 = 20;
version1.Property2 = new int[] { 0, 1, 2, 3, 11, 12, 13 };

byte[] data = formatter.Serialize<Version1>(version1);
Version1 deserialized = formatter.Deserialize<Version1>(data);
```

## Output example (version1.zip)

File stucture:

```
information.xml
Property 2/information.xml
Property 2/collection.data
```

information.xml

```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
	<class name="Version 1">
		<Property 1>20</Property 1>
	</class>
</root>
```

Property 2/information.xml

```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
	<class name="array">
		<primitive>true</primitive>
		<count>7</count>
	</class>
</root>
```

Property 2/collection.data

```
0
1
2
3
11
12
13
```
