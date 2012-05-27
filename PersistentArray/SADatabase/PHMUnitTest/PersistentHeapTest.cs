using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ByteHelperUnitTest;
using FilePersistence.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PA.Exceptions;
using PHM;

namespace PHMUnitTest
{
    [TestClass]
    public class PersistentHeapTest
    {
        private PersistentHeap InitHeap(string heapName, int userHeaderSize)
        {
            PersistentHeap heap;
            try
            {
                heap = new PersistentHeap(heapName, userHeaderSize);
            }
            catch (FileNameConflictException)
            {
                heap = new PersistentHeap(heapName);
                heap.Delete();
                heap = new PersistentHeap(heapName, userHeaderSize);
            }
            return heap;
        }

        [TestMethod]
        public void CtorCreateTest()
        {
            const int userHeaderSize = 7;
            PersistentHeap heap = InitHeap("CtorCreateHeapTest", userHeaderSize);
            Assert.AreEqual(userHeaderSize, heap.GetUserHeaderSize());
            heap.Close();

            const int userHeaderSize2 = 25;
            PersistentHeap heap2 = InitHeap("CtorCreateHeapTest", userHeaderSize2);
            Assert.AreEqual(userHeaderSize2, heap2.GetUserHeaderSize());
            heap2.Close();
        }

        [TestMethod]
        public void CtorOpenTest()
        {
            const int userHeaderSize = 7;
            string heapName = "CtorOpenHeapTest";
            PersistentHeap heap = InitHeap(heapName, userHeaderSize);
            heap.Close();
            heap = new PersistentHeap(heapName);
            Assert.AreEqual(userHeaderSize, heap.GetUserHeaderSize());
            heap.Close();
        }

        [TestMethod]
        public void PutGetTest()
        {
            const int userHeaderSize = 7;
            PersistentHeap heap = InitHeap("PutGetHeapTest", userHeaderSize);
            try
            {
                PutGetTestAssert(new byte[] { 1, 2, 3, 4 }, heap);
                PutGetTestAssert(new byte[] { 5, 6, 7, 8 }, heap);
                PutGetTestAssert(new byte[] { byte.MinValue, byte.MinValue, byte.MinValue, byte.MinValue, }, heap);
                PutGetTestAssert(new byte[] { byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue }, heap);
            }
            finally
            {
                heap.Close();
            }
        }

        private static void PutGetTestAssert(byte[] buffer, PersistentHeap heap)
        {
            int token = heap.Allocate(buffer.Length);
            heap.Put(token, buffer);
            TestHelper.AssertByteArraysAreSame(buffer, heap.Get(token));
        }

        [TestMethod]
        public void PutGetUserHeaderTest()
        {
            PutGetUserHeaderTestAssert(new byte[] { 1, 2, 3, 4, 5, 6, 7 });
            PutGetUserHeaderTestAssert(new byte[] { 1, 2, 3, 4 });
            PutGetUserHeaderTestAssert(new byte[] { 0 });
            PutGetUserHeaderTestAssert(new byte[] {  });
            PutGetUserHeaderTestAssert(new byte[] { byte.MaxValue });
            

        }

        private void PutGetUserHeaderTestAssert(byte[] newUserHeader)
        {
            PersistentHeap heap = InitHeap("PutGetHeapUserHeaderTest", newUserHeader.Length);
            try
            {
                heap.PutUserHeader(newUserHeader);
                byte[] actual = heap.GetUserHeader();

                TestHelper.AssertByteArraysAreSame(newUserHeader, actual);
            }
            finally
            {
                heap.Close();
            }
        }

        [TestMethod]
        public void GetUserHeaderSizeTest()
        {
            const int userHeaderSize = 77;
            PersistentHeap heap = InitHeap("GetHeapUserHeaderSizeTest", userHeaderSize);
            try
            {
                Assert.AreEqual(userHeaderSize, heap.GetUserHeaderSize());
            }
            finally
            {
                heap.Close();
            }
        }

        private static int PutGetAssert(byte[] bytes, PersistentHeap heap)
        {
            Debug.Assert(bytes.Length >= 4, "Your byte[] is too small to be put");
            int space1 = heap.Allocate(bytes.Length);
            heap.Put(space1, bytes);
            TestHelper.AssertByteArraysAreSame(bytes, heap.Get(space1));
            return space1;
        }

        [TestMethod]
        public void AllocateInARowTest()
        {
            const int userHeaderSize = 7;
            PersistentHeap heap = InitHeap("HeapAllocateInARowTest", userHeaderSize);
            try
            {
                int space1 = PutGetAssert(new byte[] {1, 2, 3, 4}, heap);
                int space2 = PutGetAssert(new byte[] {5, 6, 7,8}, heap);
                int space3 = PutGetAssert(new byte[] {  9, 10, 11,12 }, heap);
                int space4 = PutGetAssert(new byte[] {  13, 14, 15,16,17 }, heap);

            }
            finally
            {
                heap.Close();
            }
        }

        [TestMethod]
        public void AllocateInMiddleTest()
        {
            const int userHeaderSize = 7;
            PersistentHeap heap = InitHeap("HeapAllocateInMiddleTest", userHeaderSize);
            try
            {
                int space1 = PutGetAssert(new byte[] { 1, 2, 3, 4 }, heap);
                int space2 = PutGetAssert(new byte[] {  5, 6, 7, 8 }, heap);
                int space3 = PutGetAssert(new byte[] {  9, 10, 11, 12 }, heap);

                heap.Free(space2);

                int sameAsSpace2 = PutGetAssert(new byte[] { 12, 13, 14, 15 }, heap);

                Assert.AreEqual(space2, sameAsSpace2);

            }
            finally
            {
                heap.Close();
            }
        }

        [TestMethod]
        public void AllocateAtEndOfFreeSpaceListTest()
        {
            const int userHeaderSize = 7;
            PersistentHeap heap = InitHeap("HeapAllocateAtEndOfFreeSpaceListTest", userHeaderSize);
            try
            {
                int space1 = PutGetAssert(new byte[] { 1, 2, 3,4 }, heap);
                int space2 = PutGetAssert(new byte[] {  5, 6, 7,8 }, heap);
                int space3 = PutGetAssert(new byte[] {  9, 10, 11, 12 }, heap);
                int space4 = PutGetAssert(new byte[] { 13, 14, 15, 16 }, heap);

                heap.Free(space2);
                heap.Free(space4);

                int biggerThanAvailable = PutGetAssert(new byte[] { 14, 15, 16,17,18,19,20 }, heap);

                Assert.AreNotEqual(space1, biggerThanAvailable);
                Assert.AreNotEqual(space2, biggerThanAvailable);
                Assert.AreNotEqual(space3, biggerThanAvailable);
                Assert.AreNotEqual(space4, biggerThanAvailable);
                Assert.IsTrue(space4 < biggerThanAvailable);

            }
            finally
            {
                heap.Close();
            }
        }

        [TestMethod]
        public void AllocateSplitTest()
        {
            PersistentHeap heap = InitHeap("HeapAllocateSplitTest", 9);
            try
            {
                int space1 = PutGetAssert(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8,9, 10, 11, 12 }, heap);
                int space2 = PutGetAssert(new byte[] { 13, 14, 15, 16,17, 18, 19, 20, 21, 22, 23, 24 }, heap);
                int space3 = PutGetAssert(new byte[] { 25, 26, 27, 28, 29, 30, 31, 32,33,34,35,36 }, heap);
                int space4 = PutGetAssert(new byte[] { 37,38,39,40,41,42,43,44,45,46,47,48 }, heap);

                heap.Free(space2);

                int space2_3 = heap.Allocate(4);

                Assert.IsTrue(space2 < space2_3);
                Assert.IsTrue(space2_3 < space3);

            }
            finally
            {
                heap.Close();
            }
        }

        [TestMethod]
        public void AllocateFillAfterSplitTest()
        {
            PersistentHeap heap = InitHeap("HeapAllocateFillAfterSplitTest", 9);
            try
            {
                //int space1 = PutGetAssert(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, heap);
                //int space2 = PutGetAssert(new byte[] { 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 }, heap);
                //int space3 = PutGetAssert(new byte[] { 33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48 }, heap);
                //int space4 = PutGetAssert(new byte[] { 49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64 }, heap);
                int space1 = PutGetAssert(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, heap);
                int space2 = PutGetAssert(new byte[] { 13, 14, 15, 16,17,18,19,20,21,22,23,24 }, heap);
                int space3 = PutGetAssert(new byte[] { 25, 26, 27, 28, 29, 30, 31, 32,33,34,35,36 }, heap);
                int space4 = PutGetAssert(new byte[] { 37,38,39,40,42,43,44,45,46,47,48 }, heap);

                heap.Free(space2);

                int newSpace2 = PutGetAssert(new byte[] { 16, 32, 64, 128 }, heap);
                Assert.AreEqual(space2+8, newSpace2);

            }
            finally
            {
                heap.Close();
            }
        }

        [TestMethod]
        public void AllocateTooSmallSize()
        {
            PersistentHeap heap = InitHeap("HeapAllocateInARowTest", 5);
            try
            {
                try
                {
                    heap.Allocate(-1);
                    Assert.Fail("Should throw exception");
                }
                catch (InvalidElementSizeException) { }

                heap.Allocate(4);

                try
                {
                    heap.Allocate(3);
                    Assert.Fail("Should throw exception");
                }
                catch (InvalidElementSizeException) { }

            }
            finally
            {
                heap.Close();
            }
        }

        [TestMethod]
        public void FreeFirstSpaceTest()
        {
            PersistentHeap heap = InitHeap("HeapFreeFirstSpaceTest", 5);
            try
            {
                int firstSpace = heap.Allocate(4);
                heap.Allocate(6);
                heap.Allocate(7);

                heap.Free(firstSpace);
            }
            finally
            {
                heap.Close();
            }
        }

        [TestMethod]
        public void FreeLastSpaceTest()
        {
            PersistentHeap heap = InitHeap("HeapFreeLastSpaceTest", 5);
            try
            {
                heap.Allocate(4);
                heap.Allocate(6);
                int lastSpace = heap.Allocate(7);

                heap.Free(lastSpace);
            }
            finally
            {
                heap.Close();
            }
        }

        [TestMethod]
        public void FreeMiddleSpaceTest()
        {
            PersistentHeap heap = InitHeap("HeapFreeMiddleSpaceTest", 5);
            try
            {
                heap.Allocate(4);
                int middle = heap.Allocate(6);
                heap.Allocate(7);

                heap.Free(middle);
            }
            finally
            {
                heap.Close();
            }
        }

        [TestMethod]
        public void FreeAddSpaceToStartOfFreeListTest()
        {
            PersistentHeap heap = InitHeap("HeapFreeAddSpaceToStartOfFreeListTest", 5);
            try
            {
                int first = heap.Allocate(4);
                heap.Allocate(6);
                int toFree1 = heap.Allocate(7);
                heap.Allocate(8);
                heap.Allocate(10);
                heap.Allocate(5);

                heap.Free(toFree1);
                heap.Free(first);
            }
            finally
            {
                heap.Close();
            }
        }

        [TestMethod]
        public void FreeAddSpaceToEndOfFreeListTest()
        {
            PersistentHeap heap = InitHeap("HeapFreeAddSpaceToEndOfFreeListTest", 5);
            try
            {
                heap.Allocate(4);
                heap.Allocate(6);
                int toFree1 = heap.Allocate(7);
                heap.Allocate(8);
                heap.Allocate(10);
                int last = heap.Allocate(5);

                heap.Free(toFree1);
                heap.Free(last);
            }
            finally
            {
                heap.Close();
            }
        }

        [TestMethod]
        public void FreeAddSpaceToMiddleOfFreeListTest()
        {
            PersistentHeap heap = InitHeap("HeapFreeAddSpaceToMiddleOfFreeListTest", 5);
            try
            {
                int before = heap.Allocate(4);
                heap.Allocate(6);
                int middle = heap.Allocate(7);
                heap.Allocate(8);
                int after = heap.Allocate(10);
                heap.Allocate(5);

                heap.Free(before);
                heap.Free(after);
                heap.Free(middle);
            }
            finally
            {
                heap.Close();
            }
        }

        [TestMethod]
        public void FreeMergeBeforeTest()
        {
            PersistentHeap heap = InitHeap("HeapFreeMergeBeforeTest", 5);
            try
            {
                heap.Allocate(4);
                heap.Allocate(6);
                int before = heap.Allocate(7);
                int next = heap.Allocate(8);
                heap.Allocate(10);
                heap.Allocate(5);

                heap.Free(before);
                heap.Free(next);
            }
            finally
            {
                heap.Close();
            }
        }

        [TestMethod]
        public void FreeMergeAfterTest()
        {
            PersistentHeap heap = InitHeap("HeapFreeMergeAfterTest", 5);
            try
            {
                heap.Allocate(4);
                heap.Allocate(6);
                heap.Allocate(7);
                int previous = heap.Allocate(8);
                int after = heap.Allocate(10);
                heap.Allocate(5);

                heap.Free(after);
                heap.Free(previous);
            }
            finally
            {
                heap.Close();
            }
        }

        [TestMethod]
        public void FreeMergeBothTest()
        {
            PersistentHeap heap = InitHeap("HeapFreeMergeBothTest", 5);
            try
            {
                heap.Allocate(4);
                heap.Allocate(6);
                int before = heap.Allocate(7);
                int middle = heap.Allocate(8);
                int after = heap.Allocate(10);
                heap.Allocate(5);

                heap.Free(after);
                heap.Free(before);
                heap.Free(middle);
            }
            finally
            {
                heap.Close();
            }
        }
    }
}
