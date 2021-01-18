/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System;

namespace AltFormatter.Utils
{
    /// <summary>
    /// Represents a <see cref="Func{TIn1, TIn2, TOut}"></see> callback.
    /// </summary>
    public interface IFunctionCallback<in TIn1, in TIn2, out TOut>
    {
        /// <summary>
        /// Invokes the <see cref="Func{TIn1, TIn2, TOut}"></see> with the specified arguments.
        /// </summary>
        /// <param name="arg1"> The first argument. </param>
        /// <param name="arg2"> The second argument. </param>
        /// <returns> Return value from the <see cref="Func{TIn1, TIn2, TOut}"></see>. </returns>
        /// <exception cref="Exception">
        /// The exception that will be thrown if exception 
        /// in <see cref="Func{TIn1, TIn2, TOut}"></see> can't be handled in the callback.
        /// </exception>
        TOut Invoke(TIn1 arg1, TIn2 arg2);
    }
}