using System;
using System.Collections.Generic;
using System.IO;
using ByteHelperUnitTest;
using FilePersistence.Exceptions;
using Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PA;
using PA.Exceptions;

namespace PAUnitTest
{
    /// <summary>
    /// Summary description for IPersistentArrayTest
    /// </summary>
    [TestClass]
    public class IPersistentArrayTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //    TestConstants.CreateTestDir();
        //}

        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //    TestConstants.DeleteTestDir();
        //}

        private IPersistentArrayNextSpace InitPA(String name, int eleSize, int uHeaderSize)
        {
            PersistentNextSpaceArray array;
            try
            {
                array = new PersistentNextSpaceArray(name, eleSize, uHeaderSize);
            }
            catch (FileNameConflictException)
            {
                array = new PersistentNextSpaceArray(name);
                array.Delete();
                array = new PersistentNextSpaceArray(name, eleSize, uHeaderSize);
            }
            return array;
        }

        /// <summary>
        ///A test for Close
        ///</summary>
        [TestMethod()]
        public void EmptyNameTest()
        {
            PersistentNextSpaceArray pnsa=null;
            try
            {
                pnsa = new PersistentNextSpaceArray("", 6, 6);
                Assert.Fail("Should throw exception");
            }
            catch (FileNotFoundException)
            {
                if(pnsa != null)
                    pnsa.Close();
            }
        }

        /// <summary>
        ///A test for reopening a PersistentArray
        ///</summary>
        [TestMethod()]
        public void ReopenTest()
        {
            string arrayName = "ReopenPersistentArrayTest";
            int eleSize = 5;
            int uHSize = 9;

            var data = new List<Tuple<int, byte[]>>();
            var record1 = new Tuple<int, byte[]>(0, new byte[]{1,2,3,4,5});
            data.Add(record1);
            var record2 = new Tuple<int, byte[]>(1, new byte[] { 255,255,255,255,255 });
            data.Add(record2);
            var record3 = new Tuple<int, byte[]>(2, new byte[] { 12,34,56,78,90 });
            data.Add(record3);
            var record4 = new Tuple<int, byte[]>(3, new byte[] { 7,7,7 });
            data.Add(record4);

            var pa = InitPA(arrayName, eleSize, uHSize);
            foreach (var tuple in data)
                pa.Put(tuple.Item1, tuple.Item2);
            pa.Close();

            var reopening = new PersistentNextSpaceArray(arrayName);

            Assert.AreEqual(eleSize, reopening.GetElementSize());
            Assert.AreEqual(uHSize, reopening.GetUserHeaderSize());

            foreach (var tuple in data)
                TestHelper.AssertByteArraysAreSame(tuple.Item2, reopening.Get(tuple.Item1));
        }

        /// <summary>
        ///A test for testing when an array doesn't exsist
        ///</summary>
        [TestMethod()]
        public void NoArrayTest()
        {
            PersistentNextSpaceArray pnsa = null;
            try
            {
                pnsa = new PersistentNextSpaceArray("PADoesntExsist");
                Assert.Fail("Should throw exception");
            }
            catch (FileNotFoundException)
            {
                if(pnsa != null)
                    pnsa.Close();
            }
        }

        /// <summary>
        ///A test for Close
        ///</summary>
        [TestMethod()]
        public void CloseTest()
        {
            InitPA("PACloseTest", 1, 1).Close();

        }

        /// <summary>
        ///A test for GetUserHeaderSize
        ///</summary>
        [TestMethod()]
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
            IPersistentArrayNextSpace target = InitPA("GetPAUserHeaderSizeTest", 1, expected);
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

        /// <summary>
        ///A test for GetElementSize
        ///</summary>
        [TestMethod()]
        public void GetElementSizeTest()
        {
            GetElementSizeTestAssert(15);
            GetElementSizeTestAssert(1);
            GetElementSizeTestAssert(1000000);

            try
            {
                GetElementSizeTestAssert(0);
                Assert.Fail("Should throw exception");
            }
            catch (InvalidElementSizeException) { }
            try{
                GetElementSizeTestAssert(-1);
                Assert.Fail("Should throw exception");
            }
            catch (InvalidElementSizeException) { }
        }

        private void GetElementSizeTestAssert(int expected)
        {
            IPersistentArrayNextSpace target = InitPA("GetPAElementSizeTest", expected, 16);
            try
            {
                int actual = target.GetElementSize();
                Assert.AreEqual(expected, actual);
            }
            finally
            {
                target.Close();
            }
        }

        /// <summary>
        ///A test for GetUserHeader & GetUserHeader
        ///</summary>
        [TestMethod()]
        public void GetPutUserHeaderTest()
        {
            GetPutUserHeaderTest(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            GetPutUserHeaderTest(new byte[] { });
            GetPutUserHeaderTest(new byte[] { byte.MaxValue, byte.MinValue });
            GetPutUserHeaderTest(new byte[] { 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16, });
            try
            {
                GetPutUserHeaderTest(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 });
                Assert.Fail("Should throw exception");
            }
            catch (InvalidUserHeaderException) { }
            try{
                GetPutUserHeaderTest(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 });
                Assert.Fail("Should throw exception");
            }
            catch (InvalidUserHeaderException) { }
        }

        private void GetPutUserHeaderTest(byte[] uHeader)
        {
            IPersistentArrayNextSpace target = InitPA("GetPutPAUserHeaderTest", 1, 16);
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

        /// <summary>
        ///A test for GetNextIndex
        ///</summary>
        [TestMethod()]
        public void GetNextIndexTest()
        {
            GetNextIndexTestAssert(1);
            GetNextIndexTestAssert(9);
            GetNextIndexTestAssert(3);
            GetNextIndexTestAssert(1000000);
            GetNextIndexTestAssert(27);
            
            try{
                GetNextIndexTestAssert(0);
                Assert.Fail("Should throw an exception");
            }catch (InvalidElementIndexException) { }
        }

        private void GetNextIndexTestAssert(int expected)
        {
            IPersistentArrayNextSpace target = InitPA("GetPANextIndexTest", 1, 16);
            try{
                target.Put(expected-1, new byte[0]);
                int actual = target.GetNextIndex();
                Assert.AreEqual(expected, actual);
            }
            finally
            {
                target.Close();
            }
        }

        /// <summary>
        ///A test for Put & Get
        ///</summary>
        [TestMethod()]
        public void PutGetTest()
        {
            PutGetTestAssert(5, new byte[] { 5, 6, 7 });
            PutGetTestAssert(1, new byte[] { 5, 6, 7 });
            PutGetTestAssert(0, new byte[] { 5, 6, 7 });
            PutGetTestAssert(1000000, new byte[] { 5, 6, 7 });
            PutGetTestAssert(10, new byte[] { 5, 6, 7 });

            try
            {
                PutGetTestAssert(-1, new byte[] { 5, 6, 7 });
                Assert.Fail("Should throw exception");
            }
            catch (InvalidElementIndexException) { }

            try
            {
                PutGetTestAssert(5, new byte[] { 1, 2, 3, 4,});
                Assert.Fail("Should throw exception");
            }
            catch (InternalBufferOverflowException) { }

            PutGetTestAssert(5, new byte[] { });
            PutGetTestAssert(5, new byte[] { byte.MaxValue, byte.MinValue });
            PutGetTestAssert(5, new byte[] { 0 });
        }

        private void PutGetTestAssert(int elementIndex, byte[] expected)
        {

            IPersistentArrayNextSpace target = InitPA("PAGetTest", 3, 16);
            try
            {
                target.Put(elementIndex, expected);
                byte[] actual = target.Get(elementIndex);
                TestHelper.AssertByteArraysAreSame(expected, actual);
            }
            finally
            {
                target.Close();
            }
        }
    }
}
