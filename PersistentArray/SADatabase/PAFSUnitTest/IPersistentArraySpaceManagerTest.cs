using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ByteHelperUnitTest;
using FilePersistence.Exceptions;
using Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PA.Exceptions;
using PAFS;

namespace PAFSUnitTest
{
    /// <summary>
    /// Summary description for IPersistentArraySpaceManagerTest
    /// </summary>
    [TestClass]
    public class IPersistentArraySpaceManagerTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
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

        private IPersistentCollectionSpaceManager InitPAFS(string arrayName, int elementSize, int uHeaderSize)
        {
            PersistentCollectionSpaceManager pcsm;
            try
            {
                pcsm = new PersistentCollectionSpaceManager(arrayName, elementSize, uHeaderSize);
            }
            catch (FileNameConflictException)
            {
                pcsm = new PersistentCollectionSpaceManager(arrayName);
                pcsm.Delete();
                pcsm = new PersistentCollectionSpaceManager(arrayName, elementSize, uHeaderSize);
            }
            return pcsm;
        }

        [TestMethod]
        public void ReopenTest()
        {
            const string arrayName = "ReopenPCSMWithSpaceManagerTest";
            const int elementSize = 16;
            const int userHeaderSize = 8;
            IPersistentCollectionSpaceManager pafs = InitPAFS(arrayName, elementSize, userHeaderSize);
            try
            {
                byte[] userHeader = new byte[userHeaderSize] {1, 2, 3, 4, 5, 6, 7, 8};
                pafs.PutUserHeader(userHeader);
                int index = 0;
                byte[] expectedBytes = new byte[]{1,2,3,4,5,6,7,8,9,0};
                pafs.Put(index, expectedBytes);

                pafs.Close();
                pafs = new PersistentCollectionSpaceManager(arrayName);

                Assert.AreEqual(elementSize, pafs.GetElementSize());
                Assert.AreEqual(userHeaderSize, pafs.GetUserHeaderSize());
                TestHelper.AssertByteArraysAreSame(userHeader, pafs.GetUserHeader());
                TestHelper.AssertByteArraysAreSame(expectedBytes, pafs.Get(index));
            }
            finally
            {
                pafs.Close();
            }
        }

        [TestMethod]
        public void AllocateBlockTest()
        {
            IPersistentCollectionSpaceManager pafs = InitPAFS("PCSMAllocateBlock", 16, 8);
            try
            {
                int token = pafs.AllocateBlock();
                Assert.AreEqual(0, token);

                int token2 = pafs.AllocateBlock();
                Assert.AreEqual(0, token2);

                pafs.Put(token2, new byte[0]);
                int token3 = pafs.AllocateBlock();
                Assert.AreEqual(1, token3);

                pafs.Put(token3, new byte[0]);
                int token4 = pafs.AllocateBlock();
                Assert.AreEqual(2, token4);
            }
            finally
            {
                pafs.Close();
            }
        }

        [TestMethod]
        public void FreeBlockTest()
        {
            IPersistentCollectionSpaceManager pafs = InitPAFS("PCSMFreeBlock", 16, 8);
            try
            {
                int token1 = pafs.AllocateBlock();
                int token2 = pafs.AllocateBlock();
                int token3 = pafs.AllocateBlock();
                int token4 = pafs.AllocateBlock();

                pafs.FreeBlock(token4);
                pafs.FreeBlock(token3);
                pafs.FreeBlock(token2);
                pafs.FreeBlock(token1);
            }
            finally
            {
                pafs.Close();
            }

        }

        [TestMethod]
        public void GetUserHeaderTest()
        {
            int uHeaderSize = 8;
            IPersistentCollectionSpaceManager pafs = InitPAFS("GetPCSMUserHeader", 16, uHeaderSize);
            try
            {
                byte[] uHeader = pafs.GetUserHeader();

                Assert.AreEqual(uHeaderSize, uHeader.Length);

                TestHelper.AssertByteArraysAreSame(new byte[0], uHeader);

                byte[] newUserHeader = new byte[] {1, 2, 3, 4, 5, 6, 7, 8};
                pafs.PutUserHeader(newUserHeader);
                byte[] actual = pafs.GetUserHeader();

                TestHelper.AssertByteArraysAreSame(newUserHeader, actual);
            }
            finally
            {
                pafs.Close();
            }
        }
        
        [TestMethod]
        public void GetHeaderTest()
        {
            IPersistentCollectionSpaceManager pafs = InitPAFS("GetPCSMHeader", 16, 20);
            try
            {
                byte[] userheader = new byte[]{1,2,3,4,5,6,7,8,9,100,110,120,130,140,150,210,220,230,240,255};
                pafs.PutUserHeader(userheader);

                TestHelper.AssertByteArraysAreSame(userheader, pafs.GetUserHeader());


                try {
                    pafs.PutUserHeader(new byte[21]);
                    Assert.Fail("Should throw an exception");
                } catch (InvalidUserHeaderException) {}
            }
            finally
            {
                pafs.Close();
            }
        }
        [TestMethod]
        public void GetUserHeaderSizeTest()
        {
            GetUserHeaderSizeTestAssert(5);
            GetUserHeaderSizeTestAssert(20);
            GetUserHeaderSizeTestAssert(80);
        }

        private void GetUserHeaderSizeTestAssert(int uHeaderSize)
        {
            IPersistentCollectionSpaceManager pafs = InitPAFS("PCSMGetUserHeaderSizeTestAssert", 16, uHeaderSize);
            try
            {
                Assert.AreEqual(uHeaderSize, pafs.GetUserHeaderSize());
            }
            finally
            {
                pafs.Close();
            }
        }
        
        [TestMethod]
        public void CreateTooSmallTest()
        {
            PersistentCollectionSpaceManager pcsm = null;
            try
            {
                pcsm = new PersistentCollectionSpaceManager("PCSMCreateTooSmall", 3, 3);
                Assert.Fail("Should throw an exception");
            } catch (InvalidElementSizeException)
            {
                if (pcsm != null) 
                    pcsm.Delete();
            }
        }

        [TestMethod]
        public void CreateTest()
        {
            //prime the pump
            string arrayName = "arrayName";
            PersistentCollectionSpaceManager pcsm = (PersistentCollectionSpaceManager) InitPAFS(arrayName, 16, 4);
            pcsm.Delete();
            pcsm = null;
            PersistentCollectionSpaceManager pcsm2 = new PersistentCollectionSpaceManager(arrayName, 16, 4);
            try
            {
                pcsm=new PersistentCollectionSpaceManager(arrayName, 16, 4);
                Assert.Fail("Should throw an exception");
            }catch (FileNameConflictException)
            {
                if(pcsm != null)
                    pcsm.Delete();
            }
        }

        [TestMethod]
        public void GetNextIndexTest()
        {
            IPersistentCollectionSpaceManager pafs = InitPAFS("PCSMGetNextIndex", 16, 8);
            try
            {
                //should just wrap AllocateBlock()
                int block = pafs.AllocateBlock();
                int nextSpace = pafs.GetNextIndex();
                Assert.AreEqual(block, nextSpace);

                pafs.Put(block, new byte[pafs.GetElementSize()]);

                int block2 = pafs.AllocateBlock();
                int nextSpace2 = pafs.GetNextIndex();
                Assert.AreEqual(block2, nextSpace2);
            }
            finally
            {
                pafs.Close();
            }
        }

        [TestMethod]
        public void WipeIndexTest()
        {
            const int elementSize = 10;
            IPersistentCollectionSpaceManager pafs = InitPAFS("PCSMWipeIndex", elementSize, 8);
            try
            {
                int block = pafs.AllocateBlock();
                byte[] putData = new byte[elementSize] {255, 255, 255, 255, 255, 255, 255, 255, 255, 255};
                pafs.Put(block, putData);
                //make sure the data was written
                TestHelper.AssertByteArraysAreSame(putData, pafs.Get(block));

                pafs.WipeElement(block);
                TestHelper.AssertByteArraysAreSame(new byte[elementSize], pafs.Get(block));
            }
            finally
            {
                pafs.Close();
            }
        }
    }
}
