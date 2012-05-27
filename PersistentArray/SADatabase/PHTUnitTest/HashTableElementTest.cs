using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ByteHelper;
using ByteHelperUnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PA;
using PHT;

namespace PHTUnitTest
{
    [TestClass]
    public class HashTableElementTest
    {
        private HashTableElement InitHashTableElement(byte[] key, byte[] value, int index)
        {
            return new HashTableElement(key, value, index);
        }

        [TestMethod]
        public void CtorTest()
        {
            byte[] key = new byte[]{128};
            byte[] value = new byte[]{7,7,7};
            int index = 9999;
            HashTableElement element = InitHashTableElement(key, value, index);

            TestHelper.AssertByteArraysAreSame(key, element.Key);
            TestHelper.AssertByteArraysAreSame(value, element.Value);
            Assert.AreEqual(index, element.Index);

        }

        [TestMethod]
        public void SerializeTest()
        {
            byte[] key = new byte[] { 128 };
            byte[] value = new byte[] { 7, 7, 7 };
            int index = 9999;
            HashTableElement element = InitHashTableElement(key, value, index);

            byte[] expected = key.Append(value);

            TestHelper.AssertByteArraysAreSame(expected, element.Serialize());

        }

        [TestMethod]
        public void DeserializeTest()
        {
            byte[] key = new byte[] { 128 };
            byte[] value = new byte[] { 7, 7, 7 };
            int index = 9999;

            byte[] expected = key.Append(value);
            HashTableElement element = HashTableElement.Deserialize(expected,index,key.Length,value.Length);

            TestHelper.AssertByteArraysAreSame(key, element.Key);
            TestHelper.AssertByteArraysAreSame(value, element.Value);
            Assert.AreEqual(index, element.Index);
        }
    }
}
