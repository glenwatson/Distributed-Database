using System.IO;
using ByteHelper;
using ByteHelperUnitTest;
using FilePersistence;
using FilePersistence.Exceptions;
using Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace FilePersistenceUnitTest
{


    /// <summary>
    ///This is a test class for UserHeaderTest and is intended
    ///to contain all UserHeaderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UserHeaderTest
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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        private IStorage InitStorage(string filePath)
        {
            return new RandomAccessFile(File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None));
        }

        /// <summary>
        ///A test for UserHeader Constructor
        ///</summary>
        [TestMethod()]
        public void UserHeaderConstructorTest()
        {
            UserHeaderConstructorSizeTest(1000000);
            try
            {
                UserHeaderConstructorSizeTest(-1);
                Assert.Fail("Should throw exception");
            }
            catch (InvalidUserHeaderException) { }
            UserHeaderConstructorSizeTest(1);
            UserHeaderConstructorSizeTest(0);
        }

        private void UserHeaderConstructorSizeTest(int size)
        {
            UserHeader target = new UserHeader(size);
            Assert.IsNotNull(target);
            Assert.AreEqual(target.Size, size);
        }

        /// <summary>
        ///A test for Data
        ///</summary>
        [TestMethod()]
        public void DataTest()
        {
            DataTestAssert(new byte[] { 5, 6, 7, 8, 9 });
            DataTestAssert(new byte[] { });
            DataTestAssert(new byte[] { 0 });
            try
            {
                DataTestAssert(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 });
                Assert.Fail("Should throw exception");
            }
            catch (InvalidUserHeaderException) { }
            DataTestAssert(new byte[] { byte.MaxValue, byte.MinValue });
        }

        private void DataTestAssert(byte[] data)
        {
            UserHeader target = new UserHeader(9);
            target.Data = data;
            byte[] actual = target.Data;
            TestHelper.AssertByteArraysAreSame(data, actual);
        }

        #region WriteToStorage

        /// <summary>
        ///A test for WriteToStorage
        ///</summary>
        [TestMethod()]
        public void WriteToStorageTest()
        {
            IStorage storage = InitStorage("WriteToStorageTest");

            int size = 8;
            byte[] data = new byte[] { byte.MaxValue, byte.MinValue };
            WriteToStorageTestAssert(storage, size, data);

            int size2 = 10000;
            byte[] data2 = new byte[] { };
            WriteToStorageTestAssert(storage, size2, data2);
            try
            {
                WriteToStorageTestAssert(storage, -1, data2);
                Assert.Fail("Should throw exception");
            }
            catch (InvalidUserHeaderException) { }

            int size3 = 10;
            byte[] data3 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            WriteToStorageTestAssert(storage, size3, data3);


            storage.Close();
        }

        private void WriteToStorageTestAssert(IStorage storage, int size, byte[] data)
        {
            var target = WriteToStorageTestHelper(storage, size, data);

            UserHeader actual = ReadUserHeader(storage);

            AssertUserHeadersAreSame(target, actual);
        }

        private UserHeader ReadUserHeader(IStorage storage)
        {
            int userHeaderSizeSize = UserHeader.GetSizeSize();
            byte[] userHeaderSizeBytes = new byte[userHeaderSizeSize];
            storage.Seek(UserHeader.GetUserHeaderPosition());
            storage.ReadByteArray(userHeaderSizeBytes);

            int userHeaderSize = UserHeader.GetSizeFromData(userHeaderSizeBytes);
            byte[] userHeaderBytes = new byte[userHeaderSize];
            storage.ReadByteArray(userHeaderBytes);

            return UserHeader.Deserialize(userHeaderSizeBytes.Append(userHeaderBytes));
        }

        #endregion

        #region ReadFromStorage
        /// <summary>
        ///A test for ReadFromStorage
        ///</summary>
        [TestMethod()]
        public void ReadFromStorageTest()
        {
            IStorage storage = new RandomAccessFile(File.Open("ReadFromStorage", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None));

            int size = 8;
            byte[] data = new byte[] { byte.MaxValue, byte.MinValue };
            ReadFromStorageTestAssert(storage, size, data);

            int size2 = 1000000;
            byte[] data2 = new byte[] { };
            ReadFromStorageTestAssert(storage, size2, data2);

            try
            {
                ReadFromStorageTestAssert(storage, -1, data2);
                Assert.Fail("Should throw exception");
            }
            catch (InvalidUserHeaderException) { }

            int size3 = 10;
            byte[] data3 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            ReadFromStorageTestAssert(storage, size3, data3);

            storage.Close();
        }

        private void ReadFromStorageTestAssert(IStorage storage, int size, byte[] data)
        {
            UserHeader expected = WriteToStorageTestHelper(storage, size, data);
            UserHeader actual = ReadFromStorageTestHelper(storage);
            AssertUserHeadersAreSame(expected, actual);
        }

        #endregion

        public static void AssertUserHeadersAreSame(UserHeader expected, UserHeader actual)
        {
            Assert.AreEqual(expected.Size, actual.Size);
            TestHelper.AssertByteArraysAreSame(expected.Data, actual.Data);
        }

        private static UserHeader WriteToStorageTestHelper(IStorage storage, int size, byte[] data)
        {
            storage.Seek(UserHeader.GetUserHeaderPosition());
            UserHeader target = new UserHeader(size);
            target.Data = data.ExtendTo(size);
            storage.WriteByteArray(target.Serialize());
            return target;
        }

        private static UserHeader ReadFromStorageTestHelper(IStorage storage)
        {
            int userHeaderSizeSize = UserHeader.GetSizeSize();
            byte[] userHeaderSizeBytes = new byte[userHeaderSizeSize];
            storage.Seek(UserHeader.GetUserHeaderPosition());
            storage.ReadByteArray(userHeaderSizeBytes);

            int userHeaderSize = UserHeader.GetSizeFromData(userHeaderSizeBytes);
            byte[] userHeaderBytes = new byte[userHeaderSize];
            storage.ReadByteArray(userHeaderBytes);

            return UserHeader.Deserialize(userHeaderSizeBytes.Append(userHeaderBytes));
        }
    }
}
