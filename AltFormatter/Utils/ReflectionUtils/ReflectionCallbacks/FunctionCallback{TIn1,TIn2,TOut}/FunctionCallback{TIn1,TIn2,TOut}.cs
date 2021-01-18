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
    internal sealed class FunctionCallback<TIn1, TIn2, TOut> : AbstractReflectionCallback, IFunctionCallback<TIn1, TIn2, TOut>
    {
        /// <summary>
        /// Activating function for the <see cref="System.Reflection.Emit.DynamicMethod"></see> callback.
        /// </summary>
        private readonly Func<Func<TIn1, TIn2, TOut>> dmActivator;

        /// <summary>
        /// Callback (<see cref="System.Reflection.Emit.DynamicMethod"></see> version).
        /// </summary>
        private Func<TIn1, TIn2, TOut> dmCallback;

        /// <summary>
        /// Callback (reflection version).
        /// </summary>
        private readonly Func<TIn1, TIn2, TOut> reflectionCallback;

        /// <summary>
        /// Creates a new <see cref="Func{TIn1, TIn2, TOut}"></see> callback.
        /// </summary>
        /// <param name="name"> Name of the callback. </param>
        /// <param name="reflectionCallback"> Callback (reflection version). </param>
        public FunctionCallback(string name, Func<TIn1, TIn2, TOut> reflectionCallback) : base(name)
        {
            this.reflectionCallback = reflectionCallback;
        }

        /// <summary>
        /// Creates a new <see cref="Func{TIn1, TIn2, TOut}"></see> callback.
        /// </summary>
        /// <param name="name"> Name of the callback. </param>
        /// <param name="dmActivator"> Activating function for the <see cref="System.Reflection.Emit.DynamicMethod"></see> callback. </param>
        public FunctionCallback(string name, Func<Func<TIn1, TIn2, TOut>> dmActivator) : base(name)
        {
            this.dmActivator = dmActivator;
            this.reflectionCallback = (v1, v2) =>
            {
                throw new ReflectionCallbackIsNotAllowableException(this.Name);
            };
        }

        /// <summary>
        /// Creates a new <see cref="Func{TIn1, TIn2, TOut}"></see> callback.
        /// </summary>
        /// <param name="name"> Name of the callback. </param>
        /// <param name="dmActivator"> Activating function for the <see cref="System.Reflection.Emit.DynamicMethod"></see> callback. </param>
        /// <param name="reflectionCallback"> Callback (reflection version). </param>
        public FunctionCallback(string name, Func<Func<TIn1, TIn2, TOut>> dmActivator, Func<TIn1, TIn2, TOut> reflectionCallback) : base(name)
        {
            this.dmActivator = dmActivator;
            this.reflectionCallback = reflectionCallback;
        }

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
        public TOut Invoke(TIn1 arg1, TIn2 arg2)
        {
            if (this.dmActivator == null)
            {
                return this.reflectionCallback(arg1, arg2);
            }

            if (this.dmCallback == null)
            {
                try
                {
                    this.dmCallback = this.dmActivator();
                }
                catch (Exception ex)
                {
                    this.RaiseCreateDMFailedMessage(ex);
                    return default(TOut);
                }
            }

            try
            {
                return this.dmCallback(arg1, arg2);
            }
            catch (Exception ex)
            {
                this.RaiseDMInvokeFailedMessage(ex, arg1, arg2);
                return this.reflectionCallback(arg1, arg2);
            }
        }
    }
}