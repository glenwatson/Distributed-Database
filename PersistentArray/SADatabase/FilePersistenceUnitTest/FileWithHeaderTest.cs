using System;
using System.IO;
using ByteHelperUnitTest;
using FilePersistence;
using FilePersistence.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilePersistenceUnitTest
{
    [TestClass]
    public class FileWithHeaderTest
    {
        public FileWithHeader InitFWH(string fileName, int userHeaderSize)
        {
            FileWithHeader fwh;
            try
            {
                fwh = new FileWithHeader(fileName, userHeaderSize);
            }
            catch (FileNameConflictException)
            {
                fwh = new FileWithHeader(fileName);
                fwh.Delete();
                fwh = new FileWithHeader(fileName, userHeaderSize);
            }
            return fwh;
        }

        [TestMethod]
        public void GetPathToNameTest()
        {
            string fileName = "GetPathToNameTest";
            string path = FileWithHeader.GetPathToName(fileName);
            DirectoryInfo dirInfo = Directory.GetParent(path);
            Assert.IsTrue(dirInfo.Exists);
        }

        [TestMethod]
        public void FileDoesntExistTest()
        {
            try
            {
                new FileWithHeader("DoesntExist");
                Assert.Fail("Should throw exception");
            }
            catch (FileNotFoundException) { }
        }

        [TestMethod]
        public void FileAlreadyExistsTest()
        {
            string fileName = "AlreadyExists";
            FileWithHeader fwh = InitFWH(fileName, 5);
            FileWithHeader fwh2 = null;
            try
            {
                fwh2 = new FileWithHeader(fileName, 5);
                Assert.Fail("Should throw exception");
            }
            catch (FileNameConflictException) { }
            finally
            {
                fwh.Close();
                if (fwh2 != null) //if for some reason it actuall worked,
                    fwh2.Close(); // still close the file

            }
        }

        [TestMethod]
        public void ReopenTest()
        {
            string fileName = "ReopenFileWithHeaderTest";
            FileWithHeader fwh = InitFWH(fileName, 5);
            try
            {
                //write values
                int expectedIndex1 = 0;
                byte expectedValue1 = 6;
                fwh.Put(expectedIndex1, expectedValue1);

                int expectedIndex2 = 55;
                byte expectedValue2 = 200;
                fwh.Put(expectedIndex2, expectedValue2);
                
                //get next index
                int expectedNextIndex = fwh.GetNextIndex();

                byte[] expectedUserHeader = new byte[5]{1,2,3,4,5};
                fwh.PutUserHeader(expectedUserHeader);

                fwh.Close();

                fwh = new FileWithHeader(fileName);

                Assert.AreEqual(expectedValue1, fwh.Get(expectedIndex1));
                Assert.AreEqual(expectedValue2, fwh.Get(expectedIndex2));

                Assert.AreEqual(expectedNextIndex, fwh.GetNextIndex());

                TestHelper.AssertByteArraysAreSame(expectedUserHeader, fwh.GetUserHeader());
            }
            finally
            {
                fwh.Close();
            }
        }

        [TestMethod]
        public void CloseTest()
        {
            FileWithHeader fwh = InitFWH("FWHCloseTest", 5);
            try
            {
                fwh.Close();
                fwh = InitFWH("FWHCloseTest", 3);
            }
            finally
            {
                fwh.Close();
            }
        }

        [TestMethod]
        public void GetUserHeaderSizeTest()
        {
            GetUserHeaderSizeTestAssert(9);
            GetUserHeaderSizeTestAssert(1);
            GetUserHeaderSizeTestAssert(0);
            GetUserHeaderSizeTestAssert(1000000);
            try
            {
                GetUserHeaderSizeTestAssert(-1);
                Assert.Fail("Should throw exception");
            }
            catch (InvalidUserHeaderException) { }
        }

        private void GetUserHeaderSizeTestAssert(int expected)
        {
            FileWithHeader target = InitFWH("GetFWHUserHeaderSizeTest", expected);
            try
            {
                int actual = target.GetUserHeaderSize();
                Assert.AreEqual(expected, actual);
            }
            finally
            {
                target.Close();
            }
        }

        [TestMethod]
        public void GetPutUserHeaderTest()
        {
            GetPutUserHeaderTest(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            GetPutUserHeaderTest(new byte[] { });
            GetPutUserHeaderTest(new byte[] { byte.MaxValue, byte.MinValue });
            GetPutUserHeaderTest(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, });
            try {
                GetPutUserHeaderTest(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 });
                Assert.Fail("Should throw exception");
            } catch (InvalidUserHeaderException) { }
            try {
                GetPutUserHeaderTest(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 });
                Assert.Fail("Should throw exception");
            } catch (InvalidUserHeaderException) { }
        }

        private void GetPutUserHeaderTest(byte[] uHeader)
        {
            FileWithHeader target = InitFWH("GetPutUserHeaderTest", 16);
            try
            {
                target.PutUserHeader(uHeader);
                byte[] actual = target.GetUserHeader();
                TestHelper.AssertByteArraysAreSame(uHeader, actual);
            }
            finally
            {
                target.Close();
            }

        }

        [TestMethod]
        public void GetNextIndexTest()
        {
            GetNextIndexTestAssert(1);
            GetNextIndexTestAssert(9);
            GetNextIndexTestAssert(3);
            GetNextIndexTestAssert(1000000);
            GetNextIndexTestAssert(27);
        }

        private void GetNextIndexTestAssert(int expected)
        {
            FileWithHeader target = InitFWH("GetFWHNextIndexTest", 16);
            try
            {
                target.Put(expected - 1, 0);
                int actual = target.GetNextIndex();
                Assert.AreEqual(expected, actual);
            }
            finally
            {
                target.Close();
            }
        }

        [TestMethod]
        public void GetPutTest()
        {
            FileWithHeader target = InitFWH("GetPutFWHTest", 5);
            try
            {
                GetPutTestAssert(8, 44, target);
                GetPutTestAssert(0, 44, target);
                GetPutTestAssert(99, 0, target);
                GetPutTestAssert(20, byte.MaxValue, target);
                GetPutTestAssert(4, 4, target);
            }
            finally
            {
                target.Close();
            }
        }

        private static void GetPutTestAssert(byte expectedValue, int expectedIndex, FileWithHeader target)
        {
            int nextIndexBefore = target.GetNextIndex();
            
            target.Put(expectedIndex, expectedValue);

            if (expectedIndex >= nextIndexBefore)
                Assert.AreEqual(expectedIndex+1, target.GetNextIndex());
            Assert.AreEqual(expectedValue, target.Get(expectedIndex));
        }

        [TestMethod]
        public void PutGetAmountTest()
        {
            FileWithHeader fwh = InitFWH("FWHPutGetAmountTest", 5);
            try
            {
                GetPutArrayTestAssert(fwh, 9, new byte[] { });
                GetPutArrayTestAssert(fwh, 3, new byte[] { byte.MaxValue, byte.MinValue });
                GetPutArrayTestAssert(fwh, 50, new byte[] {1,2,3,4,5,6,7,8,9,10,11 });
                GetPutArrayTestAssert(fwh, 10, new byte[] { 7, 7, 7 });
                GetPutArrayTestAssert(fwh, 30, new byte[] { 12,34,56,78,90 });
                GetPutArrayTestAssert(fwh, 25, new byte[] { 0,0,0,0,0,0,0 });
            }
            finally
            {
                fwh.Close();
            }
        }

        private static void GetPutArrayTestAssert(FileWithHeader fwh, int index, byte[] data)
        {
            int nextIndexBefore = fwh.GetNextIndex();
            fwh.Put(index, data);

            if(index+data.Length >= nextIndexBefore)
                Assert.AreEqual(index+data.Length, fwh.GetNextIndex());

            byte[] actual = fwh.GetAmount(index, data.Length);
            TestHelper.AssertByteArraysAreSame(data, actual);
        }

        [TestMethod]
        public void GetRangeTest()
        {
            FileWithHeader fwh = InitFWH("FWHGetRangeTest", 5);
            try
            {
                GetRangeTestAssert(new byte[] { 1, 2, 3, 4 }, 0, fwh);
                GetRangeTestAssert(new byte[] { }, 6, fwh);
                GetRangeTestAssert(new byte[] { 0 }, 8, fwh);
                GetRangeTestAssert(new byte[] { byte.MaxValue, byte.MaxValue, byte.MaxValue }, 10, fwh);
                GetRangeTestAssert(new byte[] { 1, 2, 3, 4,5,6,7,8,9,10,11,12,13,14,15,16 }, 20, fwh);
            }
            finally
            {
                fwh.Close();
            }
        }

        private static void GetRangeTestAssert(byte[] bytes, int startIndex, FileWithHeader fwh)
        {
            fwh.Put(startIndex, bytes);
            byte[] actual = fwh.GetRange(startIndex, startIndex+bytes.Length - 1);
            TestHelper.AssertByteArraysAreSame(bytes, actual);
        }
    }
}
