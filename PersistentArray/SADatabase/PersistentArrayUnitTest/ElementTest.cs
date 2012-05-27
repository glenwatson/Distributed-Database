using System.IO;
using ByteHelper;
using ByteHelper.Exceptions;
using ByteHelperUnitTest;
using Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PA;
using PA.Exceptions;
using Persistence;

namespace PAUnitTest
{
    [TestClass]
    public class ElementTest
    {
        /// <summary>
        ///A test for Element's ctor
        ///</summary>
        [TestMethod]
        public void ElementCtorTest()
        {
            ElementCtorTestAssert(new byte[] { 1, 2, 3, 4, 5, 6 });
            ElementCtorTestAssert(new byte[] { });
            ElementCtorTestAssert(new byte[] { 0 });
            ElementCtorTestAssert(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 });
            ElementCtorTestAssert(new byte[] { byte.MaxValue, byte.MinValue });
        }

        private static void ElementCtorTestAssert(byte[] data)
        {
            Element element = new Element(data);
            Assert.IsNotNull(element);
            TestHelper.AssertByteArraysAreSame(element.Data, data);
        }

        /// <summary>
        ///A test for WriteToStorage & ReadFromStorage
        ///</summary>
        [TestMethod]
        public void WriteReadStorageTest()
        {
            IStorage storage = new RandomAccessFile(File.Open("WriteReadStorageTest", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None));

            WriteReadStorageTestAssert(10, new byte[] { 1, 2, 3, 4, 5, 6, 7 }, storage);
            WriteReadStorageTestAssert(10, new byte[] { }, storage);
            WriteReadStorageTestAssert(10, new byte[] { byte.MaxValue, byte.MinValue }, storage);
            try {
                WriteReadStorageTestAssert(5, new byte[] { 1, 2, 3, 4, 5, 6, 7 }, storage);
                Assert.Fail("Should throw exception");
            }catch (SizeTooSmallException) { }

            storage.Close();
        }

        private void WriteReadStorageTestAssert(int elementSize, byte[] data, IStorage storage)
        {
            Element expected = new Element(data);
            byte[] expectedBytes = expected.Serialize().ExtendTo(elementSize);
            storage.WriteByteArray(expectedBytes);
            storage.Seek(0);
            Element actual = Element.ReadFromStorage(storage, elementSize);
            storage.Seek(0);
            AssertElementsAreSame(expected, actual);
        }

        private void AssertElementsAreSame(Element expected, Element actual)
        {
            TestHelper.AssertByteArraysAreSame(expected.Data, actual.Data);
        }
    }
}
