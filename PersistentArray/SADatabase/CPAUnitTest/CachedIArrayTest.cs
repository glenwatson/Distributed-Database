using ByteHelperUnitTest;
using FilePersistence.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CPA;
using PA.Exceptions;

namespace CPAUnitTest
{
    [TestClass]
    public class CachedIArrayTest
    {
        private CachedArray InitCachedIArray(string arrayName, int elementSize, int userHeaderSize, int cacheSize)
        {
            CachedArray cache;
            try
            {
                cache = new CachedArray(arrayName, elementSize, userHeaderSize, cacheSize);
            }
            catch (FileNameConflictException)
            {
                cache = new CachedArray(arrayName);
                cache.Delete();
                cache = new CachedArray(arrayName, elementSize, userHeaderSize, cacheSize);
            }
            return cache;
        }

        [TestMethod]
        public void CreateCtorTest()
        {
            const int elementSize = 6;
            const int userHeaderSize = 7;
            const int cacheSize = 15;
            CachedArray cache = InitCachedIArray("CreateCtorTest", elementSize, userHeaderSize, cacheSize);
            try
            {
                Assert.AreEqual(elementSize, cache.GetElementSize());
                Assert.AreEqual(userHeaderSize, cache.GetUserHeaderSize());
                Assert.AreEqual(cacheSize, cache.GetCacheSize());
            }
            finally
            {
                cache.Close();
            }
        }

        [TestMethod]
        public void ReopenTest()
        {
            string arrayName = "ReopenCachedArrayTest";
            const int elementSize = 10;
            const int userHeaderSize = 4;
            const int cacheSize = 15;
            CachedArray cache = InitCachedIArray(arrayName, elementSize, userHeaderSize, cacheSize);
            try
            {
                int index1 = 4;
                byte[] bytes1 = new byte[]{1,2,3,4,5,6,7,8,9,0};
                cache.Put(index1, bytes1);

                int index2 = 5;
                byte[] bytes2 = new byte[] { 255,254,253,252,251,250,249,248,247,246 };
                cache.Put(index2, bytes2);

                cache.Close();

                cache = new CachedArray(arrayName);
                Assert.AreEqual(elementSize, cache.GetElementSize());
                Assert.AreEqual(userHeaderSize, cache.GetUserHeaderSize());
                Assert.AreEqual(cacheSize, cache.GetCacheSize());
                TestHelper.AssertByteArraysAreSame(bytes1, cache.Get(index1));
                TestHelper.AssertByteArraysAreSame(bytes2, cache.Get(index2));
            }
            finally
            {
                cache.Close();
            }
        }

        [TestMethod]
        public void PutTest()
        {
            CachedArray cache = InitCachedIArray("CachedArrayPut", 6, 7, 10);
            try
            {
                PutGetTestAssert(new byte[] { 1, 2, 3 }, 0, cache);
                PutGetTestAssert(new byte[] { 1 }, 0, cache);
                PutGetTestAssert(new byte[] { 12, 23, 34, 45, 56 }, 1, cache);
                PutGetTestAssert(new byte[] { 123, 234 }, 1, cache);
                PutGetTestAssert(new byte[] { 5, 4, 90 }, 2, cache);
                PutGetTestAssert(new byte[] { 6, 6, 6 }, 6, cache);
            }
            finally
            {
                cache.Close();
            }
        }

        [TestMethod]
        public void GetTest()
        {
            CachedArray cache = InitCachedIArray("CachedArrayGet", 8, 7, 10);
            try
            {
                PutGetTestAssert(new byte[] { 1, 2, 3 }, 0, cache);
                PutGetTestAssert(new byte[] { }, 0, cache);
                PutGetTestAssert(new byte[] { 7, 7, 7 }, 7, cache);
                PutGetTestAssert(new byte[] { 1, 2, 34, 5, 6, 76, 8 }, 0, cache);

                try {
                    cache.Get(99);
                    Assert.Fail("Should throw exception");
                } catch (InvalidElementIndexException) { }
            }
            finally
            {
                cache.Close();
            }
        }

        private static void PutGetTestAssert(byte[] bytes, int elementIdx, CachedArray cache)
        {
            cache.Put(elementIdx, bytes);
            byte[] actual = cache.Get(elementIdx);

            TestHelper.AssertByteArraysAreSame(bytes, actual);
        }
        [TestMethod]
        public void CloseTest()
        {
            CachedArray cache = InitCachedIArray("CachedArrayClose", 8, 7, 10);
            try
            {
                cache.Close();
                InitCachedIArray("Get", 8, 7, 10);
            }
            finally
            {
                cache.Close();
            }
        }

        [TestMethod]
        public void GetElementSizeTest()
        {
            int elementSize = 7;
            CachedArray cache = InitCachedIArray("CachedArrayGetElementSize", elementSize, 7, 10);
            try
            {
                int actual = cache.GetElementSize();
                Assert.AreEqual(elementSize, actual);
            }
            finally
            {
                cache.Close();
            }
        }

        [TestMethod]
        public void PutGetUserHeaderTest()
        {
            CachedArray cache = InitCachedIArray("CachedArrayPutGetUserHeader", 8, 7, 10);
            try
            {
                GetPutUserHeaderSizeTestAssert(new byte[] { 3, 4, 5 }, cache);
                GetPutUserHeaderSizeTestAssert(new byte[] { }, cache);
                GetPutUserHeaderSizeTestAssert(new byte[] { 1,2,3,4,5,6,7 }, cache);
            }
            finally
            {
                cache.Close();
            }
        }

        private static void GetPutUserHeaderSizeTestAssert(byte[] uHeader, CachedArray cache)
        {
            cache.PutUserHeader(uHeader);
            byte[] actual = cache.GetUserHeader();
            TestHelper.AssertByteArraysAreSame(uHeader, actual);
        }

        [TestMethod]
        public void GetUserHeaderSizeTest()
        {
            int uHeaderSize = 9;
            CachedArray cache = InitCachedIArray("CachedArrayGetUserHeaderSize", 8, uHeaderSize, 10);
            try
            {
                int actual = cache.GetUserHeaderSize();
                Assert.AreEqual(uHeaderSize, actual);
            }
            finally
            {
                cache.Close();
            }
        }

        [TestMethod]
        public void GetNextIndexTest()
        {
            CachedArray cache = InitCachedIArray("CachedArrayGetNextIndex", 8, 7, 10);
            try
            {
                Assert.AreEqual(0, cache.GetNextIndex());
            }
            finally
            {
                cache.Close();
            }
        }

        [TestMethod]
        public void WipeIndexTest()
        {
            CachedArray cache = InitCachedIArray("CachedArrayWipeIndex", 8, 7, 10);
            try
            {
                int elementIndex = 0;
                cache.Put(elementIndex, new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 });
                cache.WipeElement(elementIndex);
                TestHelper.AssertByteArraysAreSame(new byte[0], cache.Get(elementIndex));
            }
            finally
            {
                cache.Close();
            }
        }
    }
}
