using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ByteHelperUnitTest;
using FilePersistence.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PA.Exceptions;
using PLL;
using PLL.Exceptions;

namespace PLLUnitTest
{
    /// <summary>
    /// Summary description for UnitTest2
    /// </summary>
    [TestClass]
    public class PersistentLinkedListTest
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

        private PersistentLinkedList InitPLL(string arrayName, int elementSize, int userHeaderSize)
        {
            PersistentLinkedList pll;
            try
            {
                pll = new PersistentLinkedList(arrayName, elementSize, userHeaderSize);
            }
            catch (FileNameConflictException)
            {
                pll = new PersistentLinkedList(arrayName);
                pll.Delete();
                pll = new PersistentLinkedList(arrayName, elementSize, userHeaderSize);                
            }
            return pll;
        }

        [TestMethod]
        public void CreateTest()
        {
            string arrayName = "CreateLLTest";
            ILinkedList ll = InitPLL(arrayName, 16, 8);
            try
            {
                Assert.IsTrue(File.Exists("C:\\DB\\" + arrayName + ".db"));
            }
            finally
            {
                ll.Close();
            }
        }

        [TestMethod]
        public void DeleteTest()
        {
            string llName = "LLDelete";
            ILinkedList ll = InitPLL(llName, 16, 8);
            try
            {
                ll.Delete();
                ll = new PersistentLinkedList(llName, 16, 16);
            }
            finally
            {
                ll.Delete();
            }
        }
        
        [TestMethod]
        public void AssertNotBadNodeIndexTest()
        {
            ILinkedList ll = InitPLL("AssertNotBadNodeIndex", 16, 8);
            try
            {
                try
                {
                    ll.AddBefore(-1, new byte[0]);
                    Assert.Fail("Should throw an exception");
                }
                catch (InvalidNodeReference) { }
            }
            finally
            {
                ll.Close();
            }
        }

        [TestMethod]
        public void InsertThrowsTest()
        {
            ILinkedList ll = InitPLL("LLInsertThrowsTest", 16, 8);
            try
            {
                try
                {
                    ll.Remove(ll.AddToStart(new byte[0]));
                    ll.AddAfter(1, new byte[0]);
                    Assert.Fail("Should throw an exception");
                } catch (InvalidNodeReference) { }
            }
            finally
            {
                ll.Close();
            }
        }

        [TestMethod]
        public void InsertLinksCorrectlyTest()
        {
            ILinkedList ll = InitPLL("LLInsertLinksCorrectly", 16, 8);
            try
            {
                int token1 = ll.AddToEnd(new byte[0]);
                int token2 = ll.AddToEnd(new byte[0]);
                int token3 = ll.AddToEnd(new byte[0]);
                int token4 = ll.AddToEnd(new byte[0]);
                int token5 = ll.AddToEnd(new byte[0]);

                Assert.AreEqual(token1, ll.GetFirst());
                Assert.AreEqual(token2, ll.GetNext(token1));
                Assert.AreEqual(token3, ll.GetNext(token2));
                Assert.AreEqual(token4, ll.GetNext(token3));
                Assert.AreEqual(token5, ll.GetNext(token4));
                Assert.AreEqual(0, ll.GetNext(token5));

                Assert.AreEqual(token5, ll.GetLast());
                Assert.AreEqual(token4, ll.GetPrevious(token5));
                Assert.AreEqual(token3, ll.GetPrevious(token4));
                Assert.AreEqual(token2, ll.GetPrevious(token3));
                Assert.AreEqual(token1, ll.GetPrevious(token2));
                Assert.AreEqual(0, ll.GetPrevious(token1));
            }
            finally
            {
                ll.Close();
            }
        }

        [TestMethod]
        public void AddToStartTest()
        {
            ILinkedList ll = InitPLL("LLAddToStart", 16, 8);
            try
            {
                AddToStartTestAssert(ll, new byte[]{1,2,3,4,5,6,7,8});
                AddToStartTestAssert(ll, new byte[] { 12,34,56,78 });
                AddToStartTestAssert(ll, new byte[] { 1,9,16,25,36,49,64 });
            }
            finally
            {
                ll.Close();
            }
        }

        private static void AddToStartTestAssert(ILinkedList ll, byte[] data)
        {
            int token = ll.AddToStart(data);
            byte[] actual = ll.GetData(token);
            TestHelper.AssertByteArraysAreSame(data, actual);
        }

        [TestMethod]
        public void AddToEndTest()
        {
            ILinkedList ll = InitPLL("LLAddToEnd", 16, 8);
            try
            {
                int token1 = AddToEndTestAssert(ll, new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
                int token2 = AddToEndTestAssert(ll, new byte[] { 12, 34, 56, 78, 90 });
                int token3 = AddToEndTestAssert(ll, new byte[] { 1, 4, 9, 16, 25, 36, 49, 4 });
                int token4 = AddToEndTestAssert(ll, new byte[] { 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16 });

                Assert.AreEqual(token2, ll.GetNext(token1));
                Assert.AreEqual(token3, ll.GetNext(token2));
                Assert.AreEqual(token4, ll.GetNext(token3));
                Assert.AreEqual(0, ll.GetNext(token4));

                Assert.AreEqual(token3, ll.GetPrevious(token4));
                Assert.AreEqual(token2, ll.GetPrevious(token3));
                Assert.AreEqual(token1, ll.GetPrevious(token2));
                Assert.AreEqual(0, ll.GetPrevious(token1));

            }
            finally
            {
                ll.Close();
            }
        }

        private int AddToEndTestAssert(ILinkedList ll, byte[] data)
        {
            int token = ll.AddToEnd(data);

            byte[] actual = ll.GetData(token);
            TestHelper.AssertByteArraysAreSame(data, actual);
            return token;
        }

        [TestMethod]
        public void AddBeforeTest()
        {
            ILinkedList ll = InitPLL("LLAddBefore", 16, 8);
            try
            {
                AddBeforeTestAssert(ll, 0, new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
                AddBeforeTestAssert(ll, 1, new byte[] { 19 });
                AddBeforeTestAssert(ll, 2, new byte[] { 2,4,8,16,32,64 });
                AddBeforeTestAssert(ll, 1, new byte[] { 13,24,35,46,57,68,79,80 });
            }
            finally
            {
                ll.Close();
            }
        }

        private void AddBeforeTestAssert(ILinkedList ll, int index, byte[] data)
        {
            int token = ll.AddBefore(index, data);
            byte[] actual = ll.GetData(token);
            TestHelper.AssertByteArraysAreSame(data, actual);
        }

        [TestMethod]
        public void AddAfterTest()
        {
            ILinkedList ll = InitPLL("LLAddAfter", 16, 8);
            try
            {
                int token = AddAfterTestAssert(ll, 0, new byte[]{1,2,3,4,5,6,7,8});
                token = AddAfterTestAssert(ll, token, new byte[] { 6, 6, 6 });
                token = AddAfterTestAssert(ll, token, new byte[] { 255, 255, 255, 255, 255 });
                token = AddAfterTestAssert(ll, token, new byte[] { 170, 170, 170, 170 });
            }
            finally
            {
                ll.Close();
            }
        }

        private static int AddAfterTestAssert(ILinkedList ll, int token, byte[] data)
        {
            int insertedToken = ll.AddAfter(token, data);
            byte[] actual = ll.GetData(insertedToken);
            TestHelper.AssertByteArraysAreSame(data, actual);
            return insertedToken;
        }

        [TestMethod]
        public void UpdateTest()
        {
            ILinkedList ll = InitPLL("LLUpdate", 16, 8);
            try
            {
                byte[] dummyData = new byte[0];
                ll.AddToStart(dummyData);
                ll.AddToStart(dummyData);
                ll.AddToStart(dummyData);
                ll.AddToStart(dummyData);

                UpdateTestAssert(ll, 0, new byte[] {1, 2, 3, 4, 5, 6, 7, 8});
                UpdateTestAssert(ll, 3, new byte[] { 1 });
                UpdateTestAssert(ll, 1, new byte[] { 255 });
            }
            finally
            {
                ll.Close();
            }
        }

        private static void UpdateTestAssert(ILinkedList ll, int index, byte[] data)
        {
            ll.Update(index, data);
            byte[] actual = ll.GetData(index);
            TestHelper.AssertByteArraysAreSame(data, actual);
        }

        [TestMethod]
        public void GetFirstTest()
        {
            ILinkedList ll = InitPLL("GetLLFirst", 16, 8);
            try
            {
                Assert.AreEqual(0, ll.GetFirst());

                int first = ll.AddToStart(new byte[0]);

                Assert.AreEqual(first, ll.GetFirst());

                first = ll.AddToStart(new byte[0]);

                Assert.AreEqual(first, ll.GetFirst());

                int last = ll.AddToEnd(new byte[0]);

                Assert.AreEqual(first, ll.GetFirst());
            }
            finally
            {
                ll.Close();
            }
        }

        [TestMethod]
        public void GetLastTest()
        {
            ILinkedList ll = InitPLL("GetLLLast", 16, 8);
            try
            {
                Assert.AreEqual(0, ll.GetLast());

                int last = ll.AddToEnd(new byte[0]);

                Assert.AreEqual(last, ll.GetLast());

                last = ll.AddToEnd(new byte[0]);

                Assert.AreEqual(last, ll.GetLast());

                int start = ll.AddToStart(new byte[0]);

                Assert.AreEqual(last, ll.GetLast());
            }
            finally
            {
                ll.Close();
            }
        }

        [TestMethod]
        public void GetNextTest()
        {
            ILinkedList ll = InitPLL("GetLLNext", 16, 8);
            try
            {
                ll.AddToEnd(new byte[] { 1 });
                ll.AddToEnd(new byte[] { 2 });
                ll.AddToEnd(new byte[] { 3 });
                ll.AddToEnd(new byte[] { 4 });

                Assert.AreEqual(1, ll.GetNext(0));
                Assert.AreEqual(2, ll.GetNext(1));
                Assert.AreEqual(3, ll.GetNext(2));
                Assert.AreEqual(4, ll.GetNext(3));
                Assert.AreEqual(0, ll.GetNext(4));

                ll.Remove(2);
                Assert.AreEqual(3, ll.GetNext(1));

            }
            finally
            {
                ll.Close();
            }
        }

        [TestMethod]
        public void GetPreviousTest()
        {
            ILinkedList ll = InitPLL("GetLLPrevious", 16, 8);
            try
            {
                ll.AddToEnd(new byte[] { 1 });
                ll.AddToEnd(new byte[] { 2 });
                ll.AddToEnd(new byte[] { 3 });
                ll.AddToEnd(new byte[] { 4 });

                Assert.AreEqual(0, ll.GetPrevious(1));
                Assert.AreEqual(1, ll.GetPrevious(2));
                Assert.AreEqual(2, ll.GetPrevious(3));
                Assert.AreEqual(3, ll.GetPrevious(4));
                Assert.AreEqual(4, ll.GetPrevious(0));

                ll.Remove(2);
                Assert.AreEqual(1, ll.GetPrevious(3));

            }
            finally
            {
                ll.Close();
            }
        }

        [TestMethod]
        public void GetDataTest()
        {
            ILinkedList ll = InitPLL("GetLLData", 16, 8);
            try
            {
                GetDataTestAssert(ll, new byte[]{1});
                GetDataTestAssert(ll, new byte[]{2});
                GetDataTestAssert(ll, new byte[]{3});
                GetDataTestAssert(ll, new byte[]{4});
            }
            finally
            {
                ll.Close();
            }
        }

        private static void GetDataTestAssert(ILinkedList ll, byte[] data)
        {
            int token = ll.AddToStart(data);
            byte[] actual = ll.GetData(token);
            TestHelper.AssertByteArraysAreSame(data, actual);
        }

        [TestMethod]
        public void RemoveTest()
        {

            RemoveFromEmptyTest();
            RemoveFromBeginningTest();
            RemoveFromMiddleTest();
            RemoveFromEndTest();

        }

        private void RemoveFromEmptyTest()
        {
            ILinkedList ll2 = InitPLL("LLRemove", 16, 8);
            try
            {
                ll2.Remove(0);
                Assert.Fail("Should throw exception");
            }
            catch (InvalidOperationException) { }
            finally
            {
                ll2.Close();
            }
        }

        private void RemoveFromBeginningTest()
        {
            ILinkedList ll = InitPLL("LLRemoveFromBeginning", 16, 8);
            try
            {
                ll.AddToEnd(new byte[] { 1 });
                ll.Remove(0);
                Assert.AreEqual(0, ll.Length());

                ll.AddToEnd(new byte[] { 1 });
                ll.AddToEnd(new byte[] { 2 });
                ll.Remove(1);
                Assert.AreEqual(1, ll.Length());

                ll.AddToEnd(new byte[] { 1 });
                ll.AddToEnd(new byte[] { 2 });
                ll.AddToEnd(new byte[] { 3 });
                ll.Remove(2);
                Assert.AreEqual(3, ll.Length());

                ll.AddToEnd(new byte[] { 1 });
                ll.AddToEnd(new byte[] { 2 });
                ll.AddToEnd(new byte[] { 3 });
                ll.AddToEnd(new byte[] { 4 });
                ll.Remove(3);
                Assert.AreEqual(6, ll.Length());
            }
            finally
            {
                ll.Close();
            }
        }

        private void RemoveFromMiddleTest()
        {
            ILinkedList ll = InitPLL("LLRemoveFromMiddle", 16, 8);
            try
            {
                int token1 = ll.AddToEnd(new byte[] { 1 });
                int token2 = ll.AddToEnd(new byte[] { 2 });
                int token3 = ll.AddToEnd(new byte[] { 3 });
                int token4 = ll.AddToEnd(new byte[] { 4 });

                ll.Remove(token3);

                Assert.AreEqual(token4, ll.GetNext(token2));
                Assert.AreEqual(token2, ll.GetPrevious(token4));
                Assert.AreEqual(3, ll.Length());
            }
            finally
            {
                ll.Close();
            }
        }

        private void RemoveFromEndTest()
        {
            ILinkedList ll = InitPLL("LLRemoveFromEnd", 16, 8);
            try
            {
                ll.AddToEnd(new byte[] { 1 });
                ll.AddToEnd(new byte[] { 2 });
                ll.AddToEnd(new byte[] { 3 });
                ll.AddToEnd(new byte[] { 4 });

                ll.Remove(4);

                Assert.AreEqual(0, ll.GetNext(3));
                Assert.AreEqual(3, ll.GetPrevious(0));
                Assert.AreEqual(3, ll.Length());
            }
            finally
            {
                ll.Close();
            }
        }
        
        [TestMethod]
        public void LengthTest()
        {
            ILinkedList ll = InitPLL("LLLength", 16, 8);
            try
            {
                Assert.AreEqual(0, ll.Length());

                int firstNode = ll.AddToEnd(new byte[0]);
                Assert.AreEqual(1, ll.Length());

                ll.AddToEnd(new byte[0]);
                Assert.AreEqual(2, ll.Length());

                ll.AddToEnd(new byte[0]);
                Assert.AreEqual(3, ll.Length());

                ll.Remove(firstNode);
                Assert.AreEqual(2, ll.Length());
            }
            finally
            {
                ll.Close();
            }
        }

        [TestMethod]
        public void GetElementSizeTest()
        {
            int eleSize = 9;
            ILinkedList ll = InitPLL("GetLLElementSize", eleSize, 8);
            try
            {
                Assert.AreEqual(eleSize, ll.GetElementSize());
            }
            finally
            {
                ll.Close();
            }
        }

        [TestMethod]
        public void PutGetUserHeaderTest()
        {
            ILinkedList ll = InitPLL("PutGetLLUserHeader", 16, 8);
            try
            {
                PutGetUserHeaderTestAssert(ll,  new byte[]{1,2,3,4,5,6,7,8});
                PutGetUserHeaderTestAssert(ll, new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
                PutGetUserHeaderTestAssert(ll, new byte[] { });
                PutGetUserHeaderTestAssert(ll, new byte[] { 255,255,255,255,255,255,255,255 });
            }
            finally
            {
                ll.Close();
            }
        }

        private static void PutGetUserHeaderTestAssert(ILinkedList ll, byte[] uheader)
        {
            ll.PutUserHeader(uheader);
            byte[] actual = ll.GetUserHeader();
            TestHelper.AssertByteArraysAreSame(uheader, actual);
        }

        [TestMethod]
        public void GetUserHeaderSizeTest()
        {
            int userHeaderSize = 16;
            ILinkedList ll = InitPLL("GetLLUserHeaderSize", 8, userHeaderSize);
            try
            {
                int actual = ll.GetUserHeaderSize();
                Assert.AreEqual(userHeaderSize, actual);
            }
            finally
            {
                ll.Close();
            }
        }

        [TestMethod]
        public void ReopenTest()
        {
            const string arrayName = "ReopenLLTest";
            const int elementSize = 7;
            const int userHeaderSize = 33;
            ILinkedList ll = InitPLL(arrayName, elementSize, userHeaderSize);
            try
            {
                byte[] firstBytes = new byte[] { 1, 1, 1, 1, 1, 1, 1, };
                byte[] secondBytes = new byte[] { 2, 2, 2, 2, 2, 2, 2, };
                byte[] thirdBytes = new byte[] { 3, 3, 3, 3, 3 ,3,3};
                byte[] fourthBytes = new byte[] { 4, 4, 4, 4, 4, 4, 4 };
                ll.AddToEnd(firstBytes);
                ll.AddToEnd(secondBytes);
                ll.AddToEnd(thirdBytes);
                ll.AddToEnd(fourthBytes);
                Assert.AreEqual(4, ll.Length());

                ll.Close();

                ll = new PersistentLinkedList(arrayName);
                Assert.AreEqual(4, ll.Length());

                Assert.AreEqual(elementSize, ll.GetElementSize());
                Assert.AreEqual(userHeaderSize, ll.GetUserHeaderSize());

                int first = ll.GetFirst();
                TestHelper.AssertByteArraysAreSame(firstBytes, ll.GetData(first));
                int second = ll.GetNext(first);
                TestHelper.AssertByteArraysAreSame(secondBytes, ll.GetData(second));
                int third = ll.GetNext(second);
                TestHelper.AssertByteArraysAreSame(thirdBytes, ll.GetData(third));
                int fourth = ll.GetNext(third);
                TestHelper.AssertByteArraysAreSame(fourthBytes, ll.GetData(fourth));

            }
            finally
            {
                ll.Close();
            }
        }
    }
}
