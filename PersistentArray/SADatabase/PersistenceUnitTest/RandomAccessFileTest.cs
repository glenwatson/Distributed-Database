using System.IO;
using ByteHelperUnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace PersistenceUnitTest
{
    
    
    /// <summary>
    ///This is a test class for RandomAccessFileTest and is intended
    ///to contain all RandomAccessFileTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RandomAccessFileTest
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

        private RandomAccessFile InitRAF(string filePath)
        {
            return new RandomAccessFile(File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None));
        }


        /// <summary>
        ///A test for RandomAccessFile Constructor
        ///</summary>
        [TestMethod()]
        public void RandomAccessFileConstructorTest()
        {
            string filePath = "RAFCtor.arr";
            RandomAccessFile target = InitRAF(filePath);
            Assert.IsNotNull(target);

            target.Close();
        }

        /// <summary>
        ///A test for Close
        ///</summary>
        [TestMethod()]
        public void CloseTest()
        {
            string filePath = "RAFCloseTest.arr";
            RandomAccessFile target = InitRAF(filePath);
            target.Close();

            Assert.IsTrue(File.Exists(filePath));

            target.Close();
        }

        /// <summary>
        ///A test for ReadInt
        ///</summary>
        [TestMethod()]
        public void ReadIntTest()
        {
            string filePath = "RAFReadIntTest.arr";
            RandomAccessFile target = InitRAF(filePath);
            int expected = 88;
            target.WriteInt(expected);
            target.Seek(0);

            int actual = target.ReadInt();
            Assert.AreEqual(expected, actual);

            target.Close();
        }

        /// <summary>
        ///A test for ReadByte
        ///</summary>
        [TestMethod()]
        public void ReadByteTest()
        {
            string filePath = "RAFReadByteTest.arr";
            RandomAccessFile target = InitRAF(filePath);
            byte expected = 88;
            target.WriteByte(expected);
            target.Seek(0);

            byte actual = target.ReadByte();
            Assert.AreEqual(expected, actual);

            target.Close();
        }

        /// <summary>
        ///A test for ReadByteArray
        ///</summary>
        [TestMethod()]
        public void ReadByteArrayTest()
        {
            string filePath = "RAFReadByteArrayTest.arr";
            RandomAccessFile target = InitRAF(filePath);
            byte[] buffer = new byte[2];
            target.ReadByteArray(buffer);
            for (int i = 0; i < buffer.Length; i++)
            {
                Assert.AreEqual(0, buffer[i]);
            }

            target.Close();
        }

        /// <summary>
        ///A test for Seek
        ///</summary>
        [TestMethod()]
        public void SeekTest()
        {
            string filePath = "RAFSeekTest.arr";
            RandomAccessFile target = InitRAF(filePath);
            int position = 3;
            target.Seek(position);

            target.Close();
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [TestMethod()]
        public void WriteByteTest()
        {
            string filePath = "WriteByteTest.arr";
            RandomAccessFile target = InitRAF(filePath);
            byte b = 7;
            target.WriteByte(b);

            target.Seek(0);
            byte actual = target.ReadByte();

            Assert.AreEqual(b, actual);

            target.Close();
        }

        /// <summary>
        ///A test for WriteInt
        ///</summary>
        [TestMethod()]
        public void WriteIntTest()
        {
            string filePath = "RAFWriteIntTest.arr";
            RandomAccessFile target = InitRAF(filePath);
            int expected = 300;
            target.WriteInt(expected);

            target.Seek(0);
            int actual = target.ReadInt();

            Assert.AreEqual(expected, actual);

            target.Close();
        }

        /// <summary>
        ///A test for WriteByteArray
        ///</summary>
        [TestMethod()]
        public void WriteByteArrayTest()
        {
            string filePath = "RAFWriteByteArrayTest.arr";
            RandomAccessFile target = InitRAF(filePath);
            byte[] buffer = new byte[] {5, 6, 7,};
            target.WriteByteArray(buffer);
            target.Seek(0);

            byte[] actual = new byte[buffer.Length];
            target.ReadByteArray(actual);

            for (int i = 0; i < buffer.Length;i++ )
                Assert.AreEqual(buffer[i], actual[i]);

            target.Close();
        }

        /// <summary>
        ///A test for WriteByteArray
        ///</summary>
        [TestMethod()]
        public void GetFileNameTest()
        {
            string filePath = "RAFGetFileNameTest.arr";
            RandomAccessFile target = InitRAF(filePath);
            Assert.IsTrue(File.Exists(target.GetFileName()));

            target.Close();
        }
    }
}
