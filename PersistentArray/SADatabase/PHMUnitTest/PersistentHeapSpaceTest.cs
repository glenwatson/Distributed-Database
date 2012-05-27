using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PHM.PHMSpaces;
using PHM.Exceptions;

namespace PHMUnitTest
{
    [TestClass]
    public class PersistentHeapSpaceTest
    {

        private static PersistentHeapSpace InitHeapSpace(int sizeIndex, int endIndex, byte[] data)
        {
            return new PersistentHeapUsedSpace(sizeIndex, endIndex, data);
        }

        [TestMethod]
        public void CtorTest()
        {
            int sizeIndex = 5;
            int endIndex = 77;
            PersistentHeapSpace space = InitHeapSpace(sizeIndex, endIndex, new byte[0]);

            Assert.AreEqual(sizeIndex, space.SizeIndex);
            Assert.AreEqual(endIndex, space.EndIndex);

            try {
                new PersistentHeapUsedSpace(55, 55, new byte[0]);
                Assert.Fail("Should throw exception");
            } catch (InvalidIndexException){}

            try {
                new PersistentHeapUsedSpace(55, 0, new byte[0]);
                Assert.Fail("Should throw exception");
            } catch (InvalidIndexException){}
        }

        [TestMethod]
        public void UserSizeTest()
        {
            UserSizeTestAssert(4, 55);
            UserSizeTestAssert(9, 55);
            UserSizeTestAssert(0, 16);
            UserSizeTestAssert(byte.MaxValue, 16);
            UserSizeTestAssert(1 << 20, 16);
        }

        private static void UserSizeTestAssert(int userSize, int sizeIndex)
        {
            PersistentHeapSpace space = InitHeapSpace(sizeIndex, sizeIndex + userSize + PersistentHeapSpace.GetUserSizeSize() - 1, new byte[0]);
            Assert.AreEqual(userSize, space.UserSize);
        }

        [TestMethod]
        public void SizeInBytesTest()
        {
            SizeInBytesTestAssert(4, 55);
            SizeInBytesTestAssert(9, 55);
            SizeInBytesTestAssert(byte.MaxValue, 16);
            SizeInBytesTestAssert(1 << 20, 16);

            try {
                SizeInBytesTestAssert(0, 16);
                Assert.Fail("Should throw exception");
            } catch (InvalidIndexException) {}
        }

        private static void SizeInBytesTestAssert(int sizeInBytes, int sizeIndex)
        {
            PersistentHeapSpace space = InitHeapSpace(sizeIndex, sizeIndex + sizeInBytes-1, new byte[0]);
            Assert.AreEqual(sizeInBytes, space.SizeInBytes);
        }

        [TestMethod]
        public void StartIndexTest()
        {
            StartIndexTestAssert(5, 5);
            StartIndexTestAssert(0,5);
            StartIndexTestAssert(byte.MaxValue,5);
            StartIndexTestAssert(1<<20,6);
            StartIndexTestAssert(42,6);
        }

        private static void StartIndexTestAssert(int startIndex, int userSize)
        {
            PersistentHeapSpace space = InitHeapSpace(startIndex - PersistentHeapSpace.GetUserSizeSize(), startIndex + userSize, new byte[0]);
            Assert.AreEqual(startIndex, space.StartIndex);
        }
    }
}
