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
    internal sealed class ActionCallback<TIn> : AbstractReflectionCallback, IActionCallback<TIn>
    {
        /// <summary>
        /// Activating function for the <see cref="System.Reflection.Emit.DynamicMethod"></see> callback.
        /// </summary>
        private readonly Func<Action<TIn>> dmActivator;

        /// <summary>
        /// Callback (<see cref="System.Reflection.Emit.DynamicMethod"></see> version).
        /// </summary>
        private Action<TIn> dmCallback;

        /// <summary>
        /// Callback (reflection version).
        /// </summary>
        private readonly Action<TIn> reflectionCallback;

        /// <summary>
        /// Creates a new <see cref="Action{TIn}"></see> callback.
        /// </summary>
        /// <param name="name"> Name of the callback. </param>
        /// <param name="reflectionCallback"> Callback (reflection version). </param>
        public ActionCallback(string name, Action<TIn> reflectionCallback) : base(name)
        {
            this.reflectionCallback = reflectionCallback;
        }

        /// <summary>
        /// Creates a new <see cref="Action{TIn}"></see> callback.
        /// </summary>
        /// <param name="name"> Name of the callback. </param>
        /// <param name="dmActivator"> Activating function for the <see cref="System.Reflection.Emit.DynamicMethod"></see> callback. </param>
        public ActionCallback(string name, Func<Action<TIn>> dmActivator) : base(name)
        {
            this.dmActivator = dmActivator;
            this.reflectionCallback = (v) =>
            {
                throw new ReflectionCallbackIsNotAllowableException(this.Name);
            };
        }

        /// <summary>
        /// Creates a new <see cref="Action{TIn}"></see> callback.
        /// </summary>
        /// <param name="name"> Name of the callback. </param>
        /// <param name="dmActivator"> Activating function for the <see cref="System.Reflection.Emit.DynamicMethod"></see> callback. </param>
        /// <param name="reflectionCallback"> Callback (reflection version). </param>
        public ActionCallback(string name, Func<Action<TIn>> dmActivator, Action<TIn> reflectionCallback) : base(name)
        {
            this.dmActivator = dmActivator;
            this.reflectionCallback = reflectionCallback;
        }

        /// <summary>
        /// Invokes the <see cref="Action{TIn}"></see> with the specified argument.
        /// </summary>
        /// <param name="arg"> The argument. </param>
        /// <exception cref="Exception">
        /// The exception that will be thrown if exception 
        /// in <see cref="Action{TIn}"></see> can't be handled in the callback.
        /// </exception>
        public void Invoke(TIn arg)
        {
            if (this.dmActivator == null)
            {
                this.reflectionCallback(arg);
                return;
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
                    return;
                }
            }

            try
            {
                this.dmCallback(arg);
            }
            catch (Exception ex)
            {
                this.RaiseDMInvokeFailedMessage(ex, arg);
                return;
            }
        }
    }
}