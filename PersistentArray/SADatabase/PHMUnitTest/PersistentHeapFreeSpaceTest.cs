using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ByteHelper;
using ByteHelperUnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PHM;
using PHM.Exceptions;
using PHM.PHMSpaces;

namespace PHMUnitTest
{
    [TestClass]
    public class PersistentHeapFreeSpaceTest
    {
        [TestMethod]
        public void CtorTest()
        {
            int next = 23;
            PersistentHeapFreeSpace freeSpace = new PersistentHeapFreeSpace(8, 10, next);

            Assert.AreEqual(next, freeSpace.NextFreeSpace);
        }

        [TestMethod]
        public void SplitTest()
        {
            SplitTestAssert(0/*46*/, 20, 5);
            SplitTestAssert(12, 20, 6);
            SplitTestAssert(8, 20, 14);
            SplitTestAssert(99, 20, 6);
            SplitTestAssert(2, 20, 6);
            SplitTestAssert(17, 21, 7);
            SplitTestAssert(99, 99, 6);
            SplitTestAssert(99, 99, 88);

            try {
                SplitTestAssert(55, 55, 0);
                Assert.Fail("Should throw exception");
            } catch (InvalidIndexException) {}

            try {
                SplitTestAssert(55, 55, 55);
                Assert.Fail("Should throw exception");
            } catch (InvalidIndexException) {}

            try {
                SplitTestAssert(60, 20, 16);
                Assert.Fail("Should throw exception");
            } catch (InvalidIndexException) {}

            try {
                SplitTestAssert(60, 80, 76);
                Assert.Fail("Should throw exception");
            } catch (InvalidIndexException) {}
            
        }

        private static void SplitTestAssert(int sizeIndex, int userSize, int newUserSize)
        {
            PersistentHeapFreeSpace freeSpace = new PersistentHeapFreeSpace(sizeIndex, sizeIndex + userSize + PersistentHeapSpace.GetUserSizeSize()-1, 29);

            PersistentHeapUsedSpace usedSpace = freeSpace.Split(newUserSize);

            Assert.AreEqual(newUserSize, freeSpace.UserSize);
            Assert.AreEqual(sizeIndex, freeSpace.SizeIndex);
            Assert.AreEqual(sizeIndex+newUserSize+PersistentHeapSpace.GetUserSizeSize()-1,freeSpace.EndIndex);

            Assert.AreEqual(userSize - newUserSize - PersistentHeapSpace.GetUserSizeSize(), usedSpace.UserSize);
            Assert.AreEqual(sizeIndex + newUserSize + PersistentHeapSpace.GetUserSizeSize()-1+1, usedSpace.SizeIndex);
        }

        [TestMethod]
        public void MergeBeforeTest()
        {
            MergeBeforeTestAssert(7, 14, 2, 6);
            MergeBeforeTestAssert(80, 99, 23, 79);

            try {
                MergeBeforeTestAssert(7, 15, 16, 18);
                Assert.Fail("Should throw exception");
            } catch (MemoryNotContinuousException) { }
        }

        private static void MergeBeforeTestAssert(int sizeIndex1, int endIndex1, int sizeIndex2, int endIndex2)
        {
            PersistentHeapFreeSpace freeSpace1 = new PersistentHeapFreeSpace(sizeIndex1, endIndex1, 5);
            PersistentHeapFreeSpace freeSpace2 = new PersistentHeapFreeSpace(sizeIndex2, endIndex2, 6);

            freeSpace1.MergeBefore(freeSpace2);

            Assert.AreEqual(endIndex1, freeSpace1.EndIndex);
            Assert.AreEqual(endIndex1 - sizeIndex2+1, freeSpace1.SizeInBytes);
            Assert.AreEqual(endIndex1 - sizeIndex2+1 - PersistentHeapSpace.GetUserSizeSize(), freeSpace1.UserSize);
        }

        [TestMethod]
        public void MergeAfterTest()
        {
            MergeAfterTestAssert(15, 18, 19, 24);

            try {
                MergeAfterTestAssert(16, 19, 21, 25);
                Assert.Fail("Should throw exception");
            }catch (MemoryNotContinuousException) { }

            try {
                MergeAfterTestAssert(16, 19, 19, 25);
                Assert.Fail("Should throw exception");
            }catch (MemoryNotContinuousException) { }
        }

        private static void MergeAfterTestAssert(int sizeIndex1, int endIndex1, int sizeIndex2, int endIndex2)
        {
            PersistentHeapFreeSpace freeSpace1 = new PersistentHeapFreeSpace(sizeIndex1, endIndex1, 5);
            PersistentHeapFreeSpace freeSpace2 = new PersistentHeapFreeSpace(sizeIndex2, endIndex2, 6);

            freeSpace1.MergeAfter(freeSpace2);

            Assert.AreEqual(endIndex2, freeSpace1.EndIndex);
            Assert.AreEqual(endIndex2 - sizeIndex1 - PersistentHeapSpace.GetUserSizeSize()+1, freeSpace1.UserSize);
        }

        [TestMethod]
        public void SerializeTest()
        {
            PersistentHeapFreeSpace freeSpace = new PersistentHeapFreeSpace(5, 20, 35);

            byte[] freeSpaceBytes = freeSpace.Serialize();
            TestHelper.AssertByteArraysAreSame(freeSpace.UserSize.ToBytes().Append(freeSpace.NextFreeSpace.ToBytes()),freeSpaceBytes);
        }
    }
}
