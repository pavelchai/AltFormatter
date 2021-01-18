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
    internal sealed class ActionCallback<TIn1, TIn2> : AbstractReflectionCallback, IActionCallback<TIn1, TIn2>
    {
        /// <summary>
        /// Activating function for the <see cref="System.Reflection.Emit.DynamicMethod"></see> callback.
        /// </summary>
        private readonly Func<Action<TIn1, TIn2>> dmActivator;

        /// <summary>
        /// Callback (<see cref="System.Reflection.Emit.DynamicMethod"></see> version).
        /// </summary>
        private Action<TIn1, TIn2> dmCallback;

        /// <summary>
        /// Callback (reflection version).
        /// </summary>
        private readonly Action<TIn1, TIn2> reflectionCallback;

        /// <summary>
        /// Creates a new <see cref="Action{TIn1, TIn2}"></see> callback.
        /// </summary>
        /// <param name="name"> Name of the callback. </param>
        /// <param name="reflectionCallback"> Callback (reflection version). </param>
        public ActionCallback(string name, Action<TIn1, TIn2> reflectionCallback) : base(name)
        {
            this.reflectionCallback = reflectionCallback;
        }

        /// <summary>
        /// Creates a new <see cref="Action{TIn1, TIn2}"></see> callback.
        /// </summary>
        /// <param name="name"> Name of the callback. </param>
        /// <param name="dmActivator"> Activating function for the <see cref="System.Reflection.Emit.DynamicMethod"></see> callback. </param>
        public ActionCallback(string name, Func<Action<TIn1, TIn2>> dmActivator) : base(name)
        {
            this.dmActivator = dmActivator;
            this.reflectionCallback = (v1, v2) =>
            {
                throw new ReflectionCallbackIsNotAllowableException(this.Name);
            };
        }

        /// <summary>
        /// Creates a new <see cref="Action{TIn1, TIn2}"></see> callback.
        /// </summary>
        /// <param name="name"> Name of the callback. </param>
        /// <param name="dmActivator"> Activating function for the <see cref="System.Reflection.Emit.DynamicMethod"></see> callback. </param>
        /// <param name="reflectionCallback"> Callback (reflection version). </param>
        public ActionCallback(string name, Func<Action<TIn1, TIn2>> dmActivator, Action<TIn1, TIn2> reflectionCallback) : base(name)
        {
            this.dmActivator = dmActivator;
            this.reflectionCallback = reflectionCallback;
        }

        /// <summary>
        /// Invokes the <see cref="Action{TIn1, TIn2}"></see>.
        /// </summary>
        /// <param name="arg1"> The first argument. </param>
        /// <param name="arg2"> The second argument. </param>
        /// <exception cref="Exception">
        /// The exception that will be thrown if exception 
        /// in <see cref="Action{TIn1, TIn2}"></see> can't be handled in the callback.
        /// </exception>
        public void Invoke(TIn1 arg1, TIn2 arg2)
        {
            if (this.dmActivator == null)
            {
                this.reflectionCallback(arg1, arg2);
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
                this.dmCallback(arg1, arg2);
            }
            catch (Exception ex)
            {
                this.RaiseDMInvokeFailedMessage(ex, arg1, arg2);
            }
        }
    }
}