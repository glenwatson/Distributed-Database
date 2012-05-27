using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ByteHelper;
using ByteHelperUnitTest;
using Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PLL;

namespace PLLUnitTest
{
    [TestClass]
    public class PLLHeaderTest
    {
        [TestMethod]
        public void CtorTest()
        {
            int size = 888;
            PLLHeader header = new PLLHeader(size);
            Assert.AreEqual(size, header.Size);
        }

        [TestMethod]
        public void SerializeTest()
        {
            int size = 888;
            PLLHeader header = new PLLHeader(size);

            byte[] headerbytes = header.Serialize();

            TestHelper.AssertByteArraysAreSame(size.ToBytes(), headerbytes);

        }

        [TestMethod]
        public void DeserializeTest()
        {
            int size = 888;
            byte[] sizeBytes = size.ToBytes();
            PLLHeader header = PLLHeader.Deserialize(sizeBytes);

            Assert.AreEqual(size, header.Size);


            try
            {
                PLLHeader.Deserialize(new byte[0]);
                Assert.Fail("Should throw exception");
            }
            catch (InsufficientDataException){}
        }
    }
}
