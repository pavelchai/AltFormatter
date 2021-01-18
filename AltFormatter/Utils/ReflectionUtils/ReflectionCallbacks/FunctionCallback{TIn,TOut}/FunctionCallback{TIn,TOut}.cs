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
    internal sealed class FunctionCallback<TIn, TOut> : AbstractReflectionCallback, IFunctionCallback<TIn, TOut>
    {
        /// <summary>
        /// Activating function for the <see cref="System.Reflection.Emit.DynamicMethod"></see> callback.
        /// </summary>
        private readonly Func<Func<TIn, TOut>> dmActivator;

        /// <summary>
        /// Callback (<see cref="System.Reflection.Emit.DynamicMethod"></see> version).
        /// </summary>
        private Func<TIn, TOut> dmCallback;

        /// <summary>
        /// Callback (reflection version).
        /// </summary>
        private readonly Func<TIn, TOut> reflectionCallback;

        /// <summary>
        /// Creates a new <see cref="Func{TIn, Tout}"></see> callback.
        /// </summary>
        /// <param name="name"> Name of the callback. </param>
        /// <param name="reflectionCallback"> Callback (reflection version). </param>
        public FunctionCallback(string name, Func<TIn, TOut> reflectionCallback) : base(name)
        {
            this.reflectionCallback = reflectionCallback;
        }

        /// <summary>
        /// Creates a new <see cref="Func{TIn, Tout}"></see> callback.
        /// </summary>
        /// <param name="name"> Name of the callback. </param>
        /// <param name="dmActivator"> Activating function for the <see cref="System.Reflection.Emit.DynamicMethod"></see> callback. </param>
        public FunctionCallback(string name, Func<Func<TIn, TOut>> dmActivator) : base(name)
        {
            this.dmActivator = dmActivator;
            this.reflectionCallback = (v) =>
            {
                throw new ReflectionCallbackIsNotAllowableException(this.Name);
            };
        }

        /// <summary>
        /// Creates a new <see cref="Func{TIn, Tout}"></see> callback.
        /// </summary>
        /// <param name="name"> Name of the callback. </param>
        /// <param name="dmActivator"> Activating function for the <see cref="System.Reflection.Emit.DynamicMethod"></see> callback. </param>
        /// <param name="reflectionCallback"> Callback (reflection version). </param>
        public FunctionCallback(string name, Func<Func<TIn, TOut>> dmActivator, Func<TIn, TOut> reflectionCallback) : base(name)
        {
            this.dmActivator = dmActivator;
            this.reflectionCallback = reflectionCallback;
        }

        /// <summary>
        /// Invokes the <see cref="Func{TIn, TOut}"></see> with the specified argument.
        /// </summary>
        /// <param name="arg"> The argument. </param>
        /// <returns> Return value from the <see cref="Func{TIn, TOut}"></see>. </returns>
        /// <exception cref="Exception">
        /// The exception that will be thrown if exception 
        /// in <see cref="Func{TIn, TOut}"></see> can't be handled in the callback.
        /// </exception>
        public TOut Invoke(TIn arg)
        {
            if (this.dmActivator == null)
            {
                return this.reflectionCallback(arg);
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
                return this.dmCallback(arg);
            }
            catch (Exception ex)
            {
                this.RaiseDMInvokeFailedMessage(ex, arg);
                return this.reflectionCallback(arg);
            }
        }
    }
}