/*
 * Modified and refactored version of the file from NanoXML [based on https://www.codeproject.com/Tips/682245/NanoXML-Simple-and-fast-XML-parser, BrokenEvent, CPOL License]
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Localization;

namespace AltFormatter.Formatter
{
    /// <summary>
    /// The exception that is thrown when predefined entity is invalid.
    /// </summary>
    internal sealed class InvalidPredefinedEntityException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when predefined entity is invalid.
        /// </summary>
        /// <param name="index"> Index. </param>
        public InvalidPredefinedEntityException(int index) : base("Invalid predefined entity at index <{0}>.", index) { }
    }

    /// <summary>
    /// The exception that is thrown when token after attribute is unexpected.
    /// </summary>
    internal sealed class UnexpectedTokenAfterAttributeException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when token after attribute is unexpected.
        /// </summary>
        /// <param name="attributeName"> Name of the attribute. </param>
        public UnexpectedTokenAfterAttributeException(string attributeName) : base("Unexpected token after <{0}> attribute.", attributeName) { }
    }

    /// <summary>
    /// The exception that is thrown when token isn't equals less than.
    /// </summary>
    internal sealed class UnexpectedTokenInsteadLessThanException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when token isn't equals less than.
        /// </summary>
        /// <param name="token"> Token. </param>
        public UnexpectedTokenInsteadLessThanException(char token) : base("Unexpected token <{0}>. Expected token is \"<\".", token) { }
    }

    /// <summary>
    /// The exception that is thrown when ending on the tag is invalid.
    /// </summary>
    internal sealed class InvalidEndingOnTagException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when ending on the tag is invalid.
        /// </summary>
        /// <param name="tagName"> Name of the tag. </param>
        /// <param name="expectedToken"> Expected token. </param>
        public InvalidEndingOnTagException(string tagName, char expectedToken) : base("Invalid ending on tag <{0}>. Expected token is \"{1}\".", tagName, expectedToken) { }
    }

    /// <summary>
    /// The exception that is thrown when start and end name of the tag are mismatch.
    /// </summary>
    internal sealed class MismatchStartEndTagNameException : LocalizedException
    {
        /// <summary>
        /// The exception that is thrown when start and end name of the tag are mismatch.
        /// </summary>
        /// <param name="startTagName"> Start name of the tag. </param>
        /// <param name="endTagName"> End name of the tag. </param>
        public MismatchStartEndTagNameException(string startTagName, string endTagName) : base("Start and end name of the tag are mismatch: <{0}> and <{1}>.", startTagName, endTagName) { }
    }
}