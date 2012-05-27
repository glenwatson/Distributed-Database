using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ByteHelper;
using ByteHelperUnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PHM;

namespace PHMUnitTest
{
    [TestClass]
    public class PersistentHeapHeaderTest
    {
        [TestMethod]
        public void CtorTest()
        {
            int freeSpaceHead = 8;
            PersistentHeapHeader header = new PersistentHeapHeader(freeSpaceHead);

            Assert.AreEqual(freeSpaceHead, header.FreeSpaceHead);
        }

        [TestMethod]
        public void SerializeTest()
        {
            int freeSpaceHead = 99;
            PersistentHeapHeader header = new PersistentHeapHeader(freeSpaceHead);

            TestHelper.AssertByteArraysAreSame(freeSpaceHead.ToBytes(), header.Serialize());
        }

        [TestMethod]
        public void DeserializeTest()
        {
            int freeSpaceHead = 1000;
            PersistentHeapHeader header = PersistentHeapHeader.Deserialize(freeSpaceHead.ToBytes());

            Assert.AreEqual(freeSpaceHead, header.FreeSpaceHead);


        }
    }
}
