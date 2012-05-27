using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ByteHelper;
using ByteHelperUnitTest;
using Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PA;
using PAFS;

namespace PAFSUnitTest
{
    [TestClass]
    public class PAFSHeaderTest
    {

        private static PAFSHeader InitHeader(int expected)
        {
            PAFSHeader header = new PAFSHeader(expected);
            return header;
        }

        [TestMethod]
        public void CtorTest()
        {
            int expected = 5;
            var header = InitHeader(expected);

            Assert.AreEqual(expected, header.NextFreeSpaceIndex);
        }

        [TestMethod]
        public void SerializeTest()
        {
            int expected = 88;
            PAFSHeader header = InitHeader(expected);

            byte[] serialied = header.Serialize();
            TestHelper.AssertByteArraysAreSame(88.ToBytes(), serialied);
        }


        [TestMethod]
        public void DeserializeTest()
        {
            int expected = 155;
            byte[] data = expected.ToBytes();

            PAFSHeader header = PAFSHeader.Deserialize(data);

            Assert.AreEqual(expected, header.NextFreeSpaceIndex);

            try
            {
                PAFSHeader.Deserialize(new byte[0]);
                Assert.Fail("Should throw exception");
            }
            catch (InsufficientDataException) { }
        }

        [TestMethod]
        public void GetHeaderSizeTest()
        {
            Assert.AreEqual(4, PAFSHeader.GetHeaderSize());
        }
    }
}
