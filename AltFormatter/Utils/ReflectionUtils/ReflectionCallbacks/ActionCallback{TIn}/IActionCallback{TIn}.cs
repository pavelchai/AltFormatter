/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System;

namespace AltFormatter.Utils
{
    /// <summary>
    /// Represents an <see cref="Action{TIn}"></see> callback.
    /// </summary>
    public interface IActionCallback<in TIn>
    {
        /// <summary>
        /// Invokes the <see cref="Action{TIn}"></see> with the specified argument.
        /// </summary>
        /// <param name="arg"> The argument. </param>
        /// <exception cref="Exception">
        /// The exception that will be thrown if exception 
        /// in <see cref="Action{TIn}"></see> can't be handled in the callback.
        /// </exception>
        void Invoke(TIn arg);
    }
}