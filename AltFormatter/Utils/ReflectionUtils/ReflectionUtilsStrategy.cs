/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */

namespace AltFormatter.Utils
{
    /// <summary>
    /// Represents a strategy for the <see cref="ReflectionUtils"></see>.
    /// </summary>
    internal enum ReflectionsUtilsStrategy
    {
        /// <summary>
        /// Dynamic methods will be used for the callbacks if they are supported,
        /// otherwise reflection will be used
        /// </summary>
        Auto = 0,

        /// <summary>
        /// Only reflection should be used for the callbacks
        /// </summary>
        UseOnlyReflection = 1,

        /// <summary>
        /// Only dynamic methods should be used for the callbacks
        /// </summary>
        UseOnlyDynamicMethods = 2
    }
}