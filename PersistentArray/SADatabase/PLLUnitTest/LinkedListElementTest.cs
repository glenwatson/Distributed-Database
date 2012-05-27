using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ByteHelper;
using ByteHelperUnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PA;
using PLL;

namespace PLLUnitTest
{
    [TestClass]
    public class LinkedListElementTest
    {
        private LinkedListElement InitLinkedListElement(byte[] data, int next, int previous, int index)
        {
            return new LinkedListElement(data, next, previous, index);
        }

        [TestMethod]
        public void CtorTest()
        {
            byte[] data = new byte[5];
            int next = 4;
            int previous = 7;
            int index = 99;
            LinkedListElement element = InitLinkedListElement(data, next, previous, index);

            TestHelper.AssertByteArraysAreSame(data, element.Data);
            Assert.AreEqual(next, element.Next);
            Assert.AreEqual(previous, element.Previous);
            Assert.AreEqual(index, element.Index);
        }

        [TestMethod]
        public void SerializeTest()
        {
            byte[] data = new byte[]{1,2,3,4};
            int next = 4;
            int previous = 7;
            int index = 99;
            LinkedListElement element = InitLinkedListElement(data, next, previous, index);
            byte[] elementBytes = element.Serialize();

            byte[] expected = next.ToBytes().Append(previous.ToBytes(), data);
            TestHelper.AssertByteArraysAreSame(expected,elementBytes);
        }


        [TestMethod]
        public void DeserializeTest()
        {
            byte[] data = new byte[] { 1, 2, 3, 4 };
            int next = 4;
            int previous = 7;
            int index = 99;
            byte[] expected = next.ToBytes().Append(previous.ToBytes(), data);

            LinkedListElement element = LinkedListElement.Deserialize(expected, index);
            TestHelper.AssertByteArraysAreSame(data, element.Data);
            Assert.AreEqual(next, element.Next);
            Assert.AreEqual(previous, element.Previous);
            Assert.AreEqual(index, element.Index);
        }

    }
}
