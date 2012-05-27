//using System.IO;
//using PA;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Linq;
//using PersistentArrayUnitTest.HelperClasses;

//namespace PersistentArrayUnitTest
//{
    
//    /// <summary>
//    ///This is a test class for PersistentArrayTest and is intended
//    ///to contain all PersistentArrayTest Unit Tests
//    ///</summary>
//    [TestClass()]
//    public class PersistentArrayTest
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


//        [ClassInitialize()]
//        public static void MyClassInitialize(TestContext testContext)
//        {
//            TestConstants.CreateTestDir();
//        }

//        [ClassCleanup()]
//        public static void MyClassCleanup()
//        {
//            TestConstants.DeleteTestDir();
//        }

//        private int initEleSize = 5;
//        private int initUserHeaderSize = 10;
//        private PersistentNextSpaceArray InitPA(String name)
//        {
//            string arrayName = "test\\" + name;
//            PersistentNextSpaceArray.Delete(arrayName);
//            PersistentNextSpaceArray.Create(arrayName, initEleSize, initUserHeaderSize);
//            var pa = PersistentNextSpaceArray.Open(arrayName);
//            return pa;
//        }


//        /// <summary>
//        ///A test for PersistentNextSpaceArray Constructor
//        ///</summary>
//        [TestMethod()]
//        [DeploymentItem("PersistentNextSpaceArray.dll")]
//        public void PersistentArrayConstructorTest()
//        {
//            string filePath = TestConstants.TestDir + "ctorTest.arr";
//            PersistentArray_Accessor target = new PersistentArray_Accessor(filePath);
//            Assert.IsNotNull(target);
//            target.Close();
//        }

//        /// <summary>
//        ///A test for Close
//        ///</summary>
//        [TestMethod()]
//        public void CloseTest()
//        {
//            PrivateObject param0 = new PrivateObject(InitPA("CloseTest"));
//            PersistentArray_Accessor target = new PersistentArray_Accessor(param0);
//            target.Close();
//        }

//        /// <summary>
//        ///A test for Create
//        ///</summary>
//        [TestMethod()]
//        public void CreateTest()
//        {
//            string arrayName = "test\\CreateTest";
//            int elementSize = 30;
//            int userHeaderSize = 10;
//            PersistentNextSpaceArray.Delete(arrayName);
//            PersistentNextSpaceArray.Create(arrayName, elementSize, userHeaderSize);
//            Assert.IsTrue(File.Exists(TestConstants.TestDir + "CreateTest.arr"));
//        }

//        /// <summary>
//        ///A test for Delete
//        ///</summary>
//        [TestMethod()]
//        public void DeleteTest()
//        {
//            string arrayName = "test\\DeleteTest";
//            PersistentNextSpaceArray.Delete(arrayName);
//            PersistentNextSpaceArray.Create(arrayName, 1, 1);
//            PersistentNextSpaceArray.Delete(arrayName);
//            Assert.IsFalse(File.Exists(TestConstants.TestDir + arrayName + ".arr"));
//        }

//        /// <summary>
//        ///A test for FillSpaceWithBuffer
//        ///</summary>
//        [TestMethod()]
//        [DeploymentItem("PersistentNextSpaceArray.dll")]
//        public void FillSpaceWithBufferTest()
//        {
//            PrivateObject param0 = new PrivateObject(InitPA("FillSpaceWithBufferTest"));
//            PersistentArray_Accessor target = new PersistentArray_Accessor(param0);
//            byte[] buffer = new byte[] { 45, 6, 7 };
//            int spaceSize = 5;
//            target._header.SeekToAfterHeader(target._storage);
//            target.FillSpaceWithBuffer(buffer, spaceSize);

//            target._header.SeekToAfterHeader(target._storage);
//            byte[] actaul = new byte[spaceSize];
//            target._storage.ReadByteArray(actaul);

//            for (int i = 0; i < actaul.Length; i++)
//            {
//                if (i < buffer.Length)
//                {
//                    Assert.AreEqual(buffer[i], actaul[i]);
//                }
//                else
//                {
//                    Assert.AreEqual(0, actaul[i]);
//                }
//            }

//            target.Close();
//        }

//        /// <summary>
//        ///A test for Get
//        ///</summary>
//        [TestMethod()]
//        public void GetTest()
//        {
//            PrivateObject param0 = new PrivateObject(InitPA("GetTest"));
//            PersistentArray_Accessor target = new PersistentArray_Accessor(param0);
//            int elementIndex = 5;
//            byte[] expected = new byte[] { 5, 6, 7 };
//            byte[] actual;
//            target.Put(elementIndex, expected);
//            actual = target.Get(elementIndex);
//            for(int i=0; i<expected.Length; i++)
//                Assert.AreEqual(expected[i], actual[i]);
//            target.Close();
//        }

//        /// <summary>
//        ///A test for GetElementSize
//        ///</summary>
//        [TestMethod()]
//        public void GetElementSizeTest()
//        {
//            PrivateObject param0 = new PrivateObject(InitPA("GetElementSizeTest"));
//            PersistentArray_Accessor target = new PersistentArray_Accessor(param0);
//            int expected = initEleSize;
//            int actual;
//            actual = target.GetElementSize();
//            Assert.AreEqual(expected, actual);

//            target.Close();            
//        }

//        /// <summary>
//        ///A test for GetNextIndex
//        ///</summary>
//        [TestMethod()]
//        public void GetNextIndexTest()
//        {
//            PrivateObject param0 = new PrivateObject(InitPA("GetNextIndexTest"));
//            PersistentArray_Accessor target = new PersistentArray_Accessor(param0);
//            int expected = 0;
//            int actual;
//            actual = target.GetNextIndex();
//            Assert.AreEqual(expected, actual);

//            target.Put(0, new byte[1]);

//            int expected2 = 1;
//            int actual2;
//            actual2 = target.GetNextIndex();
//            Assert.AreEqual(expected2, actual2);

//            target.Close();
//        }

//        /// <summary>
//        ///A test for GetPathToName
//        ///</summary>
//        [TestMethod()]
//        [DeploymentItem("PersistentNextSpaceArray.dll")]
//        public void GetPathToArrayNameTest()
//        {
//            string arrayName = "test\\GetPathToArrayNameTest";
//            string expected = TestConstants.TestDir + "GetPathToArrayNameTest.arr";
//            string actual;
//            actual = PersistentArray_Accessor.GetPathToName(arrayName);
//            Assert.AreEqual(expected, actual);

//        }

//        /// <summary>
//        ///A test for GetUserHeader
//        ///</summary>
//        [TestMethod()]
//        public void GetUserHeaderTest()
//        {
//            PrivateObject param0 = new PrivateObject(InitPA("GetUserHeaderTest"));
//            PersistentArray_Accessor target = new PersistentArray_Accessor(param0);
//            byte[] expected = new byte[initUserHeaderSize];
//            byte[] actual;
//            actual = target.GetUserHeader();
//            for (int i = 0; i < expected.Length; i++)
//                Assert.AreEqual(expected[i], actual[i]);

//            target.Close();
//        }

//        /// <summary>
//        ///A test for GetUserHeaderSize
//        ///</summary>
//        [TestMethod()]
//        public void GetUserHeaderSizeTest()
//        {
//            PrivateObject param0 = new PrivateObject(InitPA("GetUserHeaderSizeTest"));
//            PersistentArray_Accessor target = new PersistentArray_Accessor(param0);
//            int expected = initUserHeaderSize;
//            int actual = target.GetUserHeaderSize();
//            Assert.AreEqual(expected, actual);

//            target.Close();
//        }

//        /// <summary>
//        ///A test for IncreaseNextIndex
//        ///</summary>
//        [TestMethod()]
//        [DeploymentItem("PersistentNextSpaceArray.dll")]
//        public void SetNextIndex()
//        {
//            PrivateObject param0 = new PrivateObject(InitPA("SetNextIndex"));
//            PersistentArray_Accessor target = new PersistentArray_Accessor(param0);
//            int expected = 256;
//            target.SetNextIndex(expected);
//            int actual = target.GetNextIndex();
//            Assert.AreEqual(expected, actual);

//            target.Close();
//        }

//        ///// <summary>
//        /////A test for InitializeFile
//        /////</summary>
//        //[TestMethod()]
//        //[DeploymentItem("PersistentNextSpaceArray.dll")]
//        //public void InitializeFileTest()
//        //{
//        //    string name = "InitializeFileTest";
//        //    Storage_Accessor storage = new RandomAccessFile_Accessor(name);
//        //    int elementSize = initEleSize;
//        //    int userHeaderSize = initUserHeaderSize;
//        //    PersistentArray_Accessor.InitializeFile(storage, elementSize, userHeaderSize);
//        //    storage.Seek(0);

//        //    byte[] headerBuffer = new byte[3*4];
//        //    storage.ReadByteArray(headerBuffer);
//        //    Assert.AreEqual(elementSize, headerBuffer[3]);
//        //    Assert.AreEqual(0, headerBuffer[7]);
//        //    Assert.AreEqual(userHeaderSize, headerBuffer[11]);
//        //    byte[] userHeaderBuffer = new byte[userHeaderSize];
//        //    storage.ReadByteArray(userHeaderBuffer);
//        //    foreach(byte b in userHeaderBuffer)
//        //        Assert.AreEqual(0, b);

//        //    storage.Close();
//        //}

//        ///// <summary>
//        /////A test for Open
//        /////</summary>
//        //[TestMethod()]
//        //public void OpenTest()
//        //{
//        //    string arrayName = "OpenTest";
//        //    PersistentNextSpaceArray expected = null; // TODO: Initialize to an appropriate value
//        //    PersistentNextSpaceArray actual;
//        //    actual = PersistentNextSpaceArray.Open(arrayName);
//        //    Assert.AreEqual(expected, actual);
//        //    Assert.Inconclusive("Verify the correctness of this test method.");
//        //}

//        ///// <summary>
//        /////A test for OpenStorage
//        /////</summary>
//        //[TestMethod()]
//        //[DeploymentItem("PersistentNextSpaceArray.dll")]
//        //public void OpenStorageTest()
//        //{
//        //    string filePath = testDir + "OpenStorageTest.arr";
//        //    Storage_Accessor expected = null; // TODO: Initialize to an appropriate value
//        //    Storage_Accessor actual;
//        //    actual = PersistentArray_Accessor.OpenStorage(filePath);
//        //    Assert.AreEqual(expected, actual);
//        //    Assert.Inconclusive("Verify the correctness of this test method.");
//        //}

//        /// <summary>
//        ///A test for Put
//        ///</summary>
//        [TestMethod()]
//        public void PutTest()
//        {
//            PrivateObject param0 = new PrivateObject(InitPA("PutTest"));
//            PersistentArray_Accessor target = new PersistentArray_Accessor(param0);
//            int elementIndex = 4;
//            byte[] buffer = new byte[] {45,6,7,};
//            target.Put(elementIndex, buffer);

//            byte[] actual = target.Get(elementIndex);

//            for (int i = 0; i < buffer.Length; i++)
//            {
//                Assert.AreEqual(buffer[i], actual[i]);
//            }

//            target.Close();
//        }

//        /// <summary>
//        ///A test for PutUserHeader
//        ///</summary>
//        [TestMethod()]
//        public void PutUserHeaderTest()
//        {
//            PrivateObject param0 = new PrivateObject(InitPA("PutUserHeaderTest"));
//            PersistentArray_Accessor target = new PersistentArray_Accessor(param0);
//            byte[] userHeader = new byte[]{2,3,4,5};
//            target.PutUserHeader(userHeader);

//            byte[] actual =  target.GetUserHeader();

//            for (int i = 0; i < userHeader.Length; i++)
//            {
//                Assert.AreEqual(userHeader[i], actual[i]);
//            }

//            target.Close();
//        }

//        #region Seek Tests
//        ///// <summary>
//        /////A test for RefreshHeader
//        /////</summary>
//        //[TestMethod()]
//        //[DeploymentItem("PersistentNextSpaceArray.dll")]
//        //public void RefreshHeaderTest()
//        //{
//        //    PrivateObject param0 = new PrivateObject(InitPA());
//        //    PersistentArray_Accessor target = new PersistentArray_Accessor(param0);
//        //    target.RefreshHeader();
//        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
//        //}

//        ///// <summary>
//        /////A test for SeekToElement
//        /////</summary>
//        //[TestMethod()]
//        //[DeploymentItem("PersistentNextSpaceArray.dll")]
//        //public void SeekToElementTest()
//        //{
//        //    PrivateObject param0 = new PrivateObject(InitPA());
//        //    PersistentArray_Accessor target = new PersistentArray_Accessor(param0);
//        //    int index = 0; // TODO: Initialize to an appropriate value
//        //    target.SeekToElement(index);
//        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
//        //}

//        ///// <summary>
//        /////A test for SeekToElementSize
//        /////</summary>
//        //[TestMethod()]
//        //[DeploymentItem("PersistentNextSpaceArray.dll")]
//        //public void SeekToElementSizeTest()
//        //{
//        //    PrivateObject param0 = new PrivateObject(InitPA());
//        //    PersistentArray_Accessor target = new PersistentArray_Accessor(param0);
//        //    target.SeekToElementSize();
//        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
//        //}

//        ///// <summary>
//        /////A test for SeekToFirstElement
//        /////</summary>
//        //[TestMethod()]
//        //[DeploymentItem("PersistentNextSpaceArray.dll")]
//        //public void SeekToFirstElementTest()
//        //{
//        //    PrivateObject param0 = new PrivateObject(InitPA());
//        //    PersistentArray_Accessor target = new PersistentArray_Accessor(param0);
//        //    target.SeekToFirstElement();
//        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
//        //}

//        ///// <summary>
//        /////A test for SeekToNextIndex
//        /////</summary>
//        //[TestMethod()]
//        //[DeploymentItem("PersistentNextSpaceArray.dll")]
//        //public void SeekToNextIndexTest()
//        //{
//        //    PrivateObject param0 = new PrivateObject(InitPA());
//        //    PersistentArray_Accessor target = new PersistentArray_Accessor(param0);
//        //    target.SeekToNextIndex();
//        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
//        //}

//        ///// <summary>
//        /////A test for SeekToUserHeader
//        /////</summary>
//        //[TestMethod()]
//        //[DeploymentItem("PersistentNextSpaceArray.dll")]
//        //public void SeekToUserHeaderTest()
//        //{
//        //    PrivateObject param0 = new PrivateObject(InitPA());
//        //    PersistentArray_Accessor target = new PersistentArray_Accessor(param0);
//        //    target.SeekToUserHeader();
//        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
//        //}

//        ///// <summary>
//        /////A test for SeekToUserHeaderSize
//        /////</summary>
//        //[TestMethod()]
//        //[DeploymentItem("PersistentNextSpaceArray.dll")]
//        //public void SeekToUserHeaderSizeTest()
//        //{
//        //    PrivateObject param0 = new PrivateObject(InitPA());
//        //    PersistentArray_Accessor target = new PersistentArray_Accessor(param0);
//        //    target.SeekToUserHeaderSize();
//        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
//        //}
//        #endregion

//        /// <summary>
//        ///A test for UpdateUserHeader
//        /////</summary>
//        [TestMethod()]
//        [DeploymentItem("PersistentNextSpaceArray.dll")]
//        public void UpdateUserHeaderTest()
//        {
//            PrivateObject param0 = new PrivateObject(InitPA("UpdateUserHeaderTest"));
//            PersistentArray_Accessor target = new PersistentArray_Accessor(param0);
//            byte[] userHeader = new byte[] {8,9,10};
//            target.UpdateUserHeader(userHeader);

//            byte[] actual = target.GetUserHeader();

//            for (int i = 0; i < userHeader.Length; i++)
//            {
//                Assert.AreEqual(userHeader[i], actual[i]);
//            }

//            target.Close();
//        }

//        ///// <summary>
//        /////A test for WriteZeros
//        /////</summary>
//        //[TestMethod()]
//        //[DeploymentItem("PersistentNextSpaceArray.dll")]
//        //public void WriteZerosTest()
//        //{
//        //    PrivateObject param0 = new PrivateObject(InitPA());
//        //    PersistentArray_Accessor target = new PersistentArray_Accessor(param0);
//        //    int count = 6; // TODO: Initialize to an appropriate value
//        //    target.WriteZeros(count);
//        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
//        //}
//    }
//}
