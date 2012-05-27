using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ByteHelper;
using ByteHelperUnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PHM.PHMSpaces;

namespace PHMUnitTest
{
    [TestClass]
    public class PersistentHeapUsedSpaceTest
    {
        [TestMethod]
        public void CtorTest()
        {
            byte[] data = new byte[]{1,2,3,4,5,6,7,8,9,0};
            PersistentHeapUsedSpace usedSpace = new PersistentHeapUsedSpace(7, 33, data);
            TestHelper.AssertByteArraysAreSame(data, usedSpace.Data);

            byte[] data2 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, };
            PersistentHeapUsedSpace usedSpace2 = new PersistentHeapUsedSpace(5, 18, data2);
            TestHelper.AssertByteArraysAreSame(data2, usedSpace2.Data);


            try {
                new PersistentHeapUsedSpace(7, 10, new byte[4]);
                Assert.Fail("Should throw exception");
            } catch (IndexOutOfRangeException) {}

            try {
                new PersistentHeapUsedSpace(7, 14, new byte[5]);
                Assert.Fail("Should throw exception");
            } catch (IndexOutOfRangeException) {}
        }

        [TestMethod]
        public void SerializeTest()
        {
            int sizeIndex = 7;
            int endIndex = 20;
            byte[] data = new byte[]{1,2,3,4,5,6,};
            PersistentHeapUsedSpace usedSpace = new PersistentHeapUsedSpace(sizeIndex, endIndex, data);
            int userSize = (endIndex - sizeIndex - PersistentHeapSpace.GetUserSizeSize() + 1);
            byte[] userSizeBytes = userSize.ToBytes();
            byte[] expected = userSizeBytes.Append(data);

            TestHelper.AssertByteArraysAreSame(expected, usedSpace.Serialize());
        }
    }
}
