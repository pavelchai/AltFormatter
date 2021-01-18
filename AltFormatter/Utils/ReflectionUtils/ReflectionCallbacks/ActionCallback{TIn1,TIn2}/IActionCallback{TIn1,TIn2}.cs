/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System;

namespace AltFormatter.Utils
{
    /// <summary>
    /// Represents an <see cref="Action{TIn1, TIn2}"></see> callback.
    /// </summary>
    public interface IActionCallback<in TIn1, in TIn2>
    {
        /// <summary>
        /// Invokes the <see cref="Action{TIn1, TIn2}"></see>.
        /// </summary>
        /// <param name="arg1"> The first argument. </param>
        /// <param name="arg2"> The second argument. </param>
        /// <exception cref="Exception">
        /// The exception that will be thrown if exception 
        /// in <see cref="Action{TIn1, TIn2}"></see> can't be handled in the callback.
        /// </exception>
        void Invoke(TIn1 arg1, TIn2 arg2);
    }
}