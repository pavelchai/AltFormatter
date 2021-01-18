/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System;

namespace AltFormatter.Utils
{
    /// <summary>
    /// Represents a base class for the reflection callbacks.
    /// </summary>
    internal abstract class AbstractReflectionCallback
    {
        /// <summary>
        /// Represents a base class for the reflection callbacks.
        /// </summary>
        /// <param name="name"> Name of the <see cref="AbstractReflectionCallback"></see>. </param>
        protected AbstractReflectionCallback(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Name of the type.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Raises the exception when creation of the dynamic method has been failed.
        /// </summary>
        /// <param name="exception"> The inner exception that is thrown when creation of the dynamic method has been failed. </param>
        /// <exception cref="Exception">
        /// New exception that is thrown when specified exception has been received.
        /// </exception>
        protected void RaiseCreateDMFailedMessage(Exception exception)
        {
            throw new Exception(string.Format(
                "Creating of the dynamic method for <{0}> has been failed. Reflection will be used in future. Full message: {1}.",
                this.Name,
                exception.GetFullMessage())
            );
        }

        /// <summary>
        /// Raises the exception when invoking of the dynamic method has been failed (zero args).
        /// </summary>
        /// <param name="exception"> The inner exception that is thrown when creation of the dynamic method has been failed. </param>
        /// <exception cref="Exception">
        /// New exception that is thrown when specified exception has been received.
        /// </exception>
        protected void RaiseDMInvokeFailedMessage(Exception exception)
        {
            throw new Exception(string.Format(
                "Invoking of the dynamic method for <{0}> has been failed. Reflection will be used in future. Full message: {1}.",
                this.Name,
                exception.GetFullMessage())
            );
        }

        /// <summary>
        /// Raises the exception when invoking of the dynamic method has been failed (one arg).
        /// </summary>
        /// <param name="exception"> The inner exception that is thrown when creation of the dynamic method has been failed. </param>
        /// <param name="arg"> Argument. </param>
        /// <exception cref="Exception">
        /// New exception that is thrown when specified exception has been received.
        /// </exception>
        protected void RaiseDMInvokeFailedMessage(Exception exception, object arg)
        {
            throw new Exception(string.Format(
                "Invoking of the dynamic method for <{0}> has been failed. Reflection will be used in future. Argument: {1}. Full message: {2}.",
                this.Name,
                (!Object.Equals(arg, null)) ? arg : "null",
                exception.GetFullMessage())
            );
        }

        /// <summary>
        /// Raises the exception when invoking of the dynamic method has been failed (two arg).
        /// </summary>
        /// <param name="exception"> The inner exception that is thrown when creation of the dynamic method has been failed. </param>
        /// <param name="arg1"> First argument. </param>
        /// <param name="arg2"> Second argument. </param>
        /// <exception cref="Exception">
        /// New exception that is thrown when specified exception has been received.
        /// </exception>
        protected void RaiseDMInvokeFailedMessage(Exception exception, object arg1, object arg2)
        {
            throw new Exception(string.Format(
                "Invoking of the dynamic method for <{0}> has been failed. Reflection will be used in future. Argument #1: {1}. Argument #2: {2}. Full message: {3}.",
                this.Name,
                (!Object.Equals(arg1, null)) ? arg1 : "null",
                (!Object.Equals(arg1, null)) ? arg2 : "null",
                exception.GetFullMessage())
            );
        }
    }
}