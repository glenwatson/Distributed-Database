//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace FilePersistenceUnitTest
//{
    
    
//    /// <summary>
//    ///This is a test class for HeaderTest and is intended
//    ///to contain all HeaderTest Unit Tests
//    ///</summary>
//    [TestClass()]
//    public class HeaderTest
//    {


//        private TestContext testContextInstance;

//        /// <summary>
//        ///Gets or sets the test context which provides
//        ///information about and functionality for the current test run.
//        ///</summary>
//        public TestContext TestContext
//        {
//            get
//            {
//                return testContextInstance;
//            }
//            set
//            {
//                testContextInstance = value;
//            }
//        }

//        #region Additional test attributes
//        // 
//        //You can use the following additional attributes as you write your tests:
//        //
//        //Use ClassInitialize to run code before running the first test in the class
//        //[ClassInitialize()]
//        //public static void MyClassInitialize(TestContext testContext)
//        //{
//        //}
//        //
//        //Use ClassCleanup to run code after all tests in a class have run
//        //[ClassCleanup()]
//        //public static void MyClassCleanup()
//        //{
//        //}
//        //
//        //Use TestInitialize to run code before running each test
//        //[TestInitialize()]
//        //public void MyTestInitialize()
//        //{
//        //}
//        //
//        //Use TestCleanup to run code after each test has run
//        //[TestCleanup()]
//        //public void MyTestCleanup()
//        //{
//        //}
//        //
//        #endregion


//        /// <summary>
//        ///A test for Header Constructor
//        ///</summary>
//        [TestMethod()]
//        public void HeaderConstructorTest()
//        {
//            HeaderConstructorTestAssert(7, 7, 7);
//            HeaderConstructorTestAssert(1, 0, 0);
//            HeaderConstructorTestAssert(int.MaxValue, int.MaxValue, 1000000);
//            HeaderConstructorTestAssert(1000000, 1000000, 1000000);

//            try
//            {
//                HeaderConstructorTestAssert(0, 5, 5);
//                Assert.Fail("Should throw an exception");
//            }
//            catch (InvalidElementSizeException) { }

//            try
//            {
//                HeaderConstructorTestAssert(6, -1, 2);
//                Assert.Fail("Should throw an exception");
//            }
//            catch (InvalidNextIndexException) { }
//            try
//            {
//                HeaderConstructorTestAssert(2, 2, -1);
//                Assert.Fail("Should throw an exception");
//            }
//            catch (InvalidUserHeaderException) { }
//        }

//        private static void HeaderConstructorTestAssert(int eleSize, int nextIdx, int userHeaderSize)
//        {
//            Header target = new Header(eleSize, nextIdx, userHeaderSize);
//            Assert.AreEqual(eleSize, target.ElementSize);
//            Assert.AreEqual(nextIdx, target.NextIndex);
//            Assert.AreEqual(userHeaderSize, target.UserHeaderSize);
//        }

//        /// <summary>
//        ///A test for UserHeaderSize
//        ///</summary>
//        [TestMethod()]
//        public void UserHeaderSizeTest()
//        {
//            UserHeaderSizeTestAssert(9);
//            UserHeaderSizeTestAssert(1000000);
//            try{
//                UserHeaderSizeTestAssert(-1);
//                Assert.Fail("Should throw an exception");
//            }
//            catch (InvalidUserHeaderException) { }
//        }

//        private static void UserHeaderSizeTestAssert(int expected)
//        {
//            Header target = new Header(7, 7, expected);
//            int actual = target.UserHeader.Size;
//            Assert.AreEqual(expected, actual);
//        }

        
//        /// <summary>
//        ///A test for HeaderSize
//        ///</summary>
//        [TestMethod()]
//        public void HeaderSizeTest()
//        {
//            HeaderSizeTestAssert(8, 8, 8);
//            HeaderSizeTestAssert(1, 1, 1);
//            HeaderSizeTestAssert(1000000, 1000000, 1000000);
//            try{
//                HeaderSizeTestAssert(1000000, 1000000, -1);
//                Assert.Fail("Should throw an exception");
//            }
//            catch (InvalidUserHeaderException) { }
//        }

//        private static void HeaderSizeTestAssert(int eleSize, int nextIdx, int uHeaderSize)
//        {
//            Header target = new Header(eleSize, nextIdx, uHeaderSize);
//            Assert.AreEqual(3*4 + uHeaderSize, target.HeaderSize);
//        }
        
//        /// <summary>
//        ///A test for SetNextIndex
//        ///</summary>
//        [TestMethod()]
//        public void SetNextIndexTest()
//        {
//            IStorage storage = new RandomAccessFile(TestConstants.TestDir + "SetNextIndexTest");

//            SetNextIndexTestAssert(9, storage);
//            try
//            {
//                SetNextIndexTestAssert(-1, storage);
//                Assert.Fail("Should throw an exception");
//            } catch (InvalidNextIndexException) { }
//            SetNextIndexTestAssert(0, storage);
//            SetNextIndexTestAssert(1000000, storage);


//            storage.Close();
//        }

//        private static void SetNextIndexTestAssert(int expected, IStorage storage)
//        {
//            Header target = new Header(3, 3, 3);
//            target.WriteToStorage(storage);
//            target.SetNextIndex(expected, storage);
//            Header read = Header.ReadFromStorage(storage);
//            int actual = read.NextIndex;
//            Assert.AreEqual(expected, actual);
//        }

//        /// <summary>
//        ///A test for WriteReadToStorage
//        ///</summary>
//        [TestMethod()]
//        public void WriteReadToStorageTest()
//        {
//            IStorage storage = new RandomAccessFile(TestConstants.TestDir + "WriteReadToStorageTest");

//            WriteReadToStorageTestAssert(8, 8, 8, storage);
//            WriteReadToStorageTestAssert(1, 1, 1, storage);
//            WriteReadToStorageTestAssert(1000000, 1000000, 1000000, storage);

//            try{
//                WriteReadToStorageTestAssert(0,0,0, storage);
//                Assert.Fail("Should throw an exception");
//            }
//            catch (InvalidElementSizeException) { }
//            try {
//                WriteReadToStorageTestAssert(0, 0, -1, storage);
//                Assert.Fail("Should throw an exception");
//            }
//            catch (InvalidElementSizeException) { }

//            storage.Close();
//        }

//        private static void WriteReadToStorageTestAssert(int eleSize, int nextIdx, int uHeaderSize, IStorage storage)
//        {
//            Header target = new Header(eleSize, nextIdx, uHeaderSize);
//            target.WriteToStorage(storage);
//            Header actual = Header.ReadFromStorage(storage);
//            AssertHeadersAreSame(target, actual);
//        }

//        private static void AssertHeadersAreSame(Header expected, Header actual)
//        {
//            Assert.AreEqual(expected.ElementSize, actual.ElementSize);
//            Assert.AreEqual(expected.NextIndex, actual.NextIndex);
//            UserHeaderTest.AssertUserHeadersAreSame(expected.UserHeader, actual.UserHeader);
//        }
//    }
//}
