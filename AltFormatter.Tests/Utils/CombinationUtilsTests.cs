/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace AltFormatter.Utils
{
    public sealed class CombinationUtilsTests
    {
        [Test]
        public void CartesianProduct_1Array_Product()
        {
            var val = CombinationUtils.CartesianProduct<int>(new int[] { 1, 2 }).ToArray();
            Assert.AreEqual(2, val.Length);
            Assert.AreEqual(1, val[0][0]);
            Assert.AreEqual(2, val[1][0]);
        }

        [Test]
        public void CartesianProduct_2Array_Product()
        {
            var val = CombinationUtils.CartesianProduct<int>(new int[] { 1, 2 }, new int[] { 1, 2 }).ToArray();
            Assert.AreEqual(4, val.Length);

            Assert.AreEqual(2, val[0].Length);
            Assert.AreEqual(1, val[0][0]);
            Assert.AreEqual(1, val[0][1]);

            Assert.AreEqual(2, val[1].Length);
            Assert.AreEqual(1, val[1][0]);
            Assert.AreEqual(2, val[1][1]);

            Assert.AreEqual(2, val[2].Length);
            Assert.AreEqual(2, val[2][0]);
            Assert.AreEqual(1, val[2][1]);

            Assert.AreEqual(2, val[3].Length);
            Assert.AreEqual(2, val[3][0]);
            Assert.AreEqual(2, val[3][1]);
        }
        
        [Test]
        public void CartesianProduct_ValuesNull_ValidateNotNull()
        {
        	AssertValidation.NotNull(
        		() => CombinationUtils.CartesianProduct<int>(null).ToArray(),
        		"Values");
        }
        
        [Test]
        public void CartesianProduct_0ValueNull_ValidateNotNull0Value()
        {
        	AssertValidation.NotNullAll(
        		() => CombinationUtils.CartesianProduct<int>(null, new int[0]).ToArray(),
        		"Values",
        		0);
        }
        
        [Test]
        public void CartesianProduct_EmptyValues_ValidateSizeMoreThan()
        {
        	AssertValidation.SizeMoreThan(
        		s => CombinationUtils.CartesianProduct<int>(new IEnumerable<int>[s]).ToArray(),
        		"Values",
        		0);
        }
    }
}