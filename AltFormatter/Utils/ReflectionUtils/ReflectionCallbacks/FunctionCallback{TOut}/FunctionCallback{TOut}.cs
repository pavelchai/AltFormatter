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
    internal sealed class FunctionCallback<TOut> : AbstractReflectionCallback, IFunctionCallback<TOut>
    {
        /// <summary>
        /// Activating function for the <see cref="System.Reflection.Emit.DynamicMethod"></see> callback.
        /// </summary>
        private readonly Func<Func<TOut>> dmActivator;

        /// <summary>
        /// Callback (<see cref="System.Reflection.Emit.DynamicMethod"></see> version).
        /// </summary>
        private Func<TOut> dmCallback;

        /// <summary>
        /// Callback (reflection version).
        /// </summary>
        private readonly Func<TOut> reflectionCallback;

        /// <summary>
        /// Creates a new <see cref="Func{TOut}"></see> callback.
        /// </summary>
        /// <param name="name"> Name of the callback. </param>
        /// <param name="reflectionCallback"> Callback (reflection version). </param>
        public FunctionCallback(string name, Func<TOut> reflectionCallback) : base(name)
        {
            this.reflectionCallback = reflectionCallback;
        }

        /// <summary>
        /// Creates a new <see cref="Func{TOut}"></see> callback.
        /// </summary>
        /// <param name="name"> Name of the callback. </param>
        /// <param name="dmActivator"> Activating function for the <see cref="System.Reflection.Emit.DynamicMethod"></see> callback. </param>
        public FunctionCallback(string name, Func<Func<TOut>> dmActivator) : base(name)
        {
            this.dmActivator = dmActivator;
            this.reflectionCallback = () =>
            {
                throw new ReflectionCallbackIsNotAllowableException(this.Name);
            };
        }

        /// <summary>
        /// Creates a new <see cref="Func{TOut}"></see> callback.
        /// </summary>
        /// <param name="name"> Name of the callback. </param>
        /// <param name="dmActivator"> Activating function for the <see cref="System.Reflection.Emit.DynamicMethod"></see> callback. </param>
        /// <param name="reflectionCallback"> Callback (reflection version). </param>
        public FunctionCallback(string name, Func<Func<TOut>> dmActivator, Func<TOut> reflectionCallback) : base(name)
        {
            this.dmActivator = dmActivator;
            this.reflectionCallback = reflectionCallback;
        }

        /// <summary>
        /// Invokes the <see cref="Func{TOut}"></see>.
        /// </summary>
        /// <returns> Return value from the <see cref="Func{TOut}"></see>. </returns>
        /// <exception cref="Exception">
        /// The exception that will be thrown if exception 
        /// in <see cref="Func{TOut}"></see> can't be handled in the callback.
        /// </exception>
        public TOut Invoke()
        {
            if (this.dmActivator == null)
            {
                return this.reflectionCallback();
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
                return this.dmCallback();
            }
            catch (Exception ex)
            {
                this.RaiseDMInvokeFailedMessage(ex);
                return this.reflectionCallback();
            }
        }
    }
}