/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System;

namespace AltFormatter.Utils
{
    /// <summary>
    /// Represents a <see cref="Func{TOut}"></see> callback.
    /// </summary>
    public interface IFunctionCallback<out TOut>
    {
        /// <summary>
        /// Invokes the <see cref="Func{TOut}"></see>.
        /// </summary>
        /// <returns> Return value from the <see cref="Func{TOut}"></see>. </returns>
        /// <exception cref="Exception">
        /// The exception that will be thrown if exception 
        /// in <see cref="Func{TOut}"></see> can't be handled in the callback.
        /// </exception>
        TOut Invoke();
    }
}