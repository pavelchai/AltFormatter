/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AltFormatter.Formatter
{
    /// <summary>
    /// Represents a type for the generic/non-generic class or 
    /// struct that have <see cref="FormattableAttribute"></see>.
    /// </summary>
    internal sealed class FormattableType
    {
        /// <summary>
        /// Host type. May be generic type definition/non-generic class or struct.
        /// </summary>
        private readonly Type hostType;

        /// <summary>
        /// Indicates whether host type is generic type definition.
        /// </summary>
        private readonly bool isGenericTypeDefinition;

        /// <summary>
        /// Related types.
        /// </summary>
        private readonly IDictionary<Type, bool> relatedTypes = new Dictionary<Type, bool>(1);

        /// <summary>
        /// Created types (declared type/created type).
        /// </summary>
        private readonly IDictionary<Type, Type> createdTypes = new Dictionary<Type, Type>(1);

        /// <summary>
        /// Factory methods for the created types (key - declared type).
        /// </summary>
        private readonly IDictionary<Type, IFunctionCallback<object>> factoryMethods = new Dictionary<Type, IFunctionCallback<object>>(1);

        /// <summary>
        /// Values for the created types (key - declared type).
        /// </summary>
        private readonly IDictionary<Type, FormattableValue[]> values = new Dictionary<Type, FormattableValue[]>(1);

        /// <summary>
        /// Creates a new type for the generic/non-generic class or 
        /// struct that have <seealso cref="FormattableAttribute"></seealso>.
        /// </summary>
        /// <param name="type"> Type. May be generic type definition/non-generic class or struct. </param>
        /// <param name="attribute"> Attribute. </param>
        public FormattableType(Type type, FormattableAttribute attribute)
        {
            this.hostType = type;
            this.isGenericTypeDefinition = type.IsGenericTypeDefinition;
            this.Name = attribute.Name;
        }

        /// <summary>
        /// Name of the type.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Determines whether specified type is related with the current type.
        /// </summary>
        /// <param name="type"> The type (class/struct/interface). </param>
        /// <param name="sameType"> Indicates whether type should be same (not derived). </param>
        /// <returns> True if type is related with the current type, otherwise false. </returns>
        public bool IsRelatedType(Type type, bool sameType)
        {
            if (this.isGenericTypeDefinition && !type.IsGenericType)
            {
                return false;
            }

            Type declaredType = (this.isGenericTypeDefinition) ? type.GetGenericTypeDefinition() : type;

            if (sameType)
            {
                return declaredType == this.hostType;
            }

            bool related = false;
            if (this.relatedTypes.TryGetValue(type, out related))
            {
                return related;
            }

            related = declaredType.IsBaseTypeOrEquals(this.hostType);
            this.relatedTypes.Add(type, related);
            return related;
        }

        /// <summary>
        /// Creates a new instance of the type.
        /// </summary>
        /// <param name="type"> The type (class/struct/interface) that is associated with the current type. </param>
        /// <returns> New instance. </returns>
        public object CreateInstance(Type type)
        {
            this.BuildType(type);
            return this.factoryMethods[type].Invoke();
        }

        /// <summary>
        /// Gets the values of type that are sorted in ascending order.
        /// </summary>
        /// <param name="type"> The type (class/struct/interface) that is associated with the current type. </param>
        /// <returns> Values of type that are sorted in ascending order. </returns>
        public FormattableValue[] GetValues(Type type)
        {
            this.BuildType(type);
            return this.values[type];
        }

        /// <summary>
        /// Raised before serialization.
        /// </summary>
        /// <param name="graph"> Graph of the objects. </param>
        public void OnSerializing(object graph)
        {
            IFormattable formattable = graph as IFormattable;
            if (formattable != null)
            {
                formattable.OnSerializing();
            }
        }

        /// <summary>
        /// Raised after serialization.
        /// </summary>
        /// <param name="graph"> Graph of the objects. </param>
        public void OnSerialized(object graph)
        {
            IFormattable formattable = graph as IFormattable;
            if (formattable != null)
            {
                formattable.OnSerialized();
            }
        }

        /// <summary>
        /// Raised before deserialization.
        /// </summary>
        /// <param name="graph"> Graph of the objects. </param>
        public void OnDeserializing(object graph)
        {
            IFormattable formattable = graph as IFormattable;
            if (formattable != null)
            {
                formattable.OnDeserializing();
            }
        }

        /// <summary>
        /// Raised after deserialization.
        /// </summary>
        /// <param name="graph"> Graph of the objects. </param>
        public void OnDeserialized(object graph)
        {
            IFormattable formattable = graph as IFormattable;
            if (formattable != null)
            {
                formattable.OnDeserialized();
            }
        }

        /// <summary>
        /// Raised after <see cref="OnDeserialized"></see> method and allows change deserialized object by the another (substitutable) object.
        /// </summary>
        /// <param name="graph"> Graph of the objects. </param>
        /// <returns> Deserialized object (this) if substitution isn't necessary, otherwise substitutable object. </returns>
        public object Substitution(object graph)
        {
            IFormattable formattable = graph as IFormattable;
            if (formattable != null)
            {
                return formattable.Substitution();
            }
            else
            {
                return graph;
            }
        }

        /// <summary>
        /// Builds a target type that based on declared type that is associated with the current type.
        /// </summary>
        /// <param name="declaredType"> Declared type. </param>
        private void BuildType(Type declaredType)
        {
            Type createdType = null;
            if (!this.createdTypes.TryGetValue(declaredType, out createdType))
            {
                // Creates a new type

                createdType = (this.isGenericTypeDefinition) ? this.hostType.MakeGenericType(declaredType.GetGenericArguments()) : this.hostType;

                // Creates an activator for the created type

                IFunctionCallback<object> factoryMethod = ReflectionUtils.CreateInstance(createdType);

                // Creates the callbacks for the get/set accessors of the fields

                KeyAttribute keyAttribute;
                FormattableValue value;
                LinkedList<FormattableValue> loadedValues = new LinkedList<FormattableValue>();
                IFunctionCallback<object, object> getAccessor = null;
                IActionCallback<object, object> setAccessor = null;

                FieldInfo[] fields = createdType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                int fieldsLength = fields.Length;
                FieldInfo field = null;

                for (int i = 0; i < fieldsLength; i++)
                {
                    field = fields[i];
                    keyAttribute = field.GetAttribute<KeyAttribute>(true);

                    if (keyAttribute != null)
                    {
                        getAccessor = ReflectionUtils.CreateGetAccessor(createdType, field);
                        setAccessor = ReflectionUtils.CreateSetAccessor(createdType, field);

                        value = new FormattableValue(
                            keyAttribute.Name,
                            keyAttribute.Optional,
                            keyAttribute.Order,
                            field.FieldType,
                            getAccessor,
                            setAccessor
                        );

                        loadedValues.AddLast(value);
                    }
                }

                // Action that used for getting the key attribute and property info of the overridden properties

                PropertyInfo property = null;
                MethodInfo propertyGetMethod = null;
                Type baseType = null;

                Action<Type, string> getPropertyKeyAttributeFromBase = null;
                getPropertyKeyAttributeFromBase = (t, n) =>
                {
                    baseType = t.BaseType;
                    if (baseType == null)
                    {
                        return;
                    }

                    property = baseType.GetProperty(n, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                    if (property != null)
                    {
                        keyAttribute = property.GetAttribute<KeyAttribute>(true);
                        if (keyAttribute == null)
                        {
                            propertyGetMethod = property.GetGetMethod(true);
                            if (propertyGetMethod == null || propertyGetMethod.GetBaseDefinition() != propertyGetMethod)
                            {
                                // Property is overridden - tests the properties of the base type
                                getPropertyKeyAttributeFromBase(baseType, n);
                            }
                        }
                    }
                    else
                    {
                        // Required property isn't exists in the type - tests the properties of the base type
                        getPropertyKeyAttributeFromBase(baseType, n);
                    }
                };

                // Creates the callbacks for the get/set accessors of the properties

                PropertyInfo[] properties = createdType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                int propertiesLength = properties.Length;

                for (int i = 0; i < propertiesLength; i++)
                {
                    property = properties[i];
                    keyAttribute = property.GetAttribute<KeyAttribute>(true);

                    if (keyAttribute == null)
                    {
                        propertyGetMethod = property.GetGetMethod(true);
                        if (propertyGetMethod == null || propertyGetMethod.GetBaseDefinition() != propertyGetMethod)
                        {
                            // Property is overridden - tests the properties of the base type

                            getPropertyKeyAttributeFromBase(createdType, property.Name);
                        }
                    }

                    if (keyAttribute == null || property.GetIndexParameters().Length != 0)
                    {
                        continue;
                    }

                    getAccessor = ReflectionUtils.CreateGetAccessor(createdType, property);
                    setAccessor = ReflectionUtils.CreateSetAccessor(createdType, property);

                    value = new FormattableValue(
                        keyAttribute.Name,
                        keyAttribute.Optional,
                        keyAttribute.Order,
                        property.PropertyType,
                        getAccessor,
                        setAccessor
                    );

                    loadedValues.AddLast(value);
                }

                // Sorts the values by order

                FormattableValue[] sortedValues = loadedValues.ToArray();
                SortUtils.QuickSort(true, sortedValues);

                // Saves the factory method, sorted values and created type

                this.factoryMethods.Add(declaredType, factoryMethod);
                this.values.Add(declaredType, sortedValues);
                this.createdTypes.Add(declaredType, createdType);
            }
        }
    }
}