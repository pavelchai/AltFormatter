/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace AltFormatter.Formatter
{
    /// <summary>
    /// Represents a base class for the formatter.
    /// </summary>
    public abstract partial class AbstractFormatter : IFormatter
    {
        /// <summary>
        /// All loaded types.
        /// </summary>
        private FormattableType[] formattableTypes = new FormattableType[0];
        
        /// <summary>
        /// Indicates whether formatter is loading (0 - false, 1 - true).
        /// </summary>
        private int formatterIsLoading = 0;

        /// <summary>
        /// Represents a base class for the formatter.
        /// </summary>
        /// <param name="assemblies"> Array of the used assemblies. </param>
        protected AbstractFormatter(params Assembly[] assemblies)
        {
            this.LoadAssemblies(assemblies);
        }

        /// <summary>
        /// Creates a new write-only storer to the data.
        /// </summary>
        /// <returns> New storer. </returns>
        protected abstract IWriteOnlyDataStorer CreateWriteOnlyDataStorer();

        /// <summary>
        /// Creates a new read-only storer from the data.
        /// </summary>
        /// <param name="data"> The data. </param>
        /// <returns> New storer. </returns>
        protected abstract IReadOnlyDataStorer CreateReadOnlyDataStorer(byte[] data);

        /// <summary>
        /// Loads the assemblies.
        /// </summary>
        /// <param name="assemblies"> Array of the used assemblies. </param>
        private void LoadAssemblies(params Assembly[] assemblies)
        {
        	Interlocked.Exchange(ref this.formatterIsLoading, 1);
        	
        	Assembly assembly;
            Type type;
            Type[] types;
            FormattableAttribute assemblyAttribute;
            FormattableAttribute typeAttribute;
            FormattableType formattableType;
            LinkedList<FormattableType> loadedTypes = new LinkedList<FormattableType>();

            int assembliesLength = assemblies.Length;
            int typesLength = 0;
            
            for (int i = 0; i < assembliesLength; i++)
            {
                assembly = assemblies[i];
                
                if (assembly == null)
                {
                    continue;
                }

                assemblyAttribute = assembly.GetAttribute<FormattableAttribute>();
                
                if (assemblyAttribute == null)
                {
                	continue;
                }

                types = assembly.GetTypes();
                typesLength = types.Length;
                
                for (int k = 0; k < typesLength; k++)
                {
                    type = types[k];
                    typeAttribute = type.GetAttribute<FormattableAttribute>(false);
                    
                    if (typeAttribute != null)
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                        	throw new AbstractTypeException(type);
                        }

                        formattableType = new FormattableType(type, typeAttribute);
                        loadedTypes.AddLast(formattableType);
                    }
                }
            }

            this.formattableTypes = loadedTypes.ToArray();

        	Interlocked.Exchange(ref this.formatterIsLoading, 0);
        }
    }
}