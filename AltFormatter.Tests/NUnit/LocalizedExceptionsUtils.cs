/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using AltFormatter.Localization;
using AltFormatter.Utils;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace NUnit.Framework
{
    public static class LocalizedExceptionsUtils
    {
        private readonly static IFormatProvider invariantCulture = CultureInfo.InvariantCulture;

        public static void AssertArguments(this LocalizedException ex, params object[] args)
        {
            string message = ex.Message.ToString(invariantCulture);

            if (args == null)
            {
                Assert.Fail("Arguments is null. Exception: {0}", message);
                return;
            }

            int argsLength = args.Length;
            if (argsLength == 0)
            {
                Assert.Fail("Count of the arguments is zero. Exception: {0}", message);
                return;
            }

            int constructorsLength = ex.GetType().GetConstructors().Length;
            if (constructorsLength != 1)
            {
                Assert.Fail("Type of the exception should has only one constructor, but count of the constructors is <{0}>. Exception: {1}", constructorsLength, message);
                return;
            }

            string[] arguments = GetArgumentsInString(message).ToArray();
            int argumentsCount = arguments.Length;
            
            if (argumentsCount != argsLength)
            {
                Assert.Fail("Expected count of the arguments <{0}> isn't equals found count <{1}>. Exception: {2}", argsLength, argumentsCount, message);
                return;
            }
            
            string expectedValue;
            for (int i = 0; i < argumentsCount; i++)
            {
                expectedValue = StringUtils.Combine("<", args[i].ConvertTo<string>(), ">");
                if (expectedValue != arguments[i])
                {
                    Assert.Fail("Expected value {0} isn't equals found value {1}. Exception: {2}", expectedValue, arguments[i], message);
                    return;
                }
            }
        }
        
        private static IEnumerable<string> GetArgumentsInString(string str)
        {
        	int length = str.Length;
        	
        	char c;
        	int i = 0;
        	int startIndex, lastIndex, lessCharCount;
        	
        	while(i < length)
        	{
        		c = str[i];
        		
        		if(c == '<')
        		{
        			startIndex = i;
        			lastIndex = startIndex;
        			lessCharCount = 1;
        			
        			while(lessCharCount != 0)
        			{
        				c = str[++lastIndex];
        				
        				if(c == '<')
        				{
        					lessCharCount++;
        				}
        				
        				if(c == '>')
        				{
        					lessCharCount--;
        				}
        				
        				if(lessCharCount == 0)
        				{
        					yield return str.Substring(startIndex, ++lastIndex - startIndex);
        					i = lastIndex;
        					break;
        				}
        			}
        		}
        		else
        		{
        			i++;
        		}
        	}
        }
    }
}