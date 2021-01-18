/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System;

namespace AltFormatter.Utils
{
    /// <summary>
    /// Represents a <see cref="Func{TIn, TOut}"></see> callback.
    /// </summary>
    public interface IFunctionCallback<in TIn, out TOut>
    {
        /// <summary>
        /// Invokes the <see cref="Func{TIn, TOut}"></see> with the specified argument.
        /// </summary>
        /// <param name="arg"> The argument. </param>
        /// <returns> Return value from the <see cref="Func{TIn, TOut}"></see>. </returns>
        /// <exception cref="Exception">
        /// The exception that will be thrown if exception 
        /// in <see cref="Func{TIn, TOut}"></see> can't be handled in the callback.
        /// </exception>
        TOut Invoke(TIn arg);
    }
}