/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System;
using System.Text;

namespace AltFormatter.Utils
{
	/// <summary>
    /// Represents the utils for the <see cref="Exception"></see>.
    /// </summary>
    public static class ExceptionUtils
    {
        /// <summary>
        /// Returns a string that represents full exception message (with all inner <see cref="Exception"></see>s) from the <see cref="Exception"></see>.
        /// </summary>
        /// <param name="exception"> The exception. </param>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when exception is null.
        /// </exception>
        /// <returns> String that represents full exception message (with all inner <see cref="Exception"></see>s) from the <see cref="Exception"></see>.
        public static string GetFullMessage(this Exception exception)
        {
        	Validation.NotNull("Exception", exception);
        	
            StringBuilder stringBuilder = new StringBuilder();
            BuildFullMessage(exception, stringBuilder);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Builds the string that represents full exception message (with all inner <see cref="Exception"></see>s) from the <see cref="Exception"></see> with the <see cref="StringBuilder"></see>.
        /// </summary>
        /// <param name="exception"> The exception. </param>
        /// <param name="stringBuilder"> The builder. </param>
        private static void BuildFullMessage(Exception exception, StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine(exception.Message);

            if (exception.InnerException != null)
            {
                stringBuilder.AppendLine("-----------------");
                BuildFullMessage(exception.InnerException, stringBuilder);
            }
            else
            {
                stringBuilder.Append("Source: ");
                stringBuilder.AppendLine(exception.Source);
                stringBuilder.Append("Stack trace: ");
                stringBuilder.Append(exception.StackTrace);
            }
        }
    }
}