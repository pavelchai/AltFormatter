/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace AltFormatter.Utils
{
    /// <summary>
    /// Represents the utils for the strings.
    /// </summary>
    /// <remarks> BSTR String (UTF-16) = Length (4 byte) + chars (2 byte per char) + /0 (2 byte). </remarks>
    public static class StringUtils
    {
        /// <summary>
        /// Gets the array of bytes (UTF-8 Encoding) from the UTF-16 string.
        /// </summary>
        /// <param name="inputString"> Input string. </param>
        /// <returns> Array of the bytes (UTF-8 Encoding). </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when input string is null.
        /// </exception>
        public static byte[] GetBytes(this string inputString)
        {
            Validation.NotNull("InputString", inputString);

            return Encoding.UTF8.GetBytes(inputString);
        }

        /// <summary>
        /// Gets the UTF-16 string from the <see cref="Array"></see> of bytes (UTF-8 Encoding).
        /// </summary>
        /// <param name="data"> Array of the bytes (UTF-8 Encoding). </param>
        /// <returns> UTF-16 string. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when data is null.
        /// </exception>
        public static string GetString(this byte[] data)
        {
            Validation.NotNull("Data", data);

            return Encoding.UTF8.GetString(data);
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"></see> that contains the substrings in this instance that are delimited by elements of a specified unicode character array.
        /// </summary>
        /// <param name="inputString"> Input string. </param>
        /// <param name="separators"> A character array that delimits the substrings in this string. </param>
        /// <returns> An <see cref="IEnumerable{T}"></see> whose elements contains the substrings in the string that are delimited by one or more characters in separators. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when input string or array of the separators is null.
        /// </exception>
        public static IEnumerable<string> Split(string inputString, params char[] separators)
        {
            Validation.NotNull("InputString", inputString);
            Validation.NotNull("Separators", separators);

            int stringLength = GetStringLength(inputString);
            if (stringLength == 0)
            {
                yield return string.Empty;
                yield break;
            }

            int separatorsLength = separators.Length;
            if (separatorsLength == 0)
            {
                yield return inputString;
                yield break;
            }

            int index = 0, start = 0;

            while (true)
            {
                string substring = Split(inputString, separators, separatorsLength, stringLength, ref start, ref index);
                if (substring != null)
                {
                    yield return substring;
                }
                else
                {
                    break;
                }
            }

        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"></see> that contains the lines from the string.
        /// </summary>
        /// <param name="inputString"> Input string. </param>
        /// <returns> An <see cref="IEnumerable{T}"></see> whose elements contains the lines from the input string. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when input string is null.
        /// </exception>
        /// <remarks> Supported separators of the lines are \n, \r and \r\n. </remarks>
        public static IEnumerable<string> SplitLines(string inputString)
        {
            Validation.NotNull("InputString", inputString);

            int stringLength = GetStringLength(inputString);
            if (stringLength == 0)
            {
                yield return string.Empty;
                yield break;
            }

            int index = 0, start = 0;

            while (true)
            {
                string substring = SplitLines(inputString, stringLength, ref start, ref index);
                if (substring != null)
                {
                    yield return substring;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Combines the array of the strings into one string.
        /// </summary>
        /// <param name="strings"> Array of the strings. </param>
        /// <returns> Combined string. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when array of the strings or any item in the array are null.
        /// </exception>
        public static unsafe string Combine(params string[] strings)
        {
            Validation.NotNull("Strings", strings);
            Validation.NotNullAll("Strings", strings);

            int arrayLength = strings.Length;
            if (arrayLength == 0)
            {
                // No items in the array - returns the empty string
                return string.Empty;
            }

            int outputLength = 0;
            int index = -1;
            int[] lengths = new int[arrayLength];

            fixed (int* lengthsPtr = lengths)
            {
                int* lPtr = lengthsPtr;
                while (++index < arrayLength)
                {
                    WriteStringLength(strings[index], lPtr);
                    outputLength += *lPtr;
                    lPtr++;
                }
            }

            if (outputLength == 0)
            {
                // Length of the string is equals zero - returns the empty string
                return string.Empty;
            }

            // Allocates a new string with length = outputLength
            string output = new string('\0', outputLength);
            index = -1;

            // Changes the output string
            fixed (char* dataPtr = output)
            {
                fixed (int* lengthsPtr = lengths)
                {
                    // Copies the data from the string to the output string

                    char* dPtr = dataPtr;
                    int* lPtr = lengthsPtr;

                    while (++index < arrayLength)
                    {
                        fixed (char* stringPtr = strings[index])
                        {
                            char* sPtr = stringPtr;
                            int sLength = *lPtr++;
                            while (sLength-- > 0)
                            {
                                *dPtr++ = *sPtr++;
                            }
                        }
                    }
                }
            }

            // Returns the new string
            return output;
        }

        /// <summary>
        /// Combines the array of the strings into one string.
        /// </summary>
        /// <param name="separator"> Separator between input strings. </param>
        /// <param name="strings"> Array of the strings. </param>
        /// <returns> Combined string. </returns>
        /// <exception cref="Localization.ValueNullException">
        /// The exception that is thrown when array of the strings or any item in the array are null.
        /// </exception>
        public static unsafe string Combine(char separator, params string[] strings)
        {
            Validation.NotNull("Strings", strings);
            Validation.NotNullAll("Strings", strings);

            int arrayLength = strings.Length;
            if (arrayLength == 0)
            {
                // No items in array - returns the empty string
                return string.Empty;
            }

            if (arrayLength == 1)
            {
                // Only one string is presented in the array - returns the string
                return strings[0];
            }

            int outputLength = 0;
            int index = -1;
            int[] lengths = new int[arrayLength];

            fixed (int* lengthsPtr = lengths)
            {
                int* lPtr = lengthsPtr;
                while (++index < arrayLength)
                {
                    WriteStringLength(strings[index], lPtr);
                    outputLength += *lPtr;
                    lPtr++;
                }
            }

            if (outputLength == 0)
            {
                // Length of the string (without separators) is equals zero - returns the string with the separators
                return new string(separator, arrayLength - 1);
            }

            // Allocates a new string with length = output length (without separators) + separators (length of the input array - 1)
            string output = new string('\0', outputLength + arrayLength - 1);
            index = 0;

            // Changes the output string
            fixed (char* dataPtr = output)
            {
                fixed (int* lengthsPtr = lengths)
                {
                    // Copies the data from the string to the output string

                    char* dPtr = dataPtr;
                    int* lPtr = lengthsPtr;

                    while (index < arrayLength)
                    {
                        fixed (char* stringPtr = strings[index])
                        {
                            char* sPtr = stringPtr;
                            int sLength = *lPtr++;
                            while (sLength-- > 0)
                            {
                                *dPtr++ = *sPtr++;
                            }
                        }
                        index++;

                        // Adds the separator (if current string isn't last string)
                        if (index != arrayLength)
                        {
                            *dPtr++ = separator;
                        }
                    }
                }
            }

            // Returns the new string
            return output;
        }

        /// <summary>
        /// Gets a length of the string.
        /// </summary>
        /// <param name="inputString"> Input string. </param>
        /// <returns> Length of the string. </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe int GetStringLength(string inputString)
        {
            fixed (char* strPtr = inputString)
            {
                // Gets a length of the string that is located at pointer - 4, 4 bytes (int)
                return *(((int*)strPtr) - 1);
            }
        }

        /// <summary>
        /// Splits an input string to the lines.
        /// </summary>
        /// <param name="inputString"> Input string. </param>
        /// <param name="separators"> Array of the separators. </param>
        /// <param name="separatorsLength"> Lenght of the array of the separators. </param>
        /// <param name="stringLength"> Length of the string. </param>
        /// <param name="start"> Start index. </param>
        /// <param name="index"> Current index. </param>
        /// <returns> String if string isn't the last string, otherwise null. </returns>
        private static unsafe string Split(string inputString, char[] separators, int separatorsLength, int stringLength, ref int start, ref int index)
        {
            if (index > stringLength)
            {
                // Last substring has been returned earlier
                return null;
            }

            // Gets an available length of the string (from the start offset)
            int avaliableLength = stringLength - start;

            if (index != stringLength)
            {
                char character;
                fixed (char* inputStringPtr = inputString, separatorPtr = separators)
                {
                    char* iPtr = &inputStringPtr[index];
                    while (avaliableLength-- > 0)
                    {
                        // Gets a current character
                        character = *iPtr;

                        char* sPtr = separatorPtr;
                        int sLength = separatorsLength;

                        while (sLength-- > 0)
                        {
                            if (*iPtr == *sPtr++)
                            {
                                // Line separator, skip 1 character
                                string substring = inputString.Substring(start, index - start);
                                start = ++index;
                                return substring;
                            }
                        }

                        // Separator hasn't been found - moves on next character
                        iPtr++;
                        index++;
                    }
                }
            }

            // Returns a last substring
            index++;
            return inputString.Substring(start, stringLength - start);
        }

        /// <summary>
        /// Splits an input string to the lines.
        /// </summary>
        /// <param name="inputString"> Input string. </param>
        /// <param name="stringLength"> Length of the string. </param>
        /// <param name="start"> Start index. </param>
        /// <param name="index"> Current index. </param>
        /// <returns> Line if line isn't the last line, otherwise null. </returns>
        private static unsafe string SplitLines(string inputString, int stringLength, ref int start, ref int index)
        {
            if (index > stringLength)
            {
                // Last substring has been returned earlier
                return null;
            }

            // Gets an available length of the string (from the start offset)
            int avaliableLength = stringLength - start;

            if (index != stringLength)
            {
                char characterC, characterN;

                fixed (char* inputStringPtr = inputString)
                {
                    char* iPtr = &inputStringPtr[index];
                    while (avaliableLength-- > 0)
                    {
                        // Gets a current character
                        characterC = *iPtr;

                        if (characterC == '\r')
                        {
                            characterN = *++iPtr;
                            if (characterN == '\n')
                            {
                                // Line separator \r\n, skip 2 character
                                string substring = inputString.Substring(start, index - start);
                                index += 2;
                                start = index;
                                return substring;
                            }
                            else
                            {
                                // Line separator \r, skip 1 character
                                string substring = inputString.Substring(start, index - start);
                                start = ++index;
                                return substring;
                            }
                        }
                        else
                        {
                            if (characterC == '\n')
                            {
                                // Line separator \n, skip 1 character
                                string substring = inputString.Substring(start, index - start);
                                start = ++index;
                                return substring;
                            }
                            else
                            {
                                // Line separator hasn't been found - moves on next character
                                iPtr++;
                                index++;
                            }
                        }
                    }
                }
            }

            // Returns a last substring
            index++;
            return inputString.Substring(start, stringLength - start);
        }

        /// <summary>
        /// Writes a length of the string to the specified at the specified pointer.
        /// </summary>
        /// <param name="inputString"> The string. </param>
        /// <param name="pointer"> The pointer. </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void WriteStringLength(string inputString, int* pointer)
        {
            fixed (char* strPtr = inputString)
            {
                // Gets a length of the string that is located at pointer - 4, 4 bytes (int)
                *pointer = *(((int*)strPtr) - 1);
            }
        }
    }
}